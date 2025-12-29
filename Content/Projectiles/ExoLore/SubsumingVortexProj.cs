using System;
using System.Collections.Generic;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Graphics.Primitives;
using CalamityMod.Items.Weapons.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using LAP.Assets.TextureRegister;

namespace CalamityInheritance.Content.Projectiles.ExoLore
{
    public class SubsumingVortexProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        #region Typedef
        public ref float Hue => ref Projectile.ai[0];
        public ref float AttackTimer => ref Projectile.ai[1];
        public int TargetIndex
        {
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }
        #endregion
        #region Others
        public const float HueShiftAcrossAfterimages = 0.2f;
        #endregion
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 35;
        }
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.alpha = 255;
            Projectile.scale = 0.01f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.MaxUpdates = 4;
            Projectile.timeLeft = Projectile.MaxUpdates * 90;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.MaxUpdates * 15;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
        }
        public override void AI()
        {
            DoGeneric();
            DoShooted();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffType<MiracleBlight>(), 300);
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => behindProjectiles.Add(index);
        public override bool PreDraw(ref Color lightColor)
        {
            //Draw Shader.
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
            Texture2D worleyNoise = Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/BlobbyNoise").Value;
            float spinRotation = Main.GlobalTimeWrappedHourly * 5.2f;

            Main.spriteBatch.EnterShaderRegion();

            //Draw the trail. Using Calamity Method
            GameShaders.Misc["CalamityMod:SideStreakTrail"].UseImage1("Images/Misc/Perlin");
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, new(SetProjWidth, SetTrailColor, PrimitiveOffsetFunction, shader: GameShaders.Misc["CalamityMod:SideStreakTrail"]), 51);
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);

            GameShaders.Misc["CalamityMod:ExoVortex"].Apply();

            //Draw Vortex With afterimages.
            for (int i = 0; i < 5; i++)
            {
                float hue = Hue % 1f + i / 4f * HueShiftAcrossAfterimages;
                Vector2 scale = MathHelper.Lerp(1f, 0.6f, i / 4f) * Projectile.Size / worleyNoise.Size() * 2f;
                Vector2 drawOffset = Vector2.UnitY * Projectile.scale * 6f;
                Color c = CalamityUtils.MulticolorLerp(hue, CalamityUtils.ExoPalette) * Projectile.Opacity;
                Vector2 drawPosition = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
                Main.spriteBatch.Draw(worleyNoise, drawPosition - drawOffset, null, c, -spinRotation, worleyNoise.Size() * 0.5f, scale, 0, 0f);
                Main.spriteBatch.Draw(worleyNoise, drawPosition + drawOffset, null, c, spinRotation, worleyNoise.Size() * 0.5f, scale, 0, 0f);
            }

            Main.spriteBatch.ExitShaderRegion();
            return false;
        }
        #region DrawMethod
        //DrawProjWidth
        public float SetProjWidth(float completionRatio, Vector2 vertexPos)
        {
            return Projectile.width * 0.6f * MathHelper.SmoothStep(0.6f, 1f, Utils.GetLerpValue(0f, 0.3f, completionRatio, clamped: true));
        }
        //DrawTrailColor
        public Color SetTrailColor(float completionRatio, Vector2 vertexPos)
        {
            float num = Hue % 1f + 0.2f;
            if (num >= 0.99f)
                num = 0.99f;
            float lerpValue = Utils.GetLerpValue(2f, 5f, Projectile.velocity.Length(), clamped: true);
            return CalamityUtils.MulticolorLerp(num, CalamityUtils.ExoPalette) * Projectile.Opacity * (1f - completionRatio) * Utils.GetLerpValue(0.04f, 0.2f, completionRatio, clamped: true) * lerpValue;

        }
        //DrawOffset
        public Vector2 PrimitiveOffsetFunction(float completionRatio, Vector2 vertexPos) => Projectile.Size * 0.5f + Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.scale * 2f;
        #endregion
        #region AIMethod
        public void DoShooted()
        {
            AttackTimer++;
            Projectile.Opacity = Utils.GetLerpValue(0f, 20f, AttackTimer, true);
            Projectile.scale = Utils.Remap(AttackTimer, 0f, Projectile.MaxUpdates * 15f, 0.01f, 1.5f) * Utils.GetLerpValue(0f, Projectile.MaxUpdates * 16f, Projectile.timeLeft, true);
            Projectile.ExpandHitboxBy((int)(Projectile.scale * 62f));
        }

        public void DoHoming()
        {
            //HomingTarget
            NPC target = Main.npc[TargetIndex];
            float flySpeed = 40f / Projectile.MaxUpdates;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.SafeDirectionTo(target.Center) * flySpeed, 0.02f);
        }

        public void DoGeneric()
        {
            //Search Closest Target
            NPC target = Projectile.FindClosestTarget(SubsumingVortex.SmallVortexTargetRange, true, true);
            if (target != null)
            {
                TargetIndex = target.whoAmI;
                DoHoming();
                Projectile.netUpdate = true;
            }

            //Rotation
            Projectile.rotation += Projectile.velocity.X * 0.04f;
            //Emit light.
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.9f);
        }
        #endregion
    }
}