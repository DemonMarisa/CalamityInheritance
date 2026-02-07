using CalamityMod;
using CalamityMod.Balancing;
using CalamityMod.Dusts;
using CalamityMod.Projectiles;
using LAP.Content.Projectiles.LifeStealProj;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.Staffs
{
    public class SanguineFlareProjLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        private int x;
        private double speed = 10;
        private float startSpeedY = 0f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0f, 0f);
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.velocity.X = Projectile.velocity.X + (Main.player[Projectile.owner].velocity.X * 0.5f);
                startSpeedY = Projectile.velocity.Y + (Main.player[Projectile.owner].velocity.Y * 0.5f);
                Projectile.velocity.Y = startSpeedY;
            }
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] >= 180f)
            {
                x++;
                speed += 0.1;
                Projectile.velocity.Y = startSpeedY + (float)(speed * Math.Sin(x / 4));
            }
            Projectile.rotation += Projectile.velocity.Y * 0.02f;
            Projectile.alpha -= 5;
            if (Projectile.alpha < 30)
            {
                Projectile.alpha = 30;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(200, 60, 60, Projectile.alpha);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            for (int i = 0; i < 3; i++)
            {
                int brimDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 1.2f);
                Main.dust[brimDust].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[brimDust].scale = 0.5f;
                    Main.dust[brimDust].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 6; j++)
            {
                int brimDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 1.7f);
                Main.dust[brimDust2].noGravity = true;
                Main.dust[brimDust2].velocity *= 5f;
                brimDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 1f);
                Main.dust[brimDust2].velocity *= 2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int heal = (int)Math.Round(hit.Damage * 0.025);
            if (heal > BalancingConstants.LifeStealCap)
                heal = BalancingConstants.LifeStealCap;

            Projectile.Owner().SpawnLifeStealProj(target, Projectile.GetSource_FromThis(), ProjectileType<StandardHealProj>(), Projectile.Center, Vector2.Zero, Main.rand.Next(1, 4));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
