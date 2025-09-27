using CalamityMod.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Accessories;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class DeificAmuletLegacy : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:48,
            itemHeight:48,
            itemRare:ItemRarityID.LightRed,
            itemValue:CIShopValue.RarityPriceLightRed
        );
        public override void ExSSD() => Type.ShimmerEach<DeificAmulet>();
        public override bool CanEquipAccessory(Player player, int slot, bool modded) => !player.Calamity().dAmulet;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer1 = player.CIMod();
            var modPlayer = player.Calamity();
            player.pStone = true;
            player.longInvince = true;
            modPlayer1.deificAmuletEffect = true;
            player.lifeRegen += 1;
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
