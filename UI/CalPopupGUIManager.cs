using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader.Core;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.UI
{
    public static class CalPopupGUIManager
    {
        // 所有GUI实例
        // Scarlet: 这里疑似没有成功实例化，但我也不知道你要怎么实例化，反正看你
        // 需要想办法解决这一段去避免数组越界
        private static readonly List<CalPopupGUI> gUIs = [];
        // 状态判断
        public static bool GUIActive(CalPopupGUI gui) => gui.Active || gui.FadeTime > 0;
        public static bool AnyGUIsActive => gUIs.Any(GUIActive);
        public static CalPopupGUI GetActiveGUI => gUIs.FirstOrDefault(GUIActive);

        // 暂停所有GUI
        public static void SuspendAll()
        {
            for (int i = 0; i < gUIs.Count; i++)
            {
                gUIs[i].Active = false;
                gUIs[i].FadeTime = 0;
            }
        }

        // 更新和绘制
        public static void UpdateAndDraw(SpriteBatch spriteBatch)
        {
            // 当游戏有其他 UI 时暂停所有弹窗UI?
            if (Main.ingameOptionsWindow || Main.inFancyUI || Main.InGameUI.IsVisible)
            {
                SuspendAll();
                return;
            }

            // 任何GUI活动
            if (AnyGUIsActive)
            {
                // 强制关闭玩家库存和 NPC 对话
                Main.playerInventory = false;
                if (Main.LocalPlayer.sign > 0 || Main.LocalPlayer.talkNPC > 0)
                    Main.CloseNPCChatOrSign();

                // 更新活动弹窗
                GetActiveGUI.Update();

                // 处理绘制状态
                if (GetActiveGUI.FadeTime == 1 && !GetActiveGUI.Active)
                {
                    spriteBatch.End();
                    spriteBatch.Begin(); // 重置绘制批次
                    return;
                }

                GetActiveGUI.Draw(spriteBatch);
            }
        }

        // 通过类型切换GUI状态
        // 这样你就不必手动处理LINQ和反射了。
        public static void FlipActivityOfGUIWithType(Type type)
        {
            // 如果列表中不存在指定的类型，则提前结束
            if (!gUIs.Any(gui => gui.GetType() == type))
                return;
            gUIs.First(gui => gui.GetType() == type).Active = !gUIs.First(gui => gui.GetType() == type).Active;
        }

        // 加载所有派生自 PopupGUI 的类
        // 我独立成CalPopupGUI了
        public static void LoadGUIs()
        {
            foreach (Mod mod in ModLoader.Mods)
            {
                foreach (Type type in AssemblyManager.GetLoadableTypes(mod.Code))
                {
                    // 不加载抽象类
                    if (type.IsAbstract)
                        continue;
                    if (type.IsSubclassOf(typeof(CalPopupGUI)))
                        gUIs.Add(Activator.CreateInstance(type) as CalPopupGUI);
                }
            }
        }
        // 卸载
        public static void UnloadGUIs() => gUIs.Clear();
    }
}
