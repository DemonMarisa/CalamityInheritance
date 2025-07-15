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
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.accessory = true;
        }
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return Item.type.SetConflictMod<DeificAmulet>(equippedItem, incomingItem);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer1 = player.CIMod();
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
