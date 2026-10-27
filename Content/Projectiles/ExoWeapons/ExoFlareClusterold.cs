using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Core.Path;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons
{
    public class ExoFlareClusterold : BaseStickyProj
    {
        public override string LocalizationCategory => LocalizationPath.ExoProj;
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public const float MinDistanceFromTarget = 45f;
        public const float MaxDistanceFromTarget = 1350f;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 180;
            Projectile.extraUpdates = 1;
            Projectile.MaxUpdates = 1;
        }

        public override void ExAI()
        {
            // localAI[0] is used by the sticky AI method, so use localAI[1] to spawn the flares.
            if (Projectile.localAI[1] == 0f)
            {
                if (Projectile.IsLocalPlayer())
                {
                    int projID = ProjectileType<ExoFlareold>();
                    int flareDamage = (int)(0.6f * Projectile.damage);
                    float flareKB = Projectile.knockBack;
                    for (int i = 0; i < 4; i++)
                    {
                        Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, projID, flareDamage, flareKB, Projectile.owner);
                        p.ai[2] = Projectile.GetByUUID(Projectile.owner, Projectile.whoAmI);
                    }
                }
                Projectile.localAI[1] = 1f;
            }

            if (Projectile.ai[0] == 0f)
            {
                NPC potentialTarget = LAPUtilities.FindClosestTarget(Projectile.Center, MaxDistanceFromTarget);
                if (potentialTarget != null)
                {
                    if (Projectile.Distance(potentialTarget.Center) > MinDistanceFromTarget)
                    {
                        float angleOffset = Projectile.AngleTo(potentialTarget.Center) - Projectile.velocity.ToRotation();
                        angleOffset = MathHelper.WrapAngle(angleOffset);
                        Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.Clamp(angleOffset, -0.1f, 0.1f));
                    }
                }
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
        }

        public override void ExOnHit(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.width == 50)
            {
                int width = (int)MathHelper.Min(target.Hitbox.Width, 60);
                int height = (int)MathHelper.Min(target.Hitbox.Height, 60);
                Projectile.Resize(width, height);
            }
            target.AddBuff(BuffType<CIMiracleBlight>(), 300);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffType<CIMiracleBlight>(), 300);
        }
    }
}
