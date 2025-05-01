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
    public class AuricTeslaHeadRogue : CIArmor, ILocalizedModType
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
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            var usPlayer = player.CIMod();
            if (usPlayer.AuricSilvaFakeDeath)
            {
                if (Main.keyState.IsKeyDown(Keys.LeftAlt))
                {
                    string Details = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Armor.AuricTeslaHeadRogue.Details");
                    tooltips.Add(new TooltipLine(Mod, "Details", Details));
                }
            }
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<AuricTeslaBodyArmorold>() && legs.type == ModContent.ItemType<AuricTeslaCuissesold>();

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityInheritancePlayer modPlayer1 = player.GetModPlayer<CalamityInheritancePlayer>();
            var modPlayer = player.Calamity();
            modPlayer.tarraSet = true;
            modPlayer.tarraThrowing = true;
            modPlayer.bloodflareSet = true;
            modPlayer.bloodflareThrowing = true;
            modPlayer.godSlayer = true;
            modPlayer.godSlayerThrowing = true;
            modPlayer1.AuricSilvaFakeDeath = true;
            const short onlyDash = 2;
            const short onlyReborn = 1; 
            int mode = CIConfig.Instance.GodSlayerSetBonusesChange;
            player.setBonus = this.GetLocalizedValue("SetBonus") + "\n" + GodSlayerChestplateold.GetSpecial(mode);
            modPlayer1.GodSlayerReborn = mode != onlyDash;
            if (modPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && modPlayer.LastUsedDashID == GodslayerArmorDash.ID && mode > onlyReborn)
            {
                modPlayer.DeferredDashID = GodslayerArmorDash.ID;
                player.dash = 0;
            }
            modPlayer1.SilvaRougeSetLegacy = true;

            modPlayer.rogueStealthMax += 1.3f;
            modPlayer.wearingRogueArmor = true;
            //继承至弑神套的视潜伏值上限增加潜伏值的效果
            float getMaxStealth = modPlayer.rogueStealthMax;
            modPlayer.rogueStealthMax += getMaxStealth / 6;
            modPlayer.wearingRogueArmor = true;
            player.thorns += 3f;
            player.ignoreWater = true;
            player.crimsonRegen = true;

            modPlayer.WearingPostMLSummonerSet = true;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.Calamity();
            var modPlayer1 = player.CIMod();
            modPlayer1.auricBoostold = true;
            player.GetDamage<RogueDamageClass>() += 0.2f;
            player.GetCritChance<RogueDamageClass>() += 20;
            player.moveSpeed += 0.25f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SilvaHeadRogue>().
                AddRecipeGroup("CalamityInheritance:AnyGodSlayerHeadRogue").
                AddIngredient<BloodflareHeadRogue>().
                AddIngredient<TarragonHeadRogue>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient<SilvaHeadRogue>().
                AddRecipeGroup("CalamityInheritance:AnyGodSlayerHeadRogue").
                AddIngredient<BloodflareHeadRogue>().
                AddIngredient<TarragonHeadRogue>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBar>(12).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
