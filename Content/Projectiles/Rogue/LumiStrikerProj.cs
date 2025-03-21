using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityMod.Particles;
using Terraria.Audio;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class LumiStrikerProj : ModProjectile, ILocalizedModType
    {
        public int HitCounter = 0;
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public static readonly SoundStyle Hitsound = new("CalamityMod/Sounds/Item/WulfrumKnifeTileHit2") { PitchVariance = 0.4f, Volume = 0.5f };
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Rogue/LumiStriker";
        public override void SetDefaults()
        {
            Projectile.width = 86;
            Projectile.height = 102;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 13;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 4;
            Projectile.tileCollide = false;
        }
        
        public override void AI()
        {
            
            if (Projectile.Calamity().stealthStrike)
            {
                StealthAI();
                //干掉普攻的AI
                return;
            }
            SpamAI();
        }
        public void SpamAI()
        {
            //飞行粒子
            if (Main.rand.NextBool(4))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BubbleBurst_Blue, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            
            //转角
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver4;

            //生成月明碎片
            if (Projectile.timeLeft % 4 == 0 && Projectile.owner == Main.myPlayer)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, -2f, ModContent.ProjectileType<LumiShard>(), (int)(Projectile.damage * 0.5), Projectile.knockBack * 0.25f, Projectile.owner);
        }
        public void StealthAI()
        {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver4;
            if (Main.rand.NextBool(4))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BubbleBurst_Blue, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            //刷新射弹属性
            if (Projectile.localAI[0] == 0f)    
            {
                Projectile.timeLeft = 1200;
                Projectile.localNPCHitCooldown = 60;
                Projectile.extraUpdates = 4;
                Projectile.penetrate = 4;
                Projectile.localAI[0] += 1f;
            }
            //保留飞行期间滞留生成夜明碎片的效果，但是这一效果被压制到一个较低的量, 同时伤害也压制了一下
            if (Projectile.timeLeft % 10 == 0) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X + Main.rand.NextFloat(-15f, 15f), Projectile.Center.Y + Main.rand.NextFloat(-15, 15f), Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<LumiShard>(), (int)(Projectile.damage * 0.20), Projectile.knockBack * 0.25f, Projectile.owner, 1f, 1f);
            //计时器
            Projectile.ai[0] += 1f;
            //本体矛投掷出去时会有一段逐渐减速的时间
            if (Projectile.ai[0] < 10f)
                Projectile.velocity *= 0.85f; 
            //中间会停顿片刻
            if (Projectile.ai[0] > 18f)
            {
                //转角
                Projectile.ai[0] = 18f;
                Projectile.ai[1] += 1f;
                //计数器置零的时候才会允许发起追踪
                if (Projectile.ai[1] % 45 == 0)
                    HitCounter = 0;
                Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver4;
                if (HitCounter != 1)
                    CIFunction.HomeInOnNPC(Projectile, true, 1800f, 27f, 75f);
            }
            //飞行过程中生成粒子轨迹
            SpawnDust(); 
        }
        public void SpawnDust()
        {
            if (Main.rand.NextBool())
            {
                Vector2 offset = new Vector2(12, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(4, 0).RotatedBy(offset.ToRotation());
                float dFlyVelX = Projectile.velocity.X * 0.4f + velOffset.X;
                float dFlyVelY = Projectile.velocity.Y * 0.4f + velOffset.Y;

                //克隆锤子在追踪时生成的粒子速度要更快也更大一点
                dFlyVelX *= 1.25f;
                dFlyVelY *= 1.25f;
                offset *= 1.05f;
                float dScale = 1.2f;
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, DustID.BubbleBurst_Blue, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(6))
            {
                Vector2 offset = new Vector2(12, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(4, 0).RotatedBy(offset.ToRotation());
                float dFlyVelX = Projectile.velocity.X * 0.5f + velOffset.X;
                float dFlyVelY = Projectile.velocity.Y * 0.5f + velOffset.Y;

                //克隆锤子在追踪时生成的粒子速度更快, 粒子大小更大, 且偏移也会更大一些
                dFlyVelX *= 1.25f;
                dFlyVelY *= 1.25f;
                offset *= 1.05f;
                float dScale = 1.2f;
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, DustID.BubbleBurst_Purple, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
                dust.noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.Calamity().stealthStrike)
            {
                HitCounter += 1;
                //每次击中一次，速度会被乘以0.8f的同时降低1eU
                Projectile.extraUpdates -=1;
                Projectile.velocity *= 0.8f;
                target.immune[Projectile.owner] = 7;
            }
            OnHitEffect(target);
            SoundEngine.PlaySound(Hitsound, target.Center);
            //火花
            OnHitSparks(target);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            OnHitEffect(target);
            SoundEngine.PlaySound(Hitsound, target.Center);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BubbleBurst_Blue, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
        }
        public void OnHitEffect(object target)
        {
            for (int i = 0; i < 7; i++)
            {
                //保证速度不为0
                Vector2 speed = new(Main.rand.NextBool() ? Main.rand.Next(1, 51) : Main.rand.Next(-50, 0),
                                    Main.rand.NextBool() ? Main.rand.Next(1, 51) : Main.rand.Next(-50, 0));
                speed.Normalize();
                speed *= Main.rand.Next(30,61) * 0.1f * 2f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, speed, ModContent.ProjectileType<LumiShard>(), (int)(Projectile.damage * 0.5), Projectile.knockBack * 0.25f, Projectile.owner);
            }
            //音效
        }
        public void OnHitSparks(NPC target)
        {
            int sparkCount = Main.rand.Next(4, 6);
            for (int i = 0; i < sparkCount; i++)
            {
                Vector2 sVel = Projectile.velocity.RotatedByRandom(0.2f) * Main.rand.NextFloat(0.6f, 1.1f);
                int sLife = Main.rand.Next(23, 25);
                float sScale = Main.rand.NextFloat(0.8f, 1f) * 0.955f;
                Color sColor = Color.Lerp(Color.DarkSlateBlue, Color.DarkBlue, Main.rand.NextFloat(0.7f));
                sColor = Color.Lerp(sColor, Color.Gold, Main.rand.NextFloat());
                SparkParticle spark = new(Projectile.Center, sVel, true, sLife, sScale, sColor);
                GeneralParticleHandler.SpawnParticle(spark);
            }
        }
    }
}