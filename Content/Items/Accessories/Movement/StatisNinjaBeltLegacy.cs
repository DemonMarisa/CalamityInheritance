using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.GlobalInstance.Players.Dash;
using LAP.Core.SystemsLoader;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Movement
{
    public class StatisNinjaBeltLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Movement;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 32;
            Item.rare = ItemRarityID.Purple;
            Item.value = CIShopValue.RarityPricePurple;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.autoJump = true;
            player.jumpSpeedBoost += 1.6f;
            player.moveSpeed += 0.1f; //斯塔提斯腰带怎么少了10%移速
            player.extraFall += 35;
            player.blackBelt = true;
            player.dashType = DashID.None;
            player.SetLAPDash(LAPContent.DashType<StatisNinjaBeltDash>());
            player.spikedBoots = 2;
            player.accFlipper = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MasterNinjaGear).
                AddIngredient(ItemID.FrogFlipper).
                AddIngredient(ItemID.Ectoplasm, 5).
                AddTile(TileID.Anvils).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.Tabi).
                AddIngredient(ItemID.BlackBelt).
                AddIngredient(ItemID.FrogGear).
                AddIngredient(ItemID.Ectoplasm, 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
