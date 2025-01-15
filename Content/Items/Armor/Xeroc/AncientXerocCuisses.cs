using CalamityInheritance.Content.Items.Materials;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.Xeroc
{
    [AutoloadEquip(EquipType.Legs)]
    public class AncientXerocCuisses : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceCyan;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 100;
            player.statManaMax2 += 100;
            player.moveSpeed += 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<NebulaBar>(6).
                AddIngredient<GalacticaSingularity>(3).
                AddTile(TileID.LunarCraftingStation).
                Register();
                
        }
    }
}