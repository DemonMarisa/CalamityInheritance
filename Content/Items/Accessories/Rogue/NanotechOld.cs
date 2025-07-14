using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Accessories.Rogue
{
    public class NanotechOld : CIAccessories, ILocalizedModType
    {
        public static int nanotechDMGStack  = 150;
        public new string LocalizationCategory => "Content.Items.Accessories.Rogue";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 46;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer modPlayer1 = player.CIMod();
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer1.nanotechold = true;
            //removed原灾radierTalisman，因为我已经给nanotechold内置了
            modPlayer.electricianGlove = true;
            modPlayer.filthyGlove = true;
            modPlayer.bloodyGlove = true;
            player.GetDamage<RogueDamageClass>() += 0.25f;
            player.Calamity().rogueVelocity += 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<RogueEmblem>().
                AddIngredient<RaidersTalisman>().
                AddIngredient<MoonstoneCrown>().
                AddIngredient<ElectriciansGlove>().
                AddIngredient(ItemID.LunarBar, 8).
                AddIngredient<GalacticaSingularity>(4).
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
