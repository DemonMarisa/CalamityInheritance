using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Materials
{
	public class NebulaBar: ModItem, ILocalizedModType
	{
        public new string LocalizationCategory => "Content.Items.Materials";
        public override void SetStaticDefaults()
		{
		}
			
		public override void SetDefaults()
		{
			Item.width = 15;
			Item.height = 12;
			Item.maxStack = 9999;
			Item.value = CIShopValue.RarityPriceCyan;
			Item.rare = ItemRarityID.Cyan;
		}
		
		public override void AddRecipes()
		{
			CreateRecipe(9).
				AddIngredient(ItemID.HallowedBar, 3).
				AddIngredient<LifeAlloy>(3).
				AddIngredient(ItemID.LunarBar, 3).
				AddTile(TileID.LunarCraftingStation).
				Register();
		}
	}
}