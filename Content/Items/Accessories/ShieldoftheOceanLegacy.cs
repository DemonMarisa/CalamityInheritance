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
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:24,
            itemHeight:28,
            itemRare:ItemRarityID.Green,
            itemValue:CIShopValue.RarityPriceGreen,
            itemDefense:2
        );

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
