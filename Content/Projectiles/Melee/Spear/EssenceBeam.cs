using CalamityMod.Buffs.DamageOverTime;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Melee.Spear
{
    public class EssenceBeam : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Essence Beam");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.aiStyle = 27;
            AIType = 156;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 10;
            Projectile.extraUpdates = 5;
            Projectile.timeLeft = 600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 0;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.6f, 0f, 0.2f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(200, 200, 200, Projectile.alpha);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > 595)
                return false;

            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300);
            target.immune[Projectile.owner] = 2;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            int dCounts;
            for (int i = 4; i < 31; i = dCounts + 1)
            {
                float pVelX = Projectile.oldVelocity.X * (30f / i);
                float pVelY = Projectile.oldVelocity.Y * (30f / i);
                int d = Dust.NewDust(new Vector2(Projectile.oldPosition.X - pVelX, Projectile.oldPosition.Y - pVelY), 8, 8, DustID.ShadowbeamStaff, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.8f);
                Main.dust[d].noGravity = true;
                Dust dust = Main.dust[d];
                dust.velocity *= 0.5f;
                d = Dust.NewDust(new Vector2(Projectile.oldPosition.X - pVelX, Projectile.oldPosition.Y - pVelY), 8, 8, DustID.ShadowbeamStaff, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.4f);
                dust = Main.dust[d];
                dust.velocity *= 0.05f;
                dCounts = i;
            }
        }
    }
}
