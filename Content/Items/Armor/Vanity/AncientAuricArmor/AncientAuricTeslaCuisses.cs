using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Tiles.Furniture.CraftingStations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Armor.Vanity.AncientAuricArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class AncientAuricTeslaCuisses : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Vanity";
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 14;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.vanity = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
