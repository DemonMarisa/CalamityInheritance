using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.Ranged.Explosions;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Flamethrower
{
    public class CleansingBlazeLegacy : CIRanged
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 130;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 64;
            Item.height = 32;
            Item.useTime = 3;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.UseSound = CISoundID.SoundFlamethrower;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<EssenceFireLegacy>();
            Item.shootSpeed = 14f;
            Item.useAmmo = AmmoID.Gel;
            Item.rare = RarityType<DeepBlue>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int n = Main.rand.Next(2, 4);
            for (int index = 0; index < n; ++index)
            {
                float SpeedX = velocity.X + Main.rand.Next(-15, 16) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-15, 16) * 0.05f;
                Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 90)
                return false;
            return true;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe()
                    .AddIngredient(CalamityMaterials.CosmiliteBar, 14)
                    .AddTile(CalamityTile.CosmicAnvilTile)
                    .Register();
            }
            else
            {

            }
        }
    }
}