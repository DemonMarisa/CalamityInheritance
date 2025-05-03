using CalamityMod;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeCryogen : LoreItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Lores";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Pink;
            Item.consumable = false;
        }

        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                player.Calamity().DashID = OrnateShieldDash.ID;
                player.dashType = 0;
                player.statDefense -= 10;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CryogenTrophy>().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
                AddIngredient<LoreArchmage>().
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
