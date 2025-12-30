using CalamityInheritance.Content.Items.Weapons.Melee.Swords;
using CalamityInheritance.Content.Projectiles.Melee.Swords;
using CalamityMod.Projectiles.Typeless;
using LAP.Assets.Sounds;
using LAP.Content.Particles;
using LAP.Core.AnimationHandle;
using LAP.Core.Enums;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.OldLordClaymoreLegacy
{
    public class OldLordOathswordLegacyHeld : ModProjectile, ILocalizedModType
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<OldLordOathswordLegacy>();
        public override string Texture => GetInstance<OldLordOathswordLegacy>().Texture;
        public int Filp => (int)Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];
        public bool CanHit = true;
        public int UseTime => Owner.ApplyWeaponAttackSpeed(Owner.HeldItem, Owner.HeldItem.useTime, 25);
        public List<float> OldRot = [];
        public AnimationHelper animationHelper = new AnimationHelper(3);
        public float SwordLength = 60;
        public float TargetRot = 0;
        public int Time;
        public int FireCount;
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 45;
            Projectile.netImportant = true;
            Projectile.extraUpdates = 5;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (!CanHit)
                return false;
            return null;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // Otherwise, perform an AABB line collision check to check the whole beam.
            float _ = float.NaN;
            bool c = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * SwordLength * Projectile.scale * 1.5f, 12f, ref _);
            return c;
        }
        public override void AI()
        {
            Time++;
            Projectile.netUpdate = true;
            if (Projectile.LAP().FirstFrame)
            {
                TargetRot = Owner.GetPlayerToMouseVector2().ToRotation();
                animationHelper.MaxAniProgress[AnimationState.Begin] = (int)(UseTime * 0.25f) * 4;
                animationHelper.MaxAniProgress[AnimationState.End] = (int)(UseTime * 0.75f) * 4;
            }
            TargetRot = Owner.GetPlayerToMouseVector2().ToRotation();
            Projectile.SetHeldProj(Owner, true);
            Projectile.Center = Owner.Center;
            if (!Owner.active || Owner.dead)
                Projectile.Kill();
            // 基础信息
            Projectile.velocity = Projectile.rotation.ToRotationVector2();
            Projectile.timeLeft = 2;
            float baseRotation = Projectile.velocity.ToRotation();
            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, baseRotation - MathHelper.PiOver2);
            HandleAni();
            OldRot.Add(Projectile.rotation);
            if (OldRot.Count > 4)
                OldRot.RemoveAt(0);
        }
        public void HandleAni()
        {
            if (!animationHelper.HasFinish[AnimationState.Begin])
            {
                HandleBeginAni();
                animationHelper.UpDateAni(AnimationState.Begin);
            }
            else if (!animationHelper.HasFinish[AnimationState.End])
            {
                HandleEndAni();
                animationHelper.UpDateAni(AnimationState.End);
            }
            else Projectile.Kill();
        }
        public void HandleBeginAni()
        {
            float easedProgress = animationHelper.GetProgress(AnimationState.Begin);
            if (easedProgress == 0)
                SoundEngine.PlaySound(LAPSoundsMenu.CarnageRightUse, Projectile.Center);
            float baseRotation = animationHelper.UpDateAngle(-135 * Filp, 135 * Filp, Owner.direction, easedProgress);
            Vector2 TargetPos = new Vector2(SwordLength, 0).BetterRotatedBy(baseRotation, Vector2.Zero, 1f, 0.6f);
            Projectile.scale = TargetPos.Distance(Vector2.Zero) / SwordLength;
            Projectile.rotation = TargetPos.ToRotation() + TargetRot;

            if (Time % 2 == 0)
            {
                Vector2 RealAimPoint = TargetPos.RotatedBy(TargetRot);
                Vector2 beginSpawnPos = Projectile.Center + RealAimPoint * 1.2f;
                Vector2 EndSpawnPos = Projectile.Center + RealAimPoint * 1.4f;
                Color TGBColor = Color.Lerp(Color.Orange, Color.DarkRed, Main.rand.NextFloat());

                new TrailGlowBall(Vector2.Lerp(beginSpawnPos, EndSpawnPos, Main.rand.NextFloat()),
                    Projectile.velocity.RotatedBy(MathHelper.PiOver2) * Main.rand.NextFloat(1, 2) * Filp * Owner.direction,
                    TGBColor, Main.rand.Next(45, 90), 0.07f, true).Spawn();
            }
        }
        public void HandleEndAni()
        {
            float easedProgress = EasingHelper.EaseOutCubic(animationHelper.GetProgress(AnimationState.End));
            float baseRotation = animationHelper.UpDateAngle(115 * Filp, 135 * Filp, Owner.direction, easedProgress);
            Vector2 TargetPos = new Vector2(SwordLength, 0).BetterRotatedBy(baseRotation, Vector2.Zero, 1f, 0.6f);
            Projectile.scale = TargetPos.Distance(Vector2.Zero) / SwordLength;
            Projectile.rotation = TargetPos.ToRotation() + TargetRot;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            CanHit = false;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ProjectileType<FuckYou>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
        }
        public override void OnKill(int timeLeft)
        {
            if (LAPUtilities.IsLocalPlayer(Projectile.owner))
            {
                if (Main.mouseRight)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<OldLordOathswordLegacyHeld_R>(), 0, 0, Projectile.owner, Filp);
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            float drawRotation = Projectile.rotation + (Projectile.spriteDirection == -1 ? MathHelper.PiOver2 + MathHelper.PiOver4 : MathHelper.PiOver4);
            Vector2 rotationPoint = Projectile.spriteDirection == -1 ? new Vector2(texture.Width, texture.Height) : new Vector2(0, texture.Height);
            SpriteEffects flipSprite = Projectile.spriteDirection * Main.player[Projectile.owner].gravDir == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.spriteBatch.Draw(texture, drawPosition, null, Color.White, drawRotation, rotationPoint, Projectile.scale, flipSprite, 0f);
            for (int i = 0; i < OldRot.Count; i++)
            {
                float Opacty = (float)i / OldRot.Count;
                float OlddrawRotation = OldRot[i] + (Projectile.spriteDirection == -1 ? MathHelper.PiOver2 + MathHelper.PiOver4 : MathHelper.PiOver4);
                Main.spriteBatch.Draw(texture, drawPosition, null, Color.Orange * Opacty, OlddrawRotation, rotationPoint, Projectile.scale, flipSprite, 0f);
            }
            Main.spriteBatch.Draw(texture, drawPosition, null, Color.White, drawRotation, rotationPoint, Projectile.scale, flipSprite, 0f);
            return false;
        }
    }
}
