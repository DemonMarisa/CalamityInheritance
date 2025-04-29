using CalamityInheritance.Content.Items.Potions.CIPotions;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Potions
{
    public class Bread: CIPotion, ILocalizedModType 
    {
        //是的，食物也是药！ 
        public new string LocalizationCategory => "Content.Items.Potions";
        public override void SetStaticDefaults() => Item.ResearchUnlockCount = 30;
        public override void SetDefaults() => Item.DefaultToFood(32, 32, BuffID.WellFed3, 43200);
        public override void AddRecipes() => CreateRecipe(1).
                                                AddIngredient(ItemID.Hay, 5).
                                                AddTile(TileID.Campfire).
                                                Register();
    }
}
