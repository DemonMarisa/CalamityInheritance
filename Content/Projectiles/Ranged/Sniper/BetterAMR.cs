using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.Debuffs;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged.Sniper
{
    public class BetterAMR : CIRangedProj
    {
        public int extraAmt => (int)Projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.light = 0.5f;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 10;
            Projectile.scale = 1.18f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            float num107 = (float)Math.Sqrt(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y);
            if (Projectile.alpha > 0)
                Projectile.alpha -= (byte)(num107 * 0.9);
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;

            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.alpha < 140)
                return new Color(255, 255, 255, 100);

            return Color.Transparent;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            OnHitEffects(target.Center, hit.Crit);

            target.AddBuff(BuffType<CIMarkedforDeath>(), 300);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            OnHitEffects(target.Center, true);

            target.AddBuff(BuffType<CIMarkedforDeath>(), 300);
        }

        private void OnHitEffects(Vector2 targetPos, bool crit)
        {
            if (extraAmt != 0)
            {
                int extraProjectileAmt = extraAmt;
                for (int x = 0; x < extraProjectileAmt; x++)
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        bool fromRight = x > (extraAmt / 2);
                        CIUtils.ProjectileBarrage(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.Center, fromRight, 500f, 500f, 0f, 500f, 10f, ProjectileType<BetterAMR2>(), (int)(Projectile.damage * 0.15f), Projectile.knockBack, Projectile.owner, false, 5f);
                    }
                }
            }
            else
            {
                int extraProjectileAmt = 8;
                for (int x = 0; x < extraProjectileAmt; x++)
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        bool fromRight = x > 3;
                        CIUtils.ProjectileBarrage(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.Center, fromRight, 500f, 500f, 0f, 500f, 10f, ProjectileType<BetterAMR2>(), (int)(Projectile.damage * 0.15f), Projectile.knockBack, Projectile.owner, false, 5f);
                    }
                }
            }
        }
    }
}
