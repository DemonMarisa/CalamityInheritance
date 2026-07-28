using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Core.Path;
using CalamityInheritance.Core.Utils;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray
{
    public class TerraBeamMain : BaseLaserbeam, ILocalizedModType
    {
        public new string LocalizationCategory => LocalizationPath.MagicProj;
        public override float MaxScale => 1f;
        public override float MaxLaserLength => 1500f;
        public override float Lifetime => 30f;
        public override Color LightCastColor => Color.White;
        public override Texture2D LaserBeginTexture => LAPTextureRegister.UltimaRayStart.Value;
        public override Texture2D LaserMiddleTexture => LAPTextureRegister.UltimaRayMid.Value;
        public override Texture2D LaserEndTexture => LAPTextureRegister.UltimaRayEnd.Value;

        public ref float ShardCooldown => ref Projectile.ai[1];
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
            Projectile.tileCollide = false;
            Projectile.timeLeft = (int)Lifetime;
        }

        public override void ExtraBehavior()
        {
            // Generate a star-like and circular burst of terra dust.
            if (!Main.dedServ && Time == 5f)
            {
                int starPoints = 6;
                for (int i = 0; i < starPoints; i++)
                {
                    float angle = MathHelper.TwoPi * i / starPoints;
                    for (int j = 0; j < 12; j++)
                    {
                        float starSpeed = MathHelper.Lerp(1f, 7f, j / 12f);
                        Color dustColor = Color.Lerp(Color.White, Color.YellowGreen, j / 12f);
                        float dustScale = MathHelper.Lerp(1.6f, 0.85f, j / 12f);

                        Dust terraMagic = Dust.NewDustPerfect(Projectile.Center, DustID.Terra);
                        terraMagic.velocity = angle.ToRotationVector2() * starSpeed;
                        terraMagic.color = dustColor;
                        terraMagic.scale = dustScale;
                        terraMagic.noGravity = true;
                    }
                }

                int ovalPoints = 42;
                for (int i = 0; i < ovalPoints; i++)
                {
                    float angle = MathHelper.TwoPi * i / ovalPoints;
                    Dust terraMagic = Dust.NewDustPerfect(Projectile.Center, DustID.Terra);
                    terraMagic.velocity = angle.ToRotationVector2() * 6f;
                    terraMagic.scale = 1.1f;
                    terraMagic.noGravity = true;
                }
                Vector2 BeginPos = Projectile.Center;
                Vector2 Vel = Projectile.velocity.SafeNormalize(Vector2.UnitX) * 7f;
                for (int i = 0; i < 220; i++)
                {
                    Lighting.AddLight(BeginPos + Vel * i, 0.6f, 0.2f, 0.9f);
                    float radiusFactor = MathHelper.Lerp(0f, 1f, Utils.GetLerpValue(1f, 7f, i, true));
                    for (int j = 0; j < 4; j++)
                    {
                        float offsetRotationAngle = Vel.ToRotation() + j / 7f;
                        float radius = (7f + (float)Math.Cos(i / 4f) * 3f) * radiusFactor;
                        Vector2 dustPosition = BeginPos + Vel * i;
                        dustPosition += offsetRotationAngle.ToRotationVector2().RotatedBy(j / 5f * MathHelper.TwoPi) * radius;
                        Dust dust = Dust.NewDustPerfect(dustPosition, DustID.Terra, default, default, Color.Green);
                        dust.noGravity = true;
                        dust.velocity = Vector2.Zero;
                        dust.scale = Main.rand.NextFloat(1f, 1.2f);
                    }
                }
                for (int i = 0; i < 8; i++)
                {
                    Vector2 SpawnPos = BeginPos + Vel * i * 50;
                    NPC target = LAPUtilities.FindClosestTarget(SpawnPos, 600);
                    if (target is not null)
                    {
                        Vector2 vel = LAPUtilities.GetVector2(SpawnPos, target.Center) * 12f;
                        Projectile.NewProj(ProjectileType<TerraBolt>(), SpawnPos, vel, 1f, 1f);
                    }
                }
            }
            if (Time == 26)
            {
                Vector2 BeginPos = Projectile.Center;
                Vector2 Vel = Projectile.velocity.SafeNormalize(Vector2.UnitX) * 7f;
                for (int i = 0; i < 8; i++)
                {
                    Vector2 SpawnPos = BeginPos + Vel * i * 50;
                    Projectile.NewProj(ProjectileType<TerraShard>(), SpawnPos, Vector2.Zero, 1f, 1f);
                }
            }
            if (ShardCooldown > 0f)
                ShardCooldown--;
        }

        public override void DetermineScale() => Projectile.scale = Projectile.timeLeft / Lifetime * MaxScale;

        public override bool PreDraw(ref Color lightColor)
        {
            DrawBeamWithColor(Color.Lime * 0.9f, Projectile.scale);
            DrawBeamWithColor(Color.Yellow * 0.9f, Projectile.scale * 0.5f);
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Projectile.numHits > 0)
                Projectile.damage = (int)(Projectile.damage * 0.95f);
            if (Projectile.damage < 1)
                Projectile.damage = 1;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (ShardCooldown > 0f)
                return;

            // The "Center" of the laser is actually the start of it in this context.
            // Collision is done separately. This might have a slight offset due to collision
            // boxes, but that should be negligible.
            float lengthFromStart = Projectile.Distance(target.Center);

            int totalShards = (int)MathHelper.Lerp(1, 3, MathHelper.Clamp(lengthFromStart / MaxLaserLength * 1.5f, 0f, 1f));
            int shardType = ProjectileType<TerraShard>();
            int shardDamage = (int)(Projectile.damage * 0.5);
            for (int i = 0; i < totalShards; i++)
            {
                int tries = 0;
                Vector2 spawnOffset;
                do
                {
                    spawnOffset = Main.rand.NextVector2CircularEdge(target.width * 0.5f + 40f, target.height * 0.5f + 40f);
                    tries++;
                }
                while (Collision.SolidCollision((target.Center + spawnOffset).ToTileCoordinates().ToVector2(), 4, 4) && tries < 10);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center + spawnOffset, Main.rand.NextVector2CircularEdge(6f, 6f), shardType, shardDamage, Projectile.knockBack, Projectile.owner);

            }

            ShardCooldown = 3f;
            Projectile.netUpdate = true;
        }
    }
}
