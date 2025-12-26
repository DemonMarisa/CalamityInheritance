using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent;
using CalamityMod.Buffs.DamageOverTime;
using LAP.Content.Configs;

namespace CalamityInheritance.NPCs.Boss.Yharon.Proj
{
    public class YharonFireballLegacy : ModProjectile
    {
        private float speedX = -3f;
        private float speedX2 = -5f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.DD2BetsyFireball;
            CooldownSlot = 1;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
            Projectile.rotation += MathHelper.Pi;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(speedX);
            writer.Write(Projectile.localAI[1]);
            writer.Write(speedX2);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            speedX = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
            speedX2 = reader.ReadSingle();
        }

        public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, Projectile.alpha);

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;
            Rectangle rectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            if (!LAPConfig.Instance.PerformanceMode)
            {
                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                    Color color2 = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                    Main.spriteBatch.Draw(texture, drawPos, new Microsoft.Xna.Framework.Rectangle?(rectangle), color2, Projectile.rotation, rectangle.Size() / 2f, Projectile.scale, spriteEffects, 0f);
                }
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
            new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, frameY, texture.Width, frameHeight)),
            Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2((float)texture.Width / 2f, (float)frameHeight / 2f), Projectile.scale, spriteEffects, 0f);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int x = 0; x < 3; x++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Center.X, Projectile.Center.Y, speedX, -50f, ModContent.ProjectileType<YharonFireball2Legacy>(), Projectile.damage, 0f, Main.myPlayer, 0f, 0f);
                    speedX += 3f;
                }
                for (int x = 0; x < 2; x++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, speedX2, -75f, ModContent.ProjectileType<YharonFireball2Legacy>(), Projectile.damage, 0f, Main.myPlayer, 0f, 0f);
                    speedX2 += 10f;
                }
            }
            CalamityUtils.ExpandHitboxBy(Projectile, 144);
            for (int d = 0; d < 2; d++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 55, 0f, 0f, 50, default, 1.5f);
            }
            for (int d = 0; d < 20; d++)
            {
                int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 55, 0f, 0f, 0, default, 2.5f);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 3f;
                idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 55, 0f, 0f, 50, default, 1.5f);
                Main.dust[idx].velocity *= 2f;
                Main.dust[idx].noGravity = true;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 180);
        }
    }
}
