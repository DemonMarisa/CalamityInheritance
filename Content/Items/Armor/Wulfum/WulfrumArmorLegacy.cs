﻿using CalamityInheritance.Content.Items.Armor.Wulfum.NewTexture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.Wulfum
{
    [AutoloadEquip(EquipType.Body)]
    public class WulfrumArmorLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Wulfrum";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<WulfrumArmorLegacy>()] = ModContent.ItemType<ANewWulfrumArmor>();
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<GenericDamageClass>() += 3;
        }
    }
}