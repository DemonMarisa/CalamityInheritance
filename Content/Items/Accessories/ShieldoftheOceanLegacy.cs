using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Armor.Victide;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class ShieldoftheOceanLegacy : CIAccessories, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.defense = 2;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir))
            {
                player.statDefense += 5;
            }
            if ((player.armor[0].type == ModContent.ItemType<AncientVictideHeadSummon>() || player.armor[0].type == ModContent.ItemType<AncientVictideHeadRogue>() ||
                player.armor[0].type == ModContent.ItemType<AncientVictideHeadRanged>() || player.armor[0].type == ModContent.ItemType<AncientVictideHeadMelee>() ||
                player.armor[0].type == ModContent.ItemType<AncientVictideHeadMagic>()) &&
                player.armor[1].type == ModContent.ItemType<AncientVictideBreastplate>() && player.armor[2].type == ModContent.ItemType<AncientVictideLeggings>())
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
