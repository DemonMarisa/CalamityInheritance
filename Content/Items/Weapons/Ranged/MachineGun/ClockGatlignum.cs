using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.MachineGun
{
    public class ClockGatlignum : CIRanged
    {
        public override void ExSD()
        {
            Item.damage = 55;
            Item.useTime = 3;
            Item.useAnimation = 9;
            Item.reuseDelay = 12;
            Item.useLimitPerAnimation = 3;
            Item.knockBack = 3.75f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item31;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Bullet;
        }
        public override Vector2? HoldoutOffset() => new Vector2(-14, 0);
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + Main.rand.Next(-15, 16) * 0.05f;
            float SpeedY = velocity.Y + Main.rand.Next(-15, 16) * 0.05f;
            //弹药全转化，先注释保留了
            //if (CIConfig.Instance.AmmoConversion)
            //{
            //    Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ProjectileID.BulletHighVelocity, damage, knockback, player.whoAmI);
            //}
            //else
            {
                if (type == ProjectileID.Bullet)
                    Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ProjectileID.BulletHighVelocity, damage, knockback, player.whoAmI);
                else
                    Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 33)
                return false;
            return true;
        }
        public override void UseItemFrame(Player player)
        {
            CIUtils.NoHeldProjUpdateAim(player, 0, 1);
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(ItemID.ClockworkAssaultRifle).
                    AddIngredient(ItemID.Gatligator).
                    AddIngredient(ItemID.VenusMagnum).
                    AddIngredient(CalamityMaterials.LifeAlloy, 3).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient(ItemID.ClockworkAssaultRifle).
                    AddIngredient(ItemID.Gatligator).
                    AddIngredient(ItemID.VenusMagnum).
                    AddIngredient(ItemID.BeetleHusk, 3).
                    AddTile(TileID.MythrilAnvil).
                    Register();

            }
        }
    }
}
