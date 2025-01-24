﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class ExoTornado : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => "CalamityInheritance/Content/Projectiles/TornadoProj";

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 80;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 0;
        }

        public override void AI()
        {
            float num1125 = 165f; //900
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = -1;
                SoundEngine.PlaySound(SoundID.Item122, Projectile.position);
            }
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= num1125)
            {
                Projectile.Kill();
            }
            if (Projectile.localAI[0] >= 30f)
            {
                Projectile.damage = 0;
                if (Projectile.ai[0] < num1125 - 120f)
                {
                    float num1126 = Projectile.ai[0] % 60f;
                    Projectile.ai[0] = num1125 - 120f + num1126;
                    Projectile.netUpdate = true;
                }
            }
            float num1127 = 30f; //15
            float num1128 = 30f; //15
            Point point8 = Projectile.Center.ToTileCoordinates();
            int num1129;
            int num1130;
            Collision.ExpandVertically(point8.X, point8.Y, out num1129, out num1130, (int)num1127, (int)num1128);
            num1129++;
            num1130--;
            Vector2 value72 = new Vector2(point8.X, num1129) * 16f + new Vector2(8f);
            Vector2 value73 = new Vector2(point8.X, num1130) * 16f + new Vector2(8f);
            Vector2 vector146 = Vector2.Lerp(value72, value73, 0.5f);
            Vector2 value74 = new Vector2(0f, value73.Y - value72.Y);
            value74.X = value74.Y * 0.2f;
            Projectile.width = (int)(value74.X * 0.65f);
            Projectile.height = (int)value74.Y;
            Projectile.Center = vector146;
            if (Projectile.owner == Main.myPlayer)
            {
                bool flag74 = false;
                Vector2 center16 = Main.player[Projectile.owner].Center;
                Vector2 top = Main.player[Projectile.owner].Top;
                for (float num1131 = 0f; num1131 < 1f; num1131 += 0.05f)
                {
                    Vector2 position2 = Vector2.Lerp(value72, value73, num1131);
                    if (Collision.CanHitLine(position2, 0, 0, center16, 0, 0) || Collision.CanHitLine(position2, 0, 0, top, 0, 0))
                    {
                        flag74 = true;
                        break;
                    }
                }
                if (!flag74 && Projectile.ai[0] < num1125 - 120f)
                {
                    float num1132 = Projectile.ai[0] % 60f;
                    Projectile.ai[0] = num1125 - 120f + num1132;
                    Projectile.netUpdate = true;
                }
            }
            if (Projectile.ai[0] < num1125 - 120f)
            {
                return;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            float num226 = 150f; //600
            float num227 = 30f; //15
            float num228 = 30f; //15
            float num229 = Projectile.ai[0];
            float maxAlpha = 0.3f;
            float scale5 = MathHelper.Clamp(num229 / 30f, 0f, maxAlpha); //Fade in
            if (num229 > num226 - 60f)
            {
                scale5 = MathHelper.Lerp(maxAlpha, 0f, (num229 - (num226 - 60f)) / 60f); //Fade out
            }
            Point point5 = Projectile.Center.ToTileCoordinates();
            int num230;
            int num231;
            Collision.ExpandVertically(point5.X, point5.Y, out num230, out num231, (int)num227, (int)num228);
            num230++;
            num231--;
            float num232 = 0.2f;
            Vector2 value32 = new Vector2(point5.X, num230) * 16f + new Vector2(8f);
            Vector2 value33 = new Vector2(point5.X, num231) * 16f + new Vector2(8f);
            Vector2.Lerp(value32, value33, 0.5f);
            Vector2 vector33 = new Vector2(0f, value33.Y - value32.Y);
            vector33.X = vector33.Y * num232;
            new Vector2(value32.X - vector33.X / 2f, value32.Y);
            Texture2D texture2D23 = ModContent.Request<Texture2D>(Texture).Value;
            Rectangle rectangle9 = texture2D23.Frame(1, 1, 0, 0);
            Vector2 origin3 = rectangle9.Size() / 2f;
            float num233 = -0.06283186f * num229;
            Vector2 spinningpoint2 = Vector2.UnitY.RotatedBy((double)(num229 * 0.1f), default);
            float num234 = 0f;
            float num235 = 5.1f;
            Color value34 = new Color(0, 250, 0);
            for (float num236 = (int)value33.Y; num236 > (int)value32.Y; num236 -= num235)
            {
                num234 += num235;
                float num237 = num234 / vector33.Y;
                float num238 = num234 * 6.28318548f / -20f;
                float num239 = num237 - 0.15f;
                Vector2 vector34 = spinningpoint2.RotatedBy((double)num238, default);
                Vector2 value35 = new Vector2(0f, num237 + 1f);
                value35.X = value35.Y * num232;
                Color color39 = Color.Lerp(Color.Transparent, value34, num237 * 2f);
                if (num237 > 0.5f)
                {
                    color39 = Color.Lerp(Color.Transparent, value34, 2f - num237 * 2f);
                }
                color39.A = (byte)(color39.A * 0.5f);
                color39 *= scale5;
                vector34 *= value35 * 100f;
                vector34.Y = 0f;
                vector34.X = 0f;
                vector34 += new Vector2(value33.X, num236) - Main.screenPosition;
                Main.spriteBatch.Draw(texture2D23, vector34, new Microsoft.Xna.Framework.Rectangle?(rectangle9), color39, num233 + num238, origin3, 1f + num239, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}
