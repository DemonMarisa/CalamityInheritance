using Terraria;
using Terraria.ID;
using CalamityInheritance.Utilities;
using CalamityMod.Items.LoreItems;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeBrainofCthulhu : LoreItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Mods.CalamityInheritance.Content.Items.Lores";
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Green;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {

            if (CalamityInheritanceConfig.Instance.BoCLoreUnconditional == true)
            {
                if (Item.favorited)
                {
                    player.CalamityInheritance().BoCLoreTeleportation = true;
                }
            }
            if (CalamityInheritanceConfig.Instance.BoCLoreUnconditional == false)
            {
                if (player.ZoneCrimson && Item.favorited)
                {
                    player.CalamityInheritance().BoCLoreTeleportation = true;
                }
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.BrainofCthulhuTrophy).
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
AddIngredient<LoreBrainofCthulhu>().
AddTile(TileID.Bookcases).
Register();
        }
    }
}
