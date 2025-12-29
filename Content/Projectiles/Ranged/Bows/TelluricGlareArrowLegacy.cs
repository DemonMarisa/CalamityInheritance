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
            LAPUtilities.ReSetToBeginShader();
            //LAPUtilities.SetRasterizerState();
            if (OldPos.Count < 2)
                return false;
            Texture2D texture = LAPTextureRegister.StandardFlow1.Value;
            List<VertexPositionColorTexture2D> Vertexlist = new List<VertexPositionColorTexture2D>();
            for (int i = 0; i < OldPos.Count; i++)
            {
                // 1. 进度计算优化
                float progress = (float)i / (OldPos.Count - 1);
                float width = 96f * MathHelper.Lerp(1f, 0f, progress);

                // 2. 使用前向和后向的中点来平滑法线方向
                Vector2 direction;
                if (i == 0) direction = OldPos[i + 1] - OldPos[i];
                else if (i == OldPos.Count - 1) direction = OldPos[i] - OldPos[i - 1];
                else direction = OldPos[i + 1] - OldPos[i - 1]; // 取前后点的连线方向，更平滑

                Vector2 normal = direction.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2);

                Vector2 DrawPos = OldPos[i] - Main.screenPosition;

                // 3. 这里的宽度偏移要对称
                Vertexlist.Add(new VertexPositionColorTexture2D(DrawPos - normal * width, Color.White, new Vector3(progress, 0, 0)));
                Vertexlist.Add(new VertexPositionColorTexture2D(DrawPos + normal * width, Color.White, new Vector3(progress, 1, 0)));
            }
            Main.graphics.GraphicsDevice.Textures[0] = texture;
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, Vertexlist.ToArray(), 0, Vertexlist.Count - 2);
            LAPUtilities.ReSetToEndShader();
            return false;
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
