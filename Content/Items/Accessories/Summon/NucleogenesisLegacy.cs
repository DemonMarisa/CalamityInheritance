using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer;
using CalamityMod;

namespace CalamityInheritance.Content.Items.Accessories.Summon
{
    public class NucleogenesisLegacy : CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.Summon";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 32;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<DeepBlue>();
        }
        public override bool CanEquipAccessory(Player player, int slot, bool modded) => !player.Calamity().nucleogenesis;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer CIplayer = player.CIMod();

            CIplayer.NucleogenesisLegacy = true;
            player.GetKnockback<SummonDamageClass>() += 3f;
            player.GetDamage<SummonDamageClass>() += 0.50f;
            player.buffImmune[ModContent.BuffType<Shadowflame>()] = true;
            player.buffImmune[ModContent.BuffType<Irradiated>()] = true;
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
