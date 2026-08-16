using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AncientAero
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientAeroArmor : CIArmor, ILocalizedModType
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
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.AerialiteBar, 15).
                    AddIngredient(ItemID.FallenStar, 5).
                    AddIngredient(ItemID.Feather, 5).
                    AddTile(TileID.SkyMill).
                    Register();
            }
            else
            {

            }
        }
    }
}