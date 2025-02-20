using CalamityMod.Projectiles.Rogue;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using CalamityInheritance.Utilities;
using CalamityMod.Projectiles.Magic;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class PhantasmalRuinProjold : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Rogue/PhantasmalRuinold";

        private const int Lifetime = 600;
        private const int FramesPerSubProjectile = 13;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = Lifetime;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 3);
            return false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.SpectreStaff, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0, default(Color), 0.85f);
            if (Projectile.timeLeft % 18 == 0)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Projectile.Calamity().stealthStrike)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, ModContent.ProjectileType<PhantasmalRuinGhost>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner);
                    }
                    else
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Center.X, Projectile.Center.Y, 0f, Main.rand.NextFloat(-2, 2), ModContent.ProjectileType<LostSoulFriendly>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, 1f);
                    }
                }
            }
            bool shouldFireSubProjectile = (Lifetime - Projectile.timeLeft) % (Projectile.MaxUpdates * FramesPerSubProjectile) == 8;
            if (Projectile.owner == Main.myPlayer && shouldFireSubProjectile)
            {
                bool ss = Projectile.Calamity().stealthStrike;
                int soulDamage = (int)(Projectile.damage * 0.7f);
                int projID = ss ? ModContent.ProjectileType<PhantasmalRuinGhost>() : ModContent.ProjectileType<LostSoulFriendly>();
                int soul = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Phantom>(), soulDamage, Projectile.knockBack, Projectile.owner);
                Main.projectile[soul].DamageType = ModContent.GetInstance<RogueDamageClass>();
                int damage = (int)(Projectile.damage * 0.25f);
                float kb = Projectile.knockBack * (ss ? 1f : 0.25f);
                Vector2 velocity = ss
                    ? (Projectile.velocity * 0.4f).RotatedBy(Main.rand.NextFloat(-0.04f, 0.04f))
                    : (Projectile.velocity * 0.08f) + Main.rand.NextVector2Circular(0.4f, 0.4f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, projID, damage, kb, Projectile.owner);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => OnHitEffects();
        public override void OnHitPlayer(Player target, Player.HurtInfo info) => OnHitEffects();

        private void OnHitEffects()
        {
            SoundEngine.PlaySound(SoundID.NPCDeath39 with { PitchVariance = 0.4f }, Projectile.position);
            int numberOfProjectiles = Main.rand.Next(10, 12);
            float spreadAngle = MathHelper.ToRadians(Main.rand.Next(25, 30));
            float baseAngle = Projectile.velocity.ToRotation();

            float angleStep = spreadAngle / (numberOfProjectiles - 1);

            for (int i = 0; i < numberOfProjectiles; i++)
            {
                float randomOffset = Main.rand.NextFloat(-MathHelper.ToRadians(2), MathHelper.ToRadians(1));
                float currentAngle = baseAngle - spreadAngle / 2 + (angleStep * i) + randomOffset;
                Vector2 direction = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));

                float angleFromBase = Math.Abs(MathHelper.ToDegrees(currentAngle - baseAngle));
                float randomSpeed;
                if (angleFromBase < 1f)
                {
                    randomSpeed = Main.rand.NextFloat(55f);
                }
                if (angleFromBase < 8f)
                {
                    randomSpeed = Main.rand.NextFloat(30f, 45f);
                }
                else
                {
                    randomSpeed = Main.rand.NextFloat(15f, 25f);
                }

                Vector2 randomizedVelocity = direction * randomSpeed;

                int newProjectileId = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, -randomizedVelocity, ModContent.ProjectileType<PhantasmalSoulold>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner);

            }
        }
    }
}
