using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeMechs : LoreItem
    {
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

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.RetinazerTrophy).
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
                AddIngredient(ItemID.SpazmatismTrophy).
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
                AddIngredient(ItemID.DestroyerTrophy).
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
                AddIngredient(ItemID.SkeletronPrimeTrophy).
                AddTile(TileID.Bookcases).
                Register();
        }
    }
}
