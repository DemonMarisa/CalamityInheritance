using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.Rogue.Boomerang;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Rogue.Boomerang
{
    public class ToxicantTwisterProjLegacy : CIRogueProj
    {
        public override string Texture => GetInstance<ToxicantTwisterLegacy>().Texture;
        private int lifeTime = 300;
        private int targetIndex = -1;

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = RogueDamage.Instance;
            Projectile.width = 42;
            Projectile.height = 46;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = lifeTime;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }

        public override void AI()
        {
            if (Projectile.CI().Stealth)
            {
                if (Projectile.timeLeft % 10 == 0 && Main.myPlayer == Projectile.owner)
                {
                    for (int i = 0; i < 2; i++)
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedByRandom(0.1f) * -0.6f, ProjectileType<ToxicantTwisterDustLegacy>(), (int)(Projectile.damage * 0.25), 0f, Projectile.owner);
                }
                Projectile.rotation += 0.06f * (Projectile.velocity.X > 0).ToDirectionInt();
            }
            // Boomerang rotation
            Projectile.rotation += 0.4f * Projectile.direction;

            // Boomerang sound
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }

            Projectile.ai[1]++;
            if (Projectile.ai[0] != 0f)
            {
                float returnSpeed = 30f;
                float acceleration = 1.4f;

                Player owner = Main.player[Projectile.owner];

                // Delete the projectile if it's excessively far away.
                Vector2 projVector = owner.Center - Projectile.Center;
                float dist = projVector.Length();
                if (dist > 3000f)
                    Projectile.Kill();

                dist = returnSpeed / dist;
                projVector.X *= dist;
                projVector.Y *= dist;

                // Home back in on the player.
                if (Projectile.velocity.X < projVector.X)
                {
                    Projectile.velocity.X += acceleration;
                    if (Projectile.velocity.X < 0f && projVector.X > 0f)
                        Projectile.velocity.X += acceleration;
                }
                else if (Projectile.velocity.X > projVector.X)
                {
                    Projectile.velocity.X -= acceleration;
                    if (Projectile.velocity.X > 0f && projVector.X < 0f)
                        Projectile.velocity.X -= acceleration;
                }
                if (Projectile.velocity.Y < projVector.Y)
                {
                    Projectile.velocity.Y += acceleration;
                    if (Projectile.velocity.Y < 0f && projVector.Y > 0f)
                        Projectile.velocity.Y += acceleration;
                }
                else if (Projectile.velocity.Y > projVector.Y)
                {
                    Projectile.velocity.Y -= acceleration;
                    if (Projectile.velocity.Y > 0f && projVector.Y < 0f)
                        Projectile.velocity.Y -= acceleration;
                }

                // Delete the projectile if it touches its owner.
                if (Main.myPlayer == Projectile.owner)
                    if (Projectile.Hitbox.Intersects(owner.Hitbox))
                        Projectile.Kill();
            }
            else if (Projectile.ai[1] > 40f)
            {
                NPC closestTarget = LAPUtilities.FindClosestTarget(Projectile.Center,1769f, true);
                if (closestTarget != null)
                {
                    Projectile.extraUpdates = 1;
                    targetIndex = closestTarget.whoAmI;
                    float inertia = 20f;
                    float homingVelocity = Projectile.CI().Stealth ? 30f : 20f;
                    Vector2 moveDirection = LAPUtilities.GetVector2(Projectile.Center, closestTarget.Center);

                    Projectile.velocity = (Projectile.velocity * inertia + moveDirection * homingVelocity) / (inertia + 1f);
                }
            }
            else
            {
                Projectile.velocity *= 0.985f;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Projectile.ai[1] <= 40f && Projectile.ai[0] != 1f)
            {
                modifiers.SourceDamage *= 0.3333f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (targetIndex == target.whoAmI)
                Projectile.ai[0] = 1f;

            target.AddBuff(BuffType<CIIrradiated>(), 180);
            SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
            for (int k = 0; k < 10; k++)
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.CursedTorch, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
        }
    }
}
