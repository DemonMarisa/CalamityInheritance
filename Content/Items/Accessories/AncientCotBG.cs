using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class AncientCotBG: CIAccessories, ILocalizedModType
    {

        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:48,
            itemHeight:48,
            itemRare:ModContent.RarityType<DeepBlue>(),
            itemValue:CIShopValue.RarityPriceDeepBlue
        );
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            usPlayer.AncientCotbg = true;
        }
        public override void AddRecipes()
        {   
            //Scarlet:旧血核与旧血契的加入已经没有必要微光转化了
            CreateRecipe().
                AddIngredient<BloodPactLegacy>().
                AddIngredient<BloodflareCoreLegacy>().
                AddIngredient<BloodyWormScarf>().
                AddIngredient<CosmiliteBar>(5).
                AddIngredient<Necroplasm>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
