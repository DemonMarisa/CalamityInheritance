using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.DesertProwlerLegacy
{
    [AutoloadEquip(EquipType.Head)]
    public class DesertProwlerHeadLegacy : CIArmor
    {
        private const int Crits = 4;
        public const int FlatDamage = 1;
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ItemRarityID.Blue;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.defense = 1;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<DesertProwlerBodyLegacy>() && legs.type == ItemType<DesertProwlerLegsLegacy>();
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus");
            player.CI().DesertProwler = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<RangedDamageClass>() += Crits;
            player.ammoCost80 = true;
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