using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Core.Utilities;

namespace CalamityInheritance.Content.Projectiles.Melee.Flails
{
    public class UrchinSpikeFuguLegacy : CIMeleeProj
    {
        public ref float Time => ref Projectile.ai[0];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 90;
            Projectile.noEnchantments = true;
        }

        public override void AI()
        {
            Time++;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.alpha = (int)Utils.Remap(Time, 0f, 12f, 255f, 0f);

            NPC potentialTarget = LAPUtilities.FindClosestTarget(Projectile.Center, 300f);
            if (potentialTarget != null && Time >= 12f)
            {
                Projectile.HomeInNPC(600f, 24f, 20f);
            }
            else if (Time >= 48f)
                Projectile.velocity *= 0.9f;

            int dustRate = (int)MathF.Max(Utils.Remap(Time, 0f, 12f, 20f, 4f), Utils.Remap(Time, 60f, 90f, 4f, 20f));
            if (Main.rand.NextBool(dustRate))
            {
                Dust offTrail = Dust.NewDustPerfect(Projectile.Center, DustID.Venom, Main.rand.NextVector2Circular(0.2f, 0.2f));
                offTrail.noGravity = true;
                offTrail.scale = Main.rand.NextFloat(0.6f, 1.2f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.Poisoned, 120);

        public override bool? CanDamage() => Time < 12f ? false : base.CanDamage();
    }
}
