using CalamityInheritance.Content.Projectiles.Magic.Staffs;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.Staffs
{
    public class SanguineFlareLegacy : CIMagic, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 60;
            Item.damage = 143;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 22;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 8f;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<SanguineFlareProjLegacy>();
            Item.shootSpeed = 21f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 7; i++)
            {
                Vector2 spreadVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(5f)) * Main.rand.NextFloat(0.8f, 1.2f);
                Projectile.NewProjectile(source, position, spreadVelocity, type, damage, knockback, Main.myPlayer);
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BloodstoneCore>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
