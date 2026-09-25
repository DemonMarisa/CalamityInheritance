using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.GlobalInstance.Players.Dash;
using LAP.Core.SystemsLoader;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Movement
{
    public class StatisVoidSash : CIAccessories
    {
        public override int AccessoriesStyle => Movement;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 32;
            Item.rare = RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
        }
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 3));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<GenericDamageClass>() += 0.10f;
            player.jumpSpeedBoost += 1.6f;
            player.moveSpeed += 0.10f;
            player.spikedBoots = 2;
            player.noFallDmg = true;
            player.blackBelt = true;
            player.autoJump = true;
            player.dashType = DashID.None;
            player.SetLAPDash(LAPContent.DashType<StatisVoidSashDashOld>());
        }

        public override void AddRecipes()
        {
        }
    }
}
