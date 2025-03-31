using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class CosmicBoltLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.extraUpdates = 100;
            Projectile.friendly = true;
            Projectile.timeLeft = 180;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 spawnPos = Projectile.position;
                    spawnPos -= Projectile.velocity * (i * 0.25f);
                    Projectile.alpha = 255;
                    int d = Dust.NewDust(spawnPos, 1, 1, DustID.GemEmerald, 0f, 0f, 0, default, 1.3f);
                    Dust dust = Main.dust[d];
                    dust.position = spawnPos;
                    dust.position.X += Projectile.width / 2;
                    dust.position.Y += Projectile.height / 2;
                    dust.scale = Main.rand.NextFloat(0.49f, 0.763f);
                    dust.velocity *= 0.2f;
                    dust.noGravity = true;
                }
            }
        }
    }
}
