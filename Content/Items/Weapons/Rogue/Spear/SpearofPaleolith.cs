using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Rogue.Spear;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Content.RecipeGroupAdd;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Spear
{
    public class SpearofPaleolith : CIRogue
    {
        public override void ExSD()
        {
            Item.width = 54;
            Item.height = 54;
            Item.damage = 65;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 27;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<SpearofPaleolithProj>();
            Item.shootSpeed = 35f;
            Item.DamageType = RogueDamage.Instance;

            Item.LAP().SkillShoot = ProjectileType<SpearofPaleolithProj>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int stabDevice = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[stabDevice].SetStealthAttack();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.AncientBattleArmorMaterial, 2).
                AddRecipeGroup(LAPRecipeGroup.AnyAdamantiteBar, 4).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
