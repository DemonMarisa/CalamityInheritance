using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Melee.Yoyos;
using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Melee.Yoyos;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Yoyos
{
    public class TheOracleLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;
        public const int AuraBaseDamage = 68;
        public const int AuraMaxDamage = 150;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 50;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = 205;
            Item.knockBack = 4f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ProjectileType<OracleYoyoLegacy>();
            Item.shootSpeed = 16f;

            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = RarityType<CatalystViolet>();
            Item.Calamity().donorItem = true;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BurningRevelationLegacy>().
                AddIngredient<LaceratorLegacy>().
                AddIngredient<Verdant>().
                AddIngredient(ItemID.Terrarian).
                AddIngredient<TheObliteratorLegacy>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient<BurningRevelationLegacy>().
                AddIngredient<LaceratorLegacy>().
                AddIngredient<Verdant>().
                AddIngredient(ItemID.Terrarian).
                AddIngredient<TheObliteratorLegacy>().
                AddIngredient<AuricBar>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
