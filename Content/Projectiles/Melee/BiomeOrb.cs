﻿using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.CalPlayer;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class BiomeOrb : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        private int dustType = 3;
        Color color = default;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Biome Orb");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 27;
            AIType = ProjectileID.LightBeam;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
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
            bool sky = player.ZoneSkyHeight;
            bool holy = player.ZoneHallow;
            if (astral)
            {
                dustType = ModContent.DustType<AstralOrange>();
                color = new Color(255, 127, 80, Projectile.alpha);
            }
            else if (jungle)
            {
                dustType = 39;
                color = new Color(128, 255, 128, Projectile.alpha);
            }
            else if (snow)
            {
                dustType = 51;
                color = new Color(128, 255, 255, Projectile.alpha);
            }
            else if (beach)
            {
                dustType = 33;
                color = new Color(0, 0, 128, Projectile.alpha);
            }
            else if (corrupt)
            {
                dustType = 14;
                color = new Color(128, 64, 255, Projectile.alpha);
            }
            else if (crimson)
            {
                dustType = 5;
                color = new Color(128, 0, 0, Projectile.alpha);
            }
            else if (dungeon)
            {
                dustType = 29;
                color = new Color(64, 0, 128, Projectile.alpha);
            }
            else if (desert)
            {
                dustType = 32;
                color = new Color(255, 255, 128, Projectile.alpha);
            }
            else if (glow)
            {
                dustType = 56;
                color = new Color(0, 255, 255, Projectile.alpha);
            }
            else if (hell)
            {
                dustType = 6;
                color = new Color(255, 128, 0, Projectile.alpha);
            }
            else if (sky)
            {
                dustType = 213;
                color = new Color(255, 255, 255, Projectile.alpha);
            }
            else if (holy)
            {
                dustType = 57;
                color = new Color(255, 255, 0, Projectile.alpha);
            }
            else
            {
                color = new Color(0, 128, 0, Projectile.alpha);
            }
            int num458 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 0.9f);
            Main.dust[num458].noGravity = true;
            Main.dust[num458].velocity *= 0.5f;
            Main.dust[num458].velocity += Projectile.velocity * 0.1f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return color;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > 295)
                return false;

            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            int num3;
            for (int num795 = 4; num795 < 31; num795 = num3 + 1)
            {
                float num796 = Projectile.oldVelocity.X * (30f / (float)num795);
                float num797 = Projectile.oldVelocity.Y * (30f / (float)num795);
                int num798 = Dust.NewDust(new Vector2(Projectile.oldPosition.X - num796, Projectile.oldPosition.Y - num797), 8, 8, dustType, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.8f);
                Main.dust[num798].noGravity = true;
                Dust dust = Main.dust[num798];
                dust.velocity *= 0.5f;
                num798 = Dust.NewDust(new Vector2(Projectile.oldPosition.X - num796, Projectile.oldPosition.Y - num797), 8, 8, dustType, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.4f);
                dust = Main.dust[num798];
                dust.velocity *= 0.05f;
                num3 = num795;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            CalamityPlayer modPlayer = player.Calamity();
            bool astral = modPlayer.ZoneAstral;
            bool jungle = player.ZoneJungle;
            bool snow = player.ZoneSnow;
            bool beach = player.ZoneBeach;
            bool dungeon = player.ZoneDungeon;
            bool desert = player.ZoneDesert;
            bool glow = player.ZoneGlowshroom;
            bool hell = player.ZoneUnderworldHeight;
            bool holy = player.ZoneHallow;
            int debuffTime = 90;
            if (astral)
            {
                target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), debuffTime);
            }
            else if (jungle)
            {
                target.AddBuff(BuffID.Venom, debuffTime);
            }
            else if (snow)
            {
                target.AddBuff(ModContent.BuffType<GlacialState>(), debuffTime / 3);
            }
            else if (beach)
            {
                target.AddBuff(ModContent.BuffType<CrushDepth>(), debuffTime);
            }
            else if (dungeon)
            {
                target.AddBuff(BuffID.Frostburn, debuffTime);
            }
            else if (desert)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), debuffTime);
            }
            else if (glow)
            {
                target.AddBuff(ModContent.BuffType<TemporalSadness>(), debuffTime / 3);
            }
            else if (hell)
            {
                target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), debuffTime);
            }
            else if (holy)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), debuffTime);
            }
            else
            {
                target.AddBuff(ModContent.BuffType<ArmorCrunch>(), debuffTime);
            }
        }
    }
}
