using LAP.Assets.Effects;
using LAP.Core.MetaBallsSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.ExtraTextures.Metaballs
{
    public class ShizukuStarMetaball : BaseMetaBall
    {
        public class ShizukuStarParticle
        {
            public float Size;
            public Vector2 Velocity;
            public Vector2 Center;
            public ShizukuStarParticle(float size, Vector2 velocity, Vector2 center)
            {
                Size = size;
                Velocity = velocity;
                Center = center;
            }
            public void Update()
            {
                Center += Velocity;
                Velocity *= 0.87f;
                Size *= 0.96f;
            }
        }
        public override Texture2D BgTexture => CITexturesRegister.ShizukuBG.Value;
        public static List<ShizukuStarParticle> Particles { get; private set; } = new();
        public override Color EdgeColor => Color.Lerp(Color.White,Color.Aqua,0.75f);
        public override void Update()
        {
            for(int i = 0;i<Particles.Count;i++)
                Particles[i].Update();
            Particles.RemoveAll(p => p.Size <= 2f);
        }
        public static void SpawnParticle(Vector2 pos, Vector2 vel, float size) => Particles.Add(new(size, vel, pos));
        public override void PrepareRenderTarget()
        {
            Texture2D tex = ModContent.Request<Texture2D>($"CalamityInheritance/ExtraTextures/Metaballs/{GetType().Name}" + "_Texture").Value;
            foreach (ShizukuStarParticle particle in Particles)
            {
                Vector2 drawPos = particle.Center - Main.screenPosition;
                Vector2 orig = tex.Size() / 2;
                Vector2 scale = Vector2.One * particle.Size / tex.Size();
                Main.spriteBatch.Draw(tex, drawPos, null, Color.White, 0f, orig, scale, SpriteEffects.None, 0f);
            }
        }
        public override void PrepareShader()
        {
            Main.graphics.GraphicsDevice.Textures[0] = AlphaTexture;
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            Main.graphics.GraphicsDevice.Textures[1] = BgTexture;
            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;

            Effect shader = LAPShaderRegister.MetaballShader.Value;
            shader.Parameters["renderTargetSize"].SetValue(AlphaTexture.Size());
            shader.Parameters["bakcGroundSize"].SetValue(BgTexture.Size());
            shader.Parameters["edgeColor"].SetValue(EdgeColor.ToVector4());
            shader.Parameters["uTime"].SetValue(Vector2.UnitX * Main.GlobalTimeWrappedHourly * 0.05f);
            shader.CurrentTechnique.Passes[0].Apply();
        }
    }
}