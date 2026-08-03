using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Armor.ArmorItems.Wulfum.NewTexture;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.Wulfum
{
    [AutoloadEquip(EquipType.Head)]
    public class ThrowerWulfrumMaskLegacy : CIArmor
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[ItemType<ThrowerWulfrumMaskLegacy>()] = ItemType<ANewWulfrumMask>();
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 1; //6
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool wulLegacy = body.type == ItemType<WulfrumArmorLegacy>() && legs.type == ItemType<WulfrumLeggingsLegacy>();
            bool wulNew = body.type == ItemType<ANewWulfrumArmor>() && legs.type == ItemType<ANewWulfrumLeggings>();
            bool wullegacynew = body.type == ItemType<WulfrumArmorLegacy>() && legs.type == ItemType<ANewWulfrumLeggings>();
            bool wulnewlegacy = body.type == ItemType<ANewWulfrumArmor>() && legs.type == ItemType<WulfrumLeggingsLegacy>();
            return wulLegacy || wulNew || wullegacynew || wulnewlegacy;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus");
            player.GetDamage<ThrowingDamageClass>() += 0.03f; //3%盗贼伤害
            player.statDefense += 3; //9
            if (player.statLife <= player.statLifeMax2 * 0.5f)
            {
                player.statDefense += 5; //14
            }
            player.pickSpeed -= 0.10f;

            player.SetRogueArmor(0.5f);
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.1f;
        }
    }
}