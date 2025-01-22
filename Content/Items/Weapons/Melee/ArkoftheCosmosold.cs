using CalamityMod.Items.Materials;
using CalamityMod.Buffs.Potions;
using CalamityMod.CalPlayer;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class ArkoftheCosmosold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 102;
            Item.damage = 140;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = false;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 9.5f;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.height = 102;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.shoot = ModContent.ProjectileType<EonBeam>();
            Item.shootSpeed = 14f;
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 15;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            switch (Main.rand.Next(4))
            {
                case 0:
                    type = ModContent.ProjectileType<EonBeam>();
                    break;
                case 1:
                    type = ModContent.ProjectileType<EonBeamV2>();
                    break;
                case 2:
                    type = ModContent.ProjectileType<EonBeamV3>();
                    break;
                case 3:
                    type = ModContent.ProjectileType<EonBeamV4>();
                    break;
            }
            int projectile = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer);
            Main.projectile[projectile].timeLeft = 160;
            Main.projectile[projectile].tileCollide = false;
            float num72 = Main.rand.Next(22, 30);
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            float num78 = Main.mouseX + Main.screenPosition.X + vector2.X;
            float num79 = Main.mouseY + Main.screenPosition.Y + vector2.Y;
            if (player.gravDir == -1f)
            {
                num79 = Main.screenPosition.Y + Main.screenHeight + Main.mouseY + vector2.Y;
            }
            float num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
            if (float.IsNaN(num78) && float.IsNaN(num79) || num78 == 0f && num79 == 0f)
            {
                num78 = player.direction;
                num79 = 0f;
                num80 = num72;
            }
            else
            {
                num80 = num72 / num80;
            }

            int num107 = 4;
            for (int num108 = 0; num108 < num107; num108++)
            {
                vector2 = new Vector2(player.position.X + player.width * 0.5f + (float)-(float)player.direction + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y);
                vector2.X = (vector2.X + player.Center.X) / 2f;
                vector2.Y -= 100 * num108;
                num78 = Main.mouseX + Main.screenPosition.X - vector2.X;
                num79 = Main.mouseY + Main.screenPosition.Y - vector2.Y;
                num80 = (float)Math.Sqrt((double)(num78 * num78 + num79 * num79));
                num80 = num72 / num80;
                num78 *= num80;
                num79 *= num80;
                float speedX4 = num78 + Main.rand.Next(-360, 361) * 0.02f;
                float speedY5 = num79 + Main.rand.Next(-360, 361) * 0.02f;
                int projectileFire = Projectile.NewProjectile(source, vector2, new Vector2(speedX4, speedY5), ModContent.ProjectileType<Galaxia2>(), damage, knockback, player.whoAmI, 0f, Main.rand.Next(3));
                Main.projectile[projectileFire].timeLeft = 160;
            }
            return false;
        }

        

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int num250 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height,DustID.RainbowTorch, player.direction * 2, 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.3f);
                Main.dust[num250].velocity *= 0.2f;
                Main.dust[num250].noGravity = true;
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            CalamityPlayer modPlayer = player.Calamity();
            bool astral = modPlayer.ZoneAstral;
            bool jungle = player.ZoneJungle;
            bool snow = player.ZoneSnow;
            bool beach = player.ZoneBeach;
            bool corrupt = player.ZoneCorrupt;
            bool crimson = player.ZoneCrimson;
            bool dungeon = player.ZoneDungeon;
            bool desert = player.ZoneDesert;
            bool glow = player.ZoneGlowshroom;
            bool hell = player.ZoneUnderworldHeight;
            bool holy = player.ZoneHallow;
            bool nebula = player.ZoneTowerNebula;
            bool stardust = player.ZoneTowerStardust;
            bool solar = player.ZoneTowerSolar;
            bool vortex = player.ZoneTowerVortex;
            bool bloodMoon = Main.bloodMoon;
            bool snowMoon = Main.snowMoon;
            bool pumpkinMoon = Main.pumpkinMoon;
            if (bloodMoon)
            {
                player.AddBuff(BuffID.Battle, 600);
            }
            if (snowMoon)
            {
                player.AddBuff(BuffID.RapidHealing, 600);
            }
            if (pumpkinMoon)
            {
                player.AddBuff(BuffID.WellFed, 600);
            }
            if (astral)
            {
                player.AddBuff(ModContent.BuffType<GravityNormalizerBuff>(), 600);
            }
            else if (jungle)
            {
                player.AddBuff(BuffID.Thorns, 600);
            }
            else if (snow)
            {
                player.AddBuff(BuffID.Warmth, 600);
            }
            else if (beach)
            {
                player.AddBuff(BuffID.Wet, 600);
            }
            else if (corrupt)
            {
                player.AddBuff(BuffID.Wrath, 600);
            }
            else if (crimson)
            {
                player.AddBuff(BuffID.Rage, 600);
            }
            else if (dungeon)
            {
                player.AddBuff(BuffID.Dangersense, 600);
            }
            else if (desert)
            {
                player.AddBuff(BuffID.Endurance, 600);
            }
            else if (glow)
            {
                player.AddBuff(BuffID.Spelunker, 600);
            }
            else if (hell)
            {
                player.AddBuff(BuffID.Inferno, 600);
            }
            else if (holy)
            {
                player.AddBuff(BuffID.Heartreach, 600);
            }
            else if (nebula)
            {
                player.AddBuff(BuffID.MagicPower, 600);
            }
            else if (stardust)
            {
                player.AddBuff(BuffID.Summoning, 600);
            }
            else if (solar)
            {
                player.AddBuff(BuffID.Titan, 600);
            }
            else if (vortex)
            {
                player.AddBuff(BuffID.AmmoReservation, 600);
            }
            else
            {
                player.AddBuff(BuffID.DryadsWard, 600);
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            bool jungle = player.ZoneJungle;
            bool snow = player.ZoneSnow;
            bool beach = player.ZoneBeach;
            bool corrupt = player.ZoneCorrupt;
            bool crimson = player.ZoneCrimson;
            bool dungeon = player.ZoneDungeon;
            bool desert = player.ZoneDesert;
            bool glow = player.ZoneGlowshroom;
            bool hell = player.ZoneUnderworldHeight;
            bool holy = player.ZoneHallow;
            bool nebula = player.ZoneTowerNebula;
            bool stardust = player.ZoneTowerStardust;
            bool solar = player.ZoneTowerSolar;
            bool vortex = player.ZoneTowerVortex;
            bool bloodMoon = Main.bloodMoon;
            bool snowMoon = Main.snowMoon;
            bool pumpkinMoon = Main.pumpkinMoon;
            if (bloodMoon)
            {
                player.AddBuff(BuffID.Battle, 600);
            }
            if (snowMoon)
            {
                player.AddBuff(BuffID.RapidHealing, 600);
            }
            if (pumpkinMoon)
            {
                player.AddBuff(BuffID.WellFed, 600);
            }
            if (jungle)
            {
                player.AddBuff(BuffID.Thorns, 600);
            }
            else if (snow)
            {
                player.AddBuff(BuffID.Warmth, 600);
            }
            else if (beach)
            {
                player.AddBuff(BuffID.Wet, 600);
            }
            else if (corrupt)
            {
                player.AddBuff(BuffID.Wrath, 600);
            }
            else if (crimson)
            {
                player.AddBuff(BuffID.Rage, 600);
            }
            else if (dungeon)
            {
                player.AddBuff(BuffID.Dangersense, 600);
            }
            else if (desert)
            {
                player.AddBuff(BuffID.Endurance, 600);
            }
            else if (glow)
            {
                player.AddBuff(BuffID.Spelunker, 600);
            }
            else if (hell)
            {
                player.AddBuff(BuffID.Inferno, 600);
            }
            else if (holy)
            {
                player.AddBuff(BuffID.Heartreach, 600);
            }
            else if (nebula)
            {
                player.AddBuff(BuffID.MagicPower, 600);
            }
            else if (stardust)
            {
                player.AddBuff(BuffID.Summoning, 600);
            }
            else if (solar)
            {
                player.AddBuff(BuffID.Titan, 600);
            }
            else if (vortex)
            {
                player.AddBuff(BuffID.AmmoReservation, 600);
            }
            else
            {
                player.AddBuff(BuffID.DryadsWard, 600);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FourSeasonsGalaxiaold>().
                AddIngredient<ArkoftheElementsold>().
                AddIngredient<AuricBarold>().
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient<FourSeasonsGalaxiaold>().
                AddIngredient<ArkoftheElementsold>().
                AddIngredient<AuricBar>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
