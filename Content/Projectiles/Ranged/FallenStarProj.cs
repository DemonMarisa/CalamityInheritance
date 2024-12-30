﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class FallenStarProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.alpha = 50;
            Projectile.light = 1f;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 20 + Main.rand.Next(40);
                SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
            }
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
            }
            Projectile.alpha += (int)(25f * Projectile.localAI[0]);
            if (Projectile.alpha > 200)
            {
                Projectile.alpha = 200;
                Projectile.localAI[0] = -1f;
            }
            if (Projectile.alpha < 50)
            {
                Projectile.alpha = 50;
                Projectile.localAI[0] = 1f;
            }
            Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.01f * Projectile.direction;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            int dustAmt = 10;
            int goreAmt = 6;
            for (int i = 0; i < dustAmt; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 58, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default, 1.2f);
            }
            for (int i = 0; i < dustAmt; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 57, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default, 1.2f);
            }
            if (Main.netMode != NetmodeID.Server)
            {
                for (int i = 0; i < goreAmt; i++)
                {
                    Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity * 0.05f, Main.rand.Next(16, 18), 1f);
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(200, 200, 200, lightColor.A - Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 offsets = new Vector2(0f, Projectile.gfxOffY) - Main.screenPosition;
            Color alpha = Projectile.GetAlpha(lightColor);
            Rectangle spriteRec = new Microsoft.Xna.Framework.Rectangle(0, 0, tex.Width, tex.Height);
            Vector2 spriteOrigin = spriteRec.Size() / 2f;
            SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Texture2D aura = ModContent.Request<Texture2D>("CalamityInheritance/Content/Projectiles/Ranged/FallenStarAura").Value;
            Vector2 drawStart = Projectile.Center + Projectile.velocity;
            Vector2 drawStart2 = Projectile.Center - Projectile.velocity * 0.5f;
            Vector2 spinPoint = new Vector2(0f, -10f);
            float time = Main.player[Projectile.owner].miscCounter % 216000f / 60f;
            Rectangle auraRec = aura.Frame();
            Color blue = Color.Blue * 0.2f;
            Color white = Color.White * 0.5f;
            white.A = 0;
            blue.A = 0;
            Vector2 auraOrigin = new Vector2(auraRec.Width / 2f, 10f);

            //Draw the aura
            Main.EntitySpriteDraw(aura, drawStart + offsets + spinPoint.RotatedBy(MathHelper.TwoPi * time), auraRec, blue, Projectile.velocity.ToRotation() + MathHelper.PiOver2, auraOrigin, 1.5f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(aura, drawStart + offsets + spinPoint.RotatedBy(MathHelper.TwoPi * time + MathHelper.TwoPi / 3f), auraRec, blue, Projectile.velocity.ToRotation() + MathHelper.PiOver2, auraOrigin, 1.1f, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(aura, drawStart + offsets + spinPoint.RotatedBy(MathHelper.TwoPi * time + MathHelper.Pi * 4f / 3f), auraRec, blue, Projectile.velocity.ToRotation() + MathHelper.PiOver2, auraOrigin, 1.3f, SpriteEffects.None, 0);
            for (float d = 0f; d < 1f; d += 0.5f)
            {
                float scaleMult = time % 0.5f / 0.5f;
                scaleMult = (scaleMult + d) % 1f;
                float colorMult = scaleMult * 2f;
                if (colorMult > 1f)
                {
                    colorMult = 2f - colorMult;
                }
                Main.EntitySpriteDraw(aura, drawStart2 + offsets, auraRec, white * colorMult, Projectile.velocity.ToRotation() + MathHelper.PiOver2, auraOrigin, 0.3f + scaleMult * 0.5f, SpriteEffects.None, 0);
            }

            //Draw the actual projectile
            Main.EntitySpriteDraw(tex, Projectile.Center + offsets, spriteRec, alpha, Projectile.rotation, spriteOrigin, Projectile.scale + 0.1f, spriteEffects, 0);
            return false;
        }
    }
}
