using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeSlimeGod : LoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.LightRed;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (player.mount.Active || !Item.favorited)
                return;

            if (player.dashDelay < 0)
                player.velocity.X *= 0.9f;

            player.slippy2 = true;

            if (Main.myPlayer == player.whoAmI)
                player.AddBuff(BuffID.Slimed, 2);

            player.statDefense -= 10;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SlimeGodTrophy>().
                AddTile(TileID.Bookcases).
                Register();
            CreateRecipe().
AddIngredient<LoreSlimeGod>().
AddTile(TileID.Bookcases).
Register();
        }
    }
}
