using Microsoft.Xna.Framework;
using LAP.Assets.TextureRegister;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Ranged 
{
    public class ExoboompersistentoldRanged : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetDefaults()
        {
            Projectile.arrow = false;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.extraUpdates = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 5;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ProjectileType<ExoboomoldRanged>(), Projectile.damage / 3, 0, Projectile.owner, 0f, 0f);
                }
            }
            target.immune[Projectile.owner] = 0;
            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ProjectileType<ExoboomoldRanged>(), Projectile.damage / 3, 0, Projectile.owner, 0f, 0f);
            }
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ProjectileType<ExoboomoldRanged>(), Projectile.damage / 3, 0, Projectile.owner, 0f, 0f);
                }
            }
        }
    }
}
