using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged.Misc
{
    public class RainbowBlast : CIRangedProj
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            //Rotation
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);

            Lighting.AddLight(Projectile.Center, new Vector3(Main.DiscoR, Main.DiscoG, Main.DiscoB) * (1.5f / 255));

            Projectile.localAI[0]++;
            if (Projectile.localAI[0] > 5f)
            {
                Vector2 dspeed = -Projectile.velocity * 0.5f;
                int rainbowDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowTorch, dspeed.X, dspeed.Y, 100, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.1f);
                Main.dust[rainbowDust].noGravity = true;
                Main.dust[rainbowDust].velocity = dspeed;
            }

            LAPUtilities.HomeInNPC(Projectile, 1500f, 12f, 35f, null, !Projectile.tileCollide);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            Color color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
            return color;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
