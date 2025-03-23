using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class NanotechOld : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.ai[1] >= 30f && target.CanBeChasedBy(Projectile);

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0.075f, 0.4f, 0.15f));

            Projectile.rotation += Projectile.velocity.X * 0.2f;
            if (Projectile.velocity.X > 0f)
                Projectile.rotation += 0.08f;
            else
                Projectile.rotation -= 0.08f;

            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] > 60f)
            {
                Projectile.alpha += 5;
                if (Projectile.alpha >= 255)
                {
                    Projectile.alpha = 255;
                    Projectile.Kill();
                    return;
                }
            }

            if (Projectile.ai[1] >= 30f)
                CalamityUtils.HomeInOnNPC(Projectile, true, 800f, 12f, 20f);
        }

        public override void OnKill(int timeLeft)
        {
            int inc;
            for (int i = 0; i < 2; i = inc + 1)
            {
                int dustScale = (int)(10f * Projectile.scale);
                int greenDust = Dust.NewDust(Projectile.Center - Vector2.One * dustScale, dustScale * 2, dustScale * 2, DustID.TerraBlade, 0f, 0f, 0, default, 1f);
                Dust nanoDust = Main.dust[greenDust];
                Vector2 dustDirection = Vector2.Normalize(nanoDust.position - Projectile.Center);
                nanoDust.position = Projectile.Center + dustDirection * dustScale * Projectile.scale;
                if (i < 30)
                {
                    nanoDust.velocity = dustDirection * nanoDust.velocity.Length();
                }
                else
                {
                    nanoDust.velocity = dustDirection * Main.rand.Next(45, 91) / 10f;
                }
                nanoDust.color = Main.hslToRgb((float)(0.4+ Main.rand.NextDouble() * 0.2), 0.9f, 0.5f);
                nanoDust.color = Color.Lerp(nanoDust.color, Color.White, 0.3f);
                nanoDust.noGravity = true;
                nanoDust.scale = 0.7f;
                inc = i;
            }
        }
    }
}
