using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.Xeroc
{
    [AutoloadEquip(EquipType.Legs)]
    public class AncientXerocCuisses : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Xeroc Cuisses");
            /* Tooltip.SetDefault("5% increased rogue damage and critical strike chance\n" +
                       "20% increased movement speed\n" +
                       "Speed of the cosmos"); */
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 24;
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 100;
            player.statManaMax2 += 75;
            player.moveSpeed += 0.15f;
        }
    }
}