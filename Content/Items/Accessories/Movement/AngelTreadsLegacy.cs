using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Movement
{
    public class AngelTreadsLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Movement;
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 36;
            Item.value = CIShopValue.RarityPriceLightPurple;
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.LAP().WingTimeMaxMult += 0.2f;
            player.accRunSpeed = 9.5f;
            player.rocketBoots = player.vanityRocketBoots = 3;
            player.moveSpeed += 0.12f;
            player.iceSkate = true;
            player.waterWalk = true;
            player.fireWalk = true;
            player.lavaMax += 240;
            player.buffImmune[BuffID.OnFire] = true;
        }

        public override void UpdateVanity(Player player)
        {
            player.vanityRocketBoots = 3;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.TerrasparkBoots).
                AddIngredient(ItemID.SoulofFright).
                AddIngredient(ItemID.SoulofMight).
                AddIngredient(ItemID.SoulofSight).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
