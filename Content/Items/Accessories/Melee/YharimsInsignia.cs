using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Accessories.Melee
{
    public class YharimsInsignia : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 38;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<Turquoise>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer modPlayer = player.CalamityInheritance();
            modPlayer.YharimsInsignia = true;
            player.GetDamage<TrueMeleeDamageClass>() += 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WarriorEmblem).
                AddIngredient<NecklaceofVexation>().
                AddIngredient<CoreofSunlight>(5).
                AddIngredient<DivineGeode>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
