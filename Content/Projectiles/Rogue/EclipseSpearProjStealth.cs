using System;
using CalamityInheritance.Content.Items.Weapons;
using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class EclipseSpearProjStealth : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => $"{Generic.WeaponRoute}/Rogue/EclipseSpear";

        public const float RainDamageMult = 0.2f;
        public const float ExplosionDamageMult = 0.5f;
        #region 别名
        public ref float AttackType => ref Projectile.ai[0];
        #endregion
        #region 攻击枚举
        const float NonStick = 0f;
        const float IsStick = 1f;
        #endregion

        private bool changedTimeLeft = false;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 6;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.MaxUpdates = 2;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 150 * Projectile.MaxUpdates;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0.8f, 0.3f);
            Projectile.StickyProjAI(10);
            switch (AttackType)
            {
                case NonStick:
                    DoNonStick();
                    break;
                case IsStick:
                    DoStick();
                    break;
            }
        }

        private void DoNonStick()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (Main.rand.NextBool(5))
            {
                Vector2 trailPos = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * Main.rand.NextFloat(-16f, 16f);
                float trailScale = Main.rand.NextFloat(0.8f, 1.2f);
                Color trailColor = Main.rand.NextBool() ? Color.Indigo : Color.DarkOrange;
                Particle eclipseTrail = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, trailColor);
                GeneralParticleHandler.SpawnParticle(eclipseTrail);
            }
        }

        private void DoStick()
        {
            if (!changedTimeLeft)
            {
                Projectile.MaxUpdates = 1;
                Projectile.timeLeft = 600;
                changedTimeLeft = true;
            }

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.localAI[1] -= 1f;

                var source = Projectile.GetSource_FromThis();
                if (Projectile.timeLeft % 20 == 0)
                {
                    int type = ModContent.ProjectileType<EclipseSpearSmall>();
                    for (int i = 0; i < 3; ++i)
                        CalamityUtils.ProjectileRain(source, Projectile.Center, 400f, 100f, 500f, 800f, 29f, type, (int)(Projectile.damage * RainDamageMult), Projectile.knockBack * RainDamageMult, Projectile.owner);
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) => Projectile.ModifyHitNPCSticky(1);

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            }
            return null;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Type], lightColor, 1);
            return false;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.ai[0] == 1f ? false : base.CanHitNPC(target);

        public override bool CanHitPvp(Player target) => Projectile.ai[0] != 1f;
    }
}