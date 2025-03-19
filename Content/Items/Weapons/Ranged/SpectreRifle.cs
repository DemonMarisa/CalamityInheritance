using CalamityMod.Items.Materials;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class SpectreRifle : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Ranged";
        public override void SetDefaults()
        {
            Item.damage = Main.zenithWorld? 350 : 150;
            Item.DamageType =Main.zenithWorld? DamageClass.Magic: DamageClass.Ranged;
            Item.mana = Main.zenithWorld? 15 : 0;
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
            Item.autoReuse = true;
            Item.shoot = ProjectileID.LostSoulFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = Main.zenithWorld ? AmmoID.None: AmmoID.Bullet;
            Item.Calamity().canFirePointBlankShots = true;
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 22;

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CIConfig.Instance.AmmoConversion == true && !Main.zenithWorld)
            {
                int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileID.LostSoulFriendly, damage, knockback, player.whoAmI, 2f, 0f);
                Main.projectile[proj].DamageType = DamageClass.Ranged;
                Main.projectile[proj].extraUpdates += 2;
            }
            if (CIConfig.Instance.AmmoConversion == false && !Main.zenithWorld)
            {
                if (type == ProjectileID.Bullet)
                {
                    int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileID.LostSoulFriendly, damage, knockback, player.whoAmI, 2f, 0f);
                    Main.projectile[proj].DamageType = DamageClass.Ranged;
                    Main.projectile[proj].extraUpdates += 2;
                    
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
                .AddCondition(Condition.NotZenithWorld)
                .AddDecraftCondition(Condition.NotZenithWorld)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        
            CreateRecipe()
                .AddIngredient(ItemID.SpectreBar, 7)
                .AddIngredient<RuinousSoul>(2)
                .AddIngredient<DarkPlasma>()
                .AddCondition(Condition.ZenithWorld)
                .AddDecraftCondition(Condition.ZenithWorld)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
