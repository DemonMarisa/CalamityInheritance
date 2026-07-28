using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.Rogue.Spear;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Spear
{
    public class IchorSpearLegacy : CIRogue
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void ExSD()
        {
            Item.width = 52;
            Item.damage = 96;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 52;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.shoot = ProjectileType<IchorSpearProjLegacy>();
            Item.shootSpeed = 20f;

            Item.LAP().SkillShoot = ProjectileType<IchorSpearProjLegacy>();
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int spear = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[spear].SetStealthAttack();
            Main.projectile[spear].extraUpdates += 2;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Ichor, 20).
                AddIngredient(ItemID.Vertebrae, 15).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
