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
using CalamityInheritance.System.Configs;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using System;
using CalamityInheritance.Content.Items.Armor.Silva;

namespace CalamityInheritance.Content.Items.Armor.AuricTesla
{
    [AutoloadEquip(EquipType.Head)]
    public class AuricTeslaHeadMagicLegacy : CIArmor, ILocalizedModType
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
            Item.defense = 24; //132
            Item.rare = RarityType<CatalystViolet>();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<AuricTeslaBodyArmorold>() && legs.type == ItemType<AuricTeslaCuissesold>();
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            var usPlayer = player.CIMod();
            if (usPlayer.AuricSilvaFakeDeath)
            {
                if (Main.keyState.IsKeyDown(Keys.LeftAlt))
                {
                    string Details = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Armor.AuricTeslaHeadMagicLegacy.Details");
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
            var calPlayer = player.Calamity();
            var usPlayer = player.CIMod();

            calPlayer.WearingPostMLSummonerSet = true;
            calPlayer.tarraSet = true;
            calPlayer.tarraMage = true;

            calPlayer.bloodflareSet = true;
            calPlayer.bloodflareMage = true;
            usPlayer.AuricDebuffImmune = true;

            calPlayer.godSlayer = true;

            usPlayer.SilvaMagicSetLegacy = true;
            usPlayer.GodSlayerMagicSet = true;

            usPlayer.AuricSilvaFakeDeath = true;

            player.setBonus = this.GetLocalizedValue("SetBonus");
            usPlayer.GodSlayerReborn = true;

            usPlayer.CanUseLegacyGodSlayerDash = true;

            player.thorns += 3f;
            player.ignoreWater = true;
            player.crimsonRegen = true;
        }

        public override void UpdateEquip(Player player)
        {
            var calPlayer = player.Calamity();
            var usPlayer = player.CIMod();
            player.manaCost -= 0.2f;
            player.GetDamage<MagicDamageClass>() += 0.3f;
            player.GetCritChance<MagicDamageClass>() += 20;
            player.statManaMax2 += 100;
            usPlayer.auricBoostold = true;
        }

        public override void AddRecipes()
        {

            CreateRecipe().
                AddIngredient<SilvaHeadMagicold>().
                AddIngredient<GodSlayerHeadMagicold>().
                AddIngredient<BloodflareHeadMagic>().
                AddIngredient<TarragonHeadMagic>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient<SilvaHeadMagicold>().
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
