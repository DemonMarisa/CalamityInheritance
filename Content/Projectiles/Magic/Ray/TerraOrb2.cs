using Terraria.ModLoader;
using LAP.Assets.TextureRegister;
using Terraria;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray
{
    public class TerraOrb2 : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            CalamityUtils.MagnetSphereHitscan(Projectile, 400f, 6f, 0f, 5, ProjectileType<TerraBolt2>());
            //CalamityUtils.MagnetSphereHitscan(Projectile, 300f, 6f, 0f, 5, ModContent.ProjectileType<TerraBolt2>());
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 8;
        }
    }
}
