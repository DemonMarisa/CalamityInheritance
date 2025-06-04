using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CalamityInheritance.Utilities.CIFunction;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI.Chat;
using Terraria;

namespace CalamityInheritance.UI.MusicUI.MusicButton
{
    public class TurnOffAll
    {
        public static int FadeTime = 0;
        public static int FadeTimeMax = 30;
        public static float Scale = 0;
        public static Vector2 Pos = Vector2.Zero;
        public static bool active = false;

        public static int SecondFadeTime = 0;
        public static int SecondFadeTimeMax = 15;

        public static bool IsHovering;
        public static bool wasMouseDown;
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
            float crdistance = 0;
            float progress = (float)FadeTime / (float)FadeTimeMax;

            crdistance = MathHelper.Lerp(0, maxDistance, EasingHelper.EaseOutBack(progress));

            Vector2 distance = new Vector2(crdistance, 0f);
            // 具体绘制位置
            // 此处是玩家上方100像素
            Pos = playerScreenPos + distance.RotatedBy(-MathHelper.PiOver2);
            #endregion

            #region 更新缩放
            float targetscale = 0.5f;
            Scale = MathHelper.Lerp(0, targetscale, EasingHelper.EaseOutBack(progress));
            #endregion
            string text = "CalTitleAll";
            if(MusicChoiceUI.turnOffAll)
                text = "CalTitleAllOn";
            #region 判定悬停
            MusicChoiceUI.FastButton(FadeTime, FadeTimeMax, ref Scale, ref Pos, ref IsHovering, ref SecondFadeTime, ref SecondFadeTimeMax, ref wasMouseDown, text, ref MusicChoiceUI.turnOffAll);
            #endregion
        }

        public static void UIDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(MusicChoiceUI.CircleTextures, Pos, null, Color.White, 0f, MusicChoiceUI.CircleTextures.Size() / 2, Scale, SpriteEffects.None, 0f);
            if (MusicChoiceUI.turnOffAll)
                spriteBatch.Draw(MusicChoiceUI.TurnOffTextures, Pos, null, Color.White, 0f, MusicChoiceUI.TurnOffTextures.Size() / 2, Scale, SpriteEffects.None, 0f);
            else
                spriteBatch.Draw(MusicChoiceUI.TurnOnTextures, Pos, null, Color.White, 0f, MusicChoiceUI.TurnOffTextures.Size() / 2, Scale, SpriteEffects.None, 0f);

            if (IsHovering && FadeTime == FadeTimeMax)
            {
                spriteBatch.Draw(MusicChoiceUI.CircleHightLightTextures, Pos, null, Color.White, 0f, MusicChoiceUI.CircleHightLightTextures.Size() / 2, Scale, SpriteEffects.None, 0f);

                string text = Language.GetTextValue("Mods.CalamityInheritance.UI.CalTitleAllOn");
                if (MusicChoiceUI.turnOffAll)
                    text = Language.GetTextValue("Mods.CalamityInheritance.UI.CalTitleAllOff");

                DynamicSpriteFont font = FontAssets.MouseText.Value;
                // 计算文本尺寸
                Vector2 textSize = ChatManager.GetStringSize(font, text, new Vector2(1f, 1f));
                Vector2 DrawPos = Main.MouseWorld - Main.screenPosition;
                if (!Main.mouseLeft)
                {
                    Utils.DrawBorderStringFourWay(spriteBatch, font, text, DrawPos.X, DrawPos.Y, Color.White, Color.Black, new Vector2(textSize.X / 2f, textSize.Y), 1f);
                }
            }
        }
    }
}
