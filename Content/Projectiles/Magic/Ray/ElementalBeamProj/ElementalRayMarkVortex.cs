using Terraria.ModLoader;
using LAP.Assets.TextureRegister;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray.ElementalBeamProj
{
    public class ElementalRayMarkVortex : ModProjectile, ILocalizedModType
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
            int shardType = ProjectileType<ElementalOrbVortex>();
            int shardDamage = (int)(Projectile.damage * 0.5);
            if (Projectile.timeLeft <= 1)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, shardType, shardDamage, Projectile.knockBack, Projectile.owner);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 8;
        }
    }
}
