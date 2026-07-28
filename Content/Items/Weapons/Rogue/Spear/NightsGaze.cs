using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Rogue.Spear;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Spear
{
    public class NightsGaze : CIRogue
    {
        public override void ExSD()
        {
            Item.width = 82;
            Item.height = 82;
            Item.damage = 531;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.shoot = ProjectileType<NightsGazeProjectile>();
            Item.shootSpeed = 30f;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
            Item.DamageType = RogueDamage.Instance;

            Item.LAP().SkillShoot = ProjectileType<NightsGazeProjectile>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile p = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            p.SetStealthAttack();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ProfanedPartisanLegacy>().
                AddIngredient(CalamityMaterials.Lumenyl, 7).
                AddIngredient(CalamityMaterials.RuinousSoul, 4).
                AddIngredient(CalamityMaterials.ExodiumCluster, 12).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
