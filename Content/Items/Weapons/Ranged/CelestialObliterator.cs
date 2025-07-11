﻿using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Tiles.Furniture.CraftingStations;
using System.Diagnostics.Metrics;
using System;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class CelestialObliterator : CIRanged, ILocalizedModType
    {
        
        private int shot;

        private int burst;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 400;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 120;
            Item.height = 38;
            Item.useTime = 4;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.rare = CIConfig.Instance.SpecialRarityColor?ModContent.RarityType<SeraphPurple>():ModContent.RarityType<CatalystViolet>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Utils.NextFloat(Main.rand) >= 0.9f;
        }
        public override void UseItemFrame(Player player)
        {
            CIFunction.NoHeldProjUpdateAim(player, 0, 1);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();

            float SpeedX = velocity.X + Main.rand.Next(-15, 16) * 0.05f;
            float SpeedY = velocity.Y + Main.rand.Next(-15, 16) * 0.05f;
            int pCounts = 10;
            Projectile.NewProjectile(source, position.X, position.Y - 6, SpeedX, SpeedY, type, damage, knockback, player.whoAmI);
            if (shot >= 8)
            {
                shot = 0;
                burst++;
                for (int i = 0; i < pCounts; i++)
                {
                    Vector2 vector = Utils.RotatedByRandom(new Vector2(SpeedX, SpeedY), (double)MathHelper.ToRadians(2f));
                    vector *= 0.4f + Utils.NextFloat(Main.rand, 10f) / 25f;
                    Projectile.NewProjectile(source, position.X, position.Y - 6, vector.X, vector.Y, type, damage, knockback, player.whoAmI, 0f, 0f);
                }
            }
            else
            {
                shot++;
                SoundEngine.PlaySound(SoundID.Item40);
            }
            if (burst >= 5)
            {
                if(usPlayer.LoreExo || usPlayer.PanelsLoreExo)
                {
                    burst = 0;
                    SoundEngine.PlaySound(SoundID.Item38);
                    Vector2 vector2 = Utils.RotatedBy(Vector2.Normalize(new Vector2(SpeedX, SpeedY)), 60, default) * 9f;
                    Vector2 vector3 = Utils.RotatedBy(Vector2.Normalize(new Vector2(SpeedX, SpeedY)), -60, default) * 9f;
                    Projectile.NewProjectile(source, position.X, position.Y - 6, vector2.X, vector2.Y, ModContent.ProjectileType<ExoGunBlast>(), damage, knockback, player.whoAmI, 1f, 0f);
                    Projectile.NewProjectile(source, position.X, position.Y - 6, vector3.X, vector3.Y, ModContent.ProjectileType<ExoGunBlast>(), damage, knockback, player.whoAmI, 1f, 0f);
                }
                else
                {
                    burst = 0;
                    SoundEngine.PlaySound(SoundID.Item38);
                    Vector2 vector4 = Utils.RotatedBy(Vector2.Normalize(new Vector2(SpeedX, SpeedY)), 45, default) * 9f;
                    Vector2 vector5 = Utils.RotatedBy(Vector2.Normalize(new Vector2(SpeedX, SpeedY)), -45, default) * 9f;
                    Projectile.NewProjectile(source, position.X, position.Y - 6, vector4.X, vector4.Y, ModContent.ProjectileType<ExoGunBlastsplit>(), damage, knockback, player.whoAmI, 1f, 0f);
                    Projectile.NewProjectile(source, position.X, position.Y - 6, vector5.X, vector5.Y, ModContent.ProjectileType<ExoGunBlastsplit>(), damage, knockback, player.whoAmI, 1f, 0f);
                }
            }
            return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>($"{Generic.WeaponPath}/Ranged/CelestialObliteratorGlow").Value);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-35f, -6f);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer usPlayer = player.CIMod();

            if (usPlayer.LoreExo == true || usPlayer.PanelsLoreExo)
            {
                string ExoLoreOn = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Ranged.CelestialObliterator.ExoLoreOn");

                tooltips.Add(new TooltipLine(Mod, "ExoLore", ExoLoreOn));
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Onyxia>().
                AddIngredient<UniversalGenesis>().
                AddIngredient<ElementalBlaster>().
                AddIngredient<PearlGod>().
                DisableDecraft().
                AddIngredient<AuricBarold>(10).
                AddTile<DraedonsForgeold>().
                Register();
                
            CreateRecipe().
                AddIngredient<Onyxia>().
                AddIngredient<UniversalGenesis>().
                AddIngredient<ElementalBlaster>().
                AddIngredient<PearlGod>().
                AddDecraftCondition(CalamityConditions.DownedExoMechs).
                AddIngredient<MiracleMatter>().
                AddTile<DraedonsForge>().
                Register();

            CreateRecipe().
                AddIngredient<Onyxia>().
                AddIngredient<UniversalGenesis>().
                AddIngredient<ElementalBlaster>().
                AddIngredient<PearlGod>().
                AddIngredient<AncientMiracleMatter>().
                DisableDecraft().
                AddConsumeItemCallback(CIRecipesCallback.DConsumeMatter).
                AddTile<DraedonsForge>().
                Register();
        }
    }
}
