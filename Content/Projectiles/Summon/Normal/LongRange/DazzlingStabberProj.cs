using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Buff.SummonBuff.Weapons;
using CalamityInheritance.Core.Utils;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Summon.Normal.LongRange
{
    public class DazzlingStabberProj : CISummonProj
    {
        public ref float AttackDelay => ref Projectile.ai[0];
        public ref float RestOffsetAngle => ref Projectile.ai[1];
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            Projectile.friendly = true;
            Projectile.width = 26;
            Projectile.height = 58;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1f;
            Projectile.timeLeft = 90000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.minion = true;
            Projectile.light = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3());

            ApplyPlayerBuffs();
            UpdateFrames();

            NPC potentialTarget = Projectile.Center.MinionHoming(1100f, Owner);

            if (potentialTarget != null)
                SliceTarget(potentialTarget);
            else
                ReturnToRestingPosition();
        }

        public void ApplyPlayerBuffs()
        {
            Owner.AddBuff(BuffType<DazzlingStabberBuffLegacy>(), 2);
        }

        public void UpdateFrames()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter % 6 == 0)
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
        }

        public void SliceTarget(NPC target)
        {
            // Don't do anything if the attack delay is still passing.
            if (AttackDelay > -60f)
            {
                AttackDelay--;
                if (AttackDelay > 0f)
                    return;
            }

            // Reset the velocity to fly upward if there is very little motion to ensure
            // that the summon does not get stuck.
            if (Projectile.velocity.Length() < 3f)
                Projectile.velocity = Vector2.UnitY * -12f;

            // If close to the target, slow down dramatically.
            if (Projectile.velocity.Length() > 5f && Projectile.WithinRange(target.Center, 90f))
                Projectile.velocity *= 0.93f;

            // Otherwise, if not close, speed up dramatically.
            else if (Projectile.velocity.Length() < 40f)
                Projectile.velocity *= 1.03f;

            float angularTurnSpeed = 0.35f;
            float angleToTargetCoords = Projectile.AngleTo(target.Center);

            if (!Projectile.WithinRange(target.Center, 200f))
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(angleToTargetCoords, angularTurnSpeed).ToRotationVector2() * Projectile.velocity.Length();

            // If not super close to the target but the target is very much in the line of sight of the summon, charge.
            if (!Projectile.WithinRange(target.Center, 75f) && Vector2.Dot(LAPUtilities.GetVector2(Projectile.Center, target.Center), Projectile.velocity.SafeNormalize(Vector2.Zero)) > 0.85f)
            {
                Projectile.velocity = LAPUtilities.GetVector2(Projectile.Center, target.Center) * 36f;
                AttackDelay = 15f;

                Projectile.netUpdate = true;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public void ReturnToRestingPosition()
        {
            Projectile.rotation = Projectile.rotation.AngleTowards(RestOffsetAngle, 0.25f);

            // Rapidly approach the resting position.
            Vector2 destination = Owner.Center + Vector2.UnitY.RotatedBy(RestOffsetAngle) * -120f;
            Projectile.velocity = (destination - Projectile.Center) / 10f;
        }
        public static void DistanceClamp(ref Vector2 start, ref Vector2 end, float maxDistance)
        {
            if (Vector2.Distance(end, start) > maxDistance)
            {
                end = start + Vector2.Normalize(end - start) * maxDistance;
            }
        }
        public override void PostDraw(Color lightColor)
        {
            for (int i = 1; i < Projectile.oldPos.Length; i++)
                DistanceClamp(ref Projectile.oldPos[i - 1], ref Projectile.oldPos[i], 6f);

            Texture2D trailSegmentTexture = LAPTextureRegister.StarProj.Value;
            for (int i = 2; i < Projectile.oldPos.Length; i++)
            {
                float completionRatio = i / (float)Projectile.oldPos.Length;
                float rotation = (Projectile.oldPos[i - 1] - Projectile.oldPos[i]).ToRotation() + MathHelper.PiOver2;
                float scale = MathHelper.Lerp(0.7f, 0.1f, completionRatio) * Projectile.scale;
                Color color = Color.Lerp(Color.LightPink, Color.Goldenrod, completionRatio * 3f % 1f) * 1.5f;

                // Become dimmer the slower the projectile is moving.
                color *= Utils.GetLerpValue(1f, 8f, Projectile.velocity.Length(), true);

                Main.EntitySpriteDraw(trailSegmentTexture,
                                 Projectile.oldPos[i] + Projectile.Size * 0.5f + new Vector2(0f, 8f).RotatedBy(Projectile.rotation) - Main.screenPosition,
                                 null,
                                 color,
                                 rotation,
                                 trailSegmentTexture.Size() * 0.5f,
                                 scale,
                                 SpriteEffects.None,
                                 0);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffType<CIHolyFlames>(), 180);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffType<CIHolyFlames>(), 180);
    }
}
