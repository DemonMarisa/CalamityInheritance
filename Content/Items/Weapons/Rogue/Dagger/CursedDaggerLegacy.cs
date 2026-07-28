using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Rogue.Dagger;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Dagger
{
    public class CursedDaggerLegacy : CIRogue
    {
        //会被用于射弹内
        public const float ShootSpeed = 12f;
        public override void ExSD()
        {
            Item.width = 34;
            Item.damage = 34;
            Item.DamageType = RogueDamage.Instance;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 16;
            Item.knockBack = 4.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 34;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ProjectileType<CursedDaggerProjLegacy>();
            Item.shootSpeed = ShootSpeed;

            Item.LAP().SkillShoot = ProjectileType<CursedDaggerProjLegacy>();
            Item.LAP().SkillShootSpeed = 12f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo src, Vector2 pos, Vector2 vel, int type, int dmg, float kb)
        {
            Projectile.NewProjectile(src, pos, vel, type, dmg, kb, player.whoAmI);
            return false;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile realProj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            realProj.penetrate = -1;
            realProj.SetStealthAttack();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.CursedFlame, 20).
                AddIngredient(ItemID.RottenChunk, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}