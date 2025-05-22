using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items.Armor.Auric;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items.Armor.Silva;
using CalamityInheritance.Content.Items.Accessories.Ranged;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using CalamityInheritance.Content.Items.Armor.GodSlayerOld;
using Humanizer;

namespace CalamityInheritance.Content.Items.Armor.AuricTesla
{
    [AutoloadEquip(EquipType.Head)]
    public class AuricTeslaHeadRanged : CIArmor, ILocalizedModType
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
            Item.defense = 40; //132
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<AuricTeslaBodyArmorold>() && legs.type == ModContent.ItemType<AuricTeslaCuissesold>();
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            var usPlayer = player.CIMod();
            if (usPlayer.AuricSilvaFakeDeath)
            {
                if (Main.keyState.IsKeyDown(Keys.LeftAlt))
                {
                    string Details = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Armor.AuricTeslaHeadRanged.Details");
                    tooltips.Add(new TooltipLine(Mod, "Details", Details));
                }
            }
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            var hotkey = CalamityKeybinds.ArmorSetBonusHotKey.TooltipHotkeyString();
            var calPlayer = player.Calamity();
            var usPlayer = player.CIMod();
            calPlayer.tarraSet = true;
            calPlayer.tarraRanged = true;
            calPlayer.bloodflareSet = true;
            calPlayer.godSlayer = true;
            usPlayer.GodSlayerRangedSet = true;
            usPlayer.AuricDebuffImmune = true;

            player.setBonus = this.GetLocalization("SetBonus").Format(hotkey);
            usPlayer.GodSlayerReborn = true;
            if (calPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && calPlayer.LastUsedDashID == GodslayerArmorDash.ID)
            {
                calPlayer.DeferredDashID = GodslayerArmorDash.ID;
                player.dash = 0;
            }
            usPlayer.SilvaRangedSetLegacy = true;
            usPlayer.AuricbloodflareRangedSoul = true;
            usPlayer.AuricSilvaFakeDeath = true;
            player.thorns += 3f;
            player.ignoreWater = true;
            player.crimsonRegen = true;
            if (player.HeldItem.useTime > 3 && player.HeldItem.DamageType == DamageClass.Ranged)
                player.GetAttackSpeed<RangedDamageClass>() += 0.2f;

            calPlayer.WearingPostMLSummonerSet = true;
        }

        public override void UpdateEquip(Player player)
        {
            var calPlayer = player.Calamity();
            var usPlayer = player.CIMod();
            usPlayer.auricBoostold = true;
            player.GetDamage<RangedDamageClass>() += 0.3f;
            player.GetCritChance<RangedDamageClass>() += 30;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("CalamityInheritance:AnyGodSlayerHeadRanged").
                AddIngredient<SilvaHeadRanged>().
                AddIngredient<BloodflareHeadRanged>().
                AddIngredient<TarragonHeadRanged>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBarold>(1).
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddRecipeGroup("CalamityInheritance:AnyGodSlayerHeadRanged").
                AddIngredient<SilvaHeadRanged>().
                AddIngredient<BloodflareHeadRanged>().
                AddIngredient<TarragonHeadRanged>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBar>(12).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
