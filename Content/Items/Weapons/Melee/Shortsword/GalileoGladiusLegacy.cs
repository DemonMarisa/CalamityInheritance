using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Weapons.Melee;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class GalileoGladiusLegacy : GeneralWeaponClass
    {
        public override void SetStaticDefaults()
        {
            Type.ShimmerEach<GalileoGladius>();
        }
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 44;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.damage = 92;
            Item.useAnimation = Item.useTime = 12;
            Item.DamageType = DamageClass.Melee;
            Item.shoot = ProjectileType<GalileoGladiusProjLegacy>();
            Item.shootSpeed = 0.9f;
            Item.knockBack = 10f;
            Item.UseSound = SoundID.Item1;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
        }

        public override bool MeleePrefix() => true;

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Gladius).
                AddIngredient<Lumenyl>(8).
                AddIngredient<RuinousSoul>(5).
                AddIngredient<ExodiumCluster>(15).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
