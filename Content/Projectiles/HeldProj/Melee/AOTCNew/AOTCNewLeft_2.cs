using CalamityInheritance.Content.Items.Weapons.Melee.Swords.AOTCNew;
using CalamityInheritance.Content.Projectiles.Melee.AOTCNew;
using CalamityInheritance.Texture;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Sounds;
using LAP.Assets.TextureRegister;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.AOTCNew
{
    public class AOTCNewLeft_2 : ModProjectile, ILocalizedModType
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<ArkoftheCosmosNew>();
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public int MaxSwingTime => 55;
        public float SwingWidth = MathHelper.PiOver2 * 1.5f;
        public int Time;
        public Vector2 targetVel;
        public int firetime;
        public int firecount;
        public bool ChargeAttack => Projectile.ai[0] != 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
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
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = MaxSwingTime;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // Otherwise, perform an AABB line collision check to check the whole beam.
            float _ = float.NaN;
            bool c = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * 180 * Projectile.scale, 128f, ref _);
            return c;
        }
        public override void AI()
        {
            Time++;
            if (Projectile.LAP().FirstFrame)
            {
                SwingWidth *= Projectile.Owner().direction;
                SoundStyle sound = SoundID.Item71;
                if (ChargeAttack)
                    sound = CommonCalamitySounds.LouderPhantomPhoenix;
                SoundEngine.PlaySound(sound, Projectile.Center);
                targetVel = Projectile.velocity.SafeNormalize(Vector2.UnitX);
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX);
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            float AniProgress = EasingHelper.EaseOutCubic(Time / (float)MaxSwingTime);
            Projectile.Center = Projectile.Owner().Center;
            Projectile.Owner().direction = MathF.Sign(Projectile.velocity.X);
            Projectile.Owner().SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
            Projectile.Owner().SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
            Projectile.SetHeldProj(Projectile.Owner());
            firetime++;
            //Really important to use projectile.timeLeft -1 instead of simply projectile.timeLeft because if it spawns a bolt on its last frame of existence the primitive drawer will break and shit itself or something idk
            if (firetime > 4 && firecount < 6 && Projectile.Owner().whoAmI == Main.myPlayer/* && (Projectile.timeLeft - 1) % Math.Ceiling(MaxSwingTime / 6f) == 0f*/)
            {
                firecount++;
                firetime = 0;
                //Slightly shift the blasts up so the final close shots don't go BELOW the cursor and instead go right on it.
                float adjustedBlastRotation = Projectile.rotation - MathHelper.PiOver4 * 1.15f * Projectile.Owner().direction;

                var source = Projectile.GetSource_FromThis();
                Projectile.NewProjectileDirect(source, Projectile.Owner().Center + adjustedBlastRotation.ToRotationVector2() * 10f, adjustedBlastRotation.ToRotationVector2() * 8, ProjectileType<EonBoltNew>(), (int)(ArkoftheCosmos.SwirlBoltDamageMultiplier / ArkoftheCosmos.SwirlBoltAmount * Projectile.damage), 0f, Projectile.Owner().whoAmI, 0.55f, MathHelper.Pi * 0.05f);
            }

            float endRot = (MathHelper.Pi - MathHelper.PiOver4 * 1.2f) * Projectile.direction;
            float startRot = -(MathHelper.TwoPi + MathHelper.PiOver4 * 1.5f) * Projectile.direction;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Lerp(startRot, endRot, AniProgress);
            Projectile.scale = 1.2f + ((float)Math.Sin(AniProgress * MathHelper.Pi) * 0.6f) + (ChargeAttack ? 0.2f : 0);

            SpawnPartic();
        }
        public void SpawnPartic()
        {
            float Ratio = Time / (float)MaxSwingTime;
            Color currentColor = Color.Chocolate * (MathHelper.Clamp((float)Math.Sin((Ratio - 0.2f) * MathHelper.Pi), 0f, 1f) * 0.8f);
            if (Main.rand.NextBool())
            {
                float maxDistance = Projectile.scale * 140f;
                Vector2 distance = Main.rand.NextVector2Circular(maxDistance, maxDistance);
                Vector2 angularVelocity = Utils.SafeNormalize(distance.RotatedBy(MathHelper.PiOver2 * Projectile.Owner().direction), Vector2.Zero) * 2 * (1f + distance.Length() / 15f);
                Particle glitter = new CritSpark(Projectile.Owner().Center + distance, Projectile.Owner().velocity + angularVelocity, Main.rand.NextBool(3) ? Color.Turquoise : Color.Coral, currentColor, 1f + 1 * (distance.Length() / maxDistance), 10, 0.05f, 3f);
                GeneralParticleHandler.SpawnParticle(glitter);
            }
            float Opacity = MathHelper.Clamp(MathHelper.Clamp((float)Math.Sin((Ratio) * MathHelper.Pi), 0f, 1f) * 2f, 0, 1) * 0.25f;
            float scaleFactor = MathHelper.Clamp(MathHelper.Clamp((float)Math.Sin((Ratio ) * MathHelper.Pi), 0f, 1f), 0, 1);

            for (float i = 0f; i <= 1; i += 0.5f)
            {
                Vector2 smokepos = Projectile.Owner().Center + (Projectile.rotation.ToRotationVector2() * (80 + 50 * i) * Projectile.scale) + Projectile.rotation.ToRotationVector2().RotatedBy(-MathHelper.PiOver2) * 30f * scaleFactor * Main.rand.NextFloat();
                Vector2 smokespeed = Projectile.rotation.ToRotationVector2().RotatedBy(-MathHelper.PiOver2 * Projectile.Owner().direction) * 20f * scaleFactor + Projectile.Owner().velocity;
                if (Main.rand.NextBool())
                {
                    Particle smoke = new HeavySmokeParticle(smokepos, smokespeed, Color.Lerp(Color.DodgerBlue, Color.MediumVioletRed, i), 6 + Main.rand.Next(5), scaleFactor * Main.rand.NextFloat(2.8f, 3.1f), Opacity + Main.rand.NextFloat(0f, 0.2f), 0f, false, 0, true);
                    GeneralParticleHandler.SpawnParticle(smoke);

                }
                if (Main.rand.NextBool(4))
                {
                    Particle smokeGlow = new HeavySmokeParticle(smokepos, smokespeed, Main.rand.NextBool(5) ? Color.Gold : Color.Chocolate, 5, scaleFactor * Main.rand.NextFloat(2f, 2.4f), Opacity * 2.5f, 0f, true, 0.004f, true);
                    GeneralParticleHandler.SpawnParticle(smokeGlow);
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawpos = Projectile.Center - Main.screenPosition;

            Texture2D smear = CITextureRegistry.TrientCircularSmear.Value;
            float AniProgress = Time / (float)MaxSwingTime;
            float opacity = (float)Math.Sin(AniProgress * MathHelper.Pi);
            float rotationOffset = Projectile.direction == -1 ? MathHelper.PiOver4 + MathHelper.Pi : -MathHelper.PiOver4 + MathHelper.Pi;
            float rotation = Projectile.rotation + rotationOffset;
            Color smearColor = Color.Chocolate with { A = 0 };
            Main.spriteBatch.Draw(smear, drawpos, null, smearColor * opacity * 0.4f, rotation, smear.Size() / 2f, Projectile.scale * 2.2f, 0, 0);
            if (ChargeAttack)
            {
                Texture2D texture = TextureAssets.Item[ItemType<ArkoftheCosmosNew>()].Value;
                Vector2 DrawOffset = new Vector2(5, 0).RotatedBy(Projectile.rotation);
                float rot = Projectile.direction == -1 ? Projectile.rotation + MathHelper.PiOver4 : Projectile.rotation - MathHelper.PiOver4 + MathHelper.Pi;
                Vector2 orig = Projectile.direction == -1 ? new Vector2(0, texture.Height) : new Vector2(texture.Width, texture.Height);
                SpriteEffects effect = Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                float Ani = 0;
                for (int i = 1; i < Projectile.oldRot.Length; ++i)
                {
                    if (Projectile.oldRot[i] == 0)
                        continue;
                    Ani++;
                    float progress = Ani / Projectile.oldRot.Length;
                    Color color = Color.Lerp(Color.Orange, Color.Gold, progress);
                    float afterimageRotation = Projectile.direction == -1 ? Projectile.oldRot[i] + MathHelper.PiOver4 : Projectile.oldRot[i] - MathHelper.PiOver4 + MathHelper.Pi;
                    Main.spriteBatch.Draw(texture, drawpos + DrawOffset, null, color * 0.05f, afterimageRotation, orig, Projectile.scale - 0.2f * ((i / (float)Projectile.oldRot.Length)), effect, 0f);
                }
                Main.spriteBatch.Draw(texture, drawpos + DrawOffset, null, lightColor, rot, orig, Projectile.scale, effect, 0);
            }
            else
            {
                Texture2D texture = CITextureRegistry.SunderingScissorsLeft.Value;
                Vector2 DrawOffset = new Vector2(5, 0).RotatedBy(Projectile.rotation);
                float rot = Projectile.direction == -1 ? Projectile.rotation + MathHelper.PiOver4 : Projectile.rotation - MathHelper.PiOver4 + MathHelper.Pi;
                Vector2 orig = Projectile.direction == -1 ? new Vector2(0, texture.Height) : new Vector2(texture.Width, texture.Height);
                SpriteEffects effect = Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                float Ani = 0;
                for (int i = 1; i < Projectile.oldRot.Length; ++i)
                {
                    if (Projectile.oldRot[i] == 0)
                        continue;
                    Ani++;
                    float progress = Ani / Projectile.oldRot.Length;
                    Color color = Color.Lerp(Color.Orange, Color.Gold, progress);
                    float afterimageRotation = Projectile.direction == -1 ? Projectile.oldRot[i] + MathHelper.PiOver4 : Projectile.oldRot[i] - MathHelper.PiOver4 + MathHelper.Pi;
                    Main.spriteBatch.Draw(texture, drawpos + DrawOffset, null, color * 0.05f, afterimageRotation, orig, Projectile.scale - 0.2f * ((i / (float)Projectile.oldRot.Length)), effect, 0f);
                }
                Main.spriteBatch.Draw(texture, drawpos + DrawOffset, null, lightColor, rot, orig, Projectile.scale, effect, 0);
            }
            return false;
        }
    }
}
