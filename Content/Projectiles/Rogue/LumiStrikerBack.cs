using CalamityMod.Buffs.DamageOverTime;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using System;
using CalamityMod;
using CalamityInheritance.Content.Items;
using CalamityMod.Particles;
using CalamityInheritance.Particles;
using Terraria.ID;
using LAP.Assets.TextureRegister;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    // Photoviscerator left click main projectile (the flamethrower itself)
    public class LumiStrikerBack : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = GetInstance<RogueDamageClass>();
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 3;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
        }
        public ref float Time => ref Projectile.ai[0];
        public override void AI()
        {
            Time++;
            Lighting.AddLight(Projectile.Center + Projectile.velocity * 0.6f, 0.6f, 0.2f, 0.9f);
            float radiusFactor = MathHelper.Lerp(0f, 1f, Utils.GetLerpValue(10f, 50f, Time, true));
            for (int i = 0; i < 9; i++)
            {
                float offsetRotationAngle = Projectile.velocity.ToRotation() + Time / 20f;
                float radius = (20f + (float)Math.Cos(Time / 3f) * 12f) * radiusFactor;
                Vector2 dustPosition = Projectile.Center;
                dustPosition += offsetRotationAngle.ToRotationVector2().RotatedBy(i / 5f * MathHelper.TwoPi) * radius;
                Dust dust = Dust.NewDustPerfect(dustPosition, Main.rand.NextBool() ? DustID.BubbleBurst_Blue : DustID.BubbleBurst_Pink);
                dust.noGravity = true;
                dust.velocity = Projectile.velocity * 0.1f;
                dust.scale = Main.rand.NextFloat(1.1f, 1.7f);
            }

        }
    }
}
