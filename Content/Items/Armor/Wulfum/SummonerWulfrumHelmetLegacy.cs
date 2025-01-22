using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityInheritance.Content.Items.Armor.Wulfum.NewTexture;

namespace CalamityInheritance.Content.Items.Armor.Wulfum
{
    [AutoloadEquip(EquipType.Head)]
    public class SummonerWulfrumHelmetLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Wulfrum";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<SummonerWulfrumHelmetLegacy>()] = ModContent.ItemType<ANewWulfrumHelmet>();
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
            bool wulLegacy = body.type == ModContent.ItemType<WulfrumArmorLegacy>() && legs.type == ModContent.ItemType<WulfrumLeggingsLegacy>();
            bool wulNew = body.type == ModContent.ItemType<ANewWulfrumArmor>() && legs.type == ModContent.ItemType<ANewWulfrumLeggings>();
            bool wullegacynew = body.type == ModContent.ItemType<WulfrumArmorLegacy>() && legs.type == ModContent.ItemType<ANewWulfrumLeggings>();
            bool wulnewlegacy = body.type == ModContent.ItemType<ANewWulfrumArmor>() && legs.type == ModContent.ItemType<WulfrumLeggingsLegacy>();
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