using CalamityMod.CalPlayer.Dashes;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using System.Collections.Generic;

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
            Item.rare = RarityType<DeepBlue>();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<GodSlayerChestplateold>() && legs.type == ItemType<GodSlayerLeggingsold>();
        public override void ArmorSetShadows(Player player) => player.armorEffectDrawShadow = true;
        public override void ModifyTooltips(List<TooltipLine> list) => list.IntegrateHotkey(CalamityKeybinds.GodSlayerDashHotKey);
        public override void UpdateArmorSet(Player player)
        {
            var modPlayer = player.Calamity();
            CalamityInheritancePlayer modPlayer2 = player.CIMod();
            modPlayer.godSlayer = true;
            modPlayer2.GodSlayerMagicSet = true;
            modPlayer2.CanUseLegacyGodSlayerDash = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            modPlayer2.GodSlayerReborn = true;
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
