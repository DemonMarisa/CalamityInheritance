﻿using CalamityMod.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray
{

    public class BloodBolt : ModProjectile
    {
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";

        public const int Lifetime = 150;
        public ref float Time => ref Projectile.ai[0];

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.extraUpdates = 100;
            Projectile.friendly = true;
            Projectile.timeLeft = 30;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            for (int i = 0; i < 2; i++)
            {
                int dustType = Main.rand.NextBool(4) ? 182 : (int)CalamityDusts.Brimstone;
                Vector2 dustSpawnPos = Projectile.position - Projectile.velocity * i / 2f;
                Dust crimtameMagic = Dust.NewDustPerfect(dustSpawnPos, dustType);
                crimtameMagic.scale = Main.rand.NextFloat(0.96f, 1.04f) * MathHelper.Lerp(1f, 1.7f, Time / Lifetime);
                crimtameMagic.noGravity = true;
                crimtameMagic.velocity *= 0.1f;
            }
        }
    }
}