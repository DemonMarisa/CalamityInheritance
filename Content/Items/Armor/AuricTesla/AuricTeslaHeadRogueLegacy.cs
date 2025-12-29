using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items.Armor.Auric;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items.Accessories.Ranged;
using CalamityInheritance.Content.Items.Armor.Silva;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using CalamityInheritance.Content.Items.Armor.GodSlayerOld;

namespace CalamityInheritance.Content.Items.Armor.AuricTesla
{
    [AutoloadEquip(EquipType.Head)]
    public class AuricTeslaHeadRogueLegacy : CIArmor, ILocalizedModType
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
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            var usPlayer = player.CIMod();
            if (usPlayer.AuricSilvaFakeDeath)
            {
                if (Main.keyState.IsKeyDown(Keys.LeftAlt))
                {
                    string Details = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Armor.AuricTeslaHeadRogueLegacy.Details");
                    tooltips.Add(new TooltipLine(Mod, "Details", Details));
                }
            }
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<AuricTeslaBodyArmorold>() && legs.type == ItemType<AuricTeslaCuissesold>();

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            var usPlayer = player.CIMod();
            var calPlayer = player.Calamity();
            calPlayer.tarraSet = true;
            calPlayer.tarraThrowing = true;
            calPlayer.bloodflareSet = true;
            calPlayer.bloodflareThrowing = true;
            calPlayer.godSlayer = true;
            calPlayer.godSlayerThrowing = true;
            usPlayer.AuricSilvaFakeDeath = true;
            usPlayer.AuricDebuffImmune = true;

            player.setBonus = this.GetLocalizedValue("SetBonus");
            usPlayer.GodSlayerReborn = true;
            if (calPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && calPlayer.LastUsedDashID == GodslayerArmorDash.ID)
            {
                calPlayer.DeferredDashID = GodslayerArmorDash.ID;
                player.dash = 0;
            }
            usPlayer.SilvaRougeSetLegacy = true;

            calPlayer.rogueStealthMax += 1.3f;
            calPlayer.wearingRogueArmor = true;
            //继承至弑神套的视潜伏值上限增加潜伏值的效果
            float getMaxStealth = calPlayer.rogueStealthMax;
            calPlayer.rogueStealthMax += getMaxStealth / 6;
            calPlayer.wearingRogueArmor = true;
            player.thorns += 3f;
            player.ignoreWater = true;
            player.crimsonRegen = true;

            calPlayer.WearingPostMLSummonerSet = true;
        }

        public override void UpdateEquip(Player player)
        {
            var calPlayer = player.Calamity();
            var usPlayer = player.CIMod();
            usPlayer.auricBoostold = true;
            player.GetDamage<RogueDamageClass>() += 0.2f;
            player.GetCritChance<RogueDamageClass>() += 20;
            player.moveSpeed += 0.25f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SilvaHeadRogue>().
                AddIngredient<GodSlayerHeadRogueold>().
                AddIngredient<BloodflareHeadRogue>().
                AddIngredient<TarragonHeadRogue>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient<SilvaHeadRogue>().
                AddIngredient<GodSlayerHeadRogueold>().
                AddIngredient<BloodflareHeadRogue>().
                AddIngredient<TarragonHeadRogue>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBar>(12).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
