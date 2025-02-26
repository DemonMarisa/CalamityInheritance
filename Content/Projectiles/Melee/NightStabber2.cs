using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using CalamityInheritance.CIPlayer;
using System;
using CalamityInheritance.Utilities;
using Terraria.Audio;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class NightStabber2 : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 dustSpawnPos = Projectile.position - Projectile.velocity * i / 2f;
                    Dust corruptMagic = Dust.NewDustPerfect(dustSpawnPos, 27);
                    corruptMagic.color = Color.Lerp(Color.Fuchsia, Color.Magenta, Main.rand.NextFloat(0.6f));
                    corruptMagic.scale = Main.rand.NextFloat(0.96f, 1.04f);
                    corruptMagic.noGravity = true;
                    corruptMagic.velocity *= 0.1f;
                }
            }
            CalamityUtils.HomeInOnNPC(Projectile, true, 600f, 12f, 20f);
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Demonite, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item103, target.Center);
            // Vanilla has several particles that can easily be used anywhere.
            // The particles from the Particle Orchestra are predefined by vanilla and most can not be customized that much.
            // Use auto complete to see the other ParticleOrchestraType types there are.
            // Here we are spawning the Excalibur particle randomly inside of the target's hitbox.
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.NightsEdge,
                new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) },
                Projectile.owner);

            // You could also spawn dusts at the enemy position. Here is simple an example:
            int dustIndex = Dust.NewDust(Main.rand.NextVector2FromRectangle(target.Hitbox), 0, 0, DustID.Enchanted_Gold);
            Main.dust[dustIndex].noGravity = true;

            // Set the target's hit direction to away from the player so the knockback is in the correct direction.
            hit.HitDirection = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);

            int tentacleNum = 3;

            Player player = Main.player[Projectile.owner];
            CalamityInheritancePlayer modPlayer = player.CalamityInheritance();
            for (int i = 0; i < tentacleNum; i++)
            {
                float randomAngle = Main.rand.NextFloat(0f, MathHelper.TwoPi);
                Vector2 tentacleVelocity = new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));

                Vector2 tentacleRandVelocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                tentacleRandVelocity.Normalize();
                tentacleVelocity = tentacleVelocity * 4f + tentacleRandVelocity;
                tentacleVelocity.Normalize();
                tentacleVelocity *= 5f;

                float tentacleYDirection = Main.rand.Next(10, 80) * 0.001f;
                if (Main.rand.NextBool())
                {
                    tentacleYDirection *= -1f;
                }
                float tentacleXDirection = Main.rand.Next(10, 80) * 0.001f;
                if (Main.rand.NextBool())
                {
                    tentacleXDirection *= -1f;
                }

                int newProjectileId1 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, tentacleVelocity, ProjectileID.ShadowFlame, Projectile.damage / 4, Projectile.knockBack, Projectile.owner, tentacleXDirection, tentacleYDirection);
                Main.projectile[newProjectileId1].DamageType = DamageClass.Melee;
            }
        }
    }
}
