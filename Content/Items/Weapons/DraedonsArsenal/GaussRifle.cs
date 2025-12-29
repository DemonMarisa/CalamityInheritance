using CalamityInheritance.Content.Projectiles.DraedonsArsenal;
using CalamityInheritance.Sounds.Custom;
using CalamityMod.CustomRecipes;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal
{
    public class GaussRifle : ModItem,ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Weapons.DraedonsArsenal";
        public override void SetDefaults()
        {
            Item.width = 112;
            Item.height = 36;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 150;
            Item.knockBack = 30f;
            Item.useTime = Item.useAnimation = 32;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CISoundMenu.GaussRifleFired;
            Item.noMelee = true;

            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;

            Item.shoot = ProjectileType<GaussRifleBlast>();
            Item.shootSpeed = 27f;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips) => CalamityGlobalItem.InsertKnowledgeTooltip(tooltips, 3);
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MysteriousCircuitry>(18).
                AddIngredient<DubiousPlating>(12).
                AddIngredient<InfectedArmorPlating>(10).
                AddIngredient<LifeAlloy>(5).
                AddCondition(ArsenalTierGatedRecipe.ConstructRecipeCondition(3, out Func<bool> condition), condition).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
