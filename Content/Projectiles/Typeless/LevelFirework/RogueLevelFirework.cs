using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Items;
using Terraria.Audio;

namespace CalamityInheritance.Content.Projectiles.Typeless.LevelFirework
{
    public class RogueLevelFirework : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override void SetDefaults()
        {
            Projectile.arrow = false;
            Projectile.width = 14;
            Projectile.height = 28;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120;
            Projectile.friendly = true;
        }
        // 从原版复制的
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 1f)
            {
                for (int i = 0; i < 8; i++)
                {
                    int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1.8f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 3f;
                    Main.dust[d].fadeIn = 0.5f;
                    Main.dust[d].position += Projectile.velocity / 2f;
                    Main.dust[d].velocity += Projectile.velocity / 4f + Main.player[Projectile.owner].velocity * 0.1f;
                }
            }
            if (Projectile.ai[0] > 2f)
            {
                int d2 = Dust.NewDust(new Vector2(Projectile.position.X + 2f, Projectile.position.Y + 20f), 8, 8, DustID.Torch, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.2f);
                Main.dust[d2].noGravity = true;
                Main.dust[d2].velocity *= 0.2f;
                Main.dust[d2].position = Main.dust[d2].position.RotatedBy(Projectile.rotation, Projectile.Center);
                d2 = Dust.NewDust(new Vector2(Projectile.position.X + 2f, Projectile.position.Y + 15f), 8, 8, DustID.Torch, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.2f);
                Main.dust[d2].noGravity = true;
                Main.dust[d2].velocity *= 0.2f;
                Main.dust[d2].position = Main.dust[d2].position.RotatedBy(Projectile.rotation, Projectile.Center);
                d2 = Dust.NewDust(new Vector2(Projectile.position.X + 2f, Projectile.position.Y + 10f), 8, 8, DustID.Torch, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.2f);
                Main.dust[d2].noGravity = true;
                Main.dust[d2].velocity *= 0.2f;
                Main.dust[d2].position = Main.dust[d2].position.RotatedBy(Projectile.rotation, Projectile.Center);
            }
        }
        public static List<Color> DustColors = new List<Color>
        {
            new Color(69, 69, 222),
            new Color(99, 66, 212),
            new Color(130, 64, 214),
            new Color(154, 75, 219),
            new Color(165, 62, 201)
        };
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(in CISoundID.SoundBomb, Projectile.position);
            int rosePetalCount = 7;
            int bigWavyPetalCount = 8;
            float speed1 = 4;
            float speed2 = 7;

            for (float k = 0f; k < MathHelper.TwoPi; k += 0.03f)
            {
                float scale = Main.rand.NextFloat(1.1f, 1.4f);
                float randomWhitingValue = Main.rand.NextFloat(0.0f, 0.2f);
                Color color = Color.Lerp(DustColors[Main.rand.Next(0, DustColors.Count)], Color.White, randomWhitingValue);

                Vector2 velocity = k.ToRotationVector2() * (2f + (float)(Math.Sin((double)(k * rosePetalCount)) + 1.0) * speed2) * Main.rand.NextFloat(0.95f, 1.05f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.FireworksRGB, new Vector2?(velocity), 0, color, scale);
                dust.noGravity = true;

                dust.velocity *= 0.65f;
            }

            for (float k = 0f; k < MathHelper.TwoPi; k += 0.08f)
            {
                float scale = Main.rand.NextFloat(1.2f, 1.6f);
                float randomWhitingValue = Main.rand.NextFloat(0.0f, 0.2f);
                Color color = Color.Lerp(DustColors[Main.rand.Next(0, DustColors.Count)], Color.White, randomWhitingValue);

                Vector2 velocity = k.ToRotationVector2() * (float)(Math.Cos((double)(k * bigWavyPetalCount)) + 5.1f) * speed1 / 2;
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.FireworksRGB, new Vector2?(velocity), 0, color, scale);
                dust.noGravity = true;

                dust.velocity *= 0.65f;
            }
        }
    }
}
