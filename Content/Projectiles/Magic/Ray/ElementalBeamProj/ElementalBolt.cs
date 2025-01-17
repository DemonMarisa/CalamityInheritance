using CalamityMod;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray.ElementalBeamProj
{
    public class ElementalBolt : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";

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
            if (base.Projectile.position.Y > Main.player[base.Projectile.owner].position.Y - 50f)
            {
                base.Projectile.tileCollide = true;
            }

            base.Projectile.ai[0] += 1f;
            if (base.Projectile.ai[0] == 48f)
            {
                base.Projectile.ai[0] = 0f;
            }
            else
            {
                for (int i = 0; i < 1; i++)
                {
                    Vector2 vector2 = Vector2.UnitX * -12f;
                    vector2 = -Vector2.UnitY.RotatedBy(base.Projectile.ai[0] * (MathF.PI / 24f) + i * MathF.PI) * vector - base.Projectile.rotation.ToRotationVector2() * 10f;
                    int num = Dust.NewDust(base.Projectile.Center, 0, 0, 66, 0f, 0f, 160, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB));
                    Main.dust[num].scale = 0.75f;
                    Main.dust[num].noGravity = true;
                    Main.dust[num].position = base.Projectile.Center + vector2;
                    Main.dust[num].velocity = base.Projectile.velocity;
                }
            }

            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] >= 29f && Projectile.owner == Main.myPlayer)
            {
                Projectile.localAI[1] = 0f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<ElementalOrb>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner, 0f, 0f);
            }

            base.Projectile.localAI[0] += 1f;
            if (base.Projectile.localAI[0] > 4f)
            {
                for (int j = 0; j < 1; j++)
                {
                    Vector2 position = base.Projectile.position;
                    position -= base.Projectile.velocity * (j * 0.25f);
                    int num2 = Dust.NewDust(position, 1, 1, DustID.RainbowTorch, 0f, 0f, 0, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB));
                    Main.dust[num2].noGravity = true;
                    Main.dust[num2].position = position;
                    Main.dust[num2].scale = Main.rand.Next(70, 110) * 0.013f;
                    Main.dust[num2].velocity *= 0.1f;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 8;
        }
    }
}
