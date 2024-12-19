﻿using CalamityMod.Projectiles;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Buffs.DamageOverTime;
using Terraria.ID;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class ExoArrowBlue : ModProjectile
    {
        public override string Texture => "CalamityInheritance/Content/Projectiles/LaserProj";
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;
            Projectile.Calamity().pointBlankShotDuration = CalamityGlobalProjectile.DefaultPointBlankDuration;
        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 25;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            float num55 = 40f;
            float num56 = 1.5f;
            if (Projectile.ai[1] == 0f)
            {
                Projectile.localAI[0] += num56;
                if (Projectile.localAI[0] > num55)
                {
                    Projectile.localAI[0] = num55;
                }
            }
            else
            {
                Projectile.localAI[0] -= num56;
                if (Projectile.localAI[0] <= 0f)
                {
                    Projectile.Kill();
                }
            }
        }

        public override Color? GetAlpha(Color lightColor) => new Color(0, 0, 250, Projectile.alpha);

        public override bool PreDraw(ref Color lightColor) => Projectile.DrawBeam(40f, 1.5f, lightColor);
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.ExoDebuffs();
            OnHitEffects(target.Center);
        }

        private void OnHitEffects(Vector2 targetPos)
        {
            var source = Projectile.GetSource_FromThis();
            for (int x = 0; x < 3; x++)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    CalamityUtils.ProjectileBarrage(source, Projectile.Center, targetPos, Main.rand.NextBool(2), 500f, 500f, 0f, 500f, 10f, ModContent.ProjectileType<ExoArrowBlue2>(), (int)(Projectile.damage * 0.7), Projectile.knockBack * 0.7f, Projectile.owner);
                }
            }
        }
    }
}
