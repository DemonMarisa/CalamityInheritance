using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;
using CalamityMod;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class DeificAmuletLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";

        public override void SetStaticDefaults()
        {
            if(CalamityInheritanceConfig.Instance.CustomShimmer == true) //微光嬗变config启用时，将会使原灾的血杯与这一速杀版本的血神核心微光相互转化
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<DeificAmulet>()] = ModContent.ItemType<DeificAmuletLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<DeificAmuletLegacy>()] = ModContent.ItemType<DeificAmulet>();
            }
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Cyan;
            Item.value = CIShopValue.RarityPriceCyan;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer1 = player.CalamityInheritance();
            var modPlayer = player.Calamity();
            player.pStone = true;
            player.longInvince = true;
            modPlayer1.deificAmuletEffect = true;
            player.GetArmorPenetration<GenericDamageClass>() += 10;
            modPlayer.jellyfishNecklace = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.StarVeil).
                AddIngredient(ItemID.CharmofMyths).
                AddIngredient(ItemID.MeteoriteBar, 10).
                AddIngredient<SeaPrism>(15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
