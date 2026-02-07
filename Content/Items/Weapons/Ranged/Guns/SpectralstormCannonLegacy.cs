using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Rogue;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Guns
{
    public class SpectralstormCannonLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Ranged;
        public override void SetDefaults()
        {
            Item.width = 66;
            Item.height = 26;
            Item.damage = 48;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 4;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Flare;
            Item.shootSpeed = 9.5f;
            Item.useAmmo = AmmoID.Flare;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 70)
                return false;
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float SpeedX = velocity.X + (float)Main.rand.Next(-40, 41) * 0.05f;
            float SpeedY = velocity.Y + (float)Main.rand.Next(-40, 41) * 0.05f;
            int flare = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ProjectileID.BlueFlare, damage, knockback, player.whoAmI);
            if (flare.WithinBounds(Main.maxProjectiles))
            {
                Main.projectile[flare].timeLeft = 200;
                Main.projectile[flare].DamageType = DamageClass.Ranged;
            }

            float SpeedX2 = velocity.X + (float)Main.rand.Next(-20, 21) * 0.05f;
            float SpeedY2 = velocity.Y + (float)Main.rand.Next(-20, 21) * 0.05f;
            int soul = Projectile.NewProjectile(source, position.X, position.Y, SpeedX2, SpeedY2, ProjectileType<LostSoulFriendlyLegacy>(), damage, knockback, player.whoAmI, 2f, 0f);
            if (soul.WithinBounds(Main.maxProjectiles))
            {
                Main.projectile[soul].timeLeft = 600;
                Main.projectile[soul].DamageType = DamageClass.Ranged;
                Main.projectile[soul].frame = Main.rand.Next(4);
            }
            return false;
        }
        public override void UseItemFrame(Player player)
        {
            LAPUtilities.UpdateWeaponAim(player, 0, 1f, true, true);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FirestormCannonLegacy>().
                AddIngredient(ItemID.FragmentSolar, 6).
                AddIngredient(ItemID.Ectoplasm, 10).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
