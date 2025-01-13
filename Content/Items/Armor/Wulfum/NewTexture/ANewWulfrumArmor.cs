using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;

namespace CalamityInheritance.Content.Items.Armor.Wulfum.NewTexture
{
    [AutoloadEquip(EquipType.Body)]
    public class ANewWulfrumArmor : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 18000;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<GenericDamageClass>() += 3;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<WulfrumMetalScrap>(10)
            .AddIngredient<EnergyCore>(1)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}