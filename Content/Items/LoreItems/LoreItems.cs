using CalamityMod;
using CalamityMod.Items;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public abstract class LoreItem : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Lore";

        // All lore items initially have a short tooltip which indicates there is more to be read.
        public override LocalizedText Tooltip => CalamityUtils.GetText($"{LocalizationCategory}.ShortTooltip");

        // By default, lore text appears in white, but this can be changed.
        public virtual Color? LoreColor => null;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override bool CanUseItem(Player player) => false;

        public override Color? GetAlpha(Color lightColor) => Color.White;

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = (ContentSamples.CreativeHelper.ItemGroup)CalamityResearchSorting.LoreItems;
        }

        // All lore items use the same code for holding SHIFT to extend tooltips.
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine fullLore = new(Mod, "CIMod:Lore", this.GetLocalizedValue("Lore"));
            if (LoreColor.HasValue)
                fullLore.OverrideColor = LoreColor.Value;
            HoldShiftTooltip(tooltips, [fullLore], true);
        }
        public static void HoldShiftTooltip(List<TooltipLine> tooltips, TooltipLine[] holdShiftTooltips, bool hideNormalTooltip = false)
        {
            if (!Main.keyState.IsKeyDown(Keys.LeftShift))
                return;
            int firstTooltipIndex = -1;
            int lastTooltipIndex = -1;
            int standardTooltipCount = 0;
            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].Name.StartsWith("Tooltip"))
                {
                    if (firstTooltipIndex == -1)
                        firstTooltipIndex = i;
                    lastTooltipIndex = i;
                    standardTooltipCount++;
                }
            }
            if (firstTooltipIndex != -1)
            {
                if (hideNormalTooltip)
                {
                    tooltips.RemoveRange(firstTooltipIndex, standardTooltipCount);
                    lastTooltipIndex -= standardTooltipCount;
                }
                tooltips.InsertRange(lastTooltipIndex + 1, holdShiftTooltips);
            }
        }
    }
}
