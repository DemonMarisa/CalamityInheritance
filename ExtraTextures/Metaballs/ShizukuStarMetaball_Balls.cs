using CalamityMod.Graphics.Metaballs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace CalamityInheritance.ExtraTextures.Metaballs
{
    public class ShizukuStarMetaball_Balls : Metaball
    {
        public class ShizukuStarParticle(float size, Vector2 velocity, Vector2 center)
        {
            public float Size = size;
            public Vector2 Velocity = velocity;
            public Vector2 Center = center;

            public void Update()
            {
                Center += Velocity;
                Velocity *= 0.87f;
                Size *= 0.96f;
            }
        }
        public static Asset<Texture2D> LayerAsset {  get; private set; }
        public static List<ShizukuStarParticle> Particles { get; private set; } = [];
        public override bool AnythingToDraw => Particles.Count != 0;
        public override IEnumerable<Texture2D> Layers
        {
            get
            {
                yield return LayerAsset.Value;
            }
        }
        public override MetaballDrawLayer DrawContext => MetaballDrawLayer.AfterProjectiles;
        public override Color EdgeColor => Color.Lerp(Color.White, Color.Aqua, 0.75f);
        public override void Load()
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            LayerAsset = ModContent.Request<Texture2D>($"CalamityInheritance/ExtraTextures/Metaballs/{nameof(ShizukuStarMetaball)}" + "_Layer", AssetRequestMode.ImmediateLoad);
        }
        public override void Update()
        {
            for(int i = 0;i<Particles.Count;i++)
                Particles[i].Update();
            Particles.RemoveAll(p => p.Size <= 2f);
        }
        public static void SpawnParticle(Vector2 pos, Vector2 vel, float size) => Particles.Add(new(size, vel, pos));
        public override Vector2 CalculateManualOffsetForLayer(int layerIndex)
        {
            return Vector2.UnitX * Main.GlobalTimeWrappedHourly * 0.05f;
        }
        public override void DrawInstances()
        {
            Texture2D tex = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/BasicCircle").Value;
            foreach (ShizukuStarParticle particle in Particles)
            {
                Vector2 drawPos = particle.Center - Main.screenPosition;
                Vector2 orig = tex.Size() / 2;
                Vector2 scale = Vector2.One * particle.Size / tex.Size();
                Main.spriteBatch.Draw(tex, drawPos, null, Color.White, 0f, orig, scale, SpriteEffects.None, 0f);
            }
        }

    }
}