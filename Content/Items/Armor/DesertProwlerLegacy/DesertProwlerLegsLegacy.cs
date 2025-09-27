using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Armor.DesertProwler;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.DesertProwlerLegacy
{
    [AutoloadEquip(EquipType.Legs)]
    public class DesertProwlerLegsLegacy : CIArmor, ILocalizedModType
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
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MoveSpeed.ToPercent());
        public override void UpdateEquip(Player player)
        {
            player.buffImmune[BuffID.WindPushed] = true;
            player.moveSpeed += MoveSpeed;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DesertProwlerPants>().
                AddIngredient<DesertFeather>().
                AddTile(TileID.Anvils).
                Register();
        }
    }
}