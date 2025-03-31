﻿using CalamityMod.Buffs.DamageOverTime;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    // Photoviscerator left click main projectile (the flamethrower itself)
    public class ExoFireold : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";

        public bool ProducedAcceleration = false;

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 3;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] <= 3f)
                return;

            float dustScale = 1f;
            if (Projectile.ai[0] == 8f)
            {
                dustScale = 0.25f;
            }
            else if (Projectile.ai[0] == 9f)
            {
                dustScale = 0.5f;
            }
            else if (Projectile.ai[0] == 10f)
            {
                dustScale = 0.75f;
            }

            int dustID = Main.rand.NextBool() ? 107 : 234;
            if (Main.rand.NextBool(4))
                dustID = CIDustID.DustSandnado;

            if (Main.rand.NextBool())
            {
                for (int i = 0; i < 2; i++)
                {
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustID, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 0.6f);
                    if (Main.rand.NextBool(3))
                    {
                        d.scale *= 1.5f;
                        d.velocity.X *= 1.2f;
                        d.velocity.Y *= 1.2f;
                    }
                    else
                        d.scale *= 0.75f;

                    d.noGravity = true;
                    d.velocity.X *= 0.8f;
                    d.velocity.Y *= 0.8f;
                    d.scale *= dustScale;
                    d.velocity += Projectile.velocity;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.ExoDebuffs();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
    }
}
