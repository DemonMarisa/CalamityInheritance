using CalamityInheritance.Content.Items.Armor.Wulfum.NewTexture;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.Wulfum
{
    [AutoloadEquip(EquipType.Head)]
    public class ThrowerWulfrumMaskLegacy : ModItem, ILocalizedModType 
    {
        public new string LocalizationCategory => "Content.Items.Wulfrum";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<ThrowerWulfrumMaskLegacy>()] = ModContent.ItemType<ANewWulfrumMask>();
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
            bool wulLegacy = body.type == ModContent.ItemType<WulfrumArmorLegacy>() && legs.type == ModContent.ItemType<WulfrumLeggingsLegacy>();
            bool wulNew = body.type == ModContent.ItemType<ANewWulfrumArmor>() && legs.type == ModContent.ItemType<ANewWulfrumLeggings>();
            bool wullegacynew = body.type == ModContent.ItemType<WulfrumArmorLegacy>() && legs.type == ModContent.ItemType<ANewWulfrumLeggings>();
            bool wulnewlegacy = body.type == ModContent.ItemType<ANewWulfrumArmor>() && legs.type == ModContent.ItemType<WulfrumLeggingsLegacy>();
            return wulLegacy || wulNew || wullegacynew || wulnewlegacy;
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityPlayer calp = player.Calamity();
            player.setBonus = this.GetLocalizedValue("SetBonus");
            calp.wearingRogueArmor = true; 
            player.GetDamage<RogueDamageClass>() += 0.03f; //3%盗贼伤害
            calp.rogueStealthMax += 0.5f; //50潜伏值
            player.setBonus = this.GetLocalizedValue("SetBonus");
            player.statDefense += 3; //9
            if (player.statLife <= (player.statLifeMax2 * 0.5f))
            {
                player.statDefense += 5; //14
            }
            player.pickSpeed -= 0.10f;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 10;
        }
    }
}