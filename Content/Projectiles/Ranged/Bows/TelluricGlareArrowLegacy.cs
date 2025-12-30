using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Graphics.Primitives;
using LAP.Assets.Effects;
using LAP.Assets.TextureRegister;
using LAP.Core.Enums;
using LAP.Core.Graphics.PixelatedRender;
using LAP.Core.Graphics.Primitives.Trail;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static tModPorter.ProgressUpdate;

namespace CalamityInheritance.Content.Projectiles.Ranged.Bows
{
    public class TelluricGlareArrowLegacy : ModProjectile, ILocalizedModType, IPixelatedRenderer
    {
        public DrawLayer LayerToRenderTo => DrawLayer.BeforeDusts;
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        private const int Lifetime = 180;
        private static Color ShaderColorOne = new Color(237, 194, 66);
        private static Color ShaderColorTwo = new Color(235, 227, 117);
        private static Color ShaderEndColor = new Color(199, 153, 26);
        public List<Vector2> OldPos = [];
        public List<float> OldRot = [];
        public override void SetStaticDefaults()
        {
            // While this projectile doesn't have afterimages, it keeps track of old positions for its primitive drawcode.
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 24;
            Projectile.arrow = true;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = Lifetime;
            Projectile.MaxUpdates = 3;
            Projectile.penetrate = 2; // Can hit up to two enemies. Will explode extremely soon after hitting the first, though.
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override bool? CanDamage() => Projectile.timeLeft < Lifetime - 4 ? null : false;

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, ShaderColorOne.ToVector3());
            Projectile.rotation = Projectile.velocity.ToRotation();
            OldPos.Add(Projectile.Center);
            OldRot.Add(Projectile.rotation);
            if (OldPos.Count > 20)
                OldPos.RemoveAt(0);
            if (OldRot.Count > 20)
                OldRot.RemoveAt(0);
        }

        private void RestrictLifetime()
        {
            if (Projectile.timeLeft > 8)
                Projectile.timeLeft = 8;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage <= 0)
                return;

            RestrictLifetime();
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            RestrictLifetime();
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > Lifetime - 10)
                return false;
            LAPUtilities.ReSetToBeginShader();
            Texture2D texture = LAPTextureRegister.StandardFlow1.Value;
            DrawTrail(texture, 1f, ShaderColorOne, ShaderColorTwo, 50f, 2f);
            DrawTrail(texture, 0.2f, Color.White, ShaderEndColor, 100f, 4);
            DrawTrail(texture, 0.2f, Color.White, ShaderEndColor, 50, 4);
            DrawTrail(texture, 0.4f, Color.Yellow, Color.Orange, 100f, 2);
            Texture2D texture_Fire = LAPTextureRegister.StandardFlow3.Value;
            DrawTrail(texture_Fire, 0.4f, Color.Yellow, Color.Orange, 25, 2);
            Texture2D texture_Bloom = LAPTextureRegister.BloomLine.Value;
            DrawTrail(texture_Bloom, 0.6f, Color.Orange * 0.75f, Color.Orange, 0f, 1f);
            DrawTrail(texture_Bloom, 0.4f, Color.Brown, Color.Orange, 0f, 1f);
            LAPUtilities.ReSetToEndShader();
            return false;
        }
        public void DrawTrail(Texture2D texture, float heigh, Color begin, Color end, float SpeedMult, float widthmult)
        {
            Effect effect = LAPShaderRegister.StandardFlowShader.Value;
            effect.Parameters["FlowTextureSize"].SetValue(texture.Size());
            effect.Parameters["targetSize"].SetValue(new Vector2(texture.Width * widthmult, texture.Height));
            effect.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * SpeedMult);
            effect.Parameters["uColor"].SetValue(begin.ToVector4());
            effect.Parameters["uFadeoutLength"].SetValue(1f);
            effect.Parameters["uFadeinLength"].SetValue(0.2f);
            effect.CurrentTechnique.Passes[0].Apply();

            List<TrailDrawDate> trailDrawDate = [];
            DrawSetting drawSetting = new(texture);
            for (int i = 0; i < OldPos.Count; i++)
            {
                float progress = (float)i / OldPos.Count;
                Color color = LAPUtilities.LerpColor(begin, end, progress);
                Vector2 DrawPos = OldPos[i] - Main.screenPosition + new Vector2(96, 0).RotatedBy(Projectile.rotation);
                TrailDrawDate TrailDrawDate = new(DrawPos, color, new Vector2(0, 40 * heigh), Projectile.rotation);
                trailDrawDate.Add(TrailDrawDate);
            }
            Main.graphics.GraphicsDevice.Textures[0] = texture;
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            TrailRender.RenderTrail(trailDrawDate.ToArray(), drawSetting);
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            // Explode into a bunch of holy fire on death.
            for (int i = 0; i < 10; i++)
            {
                Dust holyFire = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, (int)CalamityDusts.ProfanedFire, 0f, 0f, 100, default, 2f);
                holyFire.velocity *= 3f;

                if (Main.rand.NextBool())
                {
                    holyFire.scale = 0.5f;
                    holyFire.fadeIn = Main.rand.NextFloat(1f, 2f);
                }
            }
            for (int i = 0; i < 20; i++)
            {
                Dust holyFire = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 246, 0f, 0f, 100, default, 3f);
                holyFire.noGravity = true;
                holyFire.velocity *= 5f;

                holyFire = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 246, 0f, 0f, 100, default, 2f);
                holyFire.velocity *= 2f;
            }
        }

        void IPixelatedRenderer.RenderPixelated(SpriteBatch spriteBatch)
        {
            throw new global::System.NotImplementedException();
        }
    }
}
