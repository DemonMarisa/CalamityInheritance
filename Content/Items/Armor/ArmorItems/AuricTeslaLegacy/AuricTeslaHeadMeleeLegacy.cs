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
    public class AuricTeslaHeadMeleeLegacy : CIArmor
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
            Item.defense = 54; //132
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
            player.CI().AuricMelee = true;
            player.CI().TarragonSet = true;
            player.CI().TarragonMelee = true;
            player.CI().BloodflareSet = true;
            player.CI().BloodflareMelee = true;
            player.CI().GodSlayerSet = true;
            player.CI().GodSlayerReborn = true;
            player.CI().GodSlayerMelee = true;
            player.CI().SilvaSet = true;
            player.CI().SilvaReborn = true;
            player.CI().SilvaMelee = true;
            player.CI().GodSlayerDash = true;
            player.crimsonRegen = true;
            player.thorns += 3f;
            player.ignoreWater = true;
            player.aggro += 1200;
        }

        public override void UpdateEquip(Player player)
        {
            player.CI().AuricBoost = true;
            player.GetDamage<MeleeDamageClass>() += 0.2f;
            player.GetCritChance<MeleeDamageClass>() += 20;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.28f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TarragonHeadMeleeLegacy>().
                AddIngredient<BloodflareHeadMeleeLegacy>().
                AddIngredient<GodSlayerHeadMeleeold>().
                AddIngredient<SilvaHeadMelee>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBarold>(1).
                AddTile<DraedonsForgeoldTile>().
                Register();
        }
    }
}
