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
