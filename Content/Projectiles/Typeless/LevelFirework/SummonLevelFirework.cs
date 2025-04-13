using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using CalamityInheritance.Content.Items;
using CalamityMod.NPCs.TownNPCs;

namespace CalamityInheritance.Content.Projectiles.Typeless.LevelFirework
{
    public class SummonLevelFirework : ModProjectile, ILocalizedModType
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
            // 复制的原版小黄色烟花的代码
            Vector2 randomCirclePointVector = Vector2.One.RotatedByRandom(MathHelper.ToRadians(32f));
            float lerpStart = Main.rand.Next(9, 14) * 0.66f;;
            float lerpEnd = Main.rand.Next(2, 4) * 0.66f;
            for (float i = 0; i < 9f; ++i)
            {
                for (int j = 0; j < 2; ++j)
                {
                    Vector2 randomCirclePointRotated = randomCirclePointVector.RotatedBy((j == 0 ? 1 : -1) * MathHelper.TwoPi / 18);
                    for (float k = 0f; k < 20f; ++k)
                    {
                        Vector2 randomCirclePointLerped = Vector2.Lerp(randomCirclePointVector, randomCirclePointRotated, k / 20f);
                        float lerpMultiplier = MathHelper.Lerp(lerpStart, lerpEnd, k / 20f);
                        int dustIndex = Dust.NewDust(Projectile.position, 6, 6,
                            DustID.Firework_Pink,
                            0f, 0f, 100, default, 1.3f);
                        Main.dust[dustIndex].velocity *= 0.1f;
                        Main.dust[dustIndex].noGravity = true;
                        Main.dust[dustIndex].velocity += randomCirclePointLerped * lerpMultiplier;
                    }
                }
                randomCirclePointVector = randomCirclePointVector.RotatedBy(MathHelper.TwoPi / 9);
            }

            for (int num835 = 0; num835 < 80; num835++)
            {
                float num836 = lerpStart;
                int num837 = 133;
                if (num835 < 80)
                {
                    num836 = lerpEnd - 0.5f;
                }
                int num838 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 6, 6, num837, 0f, 0f, 100);
                float num839 = Main.dust[num838].velocity.X;
                float y7 = Main.dust[num838].velocity.Y;
                if (num839 == 0f && y7 == 0f)
                {
                    num839 = 1f;
                }
                float num840 = (float)Math.Sqrt(num839 * num839 + y7 * y7);
                num840 = num836 / num840;
                num839 *= num840;
                y7 *= num840;
                Dust dust265 = Main.dust[num838];
                Dust dust3 = dust265;
                dust3.velocity *= 0.25f;
                Main.dust[num838].velocity.X += num839;
                Main.dust[num838].velocity.Y += y7;
                Main.dust[num838].scale = 1.3f;
                Main.dust[num838].noGravity = true;
            }
        }
    }
}
