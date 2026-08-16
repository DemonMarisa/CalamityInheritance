using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.CDs;
using CalamityInheritance.Content.Items.Accessories.Misc;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.SystemsLoader;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.Silva
{
    [AutoloadEquip(EquipType.Head)]
    public class SilvaHeadRanged : CIArmor
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 36; //96
            Item.rare = RarityType<DeepBlue>();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<SilvaArmorold>() && legs.type == ItemType<SilvaLeggingsold>();
        public override void UpdateArmorSet(Player player)
        {
            player.CI().SilvaSet = true;
            player.CI().SilvaReborn = true;
            player.CI().SilvaRanged = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            if (player.HeldItem.useTime > 3 && player.HeldItem.DamageType.CountsAsClass<RangedDamageClass>())
                player.GetAttackSpeed<RangedDamageClass>() += 0.1f;
            if (player.HasCD<SilvaReviveCD>())
                player.GetDamage<RangedDamageClass>() += 0.40f;
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
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.PlantyMush, 6).
                    AddIngredient(CalamityMaterials.EffulgentFeather, 5).
                    AddIngredient(CalamityMaterials.DarksunFragment, 10).
                    AddIngredient<LeadCore>().
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
            else
            {

            }
        }
    }
}
