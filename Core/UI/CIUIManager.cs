using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalamityInheritance.Core.UI
{
    public class CIUIManager : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");
            if (mouseIndex != -1)
            {
                if (AstralArcanumUI.Open)
                {
                    layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("CI Astral Arcanum UI", delegate ()
                    {
                        AstralArcanumUI.UpdateAndDraw(Main.spriteBatch);
                        return true;
                    }, InterfaceScaleType.UI));
                }
            }
        }
    }
}
