using CalamityMod.Items;
using CalamityMod;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeBrainofCthulhu : LoreItem
    {
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
        }
    }
}
