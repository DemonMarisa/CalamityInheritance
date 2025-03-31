﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class MeowFireLegacy2 : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 90;
        }

        public override void AI()
        {
            if (Projectile.ai[0] > 7f)
            {
                if (Main.zenithWorld)
                    CIFunction.HomeInOnNPC(Projectile, true, 1800f, 24f, 20f);
                float pScale = 1f;
                if (Projectile.ai[0] == 8f)
                {
                    pScale = 0.25f;
                }
                else if (Projectile.ai[0] == 9f)
                {
                    pScale = 0.5f;
                }
                else if (Projectile.ai[0] == 10f)
                {
                    pScale = 0.75f;
                }
                Projectile.ai[0] += 1f;
                int num297 = 73;
                for (int i = 0; i < 2; i++)
                {
                    int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, num297, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1f);
                    Dust dust = Main.dust[d];
                    if (Main.rand.NextBool(3))
                    {
                        dust.noGravity = true;
                        dust.scale *= 1.75f;
                        dust.velocity.X *= 2f;
                        dust.velocity.Y *= 2f;
                    }
                    else
                    {
                        dust.scale *= 0.5f;
                    }
                    dust.velocity.X *= 1.2f;
                    dust.velocity.Y *= 1.2f;
                    dust.scale *= pScale;
                    dust.velocity += Projectile.velocity;
                    if (!dust.noGravity)
                    {
                        dust.velocity *= 0.5f;
                    }
                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
            Projectile.rotation += 0.3f * Projectile.direction;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 8;
            SoundEngine.PlaySound(SoundID.Item58, Projectile.position);
            Projectile.position.X = Projectile.position.X + Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y + Projectile.height / 2;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
            for (int num621 = 0; num621 < 10; num621++)
            {
                int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.PinkFairy, 0f, 0f, 100, default, 1f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num623 = 0; num623 < 20; num623++)
            {
                int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.PinkFairy, 0f, 0f, 100, default, 1.5f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.PinkFairy, 0f, 0f, 100, default, 1f);
                Main.dust[num624].velocity *= 2f;
            }
        }
    }
}
