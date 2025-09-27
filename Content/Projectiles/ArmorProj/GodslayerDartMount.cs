using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ArmorProj
{
    public class GodslayerDartMount : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";
        public int hasfirecount = 0;
        public int firedely = 2;
        public int mountdartfirerelay = 3;
        public int randomstart = Main.rand.Next(0, 9);
        public override void SetDefaults()
        {
            Projectile.width = 0;
            Projectile.height = 0;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 50;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void AI()
        {
            Player Owner = Main.player[Projectile.owner];

            Vector2 armPosition = Owner.RotatedRelativePoint(Owner.MountedCenter, true);
            var source = Projectile.GetSource_FromThis();
            firedely--;
            float baseAngle = Projectile.velocity.ToRotation();
            int numberOfProjectiles = 8;
            float spreadAngle = MathHelper.ToRadians(360);
            float angleStep = spreadAngle / numberOfProjectiles;
            float randStartAngle = 22.5f + 45 * randomstart;
            float currentAngle = baseAngle - spreadAngle / 2 + (angleStep * hasfirecount) + MathHelper.ToRadians(randStartAngle);
            Vector2 direction = new((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));

            if (firedely == 0 && hasfirecount < 8)
            {
                Projectile.NewProjectile(source, armPosition, direction * 32f, ModContent.ProjectileType<GodSlayerDart>(), Projectile.damage, 2f, Projectile.owner, 1, 0f);
                firedely = 3;
                hasfirecount++;
                Projectile.netUpdate = true;
            }
        }
    }
}
