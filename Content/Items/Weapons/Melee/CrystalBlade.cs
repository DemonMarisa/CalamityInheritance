using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Melee;
using LAP.Content.RecipeGroupAdd;
using CalamityMod.Items.Placeables.SunkenSea;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class CrystalBlade : CIMelee, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.width = 66;
            Item.damage = 45;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.knockBack = 4.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 66;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<CrystalDust>();
            Item.shootSpeed = 3f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.CrystalShard, 10).
                AddIngredient(ItemID.PixieDust, 10).
                AddRecipeGroup(LAPRecipeGroup.AnyCobaltBar, 8).
                AddIngredient<SeaPrism>(15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
