using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.CDs;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.GlobalInstance.Players.Dash;
using CalamityInheritance.Core.Keys;
using CalamityInheritance.Core.Utils;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AncientGodSlayer
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientGodSlayerHelm : CIArmor
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 30; //130
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<AncientGodSlayerChestplate>() && legs.type == ItemType<AncientGodSlayerLeggings>();

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus");

            player.SetRogueArmor(1.25f);
            player.CI().AncientGodSlayer = true;
            player.CI().GodSlayerReborn = true;
            player.CI().ContactDamageReduction *= 0.85f;
            player.CI().LifeRegen += 8;
            player.LAP().healingPotionMult += 0.7f;
            if (CIKeybinds.GodSlayerDash.JustPressed && !player.HasCD<GodSlayerDashLegacyCD>())
            {
                player.ImmediatelyDash(LAPContent.DashType<GodSlayerDashLegacy>());
                if (LAPUtilities.IsLocalPlayer(player.whoAmI))
                    player.AddCD(LAPContent.CDType<GodSlayerDashLegacyCD>(), 60 * 15);
                player.CostRogueStealth(0.75f);
                player.CheckFocus((int)(player.LAP().statFocusMax2 * 0.1f));
            }
            if (player.LAP().statFocus == player.LAP().statFocusMax2)
            {
                player.RemoveCD(LAPContent.CDType<GodSlayerDashLegacyCD>());
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 5;
            player.maxTurrets += 3;
            player.statLifeMax2 += 200;
            player.GetDamage<GenericDamageClass>() += 0.40f;
            player.GetCritChance<GenericDamageClass>() += 25;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.CosmiliteBar, 25).
                    AddIngredient(CalamityMaterials.AscendantSpiritEssence, 10).
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
            else
            {

            }
        }
    }
}