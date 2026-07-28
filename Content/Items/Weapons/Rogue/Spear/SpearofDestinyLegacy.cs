using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Misc;
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
    public class SpearofDestinyLegacy : CIRogue
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void ExSD()
        {
            Item.width = 52;
            Item.damage = 42;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.knockBack = 2f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 52;
            Item.rare = RarityType<MaliceChallengeDrop>();
            Item.shoot = ProjectileType<SpearofDestinyProjLegacy>();
            Item.shootSpeed = 20f;
            Item.value = CIShopValue.RarityMaliceDrop;

            Item.LAP().SkillShoot = ProjectileType<SpearofDestinyProjLegacy>();
            Item.LAP().SkillShootSpeed = 20f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int index = 7;
            for (int i = -index; i <= index; i += index)
            {
                int projType = i != 0 ? type : ProjectileType<IchorSpearProjLegacy>();
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(i));
                 Projectile.NewProjectile(source, position, perturbedSpeed, projType, damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int index = 7;
            for (int i = -2 * index; i <= 2 * index; i += index)
            {
                int projType = i != 0 ? type : ProjectileType<IchorSpearProjLegacy>();
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(i));
                int spear = Projectile.NewProjectile(source, position, perturbedSpeed, projType, damage, knockback, player.whoAmI);
                Main.projectile[spear].SetStealthAttack();
                Main.projectile[spear].extraUpdates += 2;
            }
        }
    }
}
