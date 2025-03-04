using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items.Armor.Auric;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items.Armor.Silva;
using CalamityInheritance.Content.Items.Accessories.Ranged;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Materials;

namespace CalamityInheritance.Content.Items.Armor.AuricTesla
{
    [AutoloadEquip(EquipType.Head)]
    public class AuricTeslaHeadMelee : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.defense = 54; //132
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool isAuricSetNEW = body.type == ModContent.ItemType<AuricTeslaBodyArmor>() && legs.type == ModContent.ItemType<AuricTeslaCuisses>();
            bool isAuricSetOLD = body.type == ModContent.ItemType<AuricTeslaBodyArmorold>() && legs.type == ModContent.ItemType<AuricTeslaCuissesold>();
            return isAuricSetNEW || isAuricSetOLD;
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            var hotkey = CalamityKeybinds.ArmorSetBonusHotKey.TooltipHotkeyString();
            player.setBonus = this.GetLocalization("SetBonus").Format(hotkey);
            var modPlayer = player.Calamity();
            var modPlayer1 = player.CalamityInheritance();
            modPlayer.tarraSet = true;
            modPlayer.tarraMelee = true;
            modPlayer.bloodflareSet = true;
            modPlayer.bloodflareMelee = true;
            modPlayer.godSlayer = true;
            modPlayer.godSlayerDamage = true;

            modPlayer1.silvaMelee = true;
            modPlayer1.godSlayerReflect = true;
            if (CIConfig.Instance.GodSlayerSetBonusesChange == 1 || (CIConfig.Instance.GodSlayerSetBonusesChange == 3) && !(CIConfig.Instance.GodSlayerSetBonusesChange == 2))
            {
                modPlayer1.GodSlayerReborn = true;
            }
            if (CIConfig.Instance.GodSlayerSetBonusesChange == 2 || (CIConfig.Instance.GodSlayerSetBonusesChange == 3))
            {
                if (modPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && modPlayer.LastUsedDashID == GodslayerArmorDash.ID)
                {
                    modPlayer.DeferredDashID = GodslayerArmorDash.ID;
                    player.dash = 0;
                }
            }

            modPlayer1.GodSlayerDMGprotect = true;

            modPlayer1.auricsilvaset = true;
            modPlayer.auricSet = true;

            player.thorns += 3f;
            player.ignoreWater = true;
            player.crimsonRegen = true;
            player.aggro += 1200;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.Calamity();
            var modPlayer1 = player.CalamityInheritance();
            modPlayer1.auricBoostold = true;
            player.GetDamage<MeleeDamageClass>() += 0.2f;
            player.GetCritChance<MeleeDamageClass>() += 20;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.28f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TarragonHeadMelee>().
                AddIngredient<BloodflareHeadMelee>().
                AddRecipeGroup("CalamityInheritance:AnyGodSlayerHeadMelee").
                AddIngredient<SilvaHeadMelee>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBarold>(1).
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient<TarragonHeadMelee>().
                AddIngredient<BloodflareHeadMelee>().
                AddRecipeGroup("CalamityInheritance:AnyGodSlayerHeadMelee").
                AddIngredient<SilvaHeadMelee>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBar>(12).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
