using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Combat
{
    public class AbyssalAmulet : CIAccessories
    {
        public override int AccessoriesStyle => Combat;
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.CalPlayerInfo().ZoneAbyss)
                player.LAP().MaxLifeMultiplier += 0.1f;
            player.CI().AbyssalAmuletLegacy = true; ;
            if (HasCalamity())
                player.buffImmune[CalDeBuff.RiptideDebuff] = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PalmWood, 12).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
