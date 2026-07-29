using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Rogue.Throw;
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
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Throws
{
    public class SearedPan : CIRogue
    {
        // Attacks must be within 40 frames of each other to count as "consecutive" hits
        // This is a little less than double the use time
        public static int ConsecutiveHitOpening = 40;
        public static int searedPanCounter;
        public override void ExSD()
        {
            Item.width = 60;
            Item.height = 36;
            Item.damage = 2222;
            Item.DamageType = RogueDamage.Instance;
            Item.knockBack = 10f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 25;
            Item.reuseDelay = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = RarityType<CatalystViolet>();
            Item.shoot = ProjectileType<SearedPanProjectile>();
            Item.shootSpeed = 15f;

            Item.LAP().SkillShoot = ProjectileType<SearedPanProjectile>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float mode = 1f;
            if (searedPanCounter >= 3)
            {
                searedPanCounter = 0;
                mode = 2f;
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, mode);
            return false;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            searedPanCounter = 0;
            int pan = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 3f);
            Main.projectile[pan].SetStealthAttack();
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.AuricBar, 5).
                    AddIngredient(ItemID.LifeCrystal).
                    AddIngredient(ItemID.Bone, 92).
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
            else
            {

            }
        }
    }
}
