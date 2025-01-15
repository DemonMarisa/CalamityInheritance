using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Buffs.Potions;
using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class Galaxia : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Mods.CalamityInheritance.Content.Content.Projectiles";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Orb");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 50;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 280;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 20 + Main.rand.Next(40);
                if (Main.rand.NextBool(5))
                {
                    SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
                }
            }
            Projectile.alpha -= 15;
            int num58 = 150;
            if (Projectile.Center.Y >= Projectile.ai[1])
            {
                num58 = 0;
            }
            if (Projectile.alpha < num58)
            {
                Projectile.alpha = num58;
            }
            Projectile.localAI[0] += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.01f * (float)Projectile.direction;
            Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.01f * (float)Projectile.direction;
            if (Main.rand.NextBool(8))
            {
                Vector2 value3 = Vector2.UnitX.RotatedByRandom(1.5707963705062866).RotatedBy((double)Projectile.velocity.ToRotation(), default);
                int num59 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.2f);
                Main.dust[num59].noGravity = true;
                Main.dust[num59].velocity = value3 * 0.66f;
                Main.dust[num59].position = Projectile.Center + value3 * 12f;
            }
            if (Main.rand.NextBool(24))
            {
                int goreIndex = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 0.2f, 16, 1f);
                Main.gore[goreIndex].velocity *= 0.66f;
                Main.gore[goreIndex].velocity += Projectile.velocity * 0.3f;
            }
            if (Projectile.ai[1] == 1f)
            {
                Projectile.light = 0.9f;
                if (Main.rand.NextBool(5))
                {
                    int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.2f);
                    Main.dust[dustIndex].noGravity = true;
                }
                if (Main.rand.NextBool(10))
                {
                    Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity * 0.2f, Main.rand.Next(16, 18), 1f);
                }
            }

            CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 1600f, 15f, 20f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
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
                target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 1200);
                player.AddBuff(ModContent.BuffType<GravityNormalizerBuff>(), 600);
                int proj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<AstralStar>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].CalamityInheritance().forceMelee = true;
            }
            else if (jungle)
            {
                target.AddBuff(ModContent.BuffType<Plague>(), 1200);
                player.AddBuff(BuffID.Thorns, 600);
                int proj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity, ProjectileID.Leaf, Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].CalamityInheritance().forceMelee = true;
            }
            else if (snow)
            {
                target.AddBuff(ModContent.BuffType<GlacialState>(), 30);
                player.AddBuff(BuffID.Warmth, 600);
                Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity, ProjectileID.IceBolt, Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            else if (beach)
            {
                target.AddBuff(ModContent.BuffType<CrushDepth>(), 1200);
                player.AddBuff(BuffID.Wet, 600);
                int proj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity * 0.25f, ProjectileID.FlaironBubble, Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].CalamityInheritance().forceMelee = true;
            }
            else if (corrupt)
            {
                player.AddBuff(BuffID.Wrath, 600);
                int ball = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity, ProjectileID.CursedFlameFriendly, Projectile.damage, Projectile.knockBack, Projectile.owner);
                if (ball.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[ball].penetrate = 1;
                }
                Main.projectile[ball].CalamityInheritance().forceMelee = true;
            }
            else if (crimson)
            {
                player.AddBuff(BuffID.Rage, 600);
                int ball = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity, ProjectileID.GoldenShowerFriendly, Projectile.damage, Projectile.knockBack, Projectile.owner);
                if (ball.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[ball].penetrate = 1;
                }
                Main.projectile[ball].CalamityInheritance().forceMelee = true;
            }
            else if (dungeon)
            {
                target.AddBuff(BuffID.Frostburn, 1200);
                player.AddBuff(BuffID.Dangersense, 600);
                int ball = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity, ProjectileID.WaterBolt, Projectile.damage, Projectile.knockBack, Projectile.owner);
                if (ball.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[ball].penetrate = 1;
                }
                Main.projectile[ball].CalamityInheritance().forceMelee = true;
            }
            else if (desert)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 1200);
                player.AddBuff(BuffID.Endurance, 600);
                int proj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity, ProjectileID.BlackBolt, Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].CalamityInheritance().forceMelee = true;
            }
            else if (glow)
            {
                target.AddBuff(ModContent.BuffType<TemporalSadness>(), 30);
                player.AddBuff(BuffID.Spelunker, 600);
                int proj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity, ProjectileID.Mushroom, Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].CalamityInheritance().forceMelee = true;
            }
            else if (hell)
            {
                target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 1200);
                player.AddBuff(BuffID.Inferno, 600);
                int proj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity, ProjectileID.BallofFire, Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].CalamityInheritance().forceMelee = true;
            }
            else if (holy)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 1200);
                player.AddBuff(BuffID.Heartreach, 600);
                int proj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity, ProjectileID.RainbowCrystalExplosion, Projectile.damage, Projectile.knockBack, Projectile.owner);
                if (proj.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[proj].usesLocalNPCImmunity = true;
                    Main.projectile[proj].localNPCHitCooldown = -1;
                }
                Main.projectile[proj].CalamityInheritance().forceMelee = true;
            }
            else if (nebula)
            {
                player.AddBuff(BuffID.MagicPower, 600);
                int proj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity, ProjectileID.NebulaBlaze1, Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].CalamityInheritance().forceMelee = true;
            }
            else if (stardust)
            {
                player.AddBuff(BuffID.Summoning, 600);
                int ball = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity, ProjectileID.StardustCellMinionShot, Projectile.damage, Projectile.knockBack, Projectile.owner);
                if (ball.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[ball].penetrate = 1;
                }
                Main.projectile[ball].CalamityInheritance().forceMelee = true;
            }
            else if (solar)
            {
                player.AddBuff(BuffID.Titan, 600);
                Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity, ProjectileID.SolarWhipSwordExplosion, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
            }
            else if (vortex)
            {
                player.AddBuff(BuffID.AmmoReservation, 600);
                int proj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity, ProjectileID.VortexBeaterRocket, Projectile.damage, Projectile.knockBack, Projectile.owner);
                if (proj.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[proj].usesLocalNPCImmunity = true;
                    Main.projectile[proj].localNPCHitCooldown = -1;
                }
                Main.projectile[proj].CalamityInheritance().forceMelee = true;
            }
            else
            {
                target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 1200);
                player.AddBuff(BuffID.DryadsWard, 600);
                int proj = Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity, ProjectileID.TerrarianBeam, Projectile.damage, Projectile.knockBack, Projectile.owner);
                if (proj.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[proj].penetrate = 1;
                }
                Main.projectile[proj].CalamityInheritance().forceMelee = true;
            }
        }

    }
}
