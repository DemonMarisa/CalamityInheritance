using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.BloodflareLegacy
{
    [AutoloadEquip(EquipType.Head)]
    public class BloodflareHeadRogueLegacy : CIArmor
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.defense = 34;
            Item.rare = RarityType<AbsoluteGreen>();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<BloodflareBodyLegacy>() && legs.type == ModContent.ItemType<BloodflareLegLegacy>();
        }
        public override void ArmorSetShadows(Player player) => player.armorEffectDrawShadowSubtle = true;

        public override void UpdateArmorSet(Player player)
        {
            player.CI().BloodflareSet = true;
            player.CI().BloodflareRanged = true;
            player.setBonus = this.GetLocalization("SetBonus").Value;
        }

    }
}
