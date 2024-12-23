﻿using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class PhantasmalRuinGhost : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 240;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }

        public override void AI()
        {
            // Set the projectile's direction correctly
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            // The projectile rapidly fades in as it starts existing
            if (Projectile.timeLeft >= 207)
                Projectile.alpha += 6;

            CalamityUtils.HomeInOnNPC(Projectile, true, 300f, 12f, 40f);
        }

        public override Color? GetAlpha(Color lightColor) => new Color(100, 200, 255, 100);

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > 239)
                return false;

            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
