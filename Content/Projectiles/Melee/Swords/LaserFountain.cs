using CalamityMod;
using CalamityMod.Projectiles.Melee;
using LAP.Assets.TextureRegister;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Swords
{
    public class LaserFountain : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            float SpeedX = (float)Main.rand.Next(-15, 15);
            float SpeedY = (float)Main.rand.Next(-20, -10);
            if (Projectile.localAI[0] >= 12f)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    int damage = (int)Main.player[Projectile.owner].GetTotalDamage<MeleeDamageClass>().ApplyTo(350f);
                    int laserShot = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, SpeedX, SpeedY, ModContent.ProjectileType<NebulaShot>(), damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                    if (laserShot.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[laserShot].DamageType = DamageClass.Melee;
                        Main.projectile[laserShot].aiStyle = ProjAIStyleID.Arrow;
                    }
                }
                Projectile.localAI[0] = 0f;
            }
        }
    }
}
