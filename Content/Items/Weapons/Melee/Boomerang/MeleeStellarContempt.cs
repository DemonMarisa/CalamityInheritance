using CalamityMod.Items.Weapons.Melee;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod.Items.Materials;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Boomerang
{
    public class MeleeStellarContempt : CIMelee, ILocalizedModType
    {
        
        public static int BaseDamage = 300;
        public static float Speed = 18f;

        public override void SetStaticDefaults()
        {
            Type.ShimmerEach<StellarContempt>();
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 74;
            Item.height = 74;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = BaseDamage;
            Item.knockBack = 9f;
            Item.useTime = 13;
            Item.useAnimation = 13;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;

            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;

            Item.shoot = ProjectileType<MeleeStellarContemptProj>();
            Item.shootSpeed = Speed;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MeleeFallenHammer>().
                AddIngredient(ItemID.LunarBar, 10).
                AddIngredient<GalacticaSingularity>(10).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
