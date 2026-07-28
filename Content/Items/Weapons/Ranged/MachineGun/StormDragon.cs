using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;

using LAP.Core.Enums;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.MachineGun
{
    public class StormDragon : CIRanged
    {
        public override void ExSD()
        {
            Item.damage = 96;
            Item.useTime = 2;
            Item.useAnimation = 20;
            Item.reuseDelay = 10;
            Item.useLimitPerAnimation = 10;
            Item.knockBack = 3.25f;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
            Item.UseSound = SoundID.Item31;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Bullet;
            Item.LAP().UseCICalStatInflation = true;
            Item.LAP().WeaponTier = AllWeaponTier.PostSentinelst;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-25, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + Main.rand.Next(-40, 41) * 0.05f;
            float SpeedY = velocity.Y + Main.rand.Next(-40, 41) * 0.05f;
            Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
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
                CreateRecipe().
                    AddIngredient(CalamityMaterials.ArmoredShell, 2).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
        }
    }
}
