using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;
using System.Collections.Generic;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class AncientReaperToothNecklace : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:44,
            itemHeight:50,
            itemRare: RarityType<AbsoluteGreen>(),
            itemValue:CIShopValue.RarityPriceAbsoluteGreen
        );

        public override void ExSSD() => Type.ShimmerEach<ReaperToothNecklace>();
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (CIFunction.ActiveWrath())
            {
                string path = "Mods.CalamityInheritance.Content.Items.Accessories.AncientReaperToothNecklace.Nerfed";
                tooltips.FuckThisTooltipAndReplace(path);
            }
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (CIFunction.ActiveWrath())
            {
                player.GetArmorPenetration<GenericDamageClass>() += 100;
                player.GetDamage<GenericDamageClass>() += 0.50f;
                player.GetCritChance<GenericDamageClass>() += 30;
                player.endurance *= 0.20f;
                player.statDefense /= 5;
                return;
            }

            player.GetArmorPenetration<GenericDamageClass>() += 300;
            player.GetDamage<GenericDamageClass>() *= 1.20f;
            player.GetCritChance<GenericDamageClass>() += 50;
            player.endurance *= 0.01f;
            player.statDefense /= 100;
            if (player.lifeRegen > 0)
                player.lifeRegen /= 100;
        }
    }
}
