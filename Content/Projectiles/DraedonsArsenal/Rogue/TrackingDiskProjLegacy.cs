using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Items.Weapons.DraedonsArsenal.Rogue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace CalamityInheritance.Content.Projectiles.DraedonsArsenal.Rogue
{
    public class TrackingDiskProj : CIRogueProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<TrackingDisk>();
        public override string Texture => GetInstance<TrackingDisk>().Texture;
        public bool ReturningToPlayer
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value.ToInt();
        }

        public float Time
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public const int LaserFireRate = 20;
        public const int LaserFireRateStealth = 16;
        public const int MaxLaserCountPerShot = 4; // This only applies to stealth strikes.
        public const float MaxTargetSearchDistance = 480f;
        public const float MaxTargetSearchStealth = 800f;
        public const float ReturnAccelerationFactor = 0.0012f;
        public const float ReturnMaxSpeed = 12f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = RogueDamage.Instance;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.Red.ToVector3());

            Player player = Main.player[Projectile.owner];

            Time++;
            if (!ReturningToPlayer)
            {
                if (Time >= 45f)
                {
                    ReturningToPlayer = true;
                    Projectile.tileCollide = false;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                float distanceFromPlayer = Projectile.Distance(player.Center);
                if (distanceFromPlayer > 3000f)
                    Projectile.Kill();

                // This is done instead of a Normalize or DirectionTo call because the variables needed are already present and calculating the square root again would be unnecessary.
                Vector2 idealVelocity = (player.Center - Projectile.Center) / distanceFromPlayer * ReturnMaxSpeed;

                Projectile.velocity.X += Math.Sign(idealVelocity.X - Projectile.velocity.X) * (ReturnAccelerationFactor * Time);
                Projectile.velocity.Y += Math.Sign(idealVelocity.Y - Projectile.velocity.Y) * (ReturnAccelerationFactor * Time);

                if (Time % (Projectile.CI().Stealth ? LaserFireRateStealth : LaserFireRate) == 0f)
                    AttemptToFireLasers((int)(Projectile.damage * 0.25));

                if (Main.myPlayer == Projectile.owner)
                {
                    if (Projectile.Hitbox.Intersects(player.Hitbox))
                        Projectile.Kill();
                }
            }

            Projectile.rotation += 0.25f;
        }

        public void AttemptToFireLasers(int damage)
        {
            if (Main.myPlayer != Projectile.owner)
                return;
            if (Projectile.CI().Stealth)
            {
                int targetCount = 0;
                List<NPC> targets = Main.npc.Where(npc =>
                {
                    return npc.active && Projectile.Distance(npc.Center) < MaxTargetSearchStealth && npc.CanBeChasedBy();
                }).ToList();
                foreach (var target in targets)
                {
                    if (targetCount >= MaxLaserCountPerShot)
                        break;
                    Projectile laser = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, LAPUtilities.GetVector2(Projectile.Center, target.Center) * 4f, ProjectileType<TrackingDiskLaserProj>(), (int)(damage * 0.6f), Projectile.knockBack, Projectile.owner, 1f);
                    laser.scale *= 1.6f;
                    laser.netUpdate = true;
                    targetCount++;
                }
            }
            else
            {
                NPC potentialTarget = LAPUtilities.FindClosestTarget(Projectile.Center,MaxTargetSearchDistance);
                if (potentialTarget != null)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, LAPUtilities.GetVector2(Projectile.Center, potentialTarget.Center) * 3f, ProjectileType<TrackingDiskLaserProj>(), damage, Projectile.knockBack, Projectile.owner);
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ReturningToPlayer = true;
            Projectile.tileCollide = false;
            Projectile.netUpdate = true;
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, ProjectileID.Sets.TrailCacheLength[Projectile.type]);
            return false;
        }
    }
}
