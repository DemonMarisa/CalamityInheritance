using CalamityInheritance.Content.Items.Materials;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.Xeroc
{
    [AutoloadEquip(EquipType.Legs)]
    public class AncientXerocCuisses : CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceCyan;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 16;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 3;
            player.statLifeMax2 += 40;
            player.statManaMax2 += 40;
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