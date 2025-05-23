﻿using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Magic;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class TheWandold : CIMagic, ILocalizedModType
    {
        
        // The actual base damage of The Wand. The damage reported on the item is just the spark, which is irrelevant.
        public static int BaseDamage = 999;

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<TheWand>(false);
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 36;
            Item.damage = 1; // same as 1.4 Wand of Sparking
            Item.mana = 150;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.useAnimation = 250;
            Item.useTime = 250;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0.5f;
            Item.UseSound = SoundID.Item102;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.shoot = ModContent.ProjectileType<SparkInfernalold>();
            Item.shootSpeed = 24f;
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(10, 10);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Torch);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(CIRecipeGroup.WandofSparking).
                AddIngredient<YharonSoulFragment>(8).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
