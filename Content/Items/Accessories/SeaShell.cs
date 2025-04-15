using CalamityMod.CalPlayer;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class SeaShell : CIAccessories, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.defense = 3;
            Item.width = 20;
            Item.height = 24;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ignoreWater = true;
            if (player.IsUnderwater())
            {
                player.statDefense += 3;
                player.endurance += 0.05f;
                player.moveSpeed += 0.1f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Seashell, 5).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}
