using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.DesertProwlerLegacy
{
    [AutoloadEquip(EquipType.Body)]
    public class DesertProwlerBodyLegacy : CIArmor
    {
        private const int Crtis = 5;
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 3;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<RangedDamageClass>() += Crtis;
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