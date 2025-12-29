using CalamityInheritance.Content.Items.Weapons.Melee.Swords;
using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Swords
{
    public class ExaltedOathBladeProj : GeneralDamageProj
    {
        public override ProjDamageType UseDamageClass => ProjDamageType.Melee;
        private const float MaxVelocity = ExaltedOathbladeLegacy.ShootSpeed * 4f;

        private const int FadeOutTime = 85;

        private const int TimeLeft = 435 + FadeOutTime;

        private const int Alpha = 100;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 46;
            Projectile.alpha = Alpha;
            Projectile.timeLeft = TimeLeft;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            Projectile.MaxUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20 * Projectile.MaxUpdates;
        }

        public override void AI()
        {
            float alphaLightScale = Projectile.alpha / (float)Alpha;
            Lighting.AddLight(Projectile.Center, 0.5f * alphaLightScale, 0f, 0.5f * alphaLightScale);

            if (Projectile.timeLeft > FadeOutTime)
            {
                if (Projectile.velocity.Length() < MaxVelocity)
                    Projectile.velocity *= 1.03f;
            }
            else
                Projectile.velocity *= 0.95f;

            if (Projectile.velocity.X < 0f)
                Projectile.spriteDirection = -1;

            Projectile.rotation += Projectile.direction * 0.025f;
            if (Projectile.timeLeft > FadeOutTime)
                Projectile.rotation += Projectile.direction * 0.25f;
            else
                Projectile.rotation += Projectile.direction * 0.25f * (Projectile.timeLeft / (float)FadeOutTime);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < FadeOutTime)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(Alpha * ((float)b2 / byte.MaxValue));
                Projectile.alpha = a2;
                return new Color(b2, b2, b2, Projectile.alpha);
            }
            Projectile.alpha = Alpha;
            return new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, Projectile.alpha);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 180);
            target.AddBuff(BuffID.ShadowFlame, 90);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
    }
}
