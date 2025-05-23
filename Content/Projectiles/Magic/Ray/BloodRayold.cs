﻿using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray
{
    public class BloodRayold : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public const int Lifetime = 200;
        public ref float Time => ref Projectile.ai[0];
        public ref float InitialDamage => ref Projectile.ai[1];
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";
        public Player Owner => Main.player[Projectile.owner];
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 10;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = Lifetime;
        }
        public override void AI()
        {
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] >= 29f && Projectile.owner == Main.myPlayer)
            {
                Projectile.localAI[1] = 0f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<BloodOrb>(), (int)(Projectile.damage * 0.6), (int)Projectile.knockBack, Projectile.owner, 0f, 0f);
            }

            if (InitialDamage == 0f)
            {
                InitialDamage = Projectile.damage;
                Projectile.netUpdate = true;
            }
            float damageboost = (float)(Time / Lifetime) * 2;

            Projectile.damage = (int)(InitialDamage * damageboost);

            Time++;
            if (Time >= 9f)
            {
                for (int i = 0; i < 2; i++)
                {
                    int dustType = Main.rand.NextBool(4) ? 182 : (int)CalamityDusts.Brimstone;
                    Vector2 dustSpawnPos = Projectile.Center - Projectile.velocity * i / 2f;
                    Dust crimtameMagic = Dust.NewDustPerfect(dustSpawnPos, dustType);
                    crimtameMagic.scale = Main.rand.NextFloat(0.96f, 1.04f) * MathHelper.Lerp(1f, 1.7f, Time / Lifetime);
                    crimtameMagic.noGravity = true;
                    crimtameMagic.velocity *= 0.1f;
                }
            }

            // Just doing damage = (int)(damage * scalar) wouldn't work here.
            // The exponential base would be too small for a weapon like this, and the
            // cast (which removes the fractional part) would overtake any increases before the damage can rise.
        }
    }
}
