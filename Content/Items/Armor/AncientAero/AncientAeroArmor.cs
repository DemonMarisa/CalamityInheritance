using CalamityMod.Items.Materials;
using ReLogic.Content;
using Steamworks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientAero
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientAeroArmor :CIArmor, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.height = 18;
            Item.width = 30;
            Item.rare = ItemRarityID.Orange;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.defense = 20;
        }
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.1f;
            player.jumpSpeedBoost += 0.5f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AerialiteBar>(15).
                AddIngredient(ItemID.FallenStar, 5).
                AddIngredient(ItemID.Feather, 5).
                AddTile(TileID.SkyMill).
                Register();
        }
    }
}