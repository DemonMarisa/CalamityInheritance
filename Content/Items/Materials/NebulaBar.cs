using CalamityInheritance.Tiles.Bars;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Materials
{
	public class NebulaBar: CIMaterials, ILocalizedModType
	{
        public new string LocalizationCategory => "Content.Items.Materials";
        public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 25;
		}
			
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(TileType<NebulaBarTile>());
			Item.value = CIShopValue.RarityPriceCyan;
			Item.rare = ItemRarityID.Cyan;
		}
		
		public override void AddRecipes()
		{
			CreateRecipe(3).
				AddIngredient(ItemID.HallowedBar, 1).
				AddIngredient(ItemID.Ectoplasm, 1).
				AddIngredient<MeldBlob>(6).
				AddTile(TileID.LunarCraftingStation).
				Register();
		}
	}
}