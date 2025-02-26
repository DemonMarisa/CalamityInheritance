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
    public class AncientCotBG: ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";

        public override void SetStaticDefaults()
        {
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

            CalamityInheritancePlayer usPlayer = player.CalamityInheritance();
            usPlayer.ancientCoreofTheBloodGod = true;
        }

        public override void AddRecipes()
        {   
            //微光嬗变config关闭时使用这个合成表
            // if(CalamityInheritanceConfig.Instance.CustomShimmer == false)
            // {
            //Scarlet:旧血核与旧血契的加入已经没有必要微光转化了
                CreateRecipe().
                    AddIngredient<BloodPactLegacy>().
                    AddIngredient<BloodflareCoreLegacy>().
                    AddIngredient<BloodyWormScarf>().
                    AddIngredient<CosmiliteBar>(5).
                    AddIngredient<Necroplasm>(5).
                    AddTile<CosmicAnvil>().
                    Register();
            // }
        }
    }
}
