using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Core.Path;
using CalamityInheritance.Core.Utils;
using CalamityMod;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray
{
    public class ElementalBeamMain : BaseLaserbeam, ILocalizedModType
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
            // Generate a burst of bubble-like nebula dust.
            if (!Main.dedServ && Time == 5f)
            {
                int totalBubbles = 24;
                for (int i = 0; i < totalBubbles; i++)
                {
                    Dust nebulaBubble = Dust.NewDustPerfect(Projectile.Center, DustID.DungeonSpirit);
                    nebulaBubble.velocity = Main.rand.NextVector2Circular(6f, 6f);
                    nebulaBubble.scale = Main.rand.NextFloat(2f, 3f);
                    nebulaBubble.noGravity = true;
                    nebulaBubble.color = Color.White;
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
                        Dust dust = Dust.NewDustPerfect(dustPosition, Main.rand.NextBool() ? 107 : 180, default, default, Color.White);
                        dust.noGravity = true;
                        dust.velocity =Vector2.Zero;
                        dust.scale = Main.rand.NextFloat(1f, 1.2f);
                    }
                }
                for (int i = 0; i < 5; i++)
                {
                    Vector2 SpawnPos = BeginPos + Vel * i * 50;
                    NPC target = LAPUtilities.FindClosestTarget(SpawnPos, 350);
                    if (target is not null)
                    {
                        Vector2 ProjPos = target.Center + new Vector2(Main.rand.Next(-300, 300), -800 + Main.rand.Next(-100, 100));
                        Vector2 ProjVel = LAPUtilities.GetVector2(ProjPos, target.Center + Main.rand.NextVector2Circular(12, 12));
                        Vector2 ProjEndPos = ProjPos + ProjVel * 1500;
                        Projectile.NewProj(ProjectileType<ElementalLightning>(), ProjPos, ProjVel, 1f, 1f, ProjEndPos.X, ProjEndPos.Y);
                    }
                }
                for (int i = 0; i < 8; i++)
                {
                    Vector2 SpawnPos = BeginPos + Vel * i * 50;
                    Projectile.NewProj(ProjectileType<ElementalStar>(), SpawnPos, Vector2.Zero, 1f, 1f);
                }
            }
        }

        public override void DetermineScale() => Projectile.scale = Projectile.timeLeft / Lifetime * MaxScale;

        public override bool PreDraw(ref Color lightColor)
        {
            DrawBeamWithColor(Color.White * 0.9f, Projectile.scale);
            DrawBeamWithColor(Color.White * 0.9f, Projectile.scale * 0.5f);
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
            target.immune[Projectile.owner] = 8;
            if (ShardCooldown > 0f)
                return;

            // The "Center" of the laser is actually the start of it in this context.
            // Collision is done separately. This might have a slight offset due to collision
            // boxes, but that should be negligible.
            float lengthFromStart = Projectile.Distance(target.Center);

            int totalShards = (int)MathHelper.Lerp(4, 7, MathHelper.Clamp(lengthFromStart / MaxLaserLength * 1.5f, 0f, 1f));
            int nebulaCounts = 8;
            float rotFactor = 360f / nebulaCounts;
            for (int j = 0; j < nebulaCounts; j++)
            {
                float newRotation = MathHelper.ToRadians(j * rotFactor);
                Vector2 pPos = new Vector2(18f, 0f).RotatedBy(newRotation);
                Vector2 pVel = new Vector2(18f, 0f).RotatedBy(newRotation);
                int nP = Projectile.NewProjectile(Projectile.GetSource_FromThis(), pPos, pVel * 0.6f, ProjectileType<ElementalNebula>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[nP].scale *= 1.5f;

            }
            int shardType = ProjectileType<ElementalNebula>();
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

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center + spawnOffset, Main.rand.NextVector2CircularEdge(24f, 24f), shardType, shardDamage, Projectile.knockBack, Projectile.owner);
            }

            ShardCooldown = 3f;
            Projectile.netUpdate = true;
            if (!Main.dedServ && Time == 5f)
            {
                int starPoints = 8;
                for (int i = 0; i < starPoints; i++)
                {
                    float angle = MathHelper.TwoPi * i / starPoints;
                    for (int j = 0; j < 12; j++)
                    {
                        float starSpeed = MathHelper.Lerp(2f, 10f, j / 12f);
                        Color dustColor = Color.Lerp(Color.White, Color.Yellow, j / 12f);
                        float dustScale = MathHelper.Lerp(1.6f, 0.85f, j / 12f);

                        Dust fire = Dust.NewDustPerfect(Projectile.Center, DustID.Torch);
                        fire.velocity = angle.ToRotationVector2() * starSpeed;
                        fire.color = dustColor;
                        fire.scale = dustScale;
                        fire.noGravity = true;
                    }
                }
            }

            int type = ProjectileID.Volcano;
            int boomDamage = (int)(hit.Damage * 1.1);
            int boom = Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, type, boomDamage, hit.Knockback, Projectile.owner, 0f, Main.rand.NextFloat(0.85f, 2f));
            Main.projectile[boom].DamageType = DamageClass.Magic;
            Main.projectile[boom].velocity = new Vector2(Main.rand.NextFloat(-1.1f, 1.1f), -Main.rand.NextFloat(1.4f, 2.4f));
            Main.projectile[boom].scale *= 1.2f;
        }
    }
}
