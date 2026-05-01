using CalamityInheritance.Content.Items.Weapons.Melee.Swords.AOTCNew;
using CalamityInheritance.Content.Projectiles.Melee.AOTCNew;
using CalamityInheritance.Texture;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Sounds;
using LAP.Assets.TextureRegister;
using LAP.Core.AnimationHandle;
using LAP.Core.Enums;
using LAP.Core.Menus.Buttoms.Depth_2;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.AOTCNew
{
    public class AOTCNewThrow_Snap : ModProjectile, ILocalizedModType
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<ArkoftheCosmosNew>();
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public ref float Opacity => ref Projectile.ai[0];
        public ref float AngleDiffence => ref Projectile.ai[1];
        public ref Vector2 ToPlayerDistance => ref Projectile.LAP().ai_vector2[0];
        public Vector2 EndPos;
        public bool FirstFrame = false;
        public float InitRot;
        public float AngleOffset;
        public AniHelper animationHelper = new AniHelper(3);
        public Vector2 ChildrenBladePos;
        public int StartDir;
        public float RotOffset;
        public bool ChargeAttack => Projectile.ai[2] != 0;
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
            Projectile.localNPCHitCooldown = -1;
            Projectile.netImportant = true;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 180;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float _ = float.NaN;
            Vector2 beamEndPos = Projectile.Center + ToPlayerDistance.SafeNormalize(Vector2.UnitX) * 200;
            bool c = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Owner().Center, beamEndPos, 128f, ref _);
            return c;
        }
        public override void AI()
        {
            if (Projectile.LAP().FirstFrame)
            {
                if (Projectile.Owner().whoAmI == Main.myPlayer)
                {
                    float rotationOffset = MathHelper.TwoPi * Main.rand.NextFloat();

                    for (int i = 0; i < 3; i++)
                    {
                        var source = Projectile.GetSource_FromThis();
                        Projectile blast = Projectile.NewProjectileDirect(source, Projectile.Center + (MathHelper.TwoPi * (i / 3f) + rotationOffset).ToRotationVector2() * 30f, (MathHelper.TwoPi * (i / 3f) + rotationOffset).ToRotationVector2() * 8, ProjectileType<EonBoltNew>(), Projectile.damage / 3, 0f, Projectile.Owner().whoAmI, 0.55f, MathHelper.Pi * 0.05f);
                    }

                    //Reset local immunity so that the snap can do damage
                    for (int i = 0; i < Main.maxNPCs; ++i)
                        Projectile.localNPCImmunity[i] = 0;
                }
                RotOffset = MathHelper.PiOver4 * 0.8f;
                InitRot = Projectile.rotation;
                animationHelper.MaxAniProgress[AniState.Begin] = 45;
                StartDir = Projectile.Owner().direction;
            }
            animationHelper.UpDateAni(AniState.Begin);
            float progress = animationHelper.GetProgress(AniState.Begin);
            Vector2 endPos = ToPlayerDistance * 0.7f;
            EndPos = Vector2.Lerp(ToPlayerDistance, endPos, EasingHelper.EaseInCubic(progress));
            ChildrenBladePos = Vector2.Lerp(ChildrenBladePos, EndPos, EasingHelper.EaseInCubic(progress));
            Projectile.Center = Projectile.Owner().Center + EndPos;
            Projectile.scale = MathHelper.Lerp(Projectile.scale, 1.6f, 0.2f);
            AngleOffset = MathHelper.Lerp(AngleOffset, AngleDiffence, 0.12f);
            Projectile.rotation = InitRot + AngleOffset;
            Opacity = MathHelper.Lerp(Opacity, 0f, 0.12f);
            RotOffset = MathHelper.Lerp(RotOffset, 0f, 0.12f);
            if (animationHelper.HasFinish[AniState.Begin])
            {
                Main.LocalPlayer.SetScreenshake(3f);
                SoundEngine.PlaySound(SoundID.Item84, Projectile.Center);
                Vector2 sliceDirection = ToPlayerDistance.SafeNormalize(Vector2.UnitX) * 40;
                Particle SliceLine = new LineVFX(Projectile.Center - sliceDirection, sliceDirection * 2f, 0.2f, Color.Orange * 0.7f, expansion: 250f)
                {
                    Lifetime = 10
                };
                GeneralParticleHandler.SpawnParticle(SliceLine);
                Projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.LAP().OnceHitEffect)
            {
                SoundEngine.PlaySound(CommonCalamitySounds.ScissorGuillotineSnapSound with { Volume = CommonCalamitySounds.ScissorGuillotineSnapSound.Volume * 1.3f }, Projectile.Center);
                Projectile.Owner().CIMod().CurAOTCCharge += 2;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Texture2D smear = CITextureRegistry.TrientCircularSmear.Value;
            float opacity = Opacity;
            float rotationOffset = Projectile.direction == 1 ? MathHelper.Pi : MathHelper.Pi;
            float rotation = Projectile.rotation + rotationOffset;
            Color smearColor = Color.Silver with { A = 0 };
            Main.spriteBatch.Draw(smear, drawPos, null, smearColor * opacity * 0.1f, rotation, smear.Size() / 2f, Projectile.scale * 1.7f, 0, 0);

            Texture2D sword = CITextureRegistry.SunderingScissorsLeft.Value;
            float drawRotation = Projectile.rotation + MathHelper.PiOver4;
            Vector2 drawOrigin = new Vector2(32, 86); //Right on the hole. Well tbh here its not the hole theres a gem on it but you get me.

            Vector2 Blade2DrawPos = Projectile.Owner().Center + ChildrenBladePos - Main.screenPosition + new Vector2(-20, -13).RotatedBy(Projectile.rotation);
            Texture2D swordR = CITextureRegistry.SunderingScissorsRight.Value;
            float rotationOffset2 = Projectile.direction == 1 ? MathHelper.PiOver4 : -MathHelper.PiOver4;
            float rotation2 = ToPlayerDistance.ToRotation() + rotationOffset2;

            if (ChargeAttack)
            {
                float drawRotation2 = Projectile.rotation + MathHelper.PiOver4 + RotOffset;
                Main.EntitySpriteDraw(swordR, drawPos + new Vector2(-18, -13).RotatedBy(Projectile.rotation), null, lightColor, drawRotation2 - 0.02f, drawOrigin, Projectile.scale, 0f, 0);
            }
            else
            {
                float progress = animationHelper.GetProgress(AniState.Begin);
                if (progress > 0.25f)
                    Main.EntitySpriteDraw(swordR, Blade2DrawPos, null, lightColor, rotation2, drawOrigin, Projectile.scale, 0f, 0);
            }
            Main.EntitySpriteDraw(sword, drawPos, null, lightColor, drawRotation, drawOrigin, Projectile.scale, 0f, 0);

            return false;
        }
    }
}
