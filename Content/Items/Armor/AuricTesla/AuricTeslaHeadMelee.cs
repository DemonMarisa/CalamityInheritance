using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items.Armor.Auric;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Items.Armor.GodSlayerOld;
using CalamityMod;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items.Armor.Silva;
using CalamityInheritance.Content.Items.Accessories.Ranged;

namespace CalamityInheritance.Content.Items.Armor.AuricTesla
{
    [AutoloadEquip(EquipType.Head)]
    public class AuricTeslaHeadMelee : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.PostMoonLord";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.defense = 54; //132
            Item.rare = ModContent.RarityType<Violet>();
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
            if (CalamityInheritanceConfig.Instance.GodSlayerSetBonusesChange == 1 || (CalamityInheritanceConfig.Instance.GodSlayerSetBonusesChange == 3) && !(CalamityInheritanceConfig.Instance.GodSlayerSetBonusesChange == 2))
            {
                modPlayer1.GodSlayerReborn = true;
            }
            if (CalamityInheritanceConfig.Instance.GodSlayerSetBonusesChange == 2 || (CalamityInheritanceConfig.Instance.GodSlayerSetBonusesChange == 3))
            {
                if (modPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && modPlayer.LastUsedDashID == GodslayerArmorDash.ID)
                {
                    modPlayer.DeferredDashID = GodslayerArmorDash.ID;
                    player.dash = 0;
                }
            }

            modPlayer1.GodSlayerDMGprotect = true;

            modPlayer.silvaSet = true;
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
            modPlayer.auricBoost = true;
            player.GetDamage<MeleeDamageClass>() += 0.2f;
            player.GetCritChance<MeleeDamageClass>() += 20;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.28f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TarragonHeadMelee>().
                AddIngredient<BloodflareHeadMelee>().
                AddIngredient<GodSlayerHeadMelee>().
                AddIngredient<SilvaHeadMelee>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBar>(12).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
