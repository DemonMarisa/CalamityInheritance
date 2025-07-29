using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class LeechingDagger : CIMelee, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Melee.Shortsword";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.BonusAttackSpeedMultiplier[Type] = 0.33f;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 15;
            Item.width = 26;
            Item.height = 26;
            Item.damage = 22;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.shoot = ModContent.ProjectileType<LeechingDaggerProj>();
            Item.shootSpeed = 2;
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
