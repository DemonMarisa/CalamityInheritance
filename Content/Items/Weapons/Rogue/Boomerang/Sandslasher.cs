using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Rogue.Boomerang;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using LAP.Content.RecipeGroupAdd;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Boomerang
{
    public class Sandslasher : CIRogueClass
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.damage = 115;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.knockBack = 5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.shoot = ProjectileType<SandslasherProj>();
            Item.shootSpeed = 8f;
            Item.DamageType = RogueDamageClass.Instance;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GrandScale>().
                AddIngredient<CoreofSunlight>(6).
                AddRecipeGroup(LAPRecipeGroup.AnyGoldBar, 10).
                AddIngredient(ItemID.HardenedSand, 25).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
