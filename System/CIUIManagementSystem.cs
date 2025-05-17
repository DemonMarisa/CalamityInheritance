﻿using System.Collections.Generic;
using CalamityInheritance.UI;
using CalamityMod.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalamityInheritance.System
{
    public class CIUIManagementSystem : ModSystem
    {
        public static Vector2 PreviousMouseWorld;

        public static Vector2 PreviousZoom;
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");
            if (mouseIndex != -1)
            {
                // Astral Arcanum overlay (if open)
                layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("Astral Arcanum UI", delegate ()
                {
                    //Main.NewText($"layers.Insert", 255, 255, 255);
                    AstralArcanumUI.UpdateAndDraw(Main.spriteBatch);
                    return true;
                }, InterfaceScaleType.UI));
                //Main.NewText($"Inserted Astral Arcanum UI at {mouseIndex}", 255, 255, 255);
            }

            // Popup GUIs.
            layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("Draedons Popup GUIs", () =>
            {
                DraedonsPanelUIManager.UpdateAndDraw(Main.spriteBatch);
                return true;
            }, InterfaceScaleType.UI));

            // 进入游戏的文字
            layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("CI Text UI", () =>
            {
                FirstText.Draw(Main.spriteBatch);
                return true;
            }, InterfaceScaleType.UI));

            // Popup GUIs.
            layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("CI Mode Indicator UI", () =>
            {
                DifficultyModeUI.Draw(Main.spriteBatch);
                return true;
            }, InterfaceScaleType.UI));
        }
    }
}
