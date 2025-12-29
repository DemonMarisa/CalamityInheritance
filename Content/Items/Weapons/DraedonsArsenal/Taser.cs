using CalamityInheritance.Content.Projectiles.DraedonsArsenal;
using CalamityMod.CustomRecipes;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.SunkenSea;
using CalamityMod.Sounds;
using LAP.Core.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal
{
    public class Taser : ModItem,ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.DraedonsArsenal";
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 26;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 20;
            Item.knockBack = 0f;
            Item.useTime = Item.useAnimation = 28;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CommonCalamitySounds.PlasmaBoltSound;
            Item.noMelee = true;

            Item.value = CIShopValue.RarityPriceOrange;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ProjectileType<TaserProj>();
            Item.shootSpeed = 25f;
        }

        public override bool CanUseItem(Player player) => !player.HasProj(Item.shoot);

        public override void ModifyTooltips(List<TooltipLine> tooltips) => CalamityGlobalItem.InsertKnowledgeTooltip(tooltips, 1);

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MysteriousCircuitry>(7).
                AddIngredient<DubiousPlating>(5).
                AddIngredient<AerialiteBar>(4).
                AddIngredient<SeaPrism>(7).
                AddCondition(ArsenalTierGatedRecipe.ConstructRecipeCondition(1, out Func<bool> condition), condition).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
