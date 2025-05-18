using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Build.Construction;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class DestroyerLegendaryLaser: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override string Texture => $"{GenericProjRoute.LaserProjRoute}";
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Magic;
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
            Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, 0.5f, 0.2f, 0.5f);
            float timerIncr = 3f;
            if (Projectile.ai[1] == 0f)
            {
                Projectile.localAI[0] += timerIncr;
                if (Projectile.localAI[0] > 100f)
                {
                    Projectile.localAI[0] = 100f;
                }
            }
            else
            {
                Projectile.localAI[0] -= timerIncr;
                if (Projectile.localAI[0] <= 0f)
                {
                    Projectile.Kill();
                    return;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor) => new Color(255, Main.DiscoG, 155, Projectile.alpha);

        public override bool PreDraw(ref Color lightColor) => Projectile.DrawBeam(100f, 3f, lightColor);
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            var usPlayer = player.CIMod();
            
            if (usPlayer.DestroyerTier1)
                modifiers.SetCrit();
            if (usPlayer.DestroyerTier2)
            {
                //取玩家当前法术暴击加成。
                float getCrits = player.GetTotalCritChance<MagicDamageClass>() + 4f;
                if (getCrits > 0f)
                {
                    //将暴击加成小数点前置两位。
                    getCrits /= 100f;
                    //暴击加成取1/3，折算掉射弹本身的伤害
                    int pDamage = (int)(Projectile.damage * getCrits / 3);
                    //直接补到伤害上
                    modifiers.FinalDamage += pDamage / 10f;
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            int dustAmt = Main.rand.Next(3, 7);
            for (int d = 0; d < dustAmt; d++)
            {
                int dustType = Utils.SelectRandom(Main.rand,
                [
                    246,
                    73,
                    187
                ]);
                int idx = Dust.NewDust(Projectile.Center - Projectile.velocity / 2f, 0, 0, dustType, 0f, 0f, 100, default, 2.1f);
                Main.dust[idx].velocity *= 2f;
                Main.dust[idx].noGravity = true;
            }
        }
    }
}
