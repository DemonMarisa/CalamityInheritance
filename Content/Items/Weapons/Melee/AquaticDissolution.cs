using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class AquaticDissolution : CIMelee, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 72;
            Item.damage = 175;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.rare = ItemRarityID.Purple;
            Item.value = CIShopValue.RarityPricePurple;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<AquaticBeam>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = Main.MouseWorld;
            for (int i = 0; i < 3; i++)
                Projectile.NewProjectile(source,position.X + Main.rand.Next(-30,31), position.Y - 600f, 0f, 12f, type, damage, knockback, Main.myPlayer, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Mariana>().
                AddIngredient(ItemID.LunarBar, 10).
                AddIngredient<LifeAlloy>(5).
                AddIngredient<Lumenyl>(20).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}