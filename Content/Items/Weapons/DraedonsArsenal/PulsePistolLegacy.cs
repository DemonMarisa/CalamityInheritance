using CalamityInheritance.Content.Projectiles.DraedonsArsenal;
using CalamityMod.CustomRecipes;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.SunkenSea;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal
{
    public class PulsePistolLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.DraedonsArsenal";
        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 22;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 27;
            Item.knockBack = 0f;
            Item.useTime = Item.useAnimation = 21;
            Item.autoReuse = true;
            Item.mana = 6;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = PulseRifle.FireSound;
            Item.noMelee = true;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ProjectileType<PulsePistolProj>();
            Item.shootSpeed = 5.2f; // This may seem low but the shot has 10 extra updates.
        }

        public override Vector2? HoldoutOffset() => new Vector2(10f, 0f);
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
