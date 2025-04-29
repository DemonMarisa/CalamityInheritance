using CalamityMod.CalPlayer.Dashes;
using CalamityMod;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using System.Collections.Generic;
using Terraria.Localization;

namespace CalamityInheritance.Content.Items.Armor.GodSlayerOld
{
    [AutoloadEquip(EquipType.Head)]
    public class GodSlayerHeadMagicold : CIArmor, ILocalizedModType
    {
        public static string Path = "Mods.CalamityInheritance.Content.Items.Armor.GodSlayerChestplateold";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 21; //96
            Item.rare = ModContent.RarityType<DeepBlue>();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<GodSlayerChestplateold>() && legs.type == ModContent.ItemType<GodSlayerLeggingsold>();
        public override void ArmorSetShadows(Player player) => player.armorEffectDrawShadow = true;

        public override void UpdateArmorSet(Player player)
        {
            const short onlyDash = 2;
            const short onlyReborn = 1; 
            var modPlayer = player.Calamity();
            CalamityInheritancePlayer modPlayer2 = player.CIMod();
            modPlayer.godSlayer = true;
            modPlayer2.GodSlayerMagicSet = true;
            int mode = CIConfig.Instance.GodSlayerSetBonusesChange;
            player.setBonus = this.GetLocalizedValue("SetBonus") + "\n" + GodSlayerChestplateold.GetSpecial(mode);
            modPlayer2.GodSlayerReborn = mode != onlyDash;
            if (modPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && modPlayer.LastUsedDashID == GodslayerArmorDash.ID && mode > onlyReborn)
            {
                modPlayer.DeferredDashID = GodslayerArmorDash.ID;
                player.dash = 0;
            }
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<MagicDamageClass>() += 0.14f;
            player.GetCritChance<MagicDamageClass>() += 14;
            player.statManaMax2 += 100;
            player.manaCost *= 0.83f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CosmiliteBar>(7).
                AddIngredient<AscendantSpiritEssence>(2).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
