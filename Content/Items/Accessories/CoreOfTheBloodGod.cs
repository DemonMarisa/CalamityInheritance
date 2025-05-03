using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class CoreOfTheBloodGod : CIAccessories, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
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
            CalamityPlayer calPlayer = player.Calamity();
            var usPlayer = player.CIMod();
            calPlayer.healingPotionMultiplier += 0.25f;
            //同时标记他是血神核心与给血上限。
            //这个标记主要是给后面血肉图腾被移除时给CD用
            usPlayer.CoreOfTheBloodGod = true;
            usPlayer.FUCKYOUREDMOON = true;
            player.endurance += 0.15f;

        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(CIRecipeGroup.BloodPact).
                AddIngredient<FleshTotem>().
                AddIngredient<CosmiliteBar>(5).
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
