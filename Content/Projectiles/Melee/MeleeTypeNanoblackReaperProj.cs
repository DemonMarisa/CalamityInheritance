using System;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    [LegacyName("NanoblackMainLegacyMelee")]
    public class MeleeTypeNanoblackReaperProj: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Melee/MeleeTypeNanoblackReaper";

        private const float RotationIncrement = 0.22f;
        private const int Lifetime = 240;
        private const float ReboundTime = 50f;
        private const int MinBladeTimer = 9;
        private const int MaxBladeTimer = 12;


        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = Lifetime;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
        }

        public override void AI()
        {
            DrawOffsetX = -11;
            DrawOriginOffsetY = -4;
            DrawOriginOffsetX = 0;

            // Initialize the frame counter and random blade delay on the very first frame.
            if (Projectile.timeLeft == Lifetime)
                Projectile.ai[1] = GetBladeDelay();

            // Produces electricity and green firework sparks constantly while in flight.
            if (Main.rand.NextBool(3))
            {
                int dustType = Main.rand.NextBool(5) ? 226 : 220;
                float scale = 0.8f + Main.rand.NextFloat(0.3f);
                float velocityMult = Main.rand.NextFloat(0.3f, 0.6f);
                int idx = Dust.NewDust(Projectile.position,
                                       Projectile.width,
                                       Projectile.height,
                                       dustType);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity = velocityMult * Projectile.velocity;
                Main.dust[idx].scale = scale;
            }

            // ai[0] is a frame counter. ai[1] is a countdown to spawning the next nanoblack energy blade.
            Projectile.ai[0] += 1f;
            Projectile.ai[1] -= 1f;

            // On the frame the scythe begins returning, send a net update.
            if (Projectile.ai[0] == ReboundTime)
                Projectile.netUpdate = true;

            // The scythe runs its returning AI if the frame counter is greater than ReboundTime.
            if (Projectile.ai[0] >= ReboundTime)
            {
                float returnSpeed = MeleeTypeNanoblackReaper.Speed;
                float acceleration = 2.4f;
                Player owner = Main.player[Projectile.owner];
                CIFunction.BoomerangReturningAI(owner, Projectile, returnSpeed, acceleration);
                // Delete the projectile if it touches its owner.
                if (Main.myPlayer == Projectile.owner)
                    if (Projectile.Hitbox.Intersects(owner.Hitbox))
                        Projectile.Kill();
            }

            // Create nanoblack energy blades at a somewhat-random rate while in flight. (or full-sized scythes afterimages if stealth strike)
            if (Projectile.ai[1] <= 0f)
            {
                SpawnEnergyBlade();
                Projectile.ai[1] = GetBladeDelay();
            }

            // Rotate the scythe as it flies.
            float spin = Projectile.direction <= 0 ? -1f : 1f;
            Projectile.rotation += spin * RotationIncrement;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public int GetBladeDelay()
        {
            return Main.rand.Next(MinBladeTimer, MaxBladeTimer + 1);
        }

        public void SpawnEnergyBlade()
        {
            int bladeID = ModContent.ProjectileType<MeleeTypeNanoblackReaperProjSplit>();
            int bladeDamage = Projectile.damage / 2;
            float bladeKB = 3f;
            float spin = Projectile.direction <= 0 ? -1f : 1f;
            float d = 16f;
            float velocityMult = 0.9f;
            Vector2 directOffset = new Vector2(Main.rand.NextFloat(-d, d), Main.rand.NextFloat(-d, d));
            Vector2 velocityOffset = Main.rand.NextFloat(-velocityMult, velocityMult) * Projectile.velocity;
            Vector2 pos = Projectile.Center + directOffset + velocityOffset;
            if (Projectile.owner == Main.myPlayer)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                                         pos,
                                         Vector2.Zero,
                                         bladeID,
                                         bladeDamage,
                                         bladeKB,
                                         Projectile.owner,
                                         0f,
                                         spin);
        }
    }
}
