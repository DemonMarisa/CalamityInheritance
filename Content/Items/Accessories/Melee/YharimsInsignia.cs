using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Items.Materials;

namespace CalamityInheritance.Content.Items.Accessories.Melee
{
    public class YharimsInsignia : CIAccessories, ILocalizedModType
    {

        public new string LocalizationCategory => "Content.Items.Accessories.Melee";
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:22,
            itemHeight:38,
            itemRare: RarityType<BlueGreen>(),
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
            if (player.statLife <= (int)(player.statLifeMax2 * 0.5))
                    player.GetDamage<GenericDamageClass>() += 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WarriorEmblem).
                AddIngredient<NecklaceofVexation>().
                AddIngredient<BadgeofBravery>().
                AddIngredient<CoreofSunlight>(5).
                AddIngredient<DivineGeode>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
