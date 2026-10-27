using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.Blance;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Rogue.Spear;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.ExoWeapons.Celestus;
using CalamityInheritance.Content.Projectiles.ExoWeapons.ExoTheApostle;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Rarity.Special;
using CalamityInheritance.Content.Tiles.CraftingStations;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoWeapons
{
    public class ExoTheApostle : CIExoWeapon
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 10000;
            Item.width = 92;
            Item.height = 100;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = RarityType<SeraphPurple>();
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.DamageType = RogueDamage.Instance;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<ExoSpearProj>();
            Item.shootSpeed = 16f;

            Item.LAP().SkillShoot = ProjectileType<ExoSpearStealthProj>();
            Item.LAP().SkillShootSpeed = 16f;
            Item.LAP().UseCustomWeaponSkill = true;
            Item.LAP().WeaponSkillFocusCost = Item.LAP().WeaponSkillRealFocusCost = CIWeaponsBlance.StealthAttackFocusCost;
        }
        public override bool CanUseWeaponSkill(Player player)
        {
            return true;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (UseAlt)
            {
                Projectile.NewProjectileDirect(source, position, -velocity * 2.6f, ProjectileType<ExoSpearBack>(), damage, knockback, player.whoAmI);
                int stealth = Projectile.NewProjectile(source, position, velocity, ProjectileType<ExoSpearStealthProj>(), damage, knockback, player.whoAmI);
                Main.projectile[stealth].SetStealthAttack();
                SoundEngine.PlaySound(CISounds.ExoApostleStealth, player.Center);
                SoundEngine.PlaySound(CISounds.RealityRupture, player.Center);
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity * 1.5f, ProjectileType<ExoSpearProjNor>(), damage, knockback, player.whoAmI);
                int stealth = Projectile.NewProjectile(source, position, velocity * 1.5f, ProjectileType<ExoSpearProjNor>(), damage, knockback, player.whoAmI);
                Main.projectile[stealth].SetStealthAttack();
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (UseAlt)
            {
                Projectile.NewProjectileDirect(source, position, velocity * 1.5f, ProjectileType<ExoSpearProj>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position, -velocity * 1.2f, ProjectileType<ExoSpearBack>(), damage, knockback, player.whoAmI);
                SoundEngine.PlaySound(CISounds.RealityRupture, player.Center);
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity * 1.5f, ProjectileType<ExoSpearProjNor>(), damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DragonSpear>().
                AddIngredient<ShardofAntumbraLegacy>().
                AddIngredient<PhantasmalRuinold>().
                AddIngredient<EclipseSpear>().
                AddIngredient<AuricBarold>(10).
                AddTile<DraedonsForgeoldTile>().
                Register();

            CreateRecipe().
                AddIngredient<DragonSpear>().
                AddIngredient<ShardofAntumbraLegacy>().
                AddIngredient<PhantasmalRuinold>().
                AddIngredient<EclipseSpear>().
                AddIngredient<AncientMiracleMatter>().
                AddTile<DraedonsForgeoldTile>().
                Register();
        }
    }
}
