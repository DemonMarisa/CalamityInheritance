using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class PhantasmalRuinold : CIRogueClass
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 655;
            Item.knockBack = 8f;
            Item.width = 102;
            Item.height = 98;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.autoReuse = true;
            Item.shootSpeed = 14.5f;
            Item.shoot = ProjectileType<PhantasmalRuinProjold>();
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
            Item.DamageType = GetInstance<RogueDamageClass>();
        }
        public override float StealthDamageMultiplier => 0.9f;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<LumiStriker>().
                AddIngredient<PhantomLance>().
                AddIngredient<RuinousSoul>(4).
                AddIngredient<Necroplasm> (20).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
