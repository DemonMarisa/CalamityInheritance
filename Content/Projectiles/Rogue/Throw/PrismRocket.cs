using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue.Throw
{
    public class PrismRocket : CIRogueProj
    {
        public float ExponentialAccelerationFactor => Projectile.CI().Stealth ? 1.027f : 1.015f;
        public float MaxHomingSpeed => Projectile.CI().Stealth ? 26f : 21f;
        public const int Lifetime = 150;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = Lifetime;
            Projectile.DamageType = RogueDamage.Instance;
        }

        public override void AI()
        {
            NPC potentialTarget = LAPUtilities.FindClosestTarget(Projectile.Center, 800f, true);
            if (potentialTarget != null)
                AttackTarget(potentialTarget);

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            EmitDust();
        }

        public void AttackTarget(NPC target)
        {
            float newSpeed = Projectile.velocity.Length() * ExponentialAccelerationFactor;
            if (newSpeed > MaxHomingSpeed)
                newSpeed = MaxHomingSpeed;

            if (!Projectile.WithinRange(target.Center, 30f))
            {
                Projectile.velocity = (Projectile.velocity * 5f + LAPUtilities.GetVector2(Projectile.Center, target.Center) * newSpeed) / 6f;
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.AngleTo(target.Center), 0.15f).ToRotationVector2() * newSpeed;
            }
        }

        public void EmitDust()
        {
            if (Main.dedServ)
                return;

            for (int i = 0; i < 2; i++)
            {
                if (!Main.rand.NextBool(3))
                    continue;

                Dust dust = Dust.NewDustPerfect(Projectile.Center - (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * 10f, DustID.AncientLight);
                dust.color = Color.Cyan;
                dust.velocity = Main.rand.NextVector2Unit();
                dust.scale = Main.rand.NextFloat(0.75f, 1.05f);
                dust.noGravity = true;
            }
        }
    }
}
