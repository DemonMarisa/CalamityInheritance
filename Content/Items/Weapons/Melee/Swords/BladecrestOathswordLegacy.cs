using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.HeldProj.Melee.Swords;
using CalamityInheritance.Content.Rarity.ShopValue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    public class BladecrestOathswordLegacy : CIMelee
    {
        public int Filp = 1;
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.width = 56;
            Item.height = 56;
            Item.damage = 25;
            Item.useAnimation = 50;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 50;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ProjectileType<BladecrestOathswordHeld>();
            Item.shootSpeed = 6f;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.noMelee = true;
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Filp == 1)
            {
                Filp = -1;
            }
            else
            {
                Filp = 1;
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, Filp);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DemonicBoneAsh>(3).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
