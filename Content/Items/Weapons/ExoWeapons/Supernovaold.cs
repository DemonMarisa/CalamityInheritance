using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.Blance;
using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Rogue.Throws;
using CalamityInheritance.Content.Projectiles.ExoWeapons.Celestus;
using CalamityInheritance.Content.Projectiles.ExoWeapons.Supernova;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using CalamityInheritance.Core.Path;
using LAP.Core.LAPSource;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoWeapons
{
    public class Supernovaold : CIExoWeapon
    {
        public override string LocalizationCategory => LocalizationPath.ExoWeapon;
        public static readonly SoundStyle ExplosionSound = CISounds.SupernovaBoom with { Volume = 0.8f };
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 36;
            Item.damage = 5200;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 24;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.shoot = ProjectileType<SupernovaBombold>();
            Item.shootSpeed = 19f;
            Item.rare = RarityType<CatalystViolet>();

            Item.LAP().SkillShoot = ProjectileType<SupernovaBombold>();
            Item.LAP().SkillShootSpeed = 27f;
            Item.LAP().UseCustomWeaponSkill = true;
            Item.LAP().WeaponSkillFocusCost = Item.LAP().WeaponSkillRealFocusCost = CIWeaponsBlance.StealthAttackFocusCost;
        }
        public override bool CanUseWeaponSkill(Player player)
        {
            return true;
        }
        public override void WeaponSkill(Player player, EntitySource_ItemUse_WeaponSkill source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile stealth = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            stealth.SetStealthAttack();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<StarofDestructionLegacy>().
                AddIngredient<AuricBarold>(10).
                AddTile<DraedonsForgeoldTile>().
                Register();

            CreateRecipe().
                AddIngredient<StarofDestructionLegacy>().
                AddIngredient<AncientMiracleMatter>().
                AddTile<DraedonsForgeoldTile>().
                Register();
        }
    }
}
