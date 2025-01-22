using CalamityInheritance.Content.Items.Armor.Wulfum.NewTexture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.Wulfum
{
    [AutoloadEquip(EquipType.Legs)]
    public class WulfrumLeggingsLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Wulfrum";
        public override void SetStaticDefaults()
        {

            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<WulfrumLeggingsLegacy>()] = ModContent.ItemType<ANewWulfrumLeggings>();
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