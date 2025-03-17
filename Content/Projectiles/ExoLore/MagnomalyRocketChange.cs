using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoLore
{
    public class MagnomalyRocketChange : GlobalProjectile
    {

        private bool spawnedAura = false;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ProjectileType<MagnomalyRocket>();
        }
        public override bool? CanHitNPC(Projectile projectile, NPC target)
        {
            if (projectile.ai[1] == 1f)
                return projectile.timeLeft < 275;
            else
                return null;
        }

        public override bool PreAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];

            var CIPlayer = player.CIMod();

            //Lighting
            Lighting.AddLight(projectile.Center, Main.DiscoR * 0.25f / 255f, Main.DiscoG * 0.25f / 255f, Main.DiscoB * 0.25f / 255f);

            //Animation
            projectile.frameCounter++;
            if (projectile.frameCounter > 7)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }

            //Rotation
            projectile.spriteDirection = projectile.direction = (projectile.velocity.X > 0).ToDirectionInt();
            projectile.rotation = projectile.velocity.ToRotation() + (projectile.spriteDirection == 1 ? 0f : MathHelper.Pi) + MathHelper.ToRadians(90) * projectile.direction;

            if (CIPlayer.LoreExo || CIPlayer.PanelsLoreExo)
            {
                projectile.extraUpdates = 1;
                if(projectile.timeLeft < 275)
                CIFunction.HomeInOnNPC(projectile, true, 2500f, 12f, 20f, 10f);
            }
            else
                CIFunction.HomeInOnNPC(projectile, true, 300f, 12f, 20f);

            int dustType = Main.rand.NextBool() ? 107 : 234;
            if (Main.rand.NextBool(4))
            {
                dustType = CIDustID.DustSandnado;
            }
            if (projectile.owner == Main.myPlayer && !spawnedAura)
            {
                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<MagnomalyAura>(), (int)(projectile.damage * 0.5f), projectile.knockBack * 0.5f, projectile.owner, projectile.identity, 0f);
                spawnedAura = true;
            }
            float dustOffsetX = projectile.velocity.X * 0.5f;
            float dustOffsetY = projectile.velocity.Y * 0.5f;
            if (Main.rand.NextBool())
            {
                int exo = Dust.NewDust(new Vector2(projectile.position.X + 3f + dustOffsetX, projectile.position.Y + 3f + dustOffsetY) - projectile.velocity * 0.5f, projectile.width - 8, projectile.height - 8, dustType, 0f, 0f, 100, default, 0.5f);
                Main.dust[exo].scale *= (float)Main.rand.Next(10) * 0.1f;
                Main.dust[exo].velocity *= 0.2f;
                Main.dust[exo].noGravity = true;
                Main.dust[exo].noLight = true;
            }
            else
            {
                int exo = Dust.NewDust(new Vector2(projectile.position.X + 3f + dustOffsetX, projectile.position.Y + 3f + dustOffsetY) - projectile.velocity * 0.5f, projectile.width - 8, projectile.height - 8, dustType, 0f, 0f, 100, default, 0.25f);
                Main.dust[exo].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
                Main.dust[exo].velocity *= 0.05f;
                Main.dust[exo].noGravity = true;
                Main.dust[exo].noLight = true;
            }
            return false;
        }
        public override void OnKill(Projectile projectile, int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            var CIPlayer = player.CIMod();

            SoundEngine.PlaySound(CIPlayer.LoreExo || CIPlayer.PanelsLoreExo ? CISoundMenu.MagnomalyHitsound.WithVolumeScale(0.8f) : SoundID.Item14, projectile.Center);
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];

            var CIPlayer = player.CIMod();
            if (CIPlayer.LoreExo || CIPlayer.PanelsLoreExo)
            {
                if (projectile.owner == Main.myPlayer && projectile.ai[1] != 1f)
                {
                    int numberOfProjectiles = 8;
                    float spreadAngle = MathHelper.ToRadians(360);
                    float baseAngle = projectile.velocity.ToRotation();
                    float angleStep = spreadAngle / (numberOfProjectiles - 1);

                    for (int i = 0; i < numberOfProjectiles; i++)
                    {
                        float randomOffset = Main.rand.NextFloat(-MathHelper.ToRadians(10), MathHelper.ToRadians(10));
                        float currentAngle = baseAngle - spreadAngle / 2 + (angleStep * i) + randomOffset;
                        Vector2 direction = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));

                        float randomSpeed = Main.rand.NextFloat(6f, 8f);

                        Vector2 randomizedVelocity = direction * randomSpeed;

                        int proj1 = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, randomizedVelocity, ModContent.ProjectileType<MagnomalyRocket>(), (int)(projectile.damage * 0.9), projectile.knockBack, projectile.owner, 0f, 1f);
                        Main.projectile[proj1].ai[1] = 1f;
                    }
                }
            }
            if (projectile.owner == Main.myPlayer)
            {
                int numberOfBeam = projectile.ai[1] == 1f ? 1 : 4;
                float random = Main.rand.Next(30, 90);
                float spread = random * 0.0174f;
                double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 8f;
                for (int i = 0; i < numberOfBeam; i++)
                {
                    double offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    int proj1 = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<MagnomalyBeam>(), projectile.damage / 4, projectile.knockBack / 4, projectile.owner, 0f, 1f);
                    int proj2 = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<MagnomalyBeam>(), projectile.damage / 4, projectile.knockBack / 4, projectile.owner, 0f, 1f);
                }
            }

            if (CIPlayer.LoreExo || CIPlayer.PanelsLoreExo)
                target.ExoDebuffs();
            else
                target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
        }

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            Player player = Main.player[projectile.owner];

            var CIPlayer = player.CIMod();

            if (projectile.owner == Main.myPlayer)
            {
                float random = Main.rand.Next(30, 90);
                float spread = random * 0.0174f;
                double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 8f;
                for (int i = 0; i < 4; i++)
                {
                    double offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    int proj1 = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<MagnomalyBeam>(), projectile.damage / 4, projectile.knockBack / 4, projectile.owner, 0f, 1f);
                    int proj2 = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<MagnomalyBeam>(), projectile.damage / 4, projectile.knockBack / 4, projectile.owner, 0f, 1f);
                }
            }
            if (CIPlayer.LoreExo || CIPlayer.PanelsLoreExo)
            {
                if (projectile.owner == Main.myPlayer && projectile.ai[1] != 1f)
                {
                    float spread = Main.rand.Next(30, 90) * 0.0174f;
                    double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - (double)(spread / 2f);
                    double deltaAngle = spread / 8f;
                    for (int i = 0; i < 8; i++)
                    {
                        double offsetAngle = startAngle + deltaAngle * (i + i * i) / 2.0 + (double)(32f * i);
                        Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, (float)((double)(0f - (float)Math.Sin(offsetAngle)) * 5.0), (float)((double)(0f - (float)Math.Cos(offsetAngle)) * 5.0), ModContent.ProjectileType<MagnomalyRocket>(), (int)((float)projectile.damage * 0.9f), projectile.knockBack / 4f, projectile.owner, 0f, 1f);
                    }
                }
            }

            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
        }
    }
}
