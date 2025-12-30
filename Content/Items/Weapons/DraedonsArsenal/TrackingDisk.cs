using CalamityInheritance.Content.Projectiles.DraedonsArsenal;
using CalamityMod;
using CalamityMod.CustomRecipes;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.SunkenSea;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal
{
    public class TrackingDisk : CIRogueClass, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.DraedonsArsenal";
        public override void ExSD()
        {

            Item.width = 30;
            Item.height = 34;
            Item.damage = 19;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = false;
            Item.knockBack = 3f;

            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;

            Item.noUseGraphic = true;

            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ProjectileType<TrackingDiskProj>();
            Item.shootSpeed = 8f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (proj.WithinBounds(Main.maxProjectiles))
                Main.projectile[proj].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) => CalamityGlobalItem.InsertKnowledgeTooltip(tooltips, 1);

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MysteriousCircuitry>(5).
                AddIngredient<DubiousPlating>(7).
                AddIngredient<AerialiteBar>(4).
                AddIngredient<SeaPrism>(7).
                AddCondition(ArsenalTierGatedRecipe.ConstructRecipeCondition(1, out Func<bool> condition), condition).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
