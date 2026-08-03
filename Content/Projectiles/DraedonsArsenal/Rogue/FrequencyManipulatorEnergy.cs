using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.DraedonsArsenal.Rogue
{
    public class FrequencyManipulatorEnergy : CIRogueProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public float Time
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public const int Lifetime = 240;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = RogueDamage.Instance;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = Lifetime;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Time++;
            GenerateIdleDust();

            Projectile.HomeInNPC(920, 12, 35);
        }

        public void GenerateIdleDust()
        {
            if (!Main.dedServ)
            {
                // Generate a helical group of dust particles that pulsate with time.
                for (int i = 0; i < 3; i++)
                {
                    float angle = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                    float pulse = (float)Math.Sin(Time / 110f + MathHelper.TwoPi / 3f * i);
                    Vector2 offset = angle.ToRotationVector2().RotatedBy(MathHelper.TwoPi / 3f * i) * pulse * 6f;

                    Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, DustID.BoneTorch);
                    dust.velocity = Vector2.Zero;
                    dust.noGravity = true;

                    dust = Dust.NewDustPerfect(Projectile.Center + offset, DustID.BoneTorch);
                    dust.velocity = Vector2.Zero;
                    dust.noGravity = true;
                }
            }
        }
    }
}
