using CalamityMod;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.DashAccessories
{
    public class StatisNinjaBeltLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Mods.CalamityInheritanceContent.Items.Accessories.DashAccessories";

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
            player.jumpSpeedBoost += 0.4f;
            player.GetDamage<GenericDamageClass>() += 0.10f;
            player.extraFall += 35;
            player.blackBelt = true;
            player.dashType = 1;
            player.Calamity().DashID = string.Empty;
            player.spikedBoots = 2;
            player.accFlipper = true;
        }

        public override void AddRecipes()
        {
            //Scarlet:修改成了全局伤害加成而非纯盗贼伤害，因此材料上多扔了一个复仇者徽章
            CreateRecipe().
                AddIngredient(ItemID.MasterNinjaGear).
                AddIngredient(ItemID.FrogFlipper).
                AddIngredient<PurifiedGel>(50).
                AddIngredient(ItemID.Ectoplasm, 50). //Scarlet:我也忘了之前要多少灵气合成了，随便写个吧
                AddIngredient(ItemID.AvengerEmblem).
                AddTile(TileID.LunarCraftingStation).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.Tabi).
                AddIngredient(ItemID.BlackBelt).
                AddIngredient(ItemID.FrogGear).
                AddIngredient<PurifiedGel>(50).
                AddIngredient(ItemID.Ectoplasm, 50).
                AddIngredient(ItemID.AvengerEmblem).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
