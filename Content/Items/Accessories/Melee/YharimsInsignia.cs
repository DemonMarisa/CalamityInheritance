using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Accessories.Melee
{
    public class YharimsInsignia : CIAccessories, ILocalizedModType
    {

        public new string LocalizationCategory => "Content.Items.Accessories.Melee";
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:22,
            itemHeight:38,
            itemRare:ModContent.RarityType<BlueGreen>(),
            itemValue:CIShopValue.RarityPriceBlueGreen
        );
        public override void ExSSD()
        {
            Type.ShimmerEach<AscendantInsignia>();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer modPlayer = player.CIMod();
            modPlayer.YharimsInsignia = true;
            player.lavaMax = 600;
            player.GetDamage<MeleeDamageClass>() += 0.15f;
            player.GetDamage<TrueMeleeDamageClass>() += 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WarriorEmblem).
                AddIngredient<NecklaceofVexation>().
                AddRecipeGroup(CIRecipeGroup.BadgeofBravery).
                AddIngredient<CoreofSunlight>(5).
                AddIngredient<DivineGeode>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
