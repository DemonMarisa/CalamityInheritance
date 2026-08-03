using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.DesertProwlerLegacy
{
    [AutoloadEquip(EquipType.Legs)]
    public class DesertProwlerLegsLegacy : CIArmor
    {
        private const float MoveSpeed = 0.1f;
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ItemRarityID.Blue;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.defense = 3;
        }
        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.WindPushed] = true;
            player.moveSpeed += MoveSpeed;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DesertFeather>().
                AddTile(TileID.Anvils).
                Register();
        }
    }
}