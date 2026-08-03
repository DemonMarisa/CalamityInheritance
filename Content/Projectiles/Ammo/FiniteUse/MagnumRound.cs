using CalamityInheritance.Content.BaseClass.Projectiles;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Ammo.FiniteUse
{
    public class MagnumRound : CITypelessProj
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.light = 0.5f;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 10;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            AIType = ProjectileID.BulletHighVelocity;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage.Flat += target.lifeMax / 35;//75
        }
    }
}
