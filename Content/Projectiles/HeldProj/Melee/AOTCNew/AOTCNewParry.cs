using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Weapons.Melee.Swords.AOTCNew;
using CalamityInheritance.Texture;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Particles;
using CalamityMod.Sounds;
using LAP.Assets.TextureRegister;
using LAP.Core.AnimationHandle;
using LAP.Core.Enums;
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
    public class AOTCNewParry : ModProjectile
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<ArkoftheCosmosNew>();
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public AniHelper AniHelper = new AniHelper(3);
        public Vector2 ToPlayerOffset;
        public Vector2 targetVector;
        public float LeftRot = 0;
        public float RightRot = 0;
        public bool End;
        public int EndCount;
        public bool HasParry;
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
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 180;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float _ = float.NaN;
            Vector2 beamEndPos = Projectile.Center + targetVector.SafeNormalize(Vector2.One) * 180f;
            bool c = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Owner().Center, beamEndPos, 128f, ref _);
            return c;
        }
        public override void AI()
        {
            if (Projectile.LAP().FirstFrame)
                Init();
            AniHelper.UpDateAni(AniState.Begin);
            float BeginAni = AniHelper.GetProgress(AniState.Begin);
            Projectile.SetHeldProj(Projectile.Owner());
            if (!End)
            {
                ToPlayerOffset = Vector2.Lerp(ToPlayerOffset, targetVector, 0.08f);
                Projectile.Center = Projectile.Owner().Center + ToPlayerOffset;
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 1.7f, 0.12f);
            }
            LeftRot = MathHelper.Lerp(-MathHelper.PiOver4, 0f, EasingHelper.EaseInCubic(BeginAni));
            RightRot = MathHelper.Lerp(MathHelper.PiOver4, 0f, EasingHelper.EaseInCubic(BeginAni));
            if (LeftRot > -0.1f && RightRot < 0.1f)
            {
                End = true;
            }
            if (End)
            {
                ToPlayerOffset = Vector2.Lerp(ToPlayerOffset, targetVector * 0.5f, 0.2f);
                Projectile.Center = Projectile.Owner().Center + ToPlayerOffset;
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 1.4f, 0.2f);
                EndCount++;
                if (EndCount > 15)
                    Projectile.Kill();
            }
            Projectile.Owner().SetArmRot(targetVector.ToRotation());

            for (int k = 0; k < Main.maxProjectiles; k++)
            {
                float _ = float.NaN;
                Projectile proj = Main.projectile[k];
                Vector2 beamEndPos = Projectile.Center + targetVector.SafeNormalize(Vector2.One) * 180f;
                if (proj.active && proj.hostile && proj.damage > 1 && //Only parry harmful projectiles
                   proj.velocity.Length() * (proj.extraUpdates + 1) > 1f && //Only parry projectiles that move semi-quickly
                   proj.Size.Length() < 300 && //Only parry projectiles that aren't too large
                   Collision.CheckAABBvLineCollision(proj.Hitbox.TopLeft(), proj.Hitbox.Size(), Projectile.Owner().Center, beamEndPos, 128f, ref _))
                {
                    if (!HasParry)
                    {
                        SoundEngine.PlaySound(CommonCalamitySounds.ScissorGuillotineSnapSound with { Volume = CommonCalamitySounds.ScissorGuillotineSnapSound.Volume * 1.3f }, Projectile.Center);
                        Projectile.Owner().CIMod().CurAOTCCharge = CalamityInheritancePlayer.MaxAOTCCharge;
                        Projectile.Owner().SetImmuneTimeForAllTypes(45);

                        SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact);
                        SoundEngine.PlaySound(CommonCalamitySounds.ScissorGuillotineSnapSound with { Volume = CommonCalamitySounds.ScissorGuillotineSnapSound.Volume * 1.3f }, Projectile.Center);
                        CombatText.NewText(Projectile.Hitbox, new Color(111, 247, 200), CalamityUtils.GetTextValue("Misc.ArkParry"), true);

                        for (int i = 0; i < 5; i++) //Don't loose your way
                        {
                            Vector2 particleDispalce = Main.rand.NextVector2Circular(Projectile.Owner().Hitbox.Width * 2f, Projectile.Owner().Hitbox.Height * 1.2f);
                            float particleScale = Main.rand.NextFloat(0.5f, 1.4f);
                            Particle shine = new FlareShine(Projectile.Owner().Center + particleDispalce, particleDispalce * 0.01f, Color.White, Color.Red, 0f, new Vector2(0.6f, 1f) * particleScale, new Vector2(1.5f, 2.7f) * particleScale, 20 + Main.rand.Next(6), bloomScale: 3f, spawnDelay: Main.rand.Next(7) * 2);
                            GeneralParticleHandler.SpawnParticle(shine);
                        }
                        HasParry = true;
                    }
                    break;
                }
            }
        }
        public void Init()
        {
            SoundEngine.PlaySound(SoundID.Item84 with { Volume = SoundID.Item84.Volume * 0.3f }, Projectile.Center);
            Projectile.velocity = Vector2.Zero;
            AniHelper.MaxAniProgress[AniState.Begin] = 15;
            ToPlayerOffset = Projectile.Owner().GetPlayerToMouseVector2();
            targetVector = Projectile.Owner().GetPlayerToMouseVector2() * 100;
            Projectile.rotation = Projectile.Owner().GetPlayerToMouseVector2().ToRotation();
            LeftRot = -MathHelper.PiOver4;
            RightRot = MathHelper.PiOver4;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.LAP().OnceHitEffect)
                SoundEngine.PlaySound(CommonCalamitySounds.ScissorGuillotineSnapSound with { Volume = CommonCalamitySounds.ScissorGuillotineSnapSound.Volume * 1.3f }, Projectile.Center);
            Projectile.Owner().CIMod().CurAOTCCharge = CalamityInheritancePlayer.MaxAOTCCharge;
            Projectile.Owner().SetImmuneTimeForAllTypes(45);

            SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact);
            SoundEngine.PlaySound(CommonCalamitySounds.ScissorGuillotineSnapSound with { Volume = CommonCalamitySounds.ScissorGuillotineSnapSound.Volume * 1.3f }, Projectile.Center);
            CombatText.NewText(Projectile.Hitbox, new Color(111, 247, 200), CalamityUtils.GetTextValue("Misc.ArkParry"), true);

            for (int i = 0; i < 5; i++) //Don't loose your way
            {
                Vector2 particleDispalce = Main.rand.NextVector2Circular(Projectile.Owner().Hitbox.Width * 2f, Projectile.Owner().Hitbox.Height * 1.2f);
                float particleScale = Main.rand.NextFloat(0.5f, 1.4f);
                Particle shine = new FlareShine(Projectile.Owner().Center + particleDispalce, particleDispalce * 0.01f, Color.White, Color.Red, 0f, new Vector2(0.6f, 1f) * particleScale, new Vector2(1.5f, 2.7f) * particleScale, 20 + Main.rand.Next(6), bloomScale: 3f, spawnDelay: Main.rand.Next(7) * 2);
                GeneralParticleHandler.SpawnParticle(shine);
            }

            Vector2 particleOrigin = target.Hitbox.Size().Length() < 140 ? target.Center : Projectile.Center + Projectile.rotation.ToRotationVector2() * 60f;
            Particle spark = new GenericSparkle(particleOrigin, Vector2.Zero, Color.White, Color.HotPink, 1.2f, 35, 0.1f, 2);
            GeneralParticleHandler.SpawnParticle(spark);

            for (int i = 0; i < 10; i++)
            {
                Vector2 particleSpeed = Main.rand.NextVector2CircularEdge(1, 1) * Main.rand.NextFloat(2.6f, 4f);
                Particle energyLeak = new SquishyLightParticle(particleOrigin, particleSpeed, Main.rand.NextFloat(0.3f, 0.6f), Color.Cyan, 60, 1, 1.5f, hueShift: 0.02f);
                GeneralParticleHandler.SpawnParticle(energyLeak);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawpos = Projectile.Center - Main.screenPosition;
            Texture2D Left = CITextureRegistry.SunderingScissorsLeft.Value;
            Texture2D Right = CITextureRegistry.SunderingScissorsRight.Value;
            float rot = Projectile.direction == 1 ? Projectile.rotation + MathHelper.PiOver4 : Projectile.rotation - MathHelper.PiOver4 + MathHelper.Pi;
            Vector2 drawOrigin = new Vector2(32, 86);

            Vector2 Blade2DrawPos = drawpos + new Vector2(-16, -12).RotatedBy(Projectile.rotation);
            Main.spriteBatch.Draw(Right, Blade2DrawPos, null, lightColor, rot + RightRot, drawOrigin, Projectile.scale, 0, 0);

            Main.spriteBatch.Draw(Left, drawpos, null, lightColor, rot + LeftRot, drawOrigin, Projectile.scale, 0, 0);

            return false;
        }
    }
}
