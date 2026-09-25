using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Materials
{
    public class BloodstoneCore : CIMaterials
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
            ItemID.Sets.SortingPriorityMaterials[Type] = 113;
        }

        public override void SetDefaults()
        {
            Item.width = 15;
            Item.height = 12;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = RarityType<BlueGreen>();
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe(2).
                    AddIngredient(CalamityMaterials.Bloodstone, 5).
                    AddIngredient(CalamityMaterials.BloodOrb).
                    AddIngredient(CalamityMaterials.Necroplasm).
                    AddTile(TileID.AdamantiteForge).
                    Register();
            }
            else
            {

            }
        }
    }
}
