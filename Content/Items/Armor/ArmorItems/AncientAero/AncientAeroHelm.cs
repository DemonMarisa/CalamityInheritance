using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AncientAero
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientAeroHelm : CIArmor
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.rare = ItemRarityID.Orange;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.defense = 5;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<AncientAeroArmor>() && legs.type == ItemType<AncientAeroLeggings>();
        public override void UpdateEquip(Player p)
        {
            p.moveSpeed += 0.1f;
            p.jumpSpeedBoost += 0.5f;
        }
        public override void UpdateArmorSet(Player player)
        {
            var usPlayer = player.CI();
            player.setBonus = this.GetLocalizedValue("SetBonus");
            usPlayer.AncientAeroSet = true;
            player.wingTimeMax += 180;
            bool usingAeroStoneLegacy = player.CI().AeroStonePower;
            if (usingAeroStoneLegacy)
                player.wingTimeMax += 360;
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.AerialiteBar, 10).
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