using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Rogue.Throw;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityMod.UI;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Throws
{
    public class OricGemstone : CIRogue
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }
        public override void ExSD()
        {
            Item.width = 12;
            Item.height = 32;
            Item.damage = 28;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = CIShopValue.RarityPricePink;
            Item.shoot = ProjectileType<OricGemstoneProj>();
            Item.shootSpeed = 16f;

            Item.LAP().SkillShoot = ProjectileType<OricGemstoneProj>();
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, 1f);
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int stealth;
            if (player.altFunctionUse == 2)
            {
                stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0, 1f);
            }
            else
            {
                stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            }
            Projectile proj = Main.projectile[stealth];
            proj.timeLeft = 480;
            proj.penetrate = -1;
            proj.SetStealthAttack();
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).
                AddIngredient(ItemID.OrichalcumBar, 12).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
