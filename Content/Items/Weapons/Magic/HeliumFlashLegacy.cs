﻿using System;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic
{
    public class HeliumFlashLegacy : CIMagic, ILocalizedModType
    {
        
        internal const float ExplosionDamageMultiplier = 0.65f;
        public static readonly int HeliumFlashDamage = 600;
        public static readonly int HeliumFlashManaCost = 15;

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 112;
            Item.height = 112;
            Item.DamageType = DamageClass.Magic;
            Item.SetWeaponValues(HeliumFlashDamage, 9.5f, 46);
            Item.mana = HeliumFlashManaCost;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item73;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.shoot = ModContent.ProjectileType<VolatileStarcoreLegacy>();
            Item.shootSpeed = 21f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 dir = velocity;
            double angle = Math.Atan2(velocity.Y, velocity.X) + MathHelper.PiOver4;
            dir = dir.SafeNormalize(Vector2.Zero);
            dir *= 80f * (float)Math.Sqrt(2);
            Vector2 dustPos = position + dir;

            int dustType = CIDustID.DustHeatRay;
            int dustCount = 72;
            float minSpeed = 4f;
            float maxSpeed = 11f;
            float minScale = 0.8f;
            float maxScale = 1.4f;
            Vector2 leftVec = new Vector2(-1f, 0f).RotatedBy(angle);
            Vector2 rightVec = new Vector2(1f, 0f).RotatedBy(angle);
            Vector2 upVec = new Vector2(0f, -1f).RotatedBy(angle);
            Vector2 downVec = new Vector2(0f, 1f).RotatedBy(angle);
            for (int i = 0; i < dustCount; i += 4)
            {
                int left = Dust.NewDust(dustPos, 1, 1, dustType, 0f, 0f);
                Main.dust[left].position = dustPos;
                Main.dust[left].velocity = leftVec * Main.rand.NextFloat(minSpeed, maxSpeed);
                Main.dust[left].scale = Main.rand.NextFloat(minScale, maxScale);
                Main.dust[left].noGravity = true;

                int right = Dust.NewDust(dustPos, 1, 1, dustType, 0f, 0f);
                Main.dust[right].position = dustPos;
                Main.dust[right].velocity = rightVec * Main.rand.NextFloat(minSpeed, maxSpeed);
                Main.dust[right].scale = Main.rand.NextFloat(minScale, maxScale);
                Main.dust[right].noGravity = true;

                int up = Dust.NewDust(dustPos, 1, 1, dustType, 0f, 0f);
                Main.dust[up].position = dustPos;
                Main.dust[up].velocity = upVec * Main.rand.NextFloat(minSpeed, maxSpeed);
                Main.dust[up].scale = Main.rand.NextFloat(minScale, maxScale);
                Main.dust[up].noGravity = true;

                int down = Dust.NewDust(dustPos, 1, 1, dustType, 0f, 0f);
                Main.dust[down].position = dustPos;
                Main.dust[down].velocity = downVec * Main.rand.NextFloat(minSpeed, maxSpeed);
                Main.dust[down].scale = Main.rand.NextFloat(minScale, maxScale);
                Main.dust[down].noGravity = true;
            }
            return true;
        }


        public override void AddRecipes()
        {
            //将氦闪的时期转移至龙前
            CreateRecipe().
                AddIngredient<VenusianTrident>().
                AddIngredient<LashesofChaos>().
                AddIngredient<ForbiddenSun>().
                AddIngredient(ItemID.FragmentSolar, 20).
                AddIngredient(ItemID.FragmentNebula, 5).
                AddIngredient<CosmiliteBar>(12).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
