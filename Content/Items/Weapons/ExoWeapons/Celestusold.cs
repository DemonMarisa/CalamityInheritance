using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.Blance;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.RogueMelee;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.ExoWeapons.Celestus;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using CalamityInheritance.Core.Path;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoWeapons
{
    public class Celestusold : CIExoWeapon
    {
        public override string LocalizationCategory => LocalizationPath.ExoWeapon;
        public static SoundStyle[] getSound =
            [
                CISounds.CelestusShoot1,
                CISounds.CelestusShoot2,
                CISounds.CelestusShoot3
            ];
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 600;
            Item.knockBack = 6f;
            Item.useAnimation = Item.useTime = 19;
            Item.DamageType = RogueDamage.Instance;
            Item.autoReuse = true;
            Item.shootSpeed = 27f;
            Item.shoot = ProjectileType<CelestusBoomerang>();

            Item.width = 106;
            Item.height = 94;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = RarityType<CatalystViolet>();

            Item.LAP().SkillShoot = ProjectileType<CelestusBoomerang>();
            Item.LAP().SkillShootSpeed = 27f;
            Item.LAP().UseCustomWeaponSkill = true;
            Item.LAP().WeaponSkillFocusCost = Item.LAP().WeaponSkillRealFocusCost = CIWeaponsBlance.StealthAttackFocusCost;

        }
        public override bool CanUseWeaponSkill(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (UseAlt)
            {
                Item.UseSound = Utils.SelectRandom(Main.rand, getSound);
            }
            else Item.UseSound = CISoundID.SoundWeaponSwing;
            return true;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile stealth = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            stealth.SetStealthAttack();
            if (UseAlt)
            {
                if (stealth.ModProjectile is CelestusBoomerang boomer2)
                    boomer2.exoLore = true;

                int fireOffset = -800;
                Vector2 mousePos = Main.MouseWorld;
                int firePosX = (int)(mousePos.X + player.Center.X) / 2;
                int firePosY = (int)player.Center.Y + fireOffset;
                Vector2 firePos = new(firePosX + Main.rand.Next(-100, 100), firePosY);
                Vector2 direction = mousePos - firePos;
                direction.Normalize();
                Vector2 newVelocity = direction * velocity.Length();
                Projectile p = Projectile.NewProjectileDirect(source, firePos, newVelocity, type, (int)(damage * 0.8f), knockback * 0.5f, player.whoAmI);
                p.SetStealthAttack();

                if (p.ModProjectile is CelestusBoomerang boomer)
                    boomer.exoLore = true;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (UseAlt)
            {
                int fireOffset = -800;
                Vector2 mousePos = Main.MouseWorld;
                int firePosX = (int)(mousePos.X + player.Center.X) / 2;
                int firePosY = (int)player.Center.Y + fireOffset;
                Vector2 firePos = new(firePosX + Main.rand.Next(-100, 100), firePosY);
                Vector2 direction = mousePos - firePos;
                direction.Normalize();
                Vector2 newVelocity = direction * velocity.Length();
                Projectile p = Projectile.NewProjectileDirect(source, firePos, newVelocity, type, (int)(damage * 0.8f), knockback * 0.5f, player.whoAmI);
                Projectile p2 = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            }
            else
            {
                Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SubductionSlicer>().
                AddIngredient<AuricBarold>(10).
                AddTile<DraedonsForgeoldTile>().
                Register();

            CreateRecipe().
                AddIngredient<SubductionSlicer>().
                AddIngredient<AncientMiracleMatter>().
                AddTile<DraedonsForgeoldTile>().
                Register();
        }
    }
}
