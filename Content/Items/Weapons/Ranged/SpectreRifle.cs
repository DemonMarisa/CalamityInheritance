using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.System.Configs;
using CalamityMod;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class SpectreRifle : CIRanged, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 170;
            Item.DamageType = DamageClass.Ranged;
            Item.mana = 0;
            Item.width = 88;
            Item.height = 30;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.LostSoulFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = Main.zenithWorld ? AmmoID.None: AmmoID.Bullet;
        }
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 22;
        public override bool CanUseItem(Player player)
        {
            if (Main.zenithWorld)
            {
                Item.mana = 15;
                Item.damage = 350;
                Item.DamageType = DamageClass.Magic;
                Item.useAmmo = AmmoID.None;
                return true;
            }
            else
            {
                Item.mana = 0;
                Item.damage = 170;
                Item.DamageType = DamageClass.Ranged;
                Item.useAmmo = AmmoID.Bullet;
                return true;
            }
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CIConfig.Instance.AmmoConversion == true && !Main.zenithWorld)
            {
                int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileID.LostSoulFriendly, damage, knockback, player.whoAmI, 2f, 0f);
                Main.projectile[proj].DamageType = DamageClass.Ranged;
                Main.projectile[proj].extraUpdates += 2;
                Main.projectile[proj].usesLocalNPCImmunity = true;
                Main.projectile[proj].localNPCHitCooldown = 12;
            }
            if (CIConfig.Instance.AmmoConversion == false && !Main.zenithWorld)
            {
                if (type == ProjectileID.Bullet)
                {
                    int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileID.LostSoulFriendly, damage, knockback, player.whoAmI, 2f, 0f);
                    Main.projectile[proj].DamageType = DamageClass.Ranged;
                    Main.projectile[proj].extraUpdates += 2;
                    Main.projectile[proj].usesLocalNPCImmunity = true;
                    Main.projectile[proj].localNPCHitCooldown = 12;
                }
                else
                    Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
            }
            if (Main.zenithWorld)
            {
                int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileID.LostSoulFriendly, damage, knockback, player.whoAmI, 2f, 0f);
                Main.projectile[proj].DamageType = DamageClass.Magic;
                Main.projectile[proj].extraUpdates += 2;
                Main.projectile[proj].timeLeft = 300;
                Main.projectile[proj].usesLocalNPCImmunity = true;
                Main.projectile[proj].localNPCHitCooldown = 12;
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
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SpectreBar, 7)
                .AddIngredient<CoreofEleum>(3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
                }
    }
}
