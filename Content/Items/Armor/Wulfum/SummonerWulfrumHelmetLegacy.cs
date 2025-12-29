using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items.Armor.Wulfum.NewTexture;

namespace CalamityInheritance.Content.Items.Armor.Wulfum
{
    [AutoloadEquip(EquipType.Head)]
    public class SummonerWulfrumHelmetLegacy : CIArmor, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Wulfrum";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[ItemType<SummonerWulfrumHelmetLegacy>()] = ItemType<ANewWulfrumHelmet>();
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.rare = ItemRarityID.Blue;
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
            player.statDefense += 5; //8
            if (player.statLife <= (int)(player.statLifeMax2 * 0.5))
            {
                player.statDefense += 5; //13
            }
            player.pickSpeed -= 0.10f;
            player.moveSpeed += 0.10f;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.06f;
            player.maxMinions++;
        }
    }
}