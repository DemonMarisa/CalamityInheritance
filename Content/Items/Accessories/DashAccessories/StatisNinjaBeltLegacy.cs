using CalamityMod;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.DashAccessories
{
    public class StatisNinjaBeltLegacy : CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.DashAccessories";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 26;
            Item.value = CIShopValue.RarityPricePurple;
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.autoJump = true;
            player.jumpSpeedBoost += 0.32f;
            player.moveSpeed += 0.1f; //斯塔提斯腰带怎么少了10%移速
            player.extraFall += 35;
            player.blackBelt = true;
            player.dashType = 1;
            player.Calamity().DashID = string.Empty;
            player.spikedBoots = 2;
            player.accFlipper = true;
        }

        public override void AddRecipes()
        {
            //Scarlet:移除复仇者徽章，合成站改为普通的铁砧
            CreateRecipe().
                AddIngredient(ItemID.MasterNinjaGear).
                AddIngredient(ItemID.FrogFlipper).
                AddIngredient<PurifiedGel>(50).
                AddIngredient(ItemID.Ectoplasm, 5).
                AddTile(TileID.Anvils).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.Tabi).
                AddIngredient(ItemID.BlackBelt).
                AddIngredient(ItemID.FrogGear).
                AddIngredient<PurifiedGel>(50).
                AddIngredient(ItemID.Ectoplasm, 5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
