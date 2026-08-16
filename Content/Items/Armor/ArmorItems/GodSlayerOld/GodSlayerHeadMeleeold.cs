using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.CDs;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.GlobalInstance.Players.Dash;
using CalamityInheritance.Core.Keys;
using CalamityInheritance.Core.Utils;
using LAP.Core.GlobalInstance.Players;
using LAP.Core.NetCode.NetUtilities;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.GodSlayerOld
{
    [AutoloadEquip(EquipType.Head)]
    public class GodSlayerHeadMeleeold : CIArmor
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 48; //96
            Item.rare = RarityType<DeepBlue>();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<GodSlayerChestplateold>() && legs.type == ItemType<GodSlayerLeggingsold>();
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void ModifyTooltips(List<TooltipLine> list) => list.IntegrateHotkey(CIKeybinds.GodSlayerDash);
        public override void UpdateArmorSet(Player player)
        {
            player.CI().GodSlayerSet = true;
            player.CI().GodSlayerReborn = true;
            player.CI().GodSlayerMelee = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            if (CIKeybinds.GodSlayerDash.JustPressed && !player.HasCD<GodSlayerDashLegacyCD>())
            {
                player.ImmediatelyDash(LAPContent.DashType<GodSlayerDashLegacy>());
                player.SyncedDash(LAPContent.DashType<GodSlayerDashLegacy>());
                if (LAPUtilities.IsLocalPlayer(player.whoAmI))
                    player.AddCD(LAPContent.CDType<GodSlayerDashLegacyCD>(), 60 * 15);
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<MeleeDamageClass>() += 0.14f;
            player.GetCritChance<MeleeDamageClass>() += 14;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.20f;
            player.aggro += 1000;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.CosmiliteBar, 7).
                    AddIngredient(CalamityMaterials.AscendantSpiritEssence, 2).
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
            else
            {

            }
        }
    }
}
