using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Armor.DesertProwler;
using Humanizer;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static CalamityInheritance.Core.Enums;

namespace CalamityInheritance.Content.Items.Armor.DesertProwlerLegacy
{
    [AutoloadEquip(EquipType.Head)]
    public class DesertProwlerHeadLegacy: CIArmor, ILocalizedModType
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
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.ThisBodyPart<DesertProwlerBodyLegacy>(ArmorPart.Body) && legs.ThisBodyPart<DesertProwlerLegsLegacy>(ArmorPart.Legs);
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Crits.ToPercent(), "80%");
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus").FormatWith(FlatDamage);
            player.CIMod().DesertProwler = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<RangedDamageClass>() += Crits;
            player.ammoCost80 = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DesertProwlerHat>().
                AddIngredient<DesertFeather>().
                AddTile(TileID.Anvils).
                Register();
        }
    }
}