using System;
using System.Collections.Generic;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Graphics.Primitives;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoLore
{
    public class SubsumingVortexProjBig : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";
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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 27;
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
            Projectile.timeLeft = Projectile.MaxUpdates * 80;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.MaxUpdates * 10;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
        }
        public override void AI()
        {
            DoGeneric();
            DoShooted();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
        //To make sure it won't block SCal proj.
        public override void OnKill(int timeLeft)
        {
            int j = Main.rand.Next(1,4);
            int pCounts = Main.rand.Next(1,4);
            float hue = (j / (float)(pCounts- 1f) + Main.rand.NextFloat(0.3f)) % 1f;
            Vector2 vel = new Vector2(6f, 0f).RotatedByRandom(MathHelper.TwoPi);
            int magic = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, vel, ModContent.ProjectileType<ExobeamSlash>(), Projectile.damage, Projectile.knockBack, Projectile.owner, hue);
            //标记这个射弹为魔法伤害
            Main.projectile[magic].DamageType = DamageClass.Magic;
            //2判，我们需要2判
            Main.projectile[magic].penetrate = 2;
            //斩击的音效
            SoundEngine.PlaySound(Exoblade.BeamHitSound, Projectile.Center);
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => behindProjectiles.Add(index);
        public override bool PreDraw(ref Color lightColor)
        {
            //Draw Shader.
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
            Texture2D worleyNoise = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/BlobbyNoise").Value;
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
            float width = Projectile.width * 0.6f;
            width *= MathHelper.SmoothStep(0.6f, 1f, Utils.GetLerpValue(0f, 0.3f, completionRatio, true));
            return width;
        }
        //DrawTrailColor
        public Color SetTrailColor(float completionRatio, Vector2 vertexPos)
        {
            float hue = Hue % 1f + HueShiftAcrossAfterimages;
            if (hue >= 0.99f)
                hue = 0.99f;

            float velocityOpacityFadeout = Utils.GetLerpValue(2f, 5f, Projectile.velocity.Length(), true);
            Color c = CalamityUtils.MulticolorLerp(hue, CalamityUtils.ExoPalette) * Projectile.Opacity * (1f - completionRatio);
            return c * Utils.GetLerpValue(0.04f, 0.2f, completionRatio, true) * velocityOpacityFadeout;
        }
        //DrawOffset
        public Vector2 PrimitiveOffsetFunction(float completionRatio, Vector2 vertexPos) => Projectile.Size * 0.5f + Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.scale * 2f;
        #endregion
        #region AIMethod
        //Expand hitbox, with some visual effect
        public void DoShooted()
        {
            AttackTimer++;
            Projectile.Opacity = Utils.GetLerpValue(0f, 20f, AttackTimer, true);
            Projectile.scale = Utils.Remap(AttackTimer, 0f, Projectile.MaxUpdates * 15f, 0.01f, 1.5f) * Utils.GetLerpValue(0f, Projectile.MaxUpdates * 16f, Projectile.timeLeft, true);
            Projectile.ExpandHitboxBy((int)(Projectile.scale * 62f));
        }

        public void DoHoming()
        {
            NPC target = Main.npc[TargetIndex];
            if (AttackTimer < Projectile.MaxUpdates * 18f)
                return;

            //HomingTarget, much more sharply
            Projectile.Center = Projectile.Center.MoveTowards(target.Center, 1f);

            float angularOffsetToTarget = MathHelper.WrapAngle(Projectile.AngleTo(target.Center) - Projectile.velocity.ToRotation()) * 0.1f;
            Projectile.velocity = Projectile.velocity.RotatedBy(angularOffsetToTarget);
            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitY) * (Projectile.velocity.Length() + 0.0025f);
        }

        public void DoGeneric()
        {
            //Search Closest Target
            NPC target = Projectile.FindClosestTarget(SubsumingVortex.SmallVortexTargetRange * 2, true, true);
            if (target != null)
            {
                TargetIndex = target.whoAmI;
                DoHoming();
                Projectile.netUpdate = true;
            }

            //Rotation
            Projectile.rotation += Projectile.velocity.X * 0.04f;
            //Emit light.
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 1.25f);
        }
        #endregion
    }
}