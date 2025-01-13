using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;

namespace CalamityInheritance.Content.Items.Tools
{
    public class WulfrumHammer : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Melee;
            Item.width = 56;
            Item.height = 56;
            Item.useTime = 8;
            Item.useAnimation = 16;
            Item.useTurn = true;
            Item.hammer = 35;
            Item.useStyle = 1;
            Item.knockBack = 4f;
            Item.value = 25000;
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
            AddIngredient<WulfrumMetalScrap>(6).
            AddTile(TileID.WorkBenches).
            Register();
        }
    }
}