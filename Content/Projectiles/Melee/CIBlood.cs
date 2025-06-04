﻿using CalamityMod.Projectiles;
using CalamityMod;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Typeless.Heal;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class CIBlood : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";

        public const int Lifetime = 150;
        public ref float Time => ref Projectile.ai[0];

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
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

            CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 600f, 6f, 12f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int heal = (int)Math.Round(hit.Damage * 0.075);
            if (heal > 100)
                heal = 100;

            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.TwoPi), ModContent.ProjectileType<GlobalHealthProj>(), 0, Projectile.knockBack, Projectile.owner, 0f, 0f, heal);
        }
    }
}
