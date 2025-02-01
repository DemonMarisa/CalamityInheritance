using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Sounds;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class TyrannysEndOld : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Ranged";
        public override void SetDefaults()
        {
            Item.width = 150;
            Item.height = 48;
            Item.damage = 2250;
            Item.knockBack = 9.5f;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 55;
            Item.useAnimation = 55;
            Item.shoot = ProjectileID.BulletHighVelocity;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.UseSound = CommonCalamitySounds.LargeWeaponFireSound;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.Calamity().donorItem = true;
            Item.Calamity().canFirePointBlankShots = true;
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 35;

        public override Vector2? HoldoutOffset() => new Vector2(-15, 0);

        public override void HoldItem(Player player) => player.scope = true;

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GoldenEagle>().
                AddIngredient<AntiMaterielRifle>().
                AddIngredient<Vortexpopper>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient<GoldenEagle>().
                AddIngredient<AntiMaterielRifle>().
                AddIngredient<Vortexpopper>().
                AddIngredient<AuricBar>(5).
                AddTile<CosmicAnvil>().
                Register();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            CalamityInheritancePlayer modPlayer = player.CalamityInheritance();

            if (CalamityInheritanceConfig.Instance.AmmoConversion == true)
            {
                type = ModContent.ProjectileType<PiercingBullet>();
            }
            if (CalamityInheritanceConfig.Instance.AmmoConversion == false)
            {
                if (type == ProjectileID.Bullet)
                {
                    type = ModContent.ProjectileType<PiercingBullet>();
                }
                if (type != ModContent.ProjectileType<PiercingBullet>())
                {
                    modPlayer.AMRextraTy = true;
                }
            }
        }
    }
}
