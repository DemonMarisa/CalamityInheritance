﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;

namespace CalamityInheritance.Content.Items.Tools
{
    public class WulfrumAxe : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Tools";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Melee;
            Item.width = 62;
            Item.height = 48;
            Item.useTime = 8;
            Item.useAnimation = 16;
            Item.useTurn = true;
            Item.axe = 7;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<WulfrumMetalScrap>(4)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}