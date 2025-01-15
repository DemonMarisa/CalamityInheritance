using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class LeechingDagger : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee.Shortsword";
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 15;
            Item.width = 26;
            Item.height = 26;
            Item.damage = 33;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.shoot = ModContent.ProjectileType<LeechingDaggerProj>();
            Item.shootSpeed = 3f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix() => true;
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DemoniteBar, 5);
            recipe.AddIngredient(ItemID.RottenChunk, 2);
            recipe.AddIngredient(ModContent.ItemType<RottenMatter>(), 5);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
