using CalamityInheritance.Assets;
using CalamityInheritance.Common.CalamityModCross.ILChange;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons.ExoArrow
{
    public class ExoArrowTeal : CIExoProj
    {
        public override string Texture => CITextureRegister.LaserProj.Path;
        public override void SetStaticDefaults()
        {
            ChangePierceResistNPC.NoPierceResist.Add(Type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 0;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;

        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
                Projectile.alpha -= 25;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;

            float num55 = 40f;
            float num56 = 1.5f;
            if (Projectile.ai[1] == 0f)
            {
                Projectile.localAI[0] += num56;
                if (Projectile.localAI[0] > num55)
                    Projectile.localAI[0] = num55;
            }
            else
            {
                Projectile.localAI[0] -= num56;
                if (Projectile.localAI[0] <= 0f)
                    Projectile.Kill();
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIMiracleBlight>(), 300);
        }

        public override Color? GetAlpha(Color lightColor) => new Color(0, 100, 100, Projectile.alpha);

        public override bool PreDraw(ref Color lightColor) => Projectile.DrawBeam(40f, 1.5f, lightColor);
    }
}
