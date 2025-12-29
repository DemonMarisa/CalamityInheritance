using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Rogue.Boomerang;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Boomerang
{
    public class VictideBoomerangMelee: CIMelee, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<VictideBoomerangRogue>(false);
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.damage = 15;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.knockBack = 5.5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.height = 34;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.shoot = ProjectileType<MeleeVictideBoomerangProj>();
            Item.shootSpeed = 11.5f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AncientVictideBar>(5).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
