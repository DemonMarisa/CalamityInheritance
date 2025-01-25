using CalamityInheritance.Content.Items.Accessories.Ranged;
using CalamityInheritance.Content.Items.Armor.AuricTesla;
using CalamityInheritance.Content.Items.Armor.GodSlayerOld;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Rarity;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items.Armor.Auric;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Items.Materials;
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
    public class AncientAuricTeslaHelm : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Vanity";
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 22;
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
