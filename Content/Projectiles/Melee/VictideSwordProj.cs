﻿using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class VictideSwordProj: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.scale = 0.5f;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 240;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0f, Projectile.scale * 1.5f);
            int dCounts = 6;
            for (int i = 0; i < dCounts; i++)
            {
                Vector2 dPosition = Vector2.Normalize(Projectile.velocity) * new Vector2(Projectile.width / 2f, Projectile.height) * 0.75f;
                dPosition = dPosition.RotatedBy((i - (dCounts / 2 - 1)) * MathHelper.Pi/ (double)(float)dCounts, default) + Projectile.Center;
                Vector2 dVelocity = ((float)(Main.rand.NextDouble() * MathHelper.Pi) - MathHelper.PiOver2).ToRotationVector2() * Main.rand.Next(3, 8);
                int d = Dust.NewDust(dPosition + dVelocity, 0, 0, DustID.DungeonWater, dVelocity.X * 2f, dVelocity.Y * 2f, 100, default, Projectile.scale);
                Main.dust[d].noGravity = true;
                Main.dust[d].noLight = true;
                Main.dust[d].velocity /= 4f;
                Main.dust[d].velocity -= Projectile.velocity;
            }
            if (Projectile.scale <= 1f)
            {
                Projectile.scale *= 1.015f;
            }
            if (Projectile.scale > 1.1f)
            {
                Projectile.scale = 1.1f;
            }
            Projectile.frame = CIFunction.FramesChanger(Projectile, 4, 3);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath19, Projectile.Center);
            int dCounts = 36;
            for (int i = 0; i < dCounts; i++)
            {
                Vector2 dPos = Vector2.Normalize(Projectile.velocity) * new Vector2(Projectile.width / 2f, Projectile.height) * 0.75f;
                dPos = dPos.RotatedBy((double)((i - (dCounts / 2 - 1)) * MathHelper.TwoPi / dCounts), default) + Projectile.Center;
                Vector2 dVel = dPos - Projectile.Center;
                int d = Dust.NewDust(dPos + dVel, 0, 0, DustID.DungeonWater, dVel.X * 2f, dVel.Y * 2f, 100, default, 1.4f);
                Main.dust[d].noGravity = true;
                Main.dust[d].noLight = true;
                Main.dust[d].velocity = dVel;
            }
        }
    }
}
