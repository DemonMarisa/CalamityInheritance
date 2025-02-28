using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using rail;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeHammerTriactisTruePaladinianMageHammerofMight";
        bool ifSummonClone = false;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 160;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.usesLocalNPCImmunity= true;
            Projectile.localNPCHitCooldown= 7; //6-7
            Projectile.timeLeft = 1000;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            Lighting.AddLight(Projectile.Center, 0f, 0.5f, 0.75f);
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 30;
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }

            switch (Projectile.ai[0])
            {
                case 0f:
                    Projectile.ai[1] += 1f;
                    if (Projectile.ai[1] >= 40f)
                    {
                        Projectile.ai[0] = 1f;
                        Projectile.ai[1] = 0f;
                        Projectile.netUpdate = true;
                    }
                    break;

                case 1f:
                    float returnSpeed = 20f;
                    float acceleration = 3.2f;
                    CIFunction.BoomerangReturningAI(owner, Projectile, returnSpeed, acceleration);
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Rectangle projHitbox = new((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                        Rectangle mplrHitbox = new((int)owner.position.X, (int)owner.position.Y,
                                                   Projectile.ai[2] != -1f ? (int)(owner.width * 2.0f) : (int)(owner.width * 1.5f),
                                                   Projectile.ai[2] != -1f ? (int)(owner.height * 2.0f) : (int)(owner.height * 1.5f));
                        if (projHitbox.Intersects(mplrHitbox))
                        {
                            if(Projectile.Calamity().stealthStrike && Projectile.ai[2] != -1f) //挂载过的锤子不会再执行一次追踪
                            {
                                Projectile.velocity *= -1.3f;
                                Projectile.timeLeft = 600;
                                Projectile.penetrate = 1;
                                Projectile.localNPCHitCooldown = -1;
                                Projectile.netUpdate = true;
                                Projectile.ai[0] = 2f;
                            }
                            else if(Projectile.ai[2] == -1f) //ai[2]用于查看锤子是否已经挂载过敌人，如果挂载过了就会赋一个-1f的值
                            {
                                ReturnDust(); //只有挂载在敌人身上的锤子回收在玩家身上的时候才会生成粒子
                                SoundEngine.PlaySound(Main.rand.NextBool(2)? SoundMenu.HammerReturnID1 with {Volume = 0.5f} : SoundMenu.HammerReturnID2 with {Volume = 0.5f}, Projectile.Center);
                                Projectile.ai[2] = 0f;
                            }
                            else
                            Projectile.Kill();
                        }
                    }
                    break;
                
                case 2f:
                    CIFunction.HomeInOnNPC(Projectile, true, 1250f, 24f, 0f, 30f);
                    ifSummonClone = true;
                    break;

                default:
                    break;
            }
            //无论状态，锤子都应当在飞行过程中旋转
            Projectile.rotation += 0.22f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(250, 250, 250, 50);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //普攻与潜伏共享的效果: 爆炸
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjExplosion>(), (int)(Projectile.damage * 0.25), Projectile.knockBack, Projectile.owner, 0f, 0f);

            if(Projectile.Calamity().stealthStrike)
            {
                //潜伏专属效果：大幅度压制的爆炸粒子
                StealthSpawnDust();
                SpawnSparks(hit);
                SoundEngine.PlaySound(SoundMenu.HammerSmashID2 with {Volume = 0.5f}, Projectile.Center);
                if(ifSummonClone) //潜伏时生成的锤子才会具备挂载属性
                {
                    SoundEngine.PlaySound(SoundMenu.HammerSmashID2 with {Volume = 0.5f}, Projectile.Center);
                    SpawnSparks(hit);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>(), (int)(Projectile.damage * 0.6f), Projectile.knockBack, Main.myPlayer);
                }
                ifSummonClone = false;
            }
            
            
            //普攻效果
            if(!Projectile.Calamity().stealthStrike) 
            {
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
                if(Projectile.ai[2] != -1f) //只有非挂载结束后收回的锤子， 才会释放原灾特有的超级粒子
                SpawnDust();
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            //打人只会造成爆炸
            if(!Projectile.Calamity().stealthStrike) 
            {
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
                if(Projectile.ai[2] != -1f) //只有非挂载结束后收回的锤子， 才会释放原灾特有的超级粒子
                SpawnDust();
            }
        }

        private void ReturnDust()
        {
            for (int i = 0; i < 36; i++)
            {
                Dust fire = Dust.NewDustPerfect(Projectile.Center, DustID.GemEmerald);
                fire.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitY).RotatedByRandom(0.8f) * new Vector2(4f, 1.25f) * Main.rand.NextFloat(0.9f, 1f);
                fire.velocity = fire.velocity.RotatedBy(Projectile.rotation - MathHelper.PiOver2);
                fire.velocity += Projectile.velocity/4 *(5*0.04f);
                fire.noGravity = true;
                fire.scale = Main.rand.NextFloat(0.6f, 0.9f) * 5;
                fire = Dust.CloneDust(fire);
                fire.velocity = Main.rand.NextVector2Circular(3f, 3f);
                fire.velocity += Projectile.velocity/2*(5*0.04f);
            }
        }

        private void StealthSpawnDust() //潜伏攻击的粒子, 被削减至一个非常非常低的值
        {
            for (int i = 0; i < 7; i++)
            {
                int triactisDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 2f);
                Main.dust[triactisDust].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[triactisDust].scale = 0.5f;
                    Main.dust[triactisDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 7; j++)
            {
                int triactisDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 2f);
                Main.dust[triactisDust2].noGravity = true;
                Main.dust[triactisDust2].velocity *= 5f;
                triactisDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 2f);
                Main.dust[triactisDust2].velocity *= 2f;
            }

        }
        private void SpawnSparks(NPC.HitInfo hit)
        {
            float getDMGLerp = Utils.GetLerpValue(670f, 2000f, hit.Damage, true); //由造成伤害的多少来获取火花的距离偏移
            float getVelLerp = MathHelper.Lerp(0.08f, 0.2f, getDMGLerp);    //根据速度直接获得偏移
            getVelLerp *= Main.rand.NextBool().ToDirectionInt() * Main.rand.NextFloat(0.75f, 1.25f); //没有作用
            Vector2 splatterDirection = Projectile.velocity * 1.1f; //火花生成的方向, 在速度的正前方(不出意外的话)
            for (int i = 0; i < 15; i++)
            {
                int getSparkTime = Main.rand.Next(15, 35); //火花的生命刻
                float getSparkSize = Main.rand.NextFloat(0.7f, Main.rand.NextFloat(3.3f, 5.5f)) + getDMGLerp * 0.85f; //火花的大小
                Color getColor = Color.Lerp(Color.Green, Color.GhostWhite, Main.rand.NextFloat(0.7f)); //火花的颜色
                getColor = Color.Lerp(getColor, Color.ForestGreen, Main.rand.NextFloat());

                Vector2 getVelocity = splatterDirection.RotatedByRandom(0.1f) * Main.rand.NextFloat(1f, 1.2f); //火花的飞行速度
                SparkParticle spark = new SparkParticle(Projectile.Center, getVelocity, false, getSparkTime, getSparkSize, getColor);
                GeneralParticleHandler.SpawnParticle(spark);
            }

        }
        //正常击中敌人生成粒子
        private void SpawnDust()
        {
            for (int i = 0; i < 40; i++)
            {
                int triactisDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 2f);
                Main.dust[triactisDust].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[triactisDust].scale = 0.5f;
                    Main.dust[triactisDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 70; j++)
            {
                int triactisDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 3f);
                Main.dust[triactisDust2].noGravity = true;
                Main.dust[triactisDust2].velocity *= 5f;
                triactisDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 2f);
                Main.dust[triactisDust2].velocity *= 2f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
