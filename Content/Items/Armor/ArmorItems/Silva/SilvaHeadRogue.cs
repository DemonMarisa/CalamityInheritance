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
    public class SilvaHeadRogue : CIArmor, ILocalizedModType
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
            Item.defense = 30; //96
            Item.rare = RarityType<DeepBlue>();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<SilvaArmorold>() && legs.type == ItemType<SilvaLeggingsold>();
        public override void UpdateArmorSet(Player player)
        {
            player.CI().SilvaSet = true;
            player.CI().SilvaReborn = true;
            player.CI().SilvaRogue = true;
            player.SetRogueArmor(1.25f);
            player.setBonus = this.GetLocalizedValue("SetBonus");
            if (player.statLife > (int)(player.statLifeMax2 * 0.5) && player.HeldItem.DamageType.CountsAsClass<ThrowingDamageClass>())
                player.GetAttackSpeed<ThrowingDamageClass>() += 0.2f;
            if (player.HasCD<SilvaReviveCD>())
                player.GetDamage<ThrowingDamageClass>() += 0.40f;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<ThrowingDamageClass>() += 0.13f;
            player.GetCritChance<ThrowingDamageClass>() += 13;
            player.moveSpeed += 0.2f;
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
