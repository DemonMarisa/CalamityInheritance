using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Keys;
using LAP.Core.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Restorative
{
    public class AstralArcanum : CIAccessories, ILocalizedModType
    {
        public override int AccessoriesStyle => Restorative;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 26;
            Item.height = 26;
            Item.rare = RarityType<BlueGreen>();
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.defense = 12;
        }
        public override void ModifyTooltips(List<TooltipLine> list) => list.IntegrateHotkey(CIKeybinds.AstralArcanumUIHotkey);
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CI().projRef = true;
            player.CI().DeificAmuletFallenStar = true;
            player.CI().AstralArcanumRegen = true;
            player.buffImmune[BuffType<CIAstralInfection>()] = true;
            if (HasCalamity())
                player.buffImmune[CalDeBuff.AstralInfectionDebuff] = true;
        }

        public override void AddRecipes()
        {
        }
    }
}
