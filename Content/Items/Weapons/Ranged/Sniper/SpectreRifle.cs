using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity.ShopValue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged.Sniper
{
    public class SpectreRifle : CIRanged
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 170;
            Item.DamageType = DamageClass.Ranged;
            Item.mana = 0;
            Item.width = 88;
            Item.height = 30;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.LostSoulFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = Main.zenithWorld ? AmmoID.None : AmmoID.Bullet;
        }
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 22;
        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileID.LostSoulFriendly, damage, knockback, player.whoAmI, 2f, 0f);
            Main.projectile[proj].DamageType = DamageClass.Ranged;
            Main.projectile[proj].extraUpdates += 2;
            Main.projectile[proj].usesLocalNPCImmunity = true;
            Main.projectile[proj].localNPCHitCooldown = 45;
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SpectreBar, 7)
                .AddIngredient<CoreofEleum>(3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
