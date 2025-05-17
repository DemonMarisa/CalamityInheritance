using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader.Core;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.UI
{
    public static class DraedonsPanelUIManager
    {

        // 暂停所有GUI
        public static void SuspendAll()
        {
            DraedonsPanelUI.Active = false;
            DraedonsPanelUI.FadeTime = 0;
        }

        // 更新和绘制
        public static void UpdateAndDraw(SpriteBatch spriteBatch)
        {
            // 当游戏有其他UI时暂停所有弹窗UI
            if (Main.ingameOptionsWindow || Main.inFancyUI || Main.InGameUI.IsVisible)
            {
                SuspendAll();
                return;
            }

            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer cIPlayer = player.CIMod();

            if (DraedonsPanelUI.Active)
            {
                if (DraedonsPanelUI.FadeTime < DraedonsPanelUI.FadeTimeMax)
                    DraedonsPanelUI.FadeTime++;
            }
            else if (DraedonsPanelUI.FadeTime > 0)
            {
                DraedonsPanelUI.FadeTime--;
            }

            if (Main.mouseLeft && !DraedonsPanelUI.HoveringOverBook && DraedonsPanelUI.FadeTime >= 30)
            {
                DraedonsPanelUI.Active = false;
            }

            // 任何GUI活动
            if (DraedonsPanelUI.Active || DraedonsPanelUI.FadeTime > 0)
            {
                // 强制关闭玩家库存和 NPC 对话
                // 改为打开库存时关闭UI

                if (Main.LocalPlayer.sign > 0 || Main.LocalPlayer.talkNPC > 0)
                    Main.CloseNPCChatOrSign();

                // 更新活动弹窗
                DraedonsPanelUI.Update();

                // 处理绘制状态
                if (DraedonsPanelUI.FadeTime == 1 && !DraedonsPanelUI.Active)
                {
                    spriteBatch.End();
                    spriteBatch.Begin(); // 重置绘制批次
                    return;
                }

                DraedonsPanelUI.Draw(spriteBatch);
                // 在所有GUI的按钮检查完成后重置状态，避免按下，但是没有松开，而是直接离开判定区域，导致下一次悬停到其它按钮时直接触发切换
                if (!Main.mouseLeft)
                    cIPlayer.wasMouseDown = false;
            }
        }
        public static void FlipActivityOfGUIWithType()
        {
            DraedonsPanelUI.Active = !DraedonsPanelUI.Active;
        }

    }
}
