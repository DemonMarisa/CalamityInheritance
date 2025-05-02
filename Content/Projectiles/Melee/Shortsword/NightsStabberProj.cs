using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Projectiles.BaseProjectiles;
using Terraria.GameContent.Drawing;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using Terraria.Audio;
using System.IO;

namespace CalamityInheritance.Content.Projectiles.Melee.Shortsword
{
    public class NightsStabberProj : BaseShortswordProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NoMeleeSpeedVelocityScaling[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(17);
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>(); ;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void SetVisualOffsets()
        {
            const int HalfSpriteWidth = 28 / 2;
            const int HalfSpriteHeight = 34 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
        }
        public override void ExtraBehavior()
        {
            if (Main.rand.NextBool(3))
            {
                int Demonite = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Demonite, Projectile.direction * 4, 0f, 15, default, 1.3f);
            }
        }
        public override Action<Projectile> EffectBeforePullback => (proj) =>
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 6f, ModContent.ProjectileType<NightStabber2>(), Projectile.damage * 1, Projectile.knockBack, Projectile.owner, 0f, 0f);
        };
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
            CalamityInheritancePlayer modPlayer = player.CIMod();
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

                int newProjectileId1 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, tentacleVelocity, ProjectileID.ShadowFlame, Projectile.damage / 4, Projectile.knockBack, Projectile.owner, tentacleXDirection, tentacleYDirection);
                Main.projectile[newProjectileId1].DamageType = DamageClass.Melee;
                //取消local无敌帧
                Main.projectile[newProjectileId1].usesLocalNPCImmunity = false;
                //启用静态无敌帧
                Main.projectile[newProjectileId1].usesIDStaticNPCImmunity = true;
                //给予10
                Main.projectile[newProjectileId1].idStaticNPCHitCooldown = 10;
            }
        }
    }
}
