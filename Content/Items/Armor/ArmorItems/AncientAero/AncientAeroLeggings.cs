using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AncientAero
{
    [AutoloadEquip(EquipType.Legs)]
    public class AncientAeroLeggings : CIArmor
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.rare = ItemRarityID.Orange;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.defense = 10;
        }
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.1f;
            player.jumpSpeedBoost += 0.5f;
            base.UpdateEquip(player);
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.AerialiteBar, 5).
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