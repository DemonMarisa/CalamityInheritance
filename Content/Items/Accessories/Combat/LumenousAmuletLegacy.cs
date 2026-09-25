using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Combat
{
    internal class LumenousAmuletLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Combat;
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.CalPlayerInfo().ZoneAbyss)
                player.LAP().MaxLifeMultiplier += 0.25f;
            player.CI().AbyssalAmuletLegacy = true;
            if (HasCalamity())
            {
                player.buffImmune[CalDeBuff.RiptideDebuff] = true;
                player.buffImmune[CalDeBuff.CrushDepth] = true;
            }
            player.buffImmune[BuffType<CICrushDepth>()] = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AbyssalAmulet>().
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
