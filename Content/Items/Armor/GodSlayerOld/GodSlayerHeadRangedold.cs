using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Armor.GodSlayerOld
{
    [AutoloadEquip(EquipType.Head)]
    public class GodSlayerHeadRangedold : CIArmor, ILocalizedModType
    {

        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 35; //96
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
            CalamityInheritancePlayer modPlayer1 = player.CIMod();
            var modPlayer = player.Calamity();
            modPlayer.godSlayer = true;
            modPlayer1.GodSlayerRangedSet = true;
            if (CIConfig.Instance.GodSlayerSetBonusesChange == 1 || (CIConfig.Instance.GodSlayerSetBonusesChange == 3) && !(CIConfig.Instance.GodSlayerSetBonusesChange == 2))
            {
                modPlayer1.GodSlayerReborn = true;
            }
            if (CIConfig.Instance.GodSlayerSetBonusesChange == 2 || (CIConfig.Instance.GodSlayerSetBonusesChange == 3))
            {
                if (modPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && modPlayer.LastUsedDashID == GodslayerArmorDash.ID)
                {
                    modPlayer.DeferredDashID = GodslayerArmorDash.ID;
                    player.dash = 0;
                }
            }
            player.setBonus = this.GetLocalizedValue("SetBonus");
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<RangedDamageClass>() += 0.14f;
            player.GetCritChance<RangedDamageClass>() += 14;
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
