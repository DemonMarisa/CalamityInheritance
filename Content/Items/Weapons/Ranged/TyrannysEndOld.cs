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
using System.Collections.Generic;
using Terraria.Localization;
using Mono.Cecil;
using Terraria.DataStructures;
using CalamityInheritance.System.Configs;

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
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityInheritancePlayer modPlayer = player.CalamityInheritance();

            if (CIConfig.Instance.AmmoConversion == true)
            {
                type = ModContent.ProjectileType<PiercingBullet>();
                Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer, 0f, 0f);
            }
            if (CIConfig.Instance.AmmoConversion == false)
            {
                if (type == ProjectileID.Bullet)
                {
                    type = ModContent.ProjectileType<PiercingBullet>();
                }
                if (type != ModContent.ProjectileType<PiercingBullet>())
                {
                    Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer, 0f, 0f);
                    proj.CalamityInheritance().AMRextraTy = true;
                }
            }
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (CIConfig.Instance.AmmoConversion == true)
            {
                string AmmoConversionOn = Language.GetTextValue("Mods.CalamityInheritance.ConfigsMessage.AmmoConversionCIWeapon");

                tooltips.Add(new TooltipLine(Mod, "AmmoConversionCIWeapon", AmmoConversionOn));
            }
        }
    }
}
