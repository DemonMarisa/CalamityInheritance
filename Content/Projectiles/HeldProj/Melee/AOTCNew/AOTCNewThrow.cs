using CalamityInheritance.Content.Items.Weapons.Melee.Swords.AOTCNew;
using CalamityInheritance.Content.Nodes;
using CalamityInheritance.Texture;
using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Particles;
using CalamityMod.Sounds;
using LAP.Assets.TextureRegister;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.AOTCNew
{
    public class AOTCNewThrow : ModProjectile, ILocalizedModType
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<ArkoftheCosmosNew>();
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public float ThrowRatio => EasingHelper.EaseOutCubic(Time / (float)MaxThrowTime);
        public float ThrowCompletion => Time / (float)MaxThrowTime;
        public float SnapWindowStart = 0.2f;
        public float SnapWindowEnd = 0.9f;
        public int MaxThrowTime = 140;
        public int Time;
        public int ThrowTime;
        public static float MaxThrowReach = 760;
        public Vector2 direction = Vector2.Zero;
        public bool spawnFirst;
        public bool BeginReturn;
        public bool ChargeAttack => Projectile.ai[0] != 0;
        public override void SetStaticDefaults()
        {
            Projectile.AddHeldProj();
        }

        public override void SetDefaults()
        {
            Projectile.width = 250;
            Projectile.height = 250;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.netImportant = true;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = MaxThrowTime;
        }
        public override void AI()
        {
            Time++;
            ThrowTime++;
            if (Projectile.LAP().FirstFrame)
            {
                SoundStyle sound = SoundID.Item71;
                if (ChargeAttack)
                {
                    sound = CommonCalamitySounds.LouderPhantomPhoenix;
                }
                SoundEngine.PlaySound(sound, Projectile.Center);
                SoundEngine.PlaySound(SoundID.Item4);
            }
            if (ThrowTime > 20 && !spawnFirst)
            {
                new AOTCNewStarLine(60, Projectile.owner, Projectile.whoAmI).Spawn();
                spawnFirst = true;
                ThrowTime = 0;
            }
            if (ThrowTime > 40 && !BeginReturn)
            {
                new AOTCNewStarLine(60, Projectile.owner, Projectile.whoAmI).Spawn();
                ThrowTime = 0;
            }
            //Rotate the blade towards the cursor
            Vector2 mouse = Projectile.Owner().LocalMouseWorld();
            if ((mouse - Projectile.Owner().Center).Length() > 760)
            {
                mouse = Projectile.Owner().Center + LAPUtilities.GetVector2(Projectile.Owner().Center, mouse) * 760;
            }
            if (BeginReturn)
            {
                Projectile.Center = MoveTowards(Projectile.Center, Projectile.Owner().Center, 40f);
                Projectile.timeLeft = 2;
                if (Projectile.Center.Distance(Projectile.Owner().Center) < 48)
                {
                    Projectile.Kill();
                }
            }
            else
                Projectile.Center = MoveTowards(Projectile.Center, mouse, 40f);

            Projectile.SetHeldProj(Projectile.Owner());
            Projectile.rotation -= MathHelper.PiOver4 * 0.3f;
            Projectile.scale = 1f + ThrowRatio * 0.5f;
            Projectile.rotation = MathHelper.WrapAngle(Projectile.rotation);

            if (ThrowCompletion > SnapWindowEnd)
            {
                BeginReturn = true;
            }
            if (Main.rand.NextBool())
            {
                float maxDistance = Projectile.scale * 100f;
                Vector2 distance = Main.rand.NextVector2Circular(maxDistance, maxDistance);
                Vector2 angularVelocity = Utils.SafeNormalize(distance.RotatedBy(-MathHelper.PiOver2), Vector2.Zero) * 2 * (1f + distance.Length() / 15f);
                Color glitterColor = Main.hslToRgb(Main.rand.NextFloat(), 1, 0.5f);
                Particle glitter = new CritSpark(Projectile.Center + distance, Projectile.Owner().velocity + angularVelocity, Color.White, glitterColor, 1f + 1 * (distance.Length() / maxDistance), 10, 0.05f, 3f);
                GeneralParticleHandler.SpawnParticle(glitter);
            }
            if (Main.rand.NextBool())
            {
                float opacity = 0.25f;
                float scaleFactor = 0.7f;
                for (float i = 0.5f; i <= 1; i += 0.5f)
                {
                    Vector2 smokepos = Projectile.Center + (Projectile.rotation.ToRotationVector2() * (75 * i) * Projectile.scale) + Projectile.rotation.ToRotationVector2().RotatedBy(-MathHelper.PiOver2) * 30f * scaleFactor * Main.rand.NextFloat();
                    Vector2 smokespeed = Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.PiOver2) * 20f * scaleFactor + Projectile.Owner().velocity;

                    Particle smoke = new HeavySmokeParticle(smokepos, smokespeed, Color.Lerp(Color.DodgerBlue, Color.MediumVioletRed, i), 10 + Main.rand.Next(5), scaleFactor * Main.rand.NextFloat(2.8f, 3.1f), opacity + Main.rand.NextFloat(0f, 0.2f), 0f, false, 0, true);
                    GeneralParticleHandler.SpawnParticle(smoke);

                    if (Main.rand.NextBool(3))
                    {
                        Particle smokeGlow = new HeavySmokeParticle(smokepos, smokespeed, Main.rand.NextBool(5) ? Color.Gold : Color.Chocolate, 7, scaleFactor * Main.rand.NextFloat(2f, 2.4f), opacity * 2.5f, 0f, true, 0.004f, true);
                        GeneralParticleHandler.SpawnParticle(smokeGlow);
                    }
                }
            }
            if (ThrowRatio > SnapWindowStart && ThrowRatio < SnapWindowEnd && !Projectile.Owner().LAP().MouseLeft)
            {
                float targetRot = Projectile.Owner().GetPlayerToMouseVector2().ToRotation();
                float diff = MathHelper.WrapAngle(targetRot - Projectile.rotation);
                if (diff > 0) diff -= MathHelper.TwoPi;
                float TotalAngle = diff;
                Vector2 OffsetToPlayer = Projectile.Center - Projectile.Owner().Center;
                float opacity = (float)Math.Sin(ThrowCompletion * MathHelper.Pi);
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<AOTCNewThrow_Snap>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].rotation = Projectile.rotation;
                Main.projectile[proj].scale = Projectile.scale;
                Main.projectile[proj].ai[0] = opacity;
                Main.projectile[proj].ai[1] = TotalAngle;
                Main.projectile[proj].ai[2] = Projectile.ai[0];
                Main.projectile[proj].LAP().ai_vector2[0] = OffsetToPlayer;
                Projectile.Kill();
            }
        }
        public static Vector2 MoveTowards(Vector2 currentPosition, Vector2 targetPosition, float maxAmountAllowedToMove)
        {
            Vector2 v = targetPosition - currentPosition;
            if (v.Length() < maxAmountAllowedToMove)
                return targetPosition;

            return currentPosition + v.SafeNormalize(Vector2.Zero) * maxAmountAllowedToMove;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (!Projectile.active)
            {
                return false;
            }
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Texture2D smear = CITextureRegistry.TrientCircularSmear.Value;
            float opacity = (float)Math.Sin(ThrowCompletion * MathHelper.Pi);
            float rotationOffset = Projectile.direction == 1 ?  MathHelper.Pi : MathHelper.Pi;
            float rotation = Projectile.rotation + rotationOffset;
            Color smearColor = Color.Silver with { A = 0 };
            Main.spriteBatch.Draw(smear, drawPos, null, smearColor * opacity * 0.1f, rotation, smear.Size() / 2f, Projectile.scale * 1.7f, 0, 0);

            Texture2D sword = CITextureRegistry.SunderingScissorsLeft.Value;
            float drawRotation = Projectile.rotation + MathHelper.PiOver4;
            Vector2 drawOrigin = new Vector2(32, 86); //Right on the hole. Well tbh here its not the hole theres a gem on it but you get me.

            if (ChargeAttack)
            {
                Texture2D sword2 = CITextureRegistry.SunderingScissorsRight.Value;
                float drawRotation2 = Projectile.rotation + MathHelper.PiOver4 + MathHelper.PiOver4 * 0.8f;
                Vector2 Blade2DrawPos = drawPos + new Vector2(-20, -13).RotatedBy(Projectile.rotation);
                Main.EntitySpriteDraw(sword2, Blade2DrawPos, null, lightColor, drawRotation2, drawOrigin, Projectile.scale, 0f, 0);
            }
            Main.EntitySpriteDraw(sword, drawPos, null, lightColor, drawRotation, drawOrigin, Projectile.scale, 0f, 0);
            return false;
        }
    }
}
