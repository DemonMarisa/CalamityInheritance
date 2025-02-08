using CalamityMod.Items.Materials;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using System.Collections.Generic;
using Terraria.Localization;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class SpectreRifle : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Ranged";
        public override void SetDefaults()
        {
            Item.damage = 150;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 88;
            Item.height = 30;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = false;
            Item.shoot = ProjectileID.LostSoulFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().canFirePointBlankShots = true;
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 22;

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CalamityInheritanceConfig.Instance.AmmoConversion == true)
            {
                int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileID.LostSoulFriendly, damage, knockback, player.whoAmI, 2f, 0f);
                Main.projectile[proj].CalamityInheritance().forceRanged = true;
                Main.projectile[proj].extraUpdates += 2;
            }
            if (CalamityInheritanceConfig.Instance.AmmoConversion == false)
            {
                if (type == ProjectileID.Bullet)
                {
                    int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileID.LostSoulFriendly, damage, knockback, player.whoAmI, 2f, 0f);
                    Main.projectile[proj].CalamityInheritance().forceRanged = true;
                    Main.projectile[proj].extraUpdates += 2;
                }
                else
                    Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
            }

            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (CalamityInheritanceConfig.Instance.AmmoConversion == true)
            {
                string AmmoConversionOn = Language.GetTextValue("Mods.CalamityInheritance.ConfigsMessage.AmmoConversionCIWeapon");

                tooltips.Add(new TooltipLine(Mod, "AmmoConversionCIWeapon", AmmoConversionOn));
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SpectreBar, 7).
                AddIngredient<CoreofEleum>(3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
