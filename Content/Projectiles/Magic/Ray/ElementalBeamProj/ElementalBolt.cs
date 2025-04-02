using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray.ElementalBeamProj
{
    public class ElementalBolt : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";

        public const int Lifetime = 150;
        public ref float Timer => ref Projectile.ai[0];

        public int Time = 0;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 10;
            Projectile.extraUpdates = 100;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 260;
        }

        public override void AI()
        {
            Vector2 vector = new Vector2(5f, 10f);
            if (Projectile.position.Y > Main.player[Projectile.owner].position.Y - 50f)
            {
                Projectile.tileCollide = true;
            }

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] == 48f)
            {
                Projectile.ai[0] = 0f;
            }
            else
            {
                for (int i = 0; i < 1; i++)
                {
                    _ = Vector2.UnitX * -12f;
                    Vector2 vector2 = -Vector2.UnitY.RotatedBy(Projectile.ai[0] * (MathF.PI / 24f) + i * MathF.PI) * vector - Projectile.rotation.ToRotationVector2() * 10f;
                    int dType = Dust.NewDust(Projectile.Center, 0, 0, DustID.RainbowTorch, 0f, 0f, 160, new Color(9, 212, 184));
                    Main.dust[dType].scale = 0.75f;
                    Main.dust[dType].noGravity = true;
                    Main.dust[dType].position = Projectile.Center + vector2;
                    Main.dust[dType].velocity = Projectile.velocity;
                }
            }

            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] >= 29f && Projectile.owner == Main.myPlayer)
            {
                Projectile.localAI[1] = 0f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<ElementalOrb>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner, 0f, 0f);
            }

            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
                for (int j = 0; j < 1; j++)
                {
                    Vector2 position = Projectile.position;
                    position -= Projectile.velocity * (j * 0.25f);
                    int dTypeAlt = Dust.NewDust(position, 1, 1, DustID.RainbowTorch, 0f, 0f, 0, new Color(9,212,184));
                    Main.dust[dTypeAlt].noGravity = true;
                    Main.dust[dTypeAlt].position = position;
                    Main.dust[dTypeAlt].scale = Main.rand.Next(70, 110) * 0.013f;
                    Main.dust[dTypeAlt].velocity *= 0.1f;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 8;
        }
    }
}
