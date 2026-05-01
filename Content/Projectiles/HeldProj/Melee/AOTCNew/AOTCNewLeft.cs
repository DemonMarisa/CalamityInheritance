using CalamityInheritance.Content.Items.Weapons.Melee.Swords.AOTCNew;
using CalamityInheritance.Texture;
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
    public class AOTCNewLeft : ModProjectile, ILocalizedModType
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<ArkoftheCosmosNew>();
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public int MaxSwingTime => 35;
        public float SwingWidth = MathHelper.PiOver2 * 1.5f;
        public int Time;
        public Vector2 direction;
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
        public override bool? CanHitNPC(NPC target) => null;
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
                direction = Projectile.velocity.SafeNormalize(Vector2.UnitX);
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX);
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            float AniProgress = EasingHelper.EaseOutCubic(Time / (float)MaxSwingTime);
            Projectile.Center = Projectile.Owner().Center;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Lerp(SwingWidth * 0.6f, -SwingWidth * 0.9f, AniProgress);
            Projectile.scale = 1f + ((float)Math.Sin(AniProgress * MathHelper.Pi) * 0.6f) + (ChargeAttack ? 0.2f : 0);
            Projectile.Owner().direction = MathF.Sign(Projectile.velocity.X);
            Projectile.Owner().SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
            Projectile.Owner().SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
            Projectile.SetHeldProj(Projectile.Owner());
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
            float opacity = (float)Math.Sin(AniProgress * MathHelper.Pi) - 0.25f;
            float rotationOffset = Projectile.direction == 1 ? MathHelper.PiOver4 + MathHelper.Pi : -MathHelper.PiOver4 + MathHelper.Pi;
            float rotation = Projectile.rotation + rotationOffset;
            Color smearColor = Color.OrangeRed with { A = 0 };
            Main.spriteBatch.Draw(smear, drawpos, null, smearColor * opacity * 0.25f, rotation, smear.Size() / 2f, Projectile.scale * 2.2f, 0, 0);

            if (ChargeAttack)
            {
                Texture2D texture =TextureAssets.Item[ItemType<ArkoftheCosmosNew>()].Value;
                Vector2 DrawOffset = new Vector2(5, 0).RotatedBy(Projectile.rotation);
                float rot = Projectile.direction == 1 ? Projectile.rotation + MathHelper.PiOver4 : Projectile.rotation - MathHelper.PiOver4 + MathHelper.Pi;
                Vector2 orig = Projectile.direction == 1 ? new Vector2(0, texture.Height) : new Vector2(texture.Width, texture.Height);
                SpriteEffects effect = Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                float Ani = 0;
                for (int i = 1; i < Projectile.oldRot.Length; ++i)
                {
                    if (Projectile.oldRot[i] == 0)
                        continue;
                    Ani++;
                    float progress = Ani / Projectile.oldRot.Length;
                    Color color = Color.Lerp(Color.Orange, Color.Gold, progress);
                    float afterimageRotation = Projectile.direction == 1 ? Projectile.oldRot[i] + MathHelper.PiOver4 : Projectile.oldRot[i] - MathHelper.PiOver4 + MathHelper.Pi;
                    Main.spriteBatch.Draw(texture, drawpos + DrawOffset, null, color * 0.05f, afterimageRotation, orig, Projectile.scale - 0.2f * ((i / (float)Projectile.oldRot.Length)), effect, 0f);
                }

                Main.spriteBatch.Draw(texture, drawpos + DrawOffset, null, lightColor, rot, orig, Projectile.scale, effect, 0);
            }
            else
            {
                Texture2D texture = CITextureRegistry.SunderingScissorsRight.Value;
                Vector2 DrawOffset = new Vector2(5, 0).RotatedBy(Projectile.rotation);
                float rot = Projectile.direction == 1 ? Projectile.rotation + MathHelper.PiOver4 : Projectile.rotation - MathHelper.PiOver4 + MathHelper.Pi;
                Vector2 orig = Projectile.direction == 1 ? new Vector2(0, texture.Height) : new Vector2(texture.Width, texture.Height);
                SpriteEffects effect = Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                float Ani = 0;
                for (int i = 1; i < Projectile.oldRot.Length; ++i)
                {
                    if (Projectile.oldRot[i] == 0)
                        continue;
                    Ani++;
                    float progress = Ani / Projectile.oldRot.Length;
                    Color color = Color.Lerp(Color.Orange, Color.Gold, progress);
                    float afterimageRotation = Projectile.direction == 1 ? Projectile.oldRot[i] + MathHelper.PiOver4 : Projectile.oldRot[i] - MathHelper.PiOver4 + MathHelper.Pi;
                    Main.spriteBatch.Draw(texture, drawpos + DrawOffset, null, color * 0.05f, afterimageRotation, orig, Projectile.scale - 0.2f * ((i / (float)Projectile.oldRot.Length)), effect, 0f);
                }

                Main.spriteBatch.Draw(texture, drawpos + DrawOffset, null, lightColor, rot, orig, Projectile.scale, effect, 0);
            }
            return false;
        }
    }
}
