using CalamityInheritance.UI.BaseUI;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using static CalamityInheritance.Utilities.CIFunction;

namespace CalamityInheritance.UI.MusicUI.MusicButton
{
    public class ArrowBehavior
    {
        public static int FadeTime = 0;
        public static int FadeTimeMax = 30;
        public static float Scale = 0;
        public static float rot = 0;
        public static Vector2 Pos = Vector2.Zero;
        public static bool active = false;
        public static bool IsHovering;
        public static void Update(Rectangle mouseRectangle)
        {
            if (active)
            {
                if (FadeTime < FadeTimeMax)
                    FadeTime++;
            }
            else if (FadeTime > 0)
                FadeTime--;
            if (FadeTime == 0)
                return;
            #region 更新位置
            Player plr = Main.player[Main.myPlayer];
            // 实现从玩家到玩家头上位置变化
            Vector2 playerScreenPos = plr.Center - Main.screenPosition;
            // 默认向右100像素的位置绘制箭头，通关旋转来控制具体绘制在哪里
            float maxDistance = -50f;
            float crdistance = MathHelper.Lerp(0, maxDistance, EasingHelper.EaseOutBack((float)FadeTime / (float)FadeTimeMax));
            Vector2 distance = new Vector2(crdistance, 0f);
            // 具体绘制位置
            // 此处是玩家上方100像素
            Pos = playerScreenPos + distance.RotatedBy(MathHelper.PiOver2);
            #endregion

            #region 更新缩放
            float scale = MathHelper.Lerp(0, 0.5f, EasingHelper.EaseOutBack((float)FadeTime / (float)FadeTimeMax));
            Scale = scale;
            #endregion
            #region 更新旋转
            if (FadeTime == 1)
                rot = (Main.MouseWorld - plr.Center).ToRotation();
            else
                rot = rot.AngleLerp((Main.MouseWorld - (plr.Center + distance.RotatedBy(MathHelper.PiOver2))).ToRotation(), 0.25f);
            #endregion

            if(FadeTime != FadeTimeMax)
                return;

            #region 判定悬停
            if (FadeTime != FadeTimeMax)
                return;

            Rectangle Rect = new Rectangle(
                (int)(Pos.X - MusicChoiceUI.CircleTextures.Width * Scale / 2),
                (int)(Pos.Y - MusicChoiceUI.CircleTextures.Height * Scale / 2),
                (int)(MusicChoiceUI.CircleTextures.Width * Scale),
                (int)(MusicChoiceUI.CircleTextures.Height * Scale));

            IsHovering = Rect.Intersects(mouseRectangle);
            #endregion
        }

        public static void UIDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(MusicChoiceUI.CircleTextures, Pos, null, Color.White, 0f, MusicChoiceUI.CircleTextures.Size() / 2, Scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(MusicChoiceUI.ArrowTextures, Pos, null, Color.White, rot, MusicChoiceUI.ArrowTextures.Size() / 2, Scale, SpriteEffects.None, 0f);
            if (IsHovering)
                spriteBatch.Draw(MusicChoiceUI.CircleHightLightTextures, Pos, null, Color.White, 0f, MusicChoiceUI.CircleHightLightTextures.Size() / 2, Scale, SpriteEffects.None, 0f);
        }

    }
}
