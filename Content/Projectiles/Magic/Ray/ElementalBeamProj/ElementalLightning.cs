using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray.ElementalBeamProj
{
    public class ElementalLightning : ModProjectile
    {
        public ref float ShardCooldown => ref Projectile.ai[1];

        public int Time = 0;
        public ref float Timer => ref Projectile.ai[0];
        public new string LocalizationCategory => "Projectiles.Magic";
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";
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
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 260;
        }

        public override void AI()
        {
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] >= 40f && Projectile.owner == Main.myPlayer)
            {
                Projectile.localAI[1] = 0f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<ElementalRayMarkVortex>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner, 0f, 0f);
            }
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 9f)
            {
                Timer++;
                Time++;
                Lighting.AddLight(Projectile.Center + Projectile.velocity * 0.6f, 0.6f, 0.2f, 0.9f);
                float radiusFactor = MathHelper.Lerp(0f, 1f, Utils.GetLerpValue(5f, 7f, Time, true));
                for (int i = 0; i < 12; i++)
                {
                    float offsetRotationAngle = Projectile.velocity.ToRotation() + Time / 7f;
                    float radius = (7f + (float)Math.Cos(Time / 4f) * 3f) * radiusFactor;
                    Vector2 dustPosition = Projectile.Center;
                    dustPosition += offsetRotationAngle.ToRotationVector2().RotatedBy(i / 5f * MathHelper.TwoPi) * radius;
                    Dust dust = Dust.NewDustPerfect(dustPosition, Main.rand.NextBool() ? 107 : 180, default, default, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB));
                    dust.noGravity = true;
                    dust.velocity = Projectile.velocity * 0.1f;
                    dust.scale = Main.rand.NextFloat(1f, 1.2f);
                }
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 8;
        }
    }
}
