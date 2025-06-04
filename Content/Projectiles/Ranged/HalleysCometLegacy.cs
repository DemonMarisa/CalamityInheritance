using CalamityMod.Buffs.DamageOverTime;
using Terraria;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class HalleysCometLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 10;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            if (Projectile.scale <= 1.5f)
            {
                Projectile.scale *= 1.01f;
            }

            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.35f / 255f, (255 - Projectile.alpha) * 0f / 255f, (255 - Projectile.alpha) * 0.45f / 255f);

            if (Projectile.ai[0]++ > 5f)
            {
                float dustScaleSize = 1f;
                if (Projectile.ai[0] == 6f)
                {
                    dustScaleSize = 0.25f;
                }
                else if (Projectile.ai[0] == 7f)
                {
                    dustScaleSize = 0.5f;
                }
                else if (Projectile.ai[0] == 8f)
                {
                    dustScaleSize = 0.75f;
                }
                Projectile.ai[0] += 1f;
                int dustType = 176;
                for (int i = 0; i < 3; i++)
                {
                    int fire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 1, default, 1f);
                    Dust dust = Main.dust[fire];
                    if (Main.rand.NextBool(3))
                    {
                        dust.noGravity = true;
                        dust.scale *= 1.75f;
                        dust.velocity.X *= 2f;
                        dust.velocity.Y *= 2f;
                    }
                    else
                    {
                        dust.scale *= 0.5f;
                    }
                    dust.velocity.X *= 1.2f;
                    dust.velocity.Y *= 1.2f;
                    dust.scale *= dustScaleSize;
                    dust.velocity += Projectile.velocity;
                    if (!dust.noGravity)
                    {
                        dust.velocity *= 0.5f;
                    }
                }
            }

            Projectile.rotation += 0.3f * Projectile.direction;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Nightwither>(), 240);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Nightwither>(), 240);
        }
    }
}
