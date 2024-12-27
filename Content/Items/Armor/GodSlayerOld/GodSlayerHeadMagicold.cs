using CalamityMod.CalPlayer.Dashes;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Armor.GodSlayerOld
{
    [AutoloadEquip(EquipType.Head)]
    public class GodSlayerHeadMagicold : ModItem, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.defense = 21; //96
            Item.rare = ModContent.RarityType<DarkBlue>();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool isGodSlayerSetNEW = body.type == ModContent.ItemType<GodSlayerChestplate>() && legs.type == ModContent.ItemType<GodSlayerLeggings>();
            bool isGodSlayerSetOLD = body.type == ModContent.ItemType<GodSlayerChestplateold>() && legs.type == ModContent.ItemType<GodSlayerLeggingsold>();
            return isGodSlayerSetNEW || isGodSlayerSetOLD;
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            if (CalamityInheritanceConfig.Instance.GodSlayerSetBonusesChange == 1)
            {
                var modPlayer = player.Calamity();
                CalamityInheritancePlayer modPlayer2 = player.CalamityInheritance();
                modPlayer.godSlayer = true;
                modPlayer2.godSlayerMagic = true;
                modPlayer2.GodSlayerReborn = true;
                player.setBonus = this.GetLocalizedValue("SetBonus");
            }
            if (CalamityInheritanceConfig.Instance.GodSlayerSetBonusesChange == 2)
            {
                var modPlayer = player.Calamity();
                player.GetAttackSpeed<MeleeDamageClass>() += 0.2f;
                var hotkey = CalamityKeybinds.GodSlayerDashHotKey.TooltipHotkeyString();
                player.thorns += 2.5f;
                player.aggro += 1000;

                if (modPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && modPlayer.LastUsedDashID == GodslayerArmorDash.ID)
                {
                    modPlayer.DeferredDashID = GodslayerArmorDash.ID;
                    player.dash = 0;
                }
            }
            if (CalamityInheritanceConfig.Instance.GodSlayerSetBonusesChange == 3)
            {
                var modPlayer = player.Calamity();
                CalamityInheritancePlayer modPlayer2 = player.CalamityInheritance();
                modPlayer2.godSlayerMagic = true;
                modPlayer2.GodSlayerReborn = true;
                player.GetAttackSpeed<MeleeDamageClass>() += 0.2f;
                var hotkey = CalamityKeybinds.GodSlayerDashHotKey.TooltipHotkeyString();
                player.thorns += 2.5f;
                player.aggro += 1000;

                if (modPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && modPlayer.LastUsedDashID == GodslayerArmorDash.ID)
                {
                    modPlayer.DeferredDashID = GodslayerArmorDash.ID;
                    player.dash = 0;
                }
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
