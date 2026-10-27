using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Accessories.Professional;
using CalamityInheritance.Content.Items.Armor.ArmorItems.BloodflareLegacy;
using CalamityInheritance.Content.Items.Armor.ArmorItems.GodSlayerOld;
using CalamityInheritance.Content.Items.Armor.ArmorItems.Silva;
using CalamityInheritance.Content.Items.Armor.ArmorItems.TarragonLegacy;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using CalamityInheritance.Core.Graphics.Insertlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AuricTeslaLegacy
{
    [AutoloadEquip(EquipType.Head)]
    public class AuricTeslaHeadSummonLegacy : CIArmor
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.defense = 12; //132
            Item.rare = RarityType<CatalystViolet>();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<AuricTeslaBodyArmorold>() && legs.type == ItemType<AuricTeslaCuissesold>();

        public override void ArmorSetShadows(Player player)
        {
            InsertDraw.DrawAuricTrail = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.CI().AuricSet = true;
            player.CI().AuricSummon = true;
            player.CI().TarragonSet = true;
            player.CI().TarragonSummon = true;
            player.CI().BloodflareSet = true;
            player.CI().BloodflareSummon = true;
            player.CI().GodSlayerSet = true;
            player.CI().GodSlayerReborn = true;
            player.CI().GodSlayerSummon = true;
            player.CI().SilvaSet = true;
            player.CI().SilvaReborn = true;
            player.CI().SilvaSummon = true;
            player.CI().GodSlayerDash = true;
            player.thorns += 3f;
            player.ignoreWater = true;
            player.crimsonRegen = true;
            player.GetDamage<SummonDamageClass>() += 1.2f;
            player.maxMinions += 1;
        }
        public override void UpdateEquip(Player player)
        {
            player.CI().AuricBoost = true;
            player.maxMinions += 7;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SilvaHeadSummonold>().
                AddIngredient<GodSlayerHeadSummonold>().
                AddIngredient<BloodflareHeadSummonLegacy>().
                AddIngredient<TarragonHeadSummonLegacy>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBarold>(1).
                AddTile<DraedonsForgeoldTile>().
                Register();
        }
    }
}
