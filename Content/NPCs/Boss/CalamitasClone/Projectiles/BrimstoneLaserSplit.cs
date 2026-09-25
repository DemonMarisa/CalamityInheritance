using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Core.CIWorlds;
using System;
using Terraria;

namespace CalamityInheritance.Content.NPCs.Boss.CalamitasClone.Projectiles
{
    public class BrimstoneLaserSplit : CIBossProj
    {
        public override string Texture => GetInstance<BrimstoneLaser>().Texture;

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.hostile = true;
            Projectile.scale = 2f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            //太长了
            //改回去了，不然战斗中都看不到了
            Projectile.timeLeft = 530;
            Projectile.alpha = 120;
        }
        public override void AI()
        {
            float followTimer = CIWorld.Malice ? 420f : 190f;
            int target = Player.FindClosest(Projectile.Center, 1, 1);
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] < followTimer && Projectile.ai[1] > 90f)
            {
                float scaleFactor2 = Projectile.velocity.Length();
                Vector2 vector11 = Main.player[target].Center - Projectile.Center;
                vector11.Normalize();
                vector11 *= scaleFactor2;
                Projectile.velocity = (Projectile.velocity * 24f + vector11) / 25f;
                Projectile.velocity.Normalize();
                Projectile.velocity *= scaleFactor2;
            }
            else if (Projectile.ai[1] >= followTimer)
            {
                if (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) < 16f)
                {
                    Projectile.velocity.X *= 1.01f;
                    Projectile.velocity.Y *= 1.01f;
                }
            }
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
            Lighting.AddLight(Projectile.Center, 0.3f, 0f, 0f);
            //查看Timeleft，在timeLeft <= 50的时候伤害就会被置零了
            if (Projectile.timeLeft <= 50)
            {
                Projectile.damage = 0;
                Projectile.alpha += 10;
            }

        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(250, 50, 50, Projectile.alpha);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffType<CIBrimstoneFlames>(), 120);
        }
    }
}
