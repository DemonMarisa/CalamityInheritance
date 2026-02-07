using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue.Bombs
{
    public class AcidBarrelDrop : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.timeLeft = 840;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = RogueDamageClass.Instance;
        }
        public override void AI()
        {
            if (Main.rand.NextBool())
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.JungleTorch);
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(0.65f, 0.8f);
                dust.velocity = -Projectile.velocity * 0.4f;
            }

            if (Projectile.ai[0] < 3)
            {
                if (Projectile.timeLeft == 839)
                    Projectile.velocity *= 0.5f;
                Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
                if (Projectile.velocity.Y <= 12f)
                {
                    Projectile.velocity.Y += 0.15f;
                }
                Projectile.tileCollide = Projectile.timeLeft <= 300;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Water drip
            for (int i = 0; i < 4; i++)
            {
                int idx = Dust.NewDust(Projectile.position - Projectile.velocity, 2, 2, DustID.JungleTorch, 0f, 0f, 0, new Color(112, 150, 42, 127), 1f);
                Dust dust = Main.dust[idx];
                dust.position.X -= 2f;
                Main.dust[idx].alpha = 38;
                Main.dust[idx].velocity *= Projectile.velocity.RotatedByRandom(0.3f) * 0.4f;
                Main.dust[idx].velocity -= Projectile.velocity * 0.025f;
                Main.dust[idx].scale = 0.85f;
            }
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Irradiated>(), 180);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], new Color(255, 255, 255, 127), 2);
            return false;
        }
    }
}
