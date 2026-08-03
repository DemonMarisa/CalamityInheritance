using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.DraedonsArsenal.Ranged
{
    public class GaussRifleBlast : CIRangedProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public float Time
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Time++;
            Projectile.tileCollide = Time > 3f;
            if (!Main.dedServ)
            {
                // Idle dust.
                for (int i = 0; i < 10; i++)
                {
                    float angle = i / 10f * MathHelper.TwoPi;
                    for (int j = 0; j < 4; j++)
                    {
                        Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.PortalBolt);
                        dust.velocity = angle.ToRotationVector2().RotatedByRandom(0.25f) * Main.rand.NextFloat(6f, 8f);
                        dust.noGravity = true;
                        dust.scale = 1.6f;
                    }
                }
                // Ring dust.
                for (int i = 0; i < 4; i++)
                {
                    float angle = Projectile.velocity.ToRotation() + i / 4f * MathHelper.TwoPi + Time / 24f;
                    float radius = (float)Math.Sin(Time / 7.5f) * 40f + 10f;
                    Dust dust = Dust.NewDustPerfect(Projectile.Center + angle.ToRotationVector2() * radius, DustID.Electric);
                    dust.velocity = Vector2.Zero;
                    dust.noGravity = true;
                    dust.scale = 1.5f;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                SoundEngine.PlaySound(CISounds.LargeMechGaussRifle, Projectile.Center);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<GaussRifleExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }
    }
}