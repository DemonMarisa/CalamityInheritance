using CalamityMod.NPCs.TownNPCs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod.Projectiles.Melee;
using CalamityInheritance.Content.Items;
using Terraria.Audio;

namespace CalamityInheritance.Content.Projectiles.Typeless.LevelFirework
{
    public class SummonLevelFirework_Final : ModProjectile, ILocalizedModType
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
            for (int j = 0; j < 400; j++)
            {
                // 阶段参数定义
                int dustType;
                float baseSpeed;
                float xMultiplier, yMultiplier;

                float randomWhitingValue = Main.rand.NextFloat(0.0f, 0.2f);
                Color color = Color.Lerp(DustColors[Main.rand.Next(0, DustColors.Count)], Color.White, randomWhitingValue);

                // 根据循环进度划分不同阶段
                if (j <= 100)
                {
                    dustType = DustID.Firework_Pink;
                    baseSpeed = 16f;
                    xMultiplier = 1f;
                    yMultiplier = 0.7f;
                }
                else if (j <= 200)
                {
                    dustType = 134;
                    color = default;
                    baseSpeed = 12f;
                    xMultiplier = 0.7f;
                    yMultiplier = 1f;
                }
                else if (j <= 300)
                {
                    dustType = DustID.Firework_Pink;
                    baseSpeed = 8f;
                    xMultiplier = 1f;
                    yMultiplier = 0.7f;
                }
                else
                {
                    dustType = 134;
                    color = default;
                    baseSpeed = 5f;
                    xMultiplier = 0.7f;
                    yMultiplier = 1f;
                }

                // 创建粒子
                int dustId = Dust.NewDust(Projectile.position, 6, 6, dustType, 0f, 0f, 100, color);
                Dust dust = Main.dust[dustId];
                Vector2 velocity = dust.velocity;

                // 初始化速度
                if (velocity == Vector2.Zero)
                    velocity.X = 1f;

                // 计算速度标准化
                float speedScale = baseSpeed / velocity.Length();
                velocity.X *= speedScale * xMultiplier;
                velocity.Y *= speedScale * yMultiplier;
                velocity *= 2f;

                // 应用速度并添加随机性
                dust.velocity = velocity * 0.5f;

                // 粒子特效设置
                if (Main.rand.NextBool(3))
                {
                    dust.scale = 1.3f;
                    dust.noGravity = true;
                }
            }
        }
    }
}
