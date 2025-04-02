using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items.Weapons;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeKnivesShadowspecProjClone: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => $"{GenericProjRoute.ProjRoute}/Rogue/RogueTypeKnivesShadowspecProj";
        public static readonly float Acceleration = 0.98f; //飞行加速度
        public int HitCounts = 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.timeLeft = 650;
            Projectile.usesLocalNPCImmunity = true;
        }
        /*********************AI逻辑************************
        *描述:  潜伏时掷出一支拥有极高速的直线飞刀，这个飞刀没有追踪能力
        *超高速的直线飞刀击中敌怪时，会先造成一定程度的滞留伤害，而后，从敌怪内部爆出10~18个普通飞刀
        *普通飞刀采用类似于宇宙灾兵的逻辑
        *还没开始写，先全部注释掉,  等需要做了就会继续写
        *Unlimited Lost Works.
        ****************************************************/
        public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            //获取超高速飞刀的双坐标飞行速度
            float tryGetVelocityX = Projectile.velocity.X; 
            float tryGetVelocityY = Projectile.velocity.Y; 
            FlyingSound(); //飞刀飞行期间的音效
            FlyingDust();  //飞行粒子
            switch (Projectile.ai[0])
            {
                case 0f:
                    Projectile.Hitbox = new Rectangle((int)Projectile.velocity.X, (int)Projectile.velocity.Y, Projectile.width, Projectile.height);
                    Projectile.extraUpdates = 10;
                    Projectile.penetrate = -1;
                    Projectile.localNPCHitCooldown = -1;
                    if(HitCounts == 1)
                    {
                        Projectile.ai[0] = 1f;
                    }
                    break;
                case 1f:
                    Projectile.ai[1] += 1f;//自增
                    Projectile.extraUpdates = 1;//降低额外更新
                    Projectile.penetrate = 4;
                    Projectile.localNPCHitCooldown = 30;//给予一个滞留无敌帧
                    CIFunction.HomeInOnNPC(Projectile, true, 1800f, 32f, 0f); //锁定这个敌人
                    if(Projectile.ai[1] >= 50f) //锁定时间符合要求
                    {
                        Projectile.ai[0] = 2f;
                        Projectile.ai[1] = 50f;
                    }
                    break;
                case 2f:
                    BlastAI(Projectile);
                    break;
            }
        }

        public static void BlastAI(Projectile projectile)
        {
            projectile.ai[2] += 1f;//计时器自增
                    if(projectile.ai[2] == 1f)
                    {
                        projectile.penetrate = 2; //因为没有计划使用timeLeft来杀死弹幕，因此这里改为了多穿
                        projectile.extraUpdates = 1;
                        projectile.localNPCHitCooldown = 30; //->降低为20
                        int knivesAmt = Main.rand.Next(10, 15); //10->15
                        float rot = 360f/knivesAmt;
                       
                        for(int i = 0; i < knivesAmt; i++)
                        {
                            float rotArg = MathHelper.ToRadians(i* rot);
                            Vector2 rotPos = new Vector2(10f, 0f).RotatedBy(rotArg);
                            Vector2 rotVel = new Vector2(10f, 0f).RotatedBy(rotArg);
                            Player player = Main.player[projectile.owner];
                            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center + rotPos, rotVel,
                                                    ModContent.ProjectileType<RogueTypeKnivesShadowspecProjClone>(),
                                                    projectile.damage, projectile.knockBack, Main.myPlayer, 2f, 0f, projectile.ai[2] + 1f);
                        }
                    }
                    if(projectile.ai[2] > 40)
                    {
                        projectile.ai[2] = 40;
                        CIFunction.HomeInOnNPC(projectile, true, 1800f, 32f, 0f); //锁定这个敌人
                    }
        }
        //Give it a custom hitbox shape so it may remain rectangular and elongated
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;
            float bladeHalfLength = 25f * Projectile.scale / 2f;
            float bladeWidth = 14f * Projectile.scale;

            Vector2 direction = (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2();

            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - direction * bladeHalfLength, Projectile.Center + direction * bladeHalfLength, bladeWidth, ref collisionPoint);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                int illustrious = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurificationPowder, 0f, 0f, 100, default, 0.8f);
                Main.dust[illustrious].noGravity = true;
                Main.dust[illustrious].velocity *= 1.2f;
                Main.dust[illustrious].velocity -= Projectile.oldVelocity * 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(Projectile.ai[0] == 0f)
            {
                HitCounts += 1;
                Projectile.velocity.X *= 0.1f;
                Projectile.velocity.Y *= 0.1f;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
        public void FlyingSound() //飞行时的音效
        {
            Projectile.localAI[0] += 1f;
            if(Projectile.localAI[0] > 5f && Projectile.localAI[0] < 10f) //飞刀速度极快，因此这5f可能仅仅只是一瞬 
                SoundEngine.PlaySound(SoundID.AbigailCry with {Volume = 0.7f}, Projectile.Center);//阿比盖尔的哭泣
        }
        public void FlyingDust()
        {
            Vector2 getPos = new Vector2(16f, 0).RotatedByRandom(MathHelper.ToRadians(360f));
            Vector2 getVel = new Vector2(16f, 0).RotatedBy(getPos.ToRotation());
            float xflyingVel = Projectile.velocity.X * 0.5f + getVel.X; 
            float yflyingVel = Projectile.velocity.Y * 0.5f + getVel.Y; 
            float dScale = 1.5f;
            Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) +getPos, DustID.GemEmerald, new Vector2(xflyingVel, yflyingVel), 50, default, dScale);
            dust.noGravity = true;
        }
    }
}
