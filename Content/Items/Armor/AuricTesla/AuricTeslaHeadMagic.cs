using CalamityMod.Items.Armor.Auric;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items.Accessories.Ranged;
using CalamityInheritance.Content.Items.Armor.GodSlayerOld;
using CalamityMod.CalPlayer.Dashes;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Rarity;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Armor.AuricTesla
{
    [AutoloadEquip(EquipType.Head)]
    public class AuricTeslaHeadMagic : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.defense = 24; //132
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
            player.setBonus = this.GetLocalizedValue("SetBonus");
            var calPlayer = player.Calamity();
            var usPlayer = player.CalamityInheritance();

            calPlayer.tarraSet = true;
            calPlayer.tarraMage = true;

            calPlayer.bloodflareSet = true;
            calPlayer.bloodflareMage = true;

            calPlayer.godSlayer = true;

            usPlayer.SilvaMagicSetLegacy = true;
            usPlayer.GodSlayerMagicSet = true;

            calPlayer.auricSet = true;
            usPlayer.AuricSilvaSet = true;
            if (CIConfig.Instance.GodSlayerSetBonusesChange == 1 || (CIConfig.Instance.GodSlayerSetBonusesChange == 3) && !(CIConfig.Instance.GodSlayerSetBonusesChange == 2))
            {
                usPlayer.GodSlayerReborn = true;
            }
            if (CIConfig.Instance.GodSlayerSetBonusesChange == 2 || (CIConfig.Instance.GodSlayerSetBonusesChange == 3))
            {
                if (calPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && calPlayer.LastUsedDashID == GodslayerArmorDash.ID)
                {
                    calPlayer.DeferredDashID = GodslayerArmorDash.ID;
                    player.dash = 0;
                }
            }
            player.thorns += 3f;
            player.ignoreWater = true;
            player.crimsonRegen = true;
        }

        public override void UpdateEquip(Player player)
        {
            var calPlayer = player.Calamity();
            var usPlayer = player.CalamityInheritance();
            player.manaCost *= 0.8f;
            player.GetDamage<MagicDamageClass>() += 0.3f;
            player.GetCritChance<MagicDamageClass>() += 20;
            player.statManaMax2 += 100;
            usPlayer.auricBoostold = true;
        }

        public override void AddRecipes()
        {

            CreateRecipe().
                AddRecipeGroup("CalamityInheritance:AnySilvaHeadMagic").
                AddIngredient<GodSlayerHeadMagicold>().
                AddIngredient<BloodflareHeadMagic>().
                AddIngredient<TarragonHeadMagic>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddRecipeGroup("CalamityInheritance:AnySilvaHeadMagic").
                AddIngredient<GodSlayerHeadMagicold>().
                AddIngredient<BloodflareHeadMagic>().
                AddIngredient<TarragonHeadMagic>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBar>(12).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
