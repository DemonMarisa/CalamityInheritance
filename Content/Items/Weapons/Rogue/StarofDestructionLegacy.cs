using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class StarofDestructionLegacy : CIRogueClass
    {
        public override void ExSD()
        {
            Item.width = Item.height = 94;
            Item.damage = 150;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 10f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.shoot = ProjectileType<StarofDestructionLegacyProj>();
            Item.shootSpeed = 5f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MeldBlob>(10).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
