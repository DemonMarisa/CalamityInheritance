﻿using CalamityMod.Projectiles;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using CalamityMod.Buffs.DamageOverTime;

namespace CalamityInheritance.NPCs.Boss.Yharon.Proj
{
    public class YharonFireball2Legacy : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3600;
            CooldownSlot = 1;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
            if (Projectile.velocity.Y < 0f)
            {
                Projectile.velocity.Y *= 0.97f;
            }
            else
            {
                Projectile.velocity.Y *= 1.03f;
                if (Projectile.velocity.Y > 16f)
                {
                    Projectile.velocity.Y = 16f;
                }
            }
            if (Projectile.velocity.Y > -1f && Projectile.localAI[1] == 0f)
            {
                Projectile.localAI[1] = 1f;
                Projectile.velocity.Y = 1f;
            }
            Projectile.velocity.X *= 0.995f;
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, Projectile.Center);
            }
            if (Projectile.ai[0] >= 2f)
            {
                Projectile.alpha -= 25;
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
            }
            if (Main.rand.NextBool(16))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 55, 0f, 0f, 200, default, 1f);
                dust.scale *= 0.7f;
                dust.velocity += Projectile.velocity * 0.25f;
            }
        }

        public override bool CanHitPlayer(Player target) => Projectile.velocity.Y >= -16f;
        public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, Projectile.alpha);

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            CalamityUtils.ExpandHitboxBy(Projectile, 144);
            for (int d = 0; d < 2; d++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 55, 0f, 0f, 100, default, 1.5f);
            }
            for (int d = 0; d < 20; d++)
            {
                int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 55, 0f, 0f, 0, default, 2.5f);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 3f;
                idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 55, 0f, 0f, 100, default, 1.5f);
                Main.dust[idx].velocity *= 2f;
                Main.dust[idx].noGravity = true;
            }
            Projectile.Damage();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Projectile.velocity.Y >= -16f)
                target.AddBuff(ModContent.BuffType<Dragonfire>(), 180);
        }

    }
}
