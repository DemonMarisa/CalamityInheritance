using CalamityInheritance.Content.Items.Placeables.Vanity;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.UI.MusicUI.MusicButton;
using static CalamityInheritance.Utilities.CIFunction;
using static tModPorter.ProgressUpdate;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.UI.Chat;
using Microsoft.CodeAnalysis.Text;
using static System.Net.Mime.MediaTypeNames;

namespace CalamityInheritance.UI.MusicUI
{
    public class MusicChoiceUI
    {
        public static string Path => "CalamityInheritance/UI/MusicUI/MusicButton";
        public static bool active = false;
        public static int currentActiveType = 0;
        public static Texture2D CircleTextures;
        public static Texture2D ArrowTextures;
        public static Texture2D CircleHightLightTextures;
        public static Texture2D MusicBoxTextures;
        public static Texture2D NorTextures;
        public static Texture2D PianoTextures;
        public static Texture2D TurnOffTextures;
        public static Texture2D TurnOnTextures;
        public static int aniProg = 0;
        public static int ChangeCd = 0;

        public static bool turnOffAll = false;
        public static void Load()
        {
            CircleTextures = ModContent.Request<Texture2D>($"{Path}/baseCircle", AssetRequestMode.ImmediateLoad).Value;
            ArrowTextures = ModContent.Request<Texture2D>($"{Path}/Arrow", AssetRequestMode.ImmediateLoad).Value;
            CircleHightLightTextures = ModContent.Request<Texture2D>($"{Path}/CircleHighLight", AssetRequestMode.ImmediateLoad).Value;
            MusicBoxTextures = ModContent.Request<Texture2D>($"{Path}/MusicBoxVer", AssetRequestMode.ImmediateLoad).Value;
            NorTextures = ModContent.Request<Texture2D>($"{Path}/NorVer", AssetRequestMode.ImmediateLoad).Value;
            PianoTextures = ModContent.Request<Texture2D>($"{Path}/PianoVer", AssetRequestMode.ImmediateLoad).Value;
            TurnOffTextures = ModContent.Request<Texture2D>($"{Path}/EffectOff", AssetRequestMode.ImmediateLoad).Value;
            TurnOnTextures = ModContent.Request<Texture2D>($"{Path}/EffectOn", AssetRequestMode.ImmediateLoad).Value;
        }

        public static void Unload()
        {
            CircleTextures = ArrowTextures = CircleHightLightTextures = MusicBoxTextures = NorTextures = PianoTextures = TurnOffTextures = TurnOnTextures = null;
        }

        public static void UpdateAndDraw(SpriteBatch spriteBatch)
        {
            int maxFadeTime = 60;
            #region 开启控制
            if (active)
            {
                // 因为是差速出现，所以要保证最大值一定要大于这个进度的最大值加打开的对应进度的最大值
                // 这一段如果过早退出会导致active的判定出问题
                if (aniProg < maxFadeTime)
                {
                    aniProg++;
                    ChangeCd = maxFadeTime / 2;
                }
                ArrowBehavior.active = true;
                TurnOffAll.active = true;
                if (aniProg == 10)
                    MusicBoxVerBehavior.active = true;
                if (aniProg == 20)
                    NorVerBehavior.active = true;
                if (aniProg == 30)
                    PianoVerBehavior.active = true;
            }
            else
            {
                if (aniProg > 0)
                {
                    aniProg--;
                    ChangeCd = maxFadeTime / 2;
                }
                if (aniProg == maxFadeTime - 30)
                    PianoVerBehavior.active = false;
                if (aniProg == maxFadeTime - 20)
                    NorVerBehavior.active = false;
                if (aniProg == maxFadeTime - 10)
                    MusicBoxVerBehavior.active = false;
                if (aniProg == maxFadeTime - 1)
                {
                    ArrowBehavior.active = false;
                    TurnOffAll.active = false;
                }
            }
            #endregion
            #region 调用绘制
            if (aniProg > 0)
            {
                ArrowBehavior.UIDraw(spriteBatch);
                MusicBoxVerBehavior.UIDraw(spriteBatch);
                NorVerBehavior.UIDraw(spriteBatch);
                PianoVerBehavior.UIDraw(spriteBatch);
                TurnOffAll.UIDraw(spriteBatch);
            }
            #endregion
            #region 调用更新
            Rectangle mouseRectangle = new((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 2, 2);
            ArrowBehavior.Update(mouseRectangle);
            MusicBoxVerBehavior.Update(mouseRectangle);
            NorVerBehavior.Update(mouseRectangle);
            PianoVerBehavior.Update(mouseRectangle);
            TurnOffAll.Update(mouseRectangle);
            #endregion
            if (ChangeCd > 0)
                ChangeCd--;
        }
        #region 绘制按钮封装
        public static void FastButton(int FadeTime, int FadeTimeMax,ref float Scale,ref Vector2 Pos 
            ,ref bool IsHovering,ref int SecondFadeTime,ref int SecondFadeTimeMax,ref bool wasMouseDown, string text
            ,ref bool changebool)
        {
            #region 更新缩放
            float targetscale = 0.5f;
            #endregion

            #region 判定悬停
            Rectangle mouseRectangle = new((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y, 2, 2);
            // 判定碰撞
            Rectangle Rect = new Rectangle(
                (int)(Pos.X - MusicChoiceUI.CircleTextures.Width * Scale / 2),
                (int)(Pos.Y - MusicChoiceUI.CircleTextures.Height * Scale / 2),
                (int)(MusicChoiceUI.CircleTextures.Width * Scale),
                (int)(MusicChoiceUI.CircleTextures.Height * Scale));
            // 判定悬停
            IsHovering = Rect.Intersects(mouseRectangle);
            float progress2 = (float)SecondFadeTime / (float)SecondFadeTimeMax;
            if (IsHovering && FadeTime == FadeTimeMax)
            {
                // 进度操作
                if (SecondFadeTime < SecondFadeTimeMax)
                    SecondFadeTime++;
                // 不按下鼠标时放大，按下后缩小
                if (!Main.mouseLeft)
                {
                    Scale = MathHelper.Lerp(targetscale, 0.6f, EasingHelper.EaseOutBack(progress2)); // 悬停时放大一点
                }
                else
                    Scale = MathHelper.Lerp(Scale, targetscale, 0.25f); // 按下时缩小一点
                // 禁用原鼠标功能
                Main.blockMouse = true;
                if (Main.mouseLeft)
                {
                    // 判定是否按下过
                    wasMouseDown = true;
                }
                // 按下过且从开才会触发
                if (!Main.mouseLeft && wasMouseDown && IsHovering)
                {
                    // 按重置
                    changebool = !changebool;
                    if (changebool)
                        SendTextOnPlayer($"Mods.CalamityInheritance.UI.{text}", Color.Orange);
                    else
                        SendTextOnPlayer($"Mods.CalamityInheritance.UI.{text}" + "Off", Color.Orange);
                    SecondFadeTime = 0;
                    wasMouseDown = false;
                }
            }
            else if (FadeTime == FadeTimeMax)
            {
                wasMouseDown = false;
                if (SecondFadeTime > 0)
                    SecondFadeTime--;
                Scale = MathHelper.Lerp(Scale, 0.5f, EasingHelper.EaseOutBack(progress2)); // 悬停时放大一点
            }

            #endregion
        }
        #endregion
    }
}
