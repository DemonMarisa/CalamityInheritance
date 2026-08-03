using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Summon.Proj.Limits.MagicHat
{
    public class MagicBulletOld : CISummonProj
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bullet");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.light = 0.5f;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 7;
            Projectile.scale = 1.18f;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            AIType = ProjectileID.BulletHighVelocity;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.BetsysCurse, 180);
            target.AddBuff(BuffID.Ichor, 180);
            target.AddBuff(BuffType<CIMarkedforDeath>(), 180);
            target.AddBuff(BuffType<CIArmorCrunch>(), 180);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(189, 51, 164, Projectile.alpha);
        }
    }
}
