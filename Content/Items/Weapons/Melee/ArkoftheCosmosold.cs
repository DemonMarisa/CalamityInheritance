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
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class ArkoftheCosmosold : CIMelee, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Melee";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 102;
            Item.damage = 150;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 12;
            Item.useTime = 12;
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
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 16;

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

            int fireOffset = -100;
            Vector2 mousePos = Main.MouseWorld;
            int totalFire = 4;
            int firePosX = (int)(mousePos.X + player.Center.X) / 2;
            int firePosY = (int)player.Center.Y;

            for (int fireCount = 0; fireCount < totalFire; fireCount++)
            {
                // 垂直偏移计算
                Vector2 finalPos = new Vector2(firePosX, firePosY + fireOffset * fireCount);
                // 计算朝向鼠标的方向
                Vector2 direction = mousePos - finalPos;
                direction.Normalize();
                // 随机30度发射
                direction = direction.RotatedByRandom(MathHelper.ToRadians(15));
                // 保持原速度并应用新方向
                Vector2 newVelocity = direction * velocity.Length();

                int projectileFire = Projectile.NewProjectile(source, finalPos, newVelocity, ModContent.ProjectileType<Galaxia2>(), damage, knockback, player.whoAmI, 0f, Main.rand.Next(3));
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
                player.AddBuff(BuffID.WellFed3, 600);   //改成了大饱食度
            }
            if (astral)
            {
                player.AddBuff(ModContent.BuffType<GravityNormalizerBuff>(), 600);
            }
            if (jungle)
            {
                player.AddBuff(BuffID.Thorns, 600);
            }
            if (snow)
            {
                player.AddBuff(BuffID.Warmth, 600);
            }
            if (beach)
            {
                player.AddBuff(BuffID.Wet, 600);
            }
            if (corrupt)
            {
                player.AddBuff(BuffID.Wrath, 600);
            }
            if (crimson)
            {
                player.AddBuff(BuffID.Rage, 600);
            }
            if (dungeon)
            {
                player.AddBuff(BuffID.Dangersense, 600);
            }
            if (desert)
            {
                player.AddBuff(BuffID.Endurance, 600);
            }
            if (glow)
            {
                player.AddBuff(BuffID.Spelunker, 600);
            }
            if (hell)
            {
                player.AddBuff(BuffID.Inferno, 600);
            }
            if (holy)
            {
                player.AddBuff(BuffID.Heartreach, 600);
            }
            if (nebula)
            {
                player.AddBuff(BuffID.MagicPower, 600);
            }
            if (stardust)
            {
                player.AddBuff(BuffID.Summoning, 600);
            }
            if (solar)
            {
                player.AddBuff(BuffID.Titan, 600);
            }
            if (vortex)
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
            if (snow)
            {
                player.AddBuff(BuffID.Warmth, 600);
            }
            if (beach)
            {
                player.AddBuff(BuffID.Wet, 600);
            }
            if (corrupt)
            {
                player.AddBuff(BuffID.Wrath, 600);
            }
             if (crimson)
            {
                player.AddBuff(BuffID.Rage, 600);
            }
             if (dungeon)
            {
                player.AddBuff(BuffID.Dangersense, 600);
            }
             if (desert)
            {
                player.AddBuff(BuffID.Endurance, 600);
            }
             if (glow)
            {
                player.AddBuff(BuffID.Spelunker, 600);
            }
             if (hell)
            {
                player.AddBuff(BuffID.Inferno, 600);
            }
             if (holy)
            {
                player.AddBuff(BuffID.Heartreach, 600);
            }
             if (nebula)
            {
                player.AddBuff(BuffID.MagicPower, 600);
            }
             if (stardust)
            {
                player.AddBuff(BuffID.Summoning, 600);
            }
             if (solar)
            {
                player.AddBuff(BuffID.Titan, 600);
            }
             if (vortex)
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
