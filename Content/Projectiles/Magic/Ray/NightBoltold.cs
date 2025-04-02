using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray
{
    public class NightBoltold : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";

        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.extraUpdates = 20;
            Projectile.friendly = true;
            Projectile.timeLeft = 30;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 dustSpawnPos = Projectile.position - Projectile.velocity * i / 2f;
                Dust corruptMagic = Dust.NewDustPerfect(dustSpawnPos, 27);
                corruptMagic.color = Color.Lerp(Color.Fuchsia, Color.Magenta, Main.rand.NextFloat(0.6f));
                corruptMagic.scale = Main.rand.NextFloat(0.96f, 1.04f);
                corruptMagic.noGravity = true;
                corruptMagic.velocity *= 0.1f;
            }
        }
    }
}
