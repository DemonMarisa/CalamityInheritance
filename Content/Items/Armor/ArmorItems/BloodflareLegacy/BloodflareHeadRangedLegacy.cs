using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.BloodflareLegacy
{
    [AutoloadEquip(EquipType.Head)]
    public class BloodflareHeadRangedLegacy : CIArmor
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.defense = 34;
            Item.rare = RarityType<AbsoluteGreen>();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<BloodflareBodyLegacy>() && legs.type == ModContent.ItemType<BloodflareLegLegacy>();
        }
        public override void ArmorSetShadows(Player player) => player.armorEffectDrawShadowSubtle = true;

        public override void UpdateArmorSet(Player player)
        {
            player.CI().BloodflareSet = true;
            player.CI().BloodflareRanged = true;
            player.setBonus = this.GetLocalization("SetBonus").Value;
            player.crimsonRegen = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<RangedDamageClass>() += 0.1f;
            player.GetCritChance<RangedDamageClass>() += 10;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BloodstoneCore>(11).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
