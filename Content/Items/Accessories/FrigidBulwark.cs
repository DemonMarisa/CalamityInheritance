using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories
{
    internal class FrigidBulwark : CIAccessories, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 40;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 12;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var usPlayer = player.CIMod();
            usPlayer.FrigidBulwark = true;
            usPlayer.RoDPaladianShieldActive = true;
            if (player.statLife <= player.statLifeMax2 * 0.5)
                player.AddBuff(BuffID.IceBarrier, 5);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.FrozenShield).
                AddIngredient(ModContent.ItemType<FrostFlare>()).
                AddIngredient(ModContent.ItemType<CoreofEleum>(), 5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
