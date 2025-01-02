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
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Armor.AuricTesla
{
    [AutoloadEquip(EquipType.Head)]
    public class AuricTeslaHeadRogue : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.PostMoonLord";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.defense = 34; //132
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
            player.setBonus = this.GetLocalizedValue("SetBonus");
            CalamityInheritancePlayer modPlayer1 = player.GetModPlayer<CalamityInheritancePlayer>();
            var modPlayer = player.Calamity();
            modPlayer.tarraSet = true;
            modPlayer.tarraThrowing = true;
            modPlayer.bloodflareSet = true;
            modPlayer.bloodflareThrowing = true;
            modPlayer.godSlayer = true;
            modPlayer.godSlayerThrowing = true;
            modPlayer.silvaSet = true;

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

            modPlayer1.silvaRogue = true;

            modPlayer.auricSet = true;

            modPlayer.rogueStealthMax += 1.3f;
            modPlayer.wearingRogueArmor = true;
            player.thorns += 3f;
            player.ignoreWater = true;
            player.crimsonRegen = true;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.Calamity();
            var modPlayer1 = player.CalamityInheritance();
            modPlayer1.auricBoostold = true;
            modPlayer.auricBoost = true;
            player.GetDamage<ThrowingDamageClass>() += 0.2f;
            player.GetCritChance<ThrowingDamageClass>() += 20;
            player.moveSpeed += 0.25f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GodSlayerHeadRogue>().
                AddIngredient<BloodflareHeadRogue>().
                AddIngredient<TarragonHeadRogue>().
                AddIngredient<AuricBar>(12).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
