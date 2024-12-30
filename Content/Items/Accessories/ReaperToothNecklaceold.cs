using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Rarities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class ReaperToothNecklaceold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 50;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<GenericDamageClass>() += 0.15f;
            player.GetArmorPenetration<GenericDamageClass>() += 100;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SandSharkToothNecklace>().
                AddIngredient<ReaperTooth>(6).
                AddIngredient<DepthCells>(15).
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }
    }
}
