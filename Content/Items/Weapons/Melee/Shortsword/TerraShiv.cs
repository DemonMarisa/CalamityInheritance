using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Materials;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class TerraShiv : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee.Shortsword";
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 13;
            Item.width = 42;
            Item.height = 42;
            Item.damage = 105;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<TerraShivProj>();
            Item.shootSpeed = 3f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix() => true;
        public override void AddRecipes()
        {
            CreateRecipe().
            AddIngredient(ModContent.ItemType<TrueNightsStabber>()).
            AddIngredient(ModContent.ItemType<TrueExcaliburShortsword>()).
            AddIngredient(ModContent.ItemType<LivingShard>(),5).
            AddIngredient(ItemID.BrokenHeroSword).
            AddTile(TileID.MythrilAnvil).
            Register();
        }
    }
}
