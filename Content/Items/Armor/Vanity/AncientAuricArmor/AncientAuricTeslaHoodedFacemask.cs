using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Rarity;
using CalamityMod.Tiles.Furniture.CraftingStations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Armor.Vanity.AncientAuricArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientAuricTeslaHoodedFacemask : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Vanity";
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.vanity = true;
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
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
