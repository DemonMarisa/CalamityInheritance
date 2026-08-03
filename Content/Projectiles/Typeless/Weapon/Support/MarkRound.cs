using CalamityInheritance.Assets;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.Debuffs;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Typeless.Weapon.Support
{
    public class MarkRound : CITypelessProj
    {
        public override string Texture => CITextureRegister.LaserProj.Path;

        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 300;
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
            Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, 0.5f, 0f, 0.7f);
            float num55 = 30f;
            float num56 = 2f;
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
        public override Color? GetAlpha(Color lightColor) => new Color(200, 0, 250, 0);

        public override bool PreDraw(ref Color lightColor) => Projectile.DrawBeam(30f, 2f, lightColor);

        public override void OnKill(int timeLeft)
        {
            int dustAmt = Main.rand.Next(3, 7);
            for (int d = 0; d < dustAmt; d++)
            {
                int purple = Dust.NewDust(Projectile.Center - Projectile.velocity / 2f, 0, 0, 173, 0f, 0f, 100, default, 2.1f);
                Main.dust[purple].velocity *= 2f;
                Main.dust[purple].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIMarkedforDeath>(), 300);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffType<CIMarkedforDeath>(), 300);
        }
    }
}
