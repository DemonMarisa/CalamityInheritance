using CalamityMod.Items.Armor.Silva;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Materials;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Placeables;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace CalamityInheritance.Content.Items.Armor.Silva
{
    [AutoloadEquip(EquipType.Head)]
    public class SilvaHeadRanged : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 36; //96
            Item.rare = ModContent.RarityType<DeepBlue>();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool isSilvaSetNEW = body.type == ModContent.ItemType<SilvaArmor>() && legs.type == ModContent.ItemType<SilvaLeggings>();
            bool isSilvaSetOLD = body.type == ModContent.ItemType<SilvaArmorold>() && legs.type == ModContent.ItemType<SilvaLeggingsold>();
            return isSilvaSetNEW || isSilvaSetOLD;
        }
        public override void UpdateArmorSet(Player player)
        {
            var modPlayer1 = player.CalamityInheritance();
            modPlayer1.AuricSilvaSet = true;
            modPlayer1.SilvaRebornMark = true;
            modPlayer1.SilvaRangedSetLegacy = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            if (player.HeldItem.useTime > 3 && player.HeldItem.DamageType == DamageClass.Ranged)
            {
                player.GetAttackSpeed<RangedDamageClass>() += 0.1f;
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<RangedDamageClass>() += 0.13f;
            player.GetCritChance<RangedDamageClass>() += 13;
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PlantyMush>(6).
                AddIngredient<EffulgentFeather>(5).
                AddIngredient<AscendantSpiritEssence>(2).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
