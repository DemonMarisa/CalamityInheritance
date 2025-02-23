using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeHammerPwnageLegacyProj: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeHammerPwnageLegacy";
        public static readonly SoundStyle AdditionHitSigSound = new("CalamityMod/Sounds/Item/PwnagehammerSound") { Volume = 0.30f };
        private static float RotationIncrement = 0.23f;
        private static readonly float StealthSpeed = MeleeTypeHammerPwnageLegacy.Speed*2;
        private static readonly int LifeTime = 240;
        private static readonly int StealthLifeTime = 1025;
        private static readonly float ReboundTime = 36f;
        public override void SetDefaults()
        {
            Projectile.width = 68;
            Projectile.height = 68;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = LifeTime;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            DrawOffsetX = -11;
            DrawOriginOffsetY = -10;
            DrawOriginOffsetX = 0;

            Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.5f);

            //锤子飞行时应当播报声音
            if(Projectile.soundDelay == 0 && Projectile.ai[0] != 2f)
            {
                Projectile.soundDelay = 60;
                SoundEngine.PlaySound(SoundID.Item7,Projectile.position);
            }
            
            /*********************圣时之锤潜伏*************************
            *继承至大部分锤子的ai: ai[0]存储锤子是(1f)否(0f)处于返程状态, 
            *圣时之锤潜伏有与其他锤子的潜伏有一定的差异:
            *当允许进行潜伏时, 圣时之锤将赋值2f给ai[0]
            *进行潜伏时, 圣时之锤不会尝试返回至玩家手上, 而是直接锁定目标并挂载
            *挂载时间与大锤子的锁定时间差不多一致，但是不会造成非常高频率的攻击
            *在此时会加入了一个ai[2]作为计时器, 这一计时器唯一的作用是用来保证挂载不会一直发生, 因此ai[2]在挂载期间应当是一直大于0f的
            *挂载一旦结束, -1f将会赋值给ai[2], 0f将再次赋值给ai[0]
            *此时, 圣时之锤将会表现出在飞行一段时间后再次回击敌怪的效果
            *实现的方法有点史山, 看情况看看咋优化
            **********************************************************/

            switch(Projectile.ai[0])
            {
                case 0f:
                    Projectile.ai[1] += 1f;
                    if(Projectile.ai[1] >= (Projectile.ai[2] == -1f? ReboundTime + 50f : ReboundTime))
                    {
                        if(Projectile.Calamity().stealthStrike && Projectile.ai[2]!= -1f)
                        {
                            Projectile.ai[0] = 2f;
                            Projectile.timeLeft = StealthLifeTime;
                        }
                        else if(Projectile.ai[2] == -1f) //如果是挂载过的锤子, 将不会返程至玩家手上而是直接清除
                        {
                            OnChasingDust();
                            OnStuckEffect();
                            SoundEngine.PlaySound(SoundID.Item4 with {Volume = 0.5f} with {Pitch = 0.5f}, Projectile.Center); //播放落星声音以提示玩家锤子脱靶
                            Projectile.ai[0] = 4f;
                        }
                        else if(Projectile.ai[2] == 0f)
                           Projectile.ai[0] = 1f;
                        Projectile.ai[1] = 0f;
                        Projectile.netUpdate = true;
                    }
                    break;

                case 1f:
                    Projectile.tileCollide = false;
                    float returnSpeed = MeleeTypeHammerPwnageLegacy.Speed;
                    float acceleration = 1.6f;
                    CalamityInheritanceUtils.BoomerangReturningAI(owner, Projectile, returnSpeed, acceleration);
                    if(Main.myPlayer == Projectile.owner)
                    {
                        if(Projectile.Hitbox.Intersects(owner.Hitbox))
                           Projectile.Kill();
                    }
                    break;
                
                case 2f:
                    Projectile.usesIDStaticNPCImmunity = true;
                    Projectile.idStaticNPCHitCooldown = 18;
                    OnChasingDust();
                    CalamityInheritanceUtils.HomeInOnNPC(Projectile, true, 1800f, 10f, 16f); //挂载只会在计时器小于120f时进行
                    if(Projectile.timeLeft < LifeTime)
                    {
                        Projectile.velocity = new Vector2(0, Main.rand.NextBool(2)? 4f : -4f) ;
                        Projectile.ai[0] = 0f; //脱靶
                        Projectile.ai[2] = -1f;
                    }
                    break;

                case 3f:
                    CalamityInheritanceUtils.HomeInOnNPC(Projectile, true, 1800f, StealthSpeed, 4f);
                    OnChasingDust();
                    break;
                default:
                    Projectile.Kill();
                    break;
            }
            //无论状态，锤子都应当在飞行过程中旋转速度增快
            Projectile.rotation += RotationIncrement;
            return;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //击中时造成神圣之火
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 240);
            if(Projectile.ai[0] == 2f)
            OnStuckEffect();
            else 
            OnHitDust();

            if(Projectile.ai[0] == 3f)
            {
                CalamityInheritanceUtils.DustCircle(Projectile.position, 42f, 2.2f, 269, true, 9f);
                SoundEngine.PlaySound(AdditionHitSigSound with {Pitch = 0.2f});
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 600);
            OnHitDust();
        }

        private void OnStuckEffect()
        {
            CalamityInheritanceUtils.DustCircle(Projectile.position, 16f, 2.2f, 269, true, 9f, default, default, 6f);
            SoundEngine.PlaySound(AdditionHitSigSound with {Pitch = 0.15f});
        }
        private void OnChasingDust()
        {
            if (Main.rand.NextBool())
            {
                Vector2 offset = new Vector2(12f, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(4f, 0).RotatedBy(offset.ToRotation());
                float dFlyVelX = Projectile.velocity.X * 0.4f + velOffset.X;
                float dFlyVelY = Projectile.velocity.Y * 0.4f + velOffset.Y;

                //追踪时不让金色粒子拥有速度
                dFlyVelX *=  0f ;
                dFlyVelY *=  0f ;
                offset  *=  1.15f ;
                float dScale =  1.6f;
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, 269, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(6))
            {
                Vector2 offset = new Vector2(12, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(4, 0).RotatedBy(offset.ToRotation());
                float dFlyVelX = Projectile.velocity.X * 0.5f + velOffset.X;
                float dFlyVelY = Projectile.velocity.Y * 0.5f + velOffset.Y;

                //克隆锤子在追踪时生成的粒子速度更快, 粒子大小更大, 且偏移也会更大一些
                dFlyVelX *= 1.25f ;
                dFlyVelY *= 1.25f ;
                offset  *= 1.05f ;
                float dScale = 1.6f ;
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, DustID.GemRuby, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
                dust.noGravity = true;
            }
        }
        private void OnHitDust() //击中时生成神圣粒子
        {
            float dCounts = 15f;
            float rotArg = 360f/10f;
            for (int i = 0; i < dCounts; ++i)
            {
                float rotate = MathHelper.ToRadians(i*rotArg);
                Vector2 dPos = new Vector2(4.8f, 0).RotatedBy(rotate * Main.rand.NextFloat(1.1f, 3.8f));
                Vector2 dVelocity = new Vector2(4f, 0).RotatedBy(rotate * Main.rand.NextFloat(1.1f, 3.8f));
                Dust dust = Dust.NewDustPerfect(Projectile.Center + dPos, 269, new Vector2(dVelocity.X, dVelocity.Y));
                dust.noGravity = true;
                dust.velocity = dVelocity;
                dust.scale = Main.rand.NextFloat(0.8f, 1.1f);
            }
            //可以考虑生成一些小爆炸, 但这三王后的锤子要啥自行车?   
        }
    }
}