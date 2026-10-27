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
    public class AuricTeslaHeadMagicLegacy : CIArmor
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
            Item.defense = 24;
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
            player.CI().AuricMagic = true;
            player.CI().TarragonSet = true;
            player.CI().TarragonMagic = true;
            player.CI().BloodflareSet = true;
            player.CI().BloodflareMagic = true;
            player.CI().GodSlayerSet = true;
            player.CI().GodSlayerReborn = true;
            player.CI().GodSlayerMagic = true;
            player.CI().SilvaSet = true;
            player.CI().SilvaReborn = true;
            player.CI().SilvaMagic = true;
            player.CI().GodSlayerDash = true;
            player.thorns += 3f;
            player.ignoreWater = true;
            player.crimsonRegen = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.manaCost -= 0.2f;
            player.GetDamage<MagicDamageClass>() += 0.3f;
            player.GetCritChance<MagicDamageClass>() += 20;
            player.statManaMax2 += 100;
            player.CI().AuricBoost = true;
        }
        public override void UpdateVisibleAccessory(Player player, bool hideVisual)
        {
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SilvaHeadMagicold>().
                AddIngredient<GodSlayerHeadMagicold>().
                AddIngredient<BloodflareHeadMagicLegacy>().
                AddIngredient<TarragonHeadMagicLegacy>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBarold>().
                AddTile<DraedonsForgeoldTile>().
                Register();
        }
    }
}
