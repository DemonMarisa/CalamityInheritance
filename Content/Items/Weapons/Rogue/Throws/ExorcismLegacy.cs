using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Rogue.Throw;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Throws
{
    public class ExorcismLegacy : CIRogue
    {
        public override void ExSD()
        {
            Item.width = 34;
            Item.height = 42;
            Item.damage = 64;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ProjectileType<ExorcismProjLegacy>();
            Item.shootSpeed = 10f;

            Item.LAP().SkillShoot = ProjectileType<ExorcismProjLegacy>();
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int spear = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[spear].SetStealthAttack();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.HolyWater, 10).
                AddIngredient(ItemID.HallowedBar, 12).
                AddIngredient(ItemID.SoulofFright, 6).
                AddIngredient(ItemID.SoulofMight, 6).
                AddIngredient(ItemID.SoulofSight, 6).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
