using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using CalamityInheritance.Utilities;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public class SoulMidPlaceholder: ModProjectile, ILocalizedModType
    {
        public ref float AttackTimer => ref Projectile.ai[0];
        public ref float BuffPing => ref Projectile.ai[2];
        
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.scale = 1.1f;
            Projectile.alpha = 100;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false; 
        }
        public override bool? CanDamage() => AttackTimer > 10f;
        public override void AI()
        {
            Projectile.FramesChanger(6, 4);
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - 1.57f;
            Lighting.AddLight(Projectile.Center, 0.5f, 0.2f, 0.9f);
            //保留这个AI
            AttackTimer++;
            if (AttackTimer > 10f)
            {
                if (Main.rand.NextBool(3))
                    CIFunction.HomeInOnNPC(Projectile, true, 1500f, 12f, 20f);
            }
            //粒子
            int ghostlyDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueFlare, 0f, 0f, 0, Color.SkyBlue, 1f);
            Dust dust = Main.dust[ghostlyDust];
            dust.velocity *= 0.1f;
            Main.dust[ghostlyDust].scale = 1.3f;
            Main.dust[ghostlyDust].noGravity = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > 595)
                return false;

            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                return new Color(b2, b2, b2, a2);
            }
            return CIConfig.Instance.DebugColor;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 120);
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 96;
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
            SoundEngine.PlaySound(SoundID.NPCDeath39, Projectile.position);
            int dustAmt = 36;
            for (int i = 0; i < dustAmt; i++)
            {
                Vector2 rotate = Vector2.Normalize(Projectile.velocity) * new Vector2(Projectile.width / 2f, Projectile.height) * 0.75f;
                rotate = rotate.RotatedBy((double)((i - (dustAmt / 2 - 1)) * 6.28318548f / dustAmt), default) + Projectile.Center;
                Vector2 faceDirection = rotate - Projectile.Center;
                int killedDust = Dust.NewDust(rotate + faceDirection, 0, 0, DustID.BlueFlare, faceDirection.X * 1.5f, faceDirection.Y * 1.5f, 100, Color.SkyBlue, 2f);
                Main.dust[killedDust].noGravity = true;
                Main.dust[killedDust].noLight = true;
                Main.dust[killedDust].velocity = faceDirection;
            }
        }
    }
}
