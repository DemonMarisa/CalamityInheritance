using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Rogue.Effect;
using CalamityInheritance.Content.Projectiles.Rogue.Spear;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Spear
{
    public class EclipseSpear : CIRogue
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void ExSD()
        {
            Item.width = 82;
            Item.height = 88;
            Item.damage = 350;
            Item.knockBack = 3.5f;
            Item.useAnimation = Item.useTime = 22;
            Item.autoReuse = true;
            Item.DamageType = RogueDamage.Instance;
            Item.shootSpeed = 16f;
            Item.shoot = ProjectileType<EclipseSpearProj>();

            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = RarityType<DeepBlue>();

            Item.LAP().SkillShoot = ProjectileType<EclipseSpearProjStealth>();
            Item.LAP().SkillShootSpeed = 16f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(CISounds.EclipseSpearAttackNor, player.Center);
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, -velocity, ProjectileType<EclipseSpearBack>(), damage, knockback, player.whoAmI);
            return false;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(CISounds.EclipseSpearAttackStealth, player.Center);
            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[p].SetStealthAttack();
            Projectile.NewProjectile(source, position, -velocity * 1.5f, ProjectileType<EclipseSpearBack>(), damage, knockback, player.whoAmI);
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient<NightsGaze>().
                    AddIngredient<CoreofSunlight>(12).
                    AddIngredient(CalamityMaterials.CosmiliteBar, 8).
                    AddIngredient(CalamityMaterials.DarksunFragment, 8).
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
            else
            {

            }
        }
    }
}