﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class ClimaxBeam2 : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.extraUpdates = 100;
            Projectile.friendly = true;
            Projectile.timeLeft = 90;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Vector2 vector33 = Projectile.position;
            vector33 -= Projectile.velocity * 0.25f;
            int num448 = Dust.NewDust(vector33, 1, 1, 206, 0f, 0f, 0, default, 1.25f);
            Main.dust[num448].position = vector33;
            Main.dust[num448].scale = Main.rand.Next(70, 110) * 0.013f;
            Main.dust[num448].velocity *= 0.1f;
        }
    }
}
