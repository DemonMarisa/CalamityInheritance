using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.DraedonsArsenal.Ranged;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal.Ranged
{
    public class HeavyLaserRifle : CIRanged
    {
        public override void SetDefaults()
        {
            Item.width = 84;
            Item.height = 28;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 210;
            Item.knockBack = 4f;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CISounds.LaserRifleFire;
            Item.noMelee = true;

            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = RarityType<BlueGreen>();

            Item.shoot = ProjectileType<LaserRifleShot>();
            Item.shootSpeed = 5f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (velocity.Length() > 5f)
            {
                velocity.Normalize();
                velocity *= 5f;
            }
            for (int i = 0; i < 2; i++)
            {
                float SpeedX = velocity.X + Main.rand.Next(-1, 2) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-1, 2) * 0.05f;
                Projectile.NewProjectile(source, position, new Vector2(SpeedX, SpeedY), ProjectileType<LaserRifleShot>(), damage, knockback, player.whoAmI, i, 0f);
            }
            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 0);
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.MysteriousCircuitry, 15).
                    AddIngredient(CalamityMaterials.DubiousPlating, 15).
                    AddIngredient(CalamityMaterials.UelibloomBar, 8).
                    AddIngredient(ItemID.LunarBar, 4).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {

            }
        }
    }
}
