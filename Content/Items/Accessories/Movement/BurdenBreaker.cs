using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.MiscDate;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Movement
{
    public class BurdenBreaker : CIAccessories
    {
        public override int AccessoriesStyle => Movement;
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
            Item.master = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (LAPInfo.AnyBossHere)
                return;
            // Completely remove movement restrictions if you're yeeting with the profaned spear
            if (player.velocity.X > 5f)
            {
                player.velocity.X *= 1.025f;
                if (player.velocity.X >= 500f)
                {
                    player.velocity.X = 0f;
                }
            }
            else if (player.velocity.X < -5f)
            {
                player.velocity.X *= 1.025f;
                if (player.velocity.X <= -500f)
                {
                    player.velocity.X = 0f;
                }
            }
        }
    }
}
