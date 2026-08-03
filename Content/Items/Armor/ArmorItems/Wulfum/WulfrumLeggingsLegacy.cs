using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Armor.ArmorItems.Wulfum.NewTexture;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.Wulfum
{
    [AutoloadEquip(EquipType.Legs)]
    public class WulfrumLeggingsLegacy : CIArmor
    {
        public override void SetStaticDefaults()
        {

            ItemID.Sets.ShimmerTransformToItem[ItemType<WulfrumLeggingsLegacy>()] = ItemType<ANewWulfrumLeggings>();
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.05f;
        }
    }
}