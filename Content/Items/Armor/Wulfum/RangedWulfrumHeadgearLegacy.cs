using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityMod.Items;


namespace CalamityInheritance.Content.Items.Armor.Wulfum
{
    [AutoloadEquip(EquipType.Head)]
    public class RangedWulfrumHeadgearLegacy : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wulfrum Headgear");
            // Tooltip.SetDefault("3% increased ranged damage");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityBlueBuyPrice;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 2; //7
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<WulfrumArmorLegacy>() && legs.type == ModContent.ItemType<WulfrumLeggingsLegacy>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus");
            player.statDefense += 3; //10
            if (player.statLife <= (player.statLifeMax2 * 0.5f))
            {
                player.statDefense += 5; //15
            }
            player.pickSpeed -= 0.10f;
            player.moveSpeed += 0.10f;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.03f;
        }
    }
}