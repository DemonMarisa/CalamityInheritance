using CalamityMod.Items.Materials;
using CalamityMod.Buffs.Potions;
using CalamityMod.CalPlayer;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Melee;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class FourSeasonsGalaxiaold : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Galaxia(old)");
            /* Tooltip.SetDefault("Fires a spread of homing projectiles that inflict different debuffs depending on what biome you're in\n" +
                "Upon hitting an enemy you are granted a buff based on what biome you're in\n" +
                "Projectiles also change based on moon events"); */
        }

        public override void SetDefaults()
        {
            Item.width = 70;
            Item.damage = 375;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 9f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 70;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
            Item.shoot = ModContent.ProjectileType<Galaxia>();
            Item.shootSpeed = 24f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int projectiles = 0; projectiles < 1; projectiles++)
            {
                float speedX = velocity.X + Main.rand.Next(-40, 41) * 0.01f;
                float speedY = velocity.Y + Main.rand.Next(-40, 41) * 0.01f;

                Projectile.NewProjectile(source, position, new Vector2(speedX, speedY), type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<OmegaBiomeBlade>());
            recipe2.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 8);
            recipe2.AddIngredient(ModContent.ItemType<DarksunFragment>(), 8);
            recipe2.AddTile(ModContent.TileType<CosmicAnvil>());
            recipe2.Register();

            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ModContent.ItemType<FourSeasonsGalaxia>());
            recipe1.Register();
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 0);
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
    }
}
