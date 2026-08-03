using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Core.Utils;
using LAP.Content.RecipeGroupAdd;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Tools
{
    public class ChargerItem : CITools
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;
            Item.rare = ItemRarityID.Red;

            Item.consumable = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 29;
            Item.useAnimation = 29;
            Item.autoReuse = true;
            Item.useTurn = true;
        }

        public override bool? UseItem(Player player)
        {
            if (CIUtils.HasCalamity())
                CalCrossUtils.ChargeCalamityItem(player);
            return true;
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.DubiousPlating, 50).
                    AddIngredient(CalamityMaterials.MysteriousCircuitry, 25).
                    AddIngredient(ItemID.Glass, 50).
                    AddRecipeGroup(LAPRecipeGroup.AnyCopperBar, 10).
                    AddIngredient(ItemID.Wire, 100).
                    AddTile(TileID.WorkBenches).
                    Register();
            }
        }
    }
}
