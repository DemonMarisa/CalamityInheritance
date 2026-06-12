using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Summon
{
    public class NucleogenesisLegacy : CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.Summon";
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth: 34,
            itemHeight: 32,
            itemRare: RarityType<DeepBlue>(),
            itemValue: CIShopValue.RarityPriceDeepBlue
        );
        public override void ExSSD()
        {
            Type.ShimmerEach<Nucleogenesis>(false);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer CIplayer = player.CIMod();

            CIplayer.NucleogenesisLegacy = true;
            player.GetKnockback<SummonDamageClass>() += 3f;
            player.GetDamage<SummonDamageClass>() += 0.50f;
            player.buffImmune[BuffType<Shadowflame>()] = true;
            player.buffImmune[BuffType<Irradiated>()] = true;
            player.whipRangeMultiplier += 0.20f;
            player.maxMinions += 5;
            player.maxTurrets += 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<StarTaintedGenerator>().
                AddIngredient<StatisCurse>().
                AddIngredient(ItemID.LunarBar, 8).
                AddIngredient<GalacticaSingularity>(4).
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
