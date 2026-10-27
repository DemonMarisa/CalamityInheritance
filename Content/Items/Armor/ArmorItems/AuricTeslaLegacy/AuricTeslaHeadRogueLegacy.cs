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
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AuricTeslaLegacy
{
    [AutoloadEquip(EquipType.Head)]
    public class AuricTeslaHeadRogueLegacy : CIArmor
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
            Item.defense = 34; //132
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
            player.LAP().MaxFocusAdd += 20;
            player.LAP().ExImmuneTime += 30;
            player.CI().AuricSet = true;
            player.CI().AuricRogue = true;
            player.CI().TarragonSet = true;
            player.CI().TarragonRogue = true;
            player.CI().BloodflareSet = true;
            player.CI().BloodflareRogue = true;
            player.CI().GodSlayerSet = true;
            player.CI().GodSlayerReborn = true;
            player.CI().GodSlayerRogue = true;
            player.CI().SilvaSet = true;
            player.CI().SilvaReborn = true;
            player.CI().SilvaRogue = true;
            player.CI().GodSlayerDash = true;
            player.thorns += 3f;
            player.ignoreWater = true;
            player.crimsonRegen = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.CI().AuricBoost = true;
            player.GetDamage<ThrowingDamageClass>() += 0.2f;
            player.GetCritChance<ThrowingDamageClass>() += 20;
            player.moveSpeed += 0.25f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SilvaHeadRogue>().
                AddIngredient<GodSlayerHeadRogueold>().
                AddIngredient<BloodflareHeadRogueLegacy>().
                AddIngredient<TarragonHeadRogueLegacy>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBarold>().
                AddTile<DraedonsForgeoldTile>().
                Register();
        }
    }
}
