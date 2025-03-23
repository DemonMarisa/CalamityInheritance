using System.Collections.Generic;
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
            layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("Cal Popup GUIs", () =>
            {
                CalPopupGUIManager.UpdateAndDraw(Main.spriteBatch);
                return true;
            }, InterfaceScaleType.UI));
        }
    }
}
