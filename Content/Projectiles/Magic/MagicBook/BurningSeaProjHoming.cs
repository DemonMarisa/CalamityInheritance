using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Magic.MagicBook
{
    public class BurningSeaProjHoming : CIMagicProj
    {
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 180;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 150 && target.CanBeChasedBy(Projectile);

        public override void AI()
        {
            Projectile.rotation += 0.7f * Projectile.direction;

            int brimstone = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.LifeDrain, 0f, 0f, 100, default, 1f);
            Main.dust[brimstone].noGravity = true;

            if (Projectile.timeLeft < 150)
                LAPUtilities.HomeInNPC(Projectile, 600f, 16f, 20f, null, !Projectile.tileCollide);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIBrimstoneFlames>(), 120);
        }
    }
}