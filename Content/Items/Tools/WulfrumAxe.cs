using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;

namespace CalamityInheritance.Content.Items.Tools
{
    public class WulfrumAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wulfrum Axe");
        }

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 62;
            Item.height = 48;
            Item.useTime = 8;
            Item.useAnimation = 16;
            Item.useTurn = true;
            Item.axe = 7;
            Item.useStyle = 1;
            Item.knockBack = 3f;
            Item.value = 25000;
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<WulfrumMetalScrap>(4)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}