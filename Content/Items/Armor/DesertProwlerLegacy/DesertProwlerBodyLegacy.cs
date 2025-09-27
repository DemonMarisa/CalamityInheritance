using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Armor.DesertProwler;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.DesertProwlerLegacy
{
    [AutoloadEquip(EquipType.Body)]
    public class DesertProwlerBodyLegacy : CIArmor, ILocalizedModType
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
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Crtis.ToPercent());
        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<RangedDamageClass>() += Crtis;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DesertProwlerShirt>().
                AddIngredient<DesertFeather>().
                AddTile(TileID.Anvils).
                Register();
        }
    }
}