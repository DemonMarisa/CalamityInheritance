using CalamityInheritance.Content.Projectiles.HeldProj.Magic;
using CalamityMod.CustomRecipes;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal
{
    public class GatlingLaserLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.DraedonsArsenal";

        // This is the amount of charge consumed every time the holdout projectile fires a laser.
        public const float HoldoutChargeUse = 0.0075f;

        public override void SetDefaults()
        {
            Item.width = 43;
            Item.height = 24;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 43;
            Item.knockBack = 1f;
            Item.useTime = 2;
            Item.useAnimation = 2;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.channel = true;
            Item.mana = 4;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = null;
            Item.noMelee = true;

            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;

            Item.shoot = ProjectileType<GatlingLaserHeldProj>();
            Item.shootSpeed = 24f;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override void ModifyTooltips(List<TooltipLine> tooltips) => CalamityGlobalItem.InsertKnowledgeTooltip(tooltips, 3);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-20, 0);

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MysteriousCircuitry>(15).
                AddIngredient<DubiousPlating>(15).
                AddIngredient<InfectedArmorPlating>(10).
                AddIngredient<LifeAlloy>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
