using CalamityInheritance.Content.Projectiles.Melee.Shortsword;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Shortsword
{
    public class TerraShiv : CIMelee, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.Melee.Shortsword";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = Item.useTime = 13;
            Item.width = 42;
            Item.height = 42;
            Item.damage = Main.zenithWorld? 280 : 140;
            Item.scale = Main.zenithWorld ? 3f : 1f;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 6f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ProjectileType<TerraShivProj>();
            Item.shootSpeed = 3f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override bool MeleePrefix() => true;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemType<TrueNightsStabber>()).
                AddIngredient(ItemType<TrueExcaliburShortsword>()).
                AddIngredient(ItemType<LivingShard>(),5).
                AddIngredient(ItemID.BrokenHeroSword).
                AddCondition(Condition.NotZenithWorld).
                AddDecraftCondition(Condition.NotZenithWorld).
                AddTile(TileID.MythrilAnvil).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.PiercingStarlight, 1).
                AddIngredient<LivingShard>(5).
                AddCondition(Condition.NotZenithWorld).
                DisableDecraft().
                AddTile(TileID.MythrilAnvil).
                Register();
            
            CreateRecipe().
                AddIngredient<Floodtide>().
                AddIngredient<Hellkite>().
                AddIngredient(ItemID.TerraBlade).
                AddIngredient<UelibloomBar>(7).
                AddCondition(Condition.ZenithWorld).
                AddDecraftCondition(Condition.ZenithWorld).
                AddTile(TileID.LunarCraftingStation).
                Register();

            CreateRecipe().
                AddIngredient<Floodtide>().
                AddIngredient<Hellkite>().
                AddIngredient<TerraEdge>().
                AddIngredient<UelibloomBar>(7).
                DisableDecraft().
                AddCondition(Condition.ZenithWorld).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
