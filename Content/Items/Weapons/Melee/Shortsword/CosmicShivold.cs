using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class CosmicShivold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee.Shortsword";

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 13;
            Item.useTime = 13;
            Item.width = 44;
            Item.height = 44;
            Item.damage = 148;
            Item.knockBack = 9f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<CosmicShivProjold>();
            Item.shootSpeed = 2.4f;

            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.Calamity().donorItem = true; //Yatagarasu#0001
        }

        public override bool MeleePrefix() => true;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("CalamityInheritance:AnyElementalShiv").
                AddIngredient<CosmiliteBar>(8).
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();
        }
    }
}
