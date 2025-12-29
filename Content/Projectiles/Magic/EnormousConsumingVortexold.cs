using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class EnormousConsumingVortexold : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public float Time
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public float IdealScale
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public const int TentacleSpawnRate = 20;
        public const int PulseInterval = 18;
        public const float PulseHitboxExpandRatio = 2.5f;
        public const float RadialOffsetVarianceFactor = 0.1f;
        public const float StartingScale = 0.0004f;

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.scale = StartingScale;
            Projectile.timeLeft = 540;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }

        // Vanilla Terraria does not sync projectile scale by default.
        public override void SendExtraAI(BinaryWriter writer) => writer.Write(Projectile.scale);
        public override void ReceiveExtraAI(BinaryReader reader) => Projectile.scale = reader.ReadSingle();

        public override void AI()
        {
            Projectile projectile = Projectile;
            projectile.rotation += MathHelper.ToRadians(8f);
            Projectile.alpha = (int)MathHelper.Lerp(255f, 0f, Utils.GetLerpValue(0f, 20f, Time, true));
            Projectile.scale = MathHelper.Lerp(0.0004f, IdealScale, Utils.GetLerpValue(0f, 30f, Time, true));
            // Determine the ideal scale in the first frame.
            if (IdealScale == 0f)
            {
                IdealScale = Main.rand.NextFloat(2.2f, 3f);
                Projectile.netUpdate = true;
            }

            // Target enemy if possible and idly spawn tentacles.
            if (Time < 150)
            {
                TargetingMovement();
                if (Time % TentacleSpawnRate == TentacleSpawnRate - 1 && Main.myPlayer == Projectile.owner)
                {
                    ProduceSubsumingHentai();
                }
            }
            // Slow down and pulse frequently.
            else if (Time < 220)
            {
                Projectile.velocity *= 0.96f;
                if (Time % PulseInterval == 0f)
                {
                    PulseEffect();
                }
            }
            else if (Time >= 240)
            {
                ExplodeEffect();
                Projectile.Kill();
            }
            CalamityUtils.ExpandHitboxBy(Projectile, (int)(Projectile.scale * 62));
            Time++;
        }

        public void ProduceSubsumingHentai()
        {
            float xStartingAcceleration = Utils.NextFloat(Main.rand, 0.001f, 0.04f) * Utils.ToDirectionInt(Utils.NextBool(Main.rand, 2));
            float yStartingAcceleration = Utils.NextFloat(Main.rand, 0.001f, 0.04f) * Utils.ToDirectionInt(Utils.NextBool(Main.rand, 2));
            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(null), Projectile.Center, Utils.RotatedByRandom(Vector2.UnitY, 6.2831854820251465) * Utils.NextFloat(Main.rand, 9f, 13f), ProjectileType<SubsumingTentacle>(), (int)(Projectile.damage * 0.6), Projectile.knockBack * 0.6f, this.Projectile.owner, xStartingAcceleration, yStartingAcceleration, 0f).tileCollide = false;
        }

        public void TargetingMovement()
        {
            NPC potentialTarget = Projectile.Center.ClosestNPCAt(600f, true, true);
            if (potentialTarget != null)
                Projectile.velocity = (Projectile.velocity * 5f + Projectile.SafeDirectionTo(potentialTarget.Center) * 7f) / 6f;
        }

        public void PulseEffect()
        {
            CalamityUtils.ExpandHitboxBy(Projectile, PulseHitboxExpandRatio);
            Projectile.Damage();
            if (!Main.dedServ)
            {
                SoundEngine.PlaySound(SoundID.Item43, Projectile.Center);
                for (int i = 0; (float)i < 85; i++)
                {
                    float angle = i / 85f * MathHelper.TwoPi;
                    Vector2 spawnPosition = Projectile.Center + angle.ToRotationVector2() * (500f + Main.rand.NextFloat(-8f, 8f));
                    Vector2 velocity = (angle - (float)Math.PI).ToRotationVector2() * Main.rand.NextFloat(27f, 38.5f);
                    Dust dust = Dust.NewDustPerfect(spawnPosition, 264, velocity);
                    dust.scale = 0.9f;
                    dust.fadeIn = 1.25f;
                    dust.color = Main.hslToRgb(i / 85f, 1f, 0.8f);
                    dust.noGravity = true;
                }
            }
            CalamityUtils.ExpandHitboxBy(Projectile, 1f / PulseHitboxExpandRatio);
        }

        public void ExplodeEffect()
        {
            SoundEngine.PlaySound(SoundID.Item29, (Vector2?)Projectile.Center, null);
            if (!Main.dedServ)
            {
                for (int i = 0; i < 200; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2Circular(50f, 50f);
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, 264, velocity);
                    dust.color = Main.hslToRgb(i / 100f % 1f, 1f, 0.8f);
                    dust.fadeIn = 1.25f;
                    dust.noGravity = true;
                }
            }
            if (Main.myPlayer == Projectile.owner)
            {
                int vortexDamage = (int)(Projectile.damage * 0.75f);
                NPC closestTarget = Projectile.Center.ClosestNPCAt(1600f, true, true);

                for (int j = 0; j < 12; j++)
                {
                    float rotation = Utils.NextFloat(Main.rand, (float)Math.PI * 2f);
                    Vector2 velocity2 = Utils.RotatedBy(Vector2.UnitY, (double)rotation, default(Vector2));
                    if (closestTarget != null)
                    {
                        velocity2 = Utils.RotatedByRandom(Projectile.DirectionTo(closestTarget.Center), 0.4);
                    }
                    velocity2 *= Utils.NextFloat(Main.rand, 3f, 5f);
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(null), Projectile.Center, velocity2, ProjectileType<Vortex>(), (int)(Projectile.damage * 0.7), this.Projectile.knockBack, this.Projectile.owner, 0f, Utils.NextFloat(Main.rand, 0.5f, 1.8f), 0f);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            int vortexesToDraw = 27;
            for (int i = 0; i < vortexesToDraw; i++)
            {
                float rotation = MathHelper.TwoPi / i * vortexesToDraw + Projectile.rotation;
                float drawScale = Projectile.scale * 0.66f;

                drawScale *= (float)(Math.Cos(Time / 18f + rotation * 2f) + 1f) * 0.3f + 0.7f; // Range of 0.7 to 1. Used to give variance with the scale.
                float offsetFactor = (float)(Math.Sin(Time / 18f + rotation * 2f) + 1f) * 0.2f + 0.8f; // Range of 0.8 to 1.

                // Due to the way RotatedBy works, the offset can be negative, giving the projectile a
                // dual buzzsaw look.
                Vector2 drawOffset = Vector2.UnitX.RotatedBy(rotation) * 30f * offsetFactor * drawScale;
                Vector2 drawPosition = Projectile.Center + drawOffset - Main.screenPosition;
                Color colorToDraw = Main.hslToRgb((i / (float)vortexesToDraw + Time / 40f) % 1f, 1f, 0.75f);
                Main.EntitySpriteDraw((Texture2D)Request<Texture2D>(Texture),
                                 drawPosition,
                                 null,
                                 colorToDraw * 0.7f,
                                 rotation,
                                 Request<Texture2D>(Texture).Size() * 0.5f,
                                 drawScale,
                                 SpriteEffects.None,
                                 0f);
            }
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.ExoDebuffs();
        }
    }
}
