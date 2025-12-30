using CalamityInheritance.Content.Items.Weapons.Melee.Swords;
using CalamityInheritance.Utilities;
using CalamityMod.NPCs.TownNPCs;
using LAP.Core.AnimationHandle;
using LAP.Core.Enums;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.OldLordClaymoreLegacy
{
    public class OldLordOathswordLegacyHeld_R : ModProjectile, ILocalizedModType
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<OldLordOathswordLegacy>();
        public override string Texture => GetInstance<OldLordOathswordLegacy>().Texture;
        public Player Owner => Main.player[Projectile.owner];
        public AnimationHelper animationHelper = new(3);
        public Vector2 Offset;
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
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void AI()
        {
            Projectile.netUpdate = true;
            if (Projectile.LAP().FirstFrame)
            {
                animationHelper.MaxAniProgress[AnimationState.Begin] = 60;
                animationHelper.MaxAniProgress[AnimationState.Middle] = 60;
                Projectile.velocity = Vector2.Zero;
            }
            if (!Owner.LAP().MouseRight)
            {
                Projectile.Kill();
            }
            Projectile.SetHeldProj(Owner, true);
            Projectile.Center = Owner.Center + Offset;
            Projectile.rotation = -MathHelper.PiOver2;
            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.Pi);
            Owner.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, MathHelper.Pi);
            HandleAni();
        }
        public void HandleAni()
        {
            if (!animationHelper.HasFinish[AnimationState.Begin])
            {
                HandleBeginAni();
                animationHelper.UpDateAni(AnimationState.Begin);
            }
            else if (!animationHelper.HasFinish[AnimationState.Middle])
            {
                HandleEndAni();
                animationHelper.UpDateAni(AnimationState.Middle);
            }
            else Projectile.Kill();
        }
        public void HandleBeginAni()
        {
            Vector2 bladeOffset = new Vector2(12, 0).RotatedByRandom(MathHelper.TwoPi);
            Vector2 SpawnPos = Projectile.Center + new Vector2(2, -26);
            for (int i = 0; i < 2; i++)
            {
                Vector2 dustSpawnPosition = SpawnPos + (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * Main.rand.NextFloat(Projectile.width) + Main.rand.NextVector2Circular(6f, 6f);
                Vector2 dustVelocity = bladeOffset.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(6f) + Main.rand.NextVector2Circular(2.4f, 2.4f) * -1;

                Dust dust = Dust.NewDustPerfect(dustSpawnPosition, DustID.RainbowMk2, dustVelocity);
                dust.color = Main.rand.NextBool(4) ? Color.Purple : Color.Red;
                dust.scale = Main.rand.NextFloat(0.85f, 1.2f);
                dust.noGravity = true;
            }
            float easedProgress = EasingHelper.EaseInCubic(animationHelper.GetProgress(AnimationState.Begin));
            Offset = new Vector2(0, 8 * easedProgress);
        }
        public void HandleEndAni()
        {
            Owner.CIMod().CanUseOldLordDash = true;
            float easedProgress = EasingHelper.EaseOutCubic(animationHelper.GetProgress(AnimationState.Middle));
            if (easedProgress == 0)
            {
                Vector2 SpawnPos = Projectile.Center + new Vector2(0, -28);
                SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);
                for (int i = 0; i < 30; i++)
                {
                    Vector2 dustSpawnPosition = SpawnPos;
                    Vector2 dustVelocity = (MathHelper.TwoPi * i / 30f).ToRotationVector2() * 5f;
                    Dust dust = Dust.NewDustPerfect(dustSpawnPosition, DustID.RainbowMk2, dustVelocity);
                    dust.color = Color.Violet;
                    dust.scale = 1.4f;
                    dust.noGravity = true;
                }
            }
            Offset = Vector2.Lerp(Offset, new Vector2(0, 0), 0.2f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            float drawRotation = Projectile.rotation + (Projectile.spriteDirection == -1 ? MathHelper.PiOver2 + MathHelper.PiOver4 : MathHelper.PiOver4);
            Vector2 rotationPoint = Projectile.spriteDirection == -1 ? new Vector2(texture.Width, texture.Height) : new Vector2(0, texture.Height);
            SpriteEffects flipSprite = Projectile.spriteDirection * Main.player[Projectile.owner].gravDir == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.spriteBatch.Draw(texture, drawPosition, null, Color.White, drawRotation, rotationPoint, Projectile.scale, flipSprite, 0f);

            float easedProgress = EasingHelper.EaseOutCubic(animationHelper.GetProgress(AnimationState.Middle));
            float opacite = easedProgress;
            Main.spriteBatch.Draw(texture, drawPosition, null, Color.Orange * opacite * 0.5f, drawRotation, rotationPoint, Projectile.scale, flipSprite, 0f);
            return false;
        }
    }
}
