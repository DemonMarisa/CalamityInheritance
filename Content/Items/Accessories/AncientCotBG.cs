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
        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = Item.height = 48;
            Item.accessory = true;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = ModContent.RarityType<DeepBlue>();
        }

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
