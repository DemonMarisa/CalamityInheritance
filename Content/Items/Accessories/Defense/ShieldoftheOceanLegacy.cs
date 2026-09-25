using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Armor.ArmorItems.Victide;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Defense
{
    public class ShieldoftheOceanLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Defense;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 24;
            Item.height = 28;
            Item.rare = ItemRarityID.Green;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.defense = 2;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.statDefense += 5;
            }
            if ((player.armor[0].type == ItemType<AncientVictideHeadSummon>() || player.armor[0].type == ItemType<AncientVictideHeadRogue>() ||
                player.armor[0].type == ItemType<AncientVictideHeadRanged>() || player.armor[0].type == ItemType<AncientVictideHeadMelee>() ||
                player.armor[0].type == ItemType<AncientVictideHeadMagic>()) &&
                player.armor[1].type == ItemType<AncientVictideBreastplate>() && player.armor[2].type == ItemType<AncientVictideLeggings>())
            {
                player.moveSpeed += 0.1f;
                player.lifeRegen += 4;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AncientVictideBar>(5).
                AddIngredient(ItemID.Coral, 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
