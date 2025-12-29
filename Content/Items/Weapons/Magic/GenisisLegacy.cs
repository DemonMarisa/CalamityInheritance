using CalamityInheritance.Content.Projectiles.Magic;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class GenisisLegacy : CIMagic, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 74;
            Item.height = 28;
            Item.damage = 70;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.useTime = 4;
            Item.useAnimation = 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = CIShopValue.RarityPricePurple;
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item33;
            Item.autoReuse = true;
            Item.shootSpeed = 6f;
            Item.shoot = ProjectileType<AlphaRayYShape>();
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
            float SpeedX = velocity.X + Main.rand.Next(-20, 21) * 0.05f;
            float SpeedY = velocity.Y + Main.rand.Next(-20, 21) * 0.05f;
            for (int index = 0; index < 3; ++index)
            {
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX * 1.05f, SpeedY * 1.05f, ProjectileID.LaserMachinegunLaser, (int)(damage * 0.65), knockback * 0.6f, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.LaserMachinegun).
                AddIngredient(ItemID.LunarBar, 5).
                AddIngredient<LifeAlloy>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
