using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Armor.GodSlayerOld
{
    [AutoloadEquip(EquipType.Head)]
    public class GodSlayerHeadRogueold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 29; //96
            Item.rare = ModContent.RarityType<DeepBlue>();
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
            CalamityInheritancePlayer modPlayer1 = player.CalamityInheritance();
            var modPlayer = player.Calamity();
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
            modPlayer.godSlayer = true;
            modPlayer.godSlayerThrowing = true;
            modPlayer.rogueStealthMax += 1.4f;
            modPlayer.wearingRogueArmor = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<RogueDamageClass>() += 0.14f;
            player.GetCritChance<RogueDamageClass>() += 14;
            player.moveSpeed += 0.18f;
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
