using CalamityInheritance.Content.Items.Weapons.Melee.Swords.AOTCNew;
using CalamityInheritance.Content.Projectiles.Melee.AOTCNew;
using CalamityInheritance.Texture;
using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Sounds;
using LAP.Assets.TextureRegister;
using LAP.Content.Particles;
using LAP.Core.AnimationHandle;
using LAP.Core.Enums;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.AOTCNew
{
    public class AOTCNewBlast : ModProjectile, ILocalizedModType
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<ArkoftheCosmosNew>();
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public AniHelper AniHelper = new AniHelper(3);
        public Vector2 BeginPos;
        public Vector2 EndPos;
        public float TargetRot;
        public float RotOffset;
        public bool AddEndParticle;
        public bool dashable;
        public List<Vector2> ParticleDrawPos = [];
        public override void SetStaticDefaults()
        {
            Projectile.AddHeldProj();
        }
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.netImportant = true;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 140;
            Projectile.scale = 1.5f;
        }
        public override void AI()
        {
            if (Projectile.LAP().FirstFrame)
            {
                Init();
            }
            AniHelper.UpDateAni(AniState.Begin, 10);
            float progress = AniHelper.GetProgress(AniState.Begin);
            Projectile.Center = Vector2.Lerp(BeginPos, EndPos, EasingHelper.EaseInCubic(progress));
            RotOffset = MathHelper.Lerp(0.7f, 0f, EasingHelper.EaseInCubic(progress));
            if (AniHelper.Auxfloat[AniState.Begin] < 1f && progress > 0.45f)
                ParticleDrawPos.Add(Projectile.Center + new Vector2(50, 0).RotatedBy(Projectile.rotation));
            else if (progress > 0.45f)
            {
                for (int m = 0; m < 2; m++)
                {
                    Vector2 smokePosition = Vector2.Lerp(BeginPos, EndPos, Main.rand.NextFloat());
                    HeavySmokeParticle smoke = new(smokePosition, Main.rand.NextVector2CircularEdge(4f, 4f), new Color(117, 36, 32), 12, 0.8f, 0.6f, 0f, true);
                    GeneralParticleHandler.SpawnParticle(smoke);
                }
            }
            if (!AddEndParticle && AniHelper.HasFinish[AniState.Begin])
            {
                Vector2 vel = new Vector2(60, 0).RotatedBy(Projectile.rotation);
                Vector2 beginPos = Projectile.Center;
                for (int i = 0; i < 11; i++)
                {
                    beginPos = beginPos - vel;
                    new LAP.Content.Particles.SparkParticle(beginPos, Projectile.rotation.ToRotationVector2(), false, 60, 3f, Color.White).Spawn();
                }
                SoundEngine.PlaySound(CommonCalamitySounds.SwiftSliceSound with { Pitch = 0f, MaxInstances = 2 }, Projectile.Center);
                Main.LocalPlayer.SetScreenshake(7.5f);
                for (int i = 0; i < 60; i++)
                {
                    Vector2 particlePosition = Vector2.Lerp(BeginPos, EndPos, Main.rand.NextFloat());
                    Color particleColor = Main.rand.NextBool() ? Color.OrangeRed : Main.rand.NextBool() ? Color.White : Color.Orange;
                    float particleScale = Main.rand.NextFloat(0.05f, 0.4f) * (0.4f + 0.6f * (float)Math.Sin(Main.rand.NextFloat(0f, 100f) / (242f) * MathHelper.Pi));

                    int particleType = Main.rand.Next(3);
                    Particle particle;

                    switch (particleType)
                    {
                        case 0:
                            particle = new StrongBloom(particlePosition, Vector2.UnitY * Main.rand.NextFloat(-4f, -1f), particleColor, particleScale, Main.rand.Next(20) + 10);
                            GeneralParticleHandler.SpawnParticle(particle);
                            break;
                        case 1:
                            particle = new GenericBloom(particlePosition, Vector2.UnitY * Main.rand.NextFloat(-4f, -1f), particleColor, particleScale, Main.rand.Next(20) + 10);
                            GeneralParticleHandler.SpawnParticle(particle);
                            break;
                        case 2:
                            particle = new CritSpark(particlePosition, Vector2.UnitY * Main.rand.NextFloat(-10f, -1f), Color.White, particleColor, particleScale * 7f, Main.rand.Next(20) + 10, 0.1f, 3);
                            GeneralParticleHandler.SpawnParticle(particle);
                            break;
                    }
                }
                float Max = 10;
                for (int i = 0; i < Max; i++)
                {
                    float progress2 = i / Max;
                    SoundStyle sewSound = i % 3 == 0 ? SoundID.Item63 : i % 3 == 1 ? SoundID.Item64 : SoundID.Item65;
                    SoundEngine.PlaySound(sewSound with { Volume = sewSound.Volume * 0.5f }, Projectile.Owner().Center);
                    Vector2 stitchCenter = Vector2.Lerp(beginPos, EndPos, progress2);
                    Particle spark = new CritSpark(stitchCenter, Vector2.Zero, Color.White, Color.Cyan, 3f, 8, 0.1f, 3);
                    GeneralParticleHandler.SpawnParticle(spark);
                }
                Projectile.timeLeft = 20;
                if (Projectile.Owner().controlUp)
                    dashable = true;
                if (LAPUtilities.IsLocalPlayer(Projectile.owner))
                {
                    int starAmt = 7;
                    for (int s = 1; s <= starAmt; s++)
                    {
                        float lerpRatio = s / (float)starAmt;
                        Vector2 starPosition = Vector2.Lerp(beginPos, EndPos, lerpRatio);
                        Projectile blast = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), starPosition, Main.rand.NextVector2CircularEdge(12, 12), ProjectileType<EonBoltNew>(), (int)(ArkoftheCosmos.BlastBoltsDamageMultiplier * Projectile.damage), 0f, Projectile.owner, 0.55f, MathHelper.Pi * 0.07f);
                    }
                }
                Projectile.Owner().SetImmuneTimeForAllTypes(45);
                AddEndParticle = true;
            }
            if (AddEndParticle)
            {
                Projectile.Opacity = MathHelper.Lerp(Projectile.Opacity, 0f, 0.12f);
                if (dashable)
                {
                    float progress3 = MathHelper.Clamp((Projectile.timeLeft / 20f), 0.1f, 1f);
                    Projectile.Owner().velocity = LAPUtilities.GetVector2(Projectile.Owner().Center, EndPos) * 80 * progress3;
                    float distanceTotarget = Vector2.Distance(Projectile.Owner().Center, EndPos);
                    if (distanceTotarget < 75f)
                    {
                        Projectile.Owner().velocity *= 0.1f;
                    }
                    if (!Projectile.Owner().controlDown)
                        Projectile.Owner().LAP().NoSlowFall = 2;
                }
            }
        }
        public void Init()
        {
            SoundEngine.PlaySound(SoundID.Item84 with { Volume = SoundID.Item84.Volume * 0.3f }, Projectile.Center);
            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX);
            Projectile.rotation = Projectile.velocity.ToRotation();
            TargetRot = Projectile.velocity.ToRotation();
            AniHelper.MaxAniProgress[AniState.Begin] = 15;
            RotOffset = 0.7f;
            BeginPos = Projectile.Center;
            EndPos = Projectile.Center + Projectile.velocity * 760;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.ReSetToBeginShader();

            for (int i = 0; i < ParticleDrawPos.Count; i++)
            {
                Texture2D star = LAPTextureRegister.StarProj.Value;
                Vector2 particlepos = ParticleDrawPos[i] - Main.screenPosition;
                LAPUtilities.Draw(star, particlepos, null, Color.OrangeRed * Projectile.Opacity, Projectile.rotation + MathHelper.PiOver2, star.Size() / 2, new Vector2(0.6f, 5f), 0, 0);
            }

            Vector2 drawpos = Projectile.Center - Main.screenPosition;
            Texture2D Left = CITextureRegistry.SunderingScissorsLeft.Value;
            Texture2D Right = CITextureRegistry.SunderingScissorsRight.Value;
            float rot = Projectile.rotation + MathHelper.PiOver4;
            Vector2 drawOrigin = new Vector2(32, 86);
            Vector2 drawOrigin2 = Projectile.direction == 1 ?  new Vector2(47, 86) : new Vector2(45, 86);
            Color drawColor = Color.Tomato * 0.9f;
            Color drawColorBack = Color.DeepSkyBlue * 0.9f;
            Main.spriteBatch.Draw(Right, drawpos, null, drawColorBack * Projectile.Opacity, rot + RotOffset, drawOrigin2, Projectile.scale, 0, 0);
            Main.spriteBatch.Draw(Left, drawpos, null, drawColor * Projectile.Opacity, rot - RotOffset, drawOrigin, Projectile.scale, 0, 0);

            Texture2D spark = LAPTextureRegister.Sparkle.Value;
            Texture2D bloom = LAPTextureRegister.BloomCircle.Value;
            Vector2 drawpos2 = Projectile.Center - Main.screenPosition + new Vector2(0, 0).RotatedBy(Projectile.rotation);
            LAPUtilities.Draw(bloom, drawpos2, null, Color.CornflowerBlue * 0.5f * Projectile.Opacity, Main.GlobalTimeWrappedHourly, bloom.Size() / 2, Projectile.scale * 1.5f, 0, 0);
            LAPUtilities.Draw(spark, drawpos2, null, Color.White * Projectile.Opacity, Main.GlobalTimeWrappedHourly * 8, spark.Size() / 2, Projectile.scale * 2f, 0, 0);
            LAPUtilities.Draw(spark, drawpos2, null, Color.White * 0.6f * Projectile.Opacity, Main.GlobalTimeWrappedHourly * 8 + MathHelper.PiOver4, spark.Size() / 2, Projectile.scale * 1.5f, 0, 0);

            LAPUtilities.ReSetToEndShader();
            return false;
        }
    }
}
