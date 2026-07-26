using CalamityInheritance.Content.Particles;
using LAP.Core.Enums;
using LAP.Core.Graphics.VFX;
using LAP.Core.SystemsLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;

namespace CalamityInheritance.Content.VFXs
{
    public class AOTCNewStarLine : VFXBehavior
    {
        public static void Spawn(int TimeLeft, int owner, int projindex)
        {
            VFXInstance vfx = LAPContent.SpawnVFX(LAPContent.VFXType<AOTCNewStarLine>(), Vector2.Zero, Vector2.Zero, Color.White, 0, 1f);
            vfx.AiInt[1] = owner;
            vfx.AiInt[2] = projindex;
        }
        public override DrawLayer Layer => DrawLayer.BeforeProjectiles;
        public override BlendState BlendState => BlendState.Additive;
        Vector2 AnchorStart => Owner.Center;
        Vector2 AnchorEnd => Father.Center;
        public Vector2 SizeVector => (AnchorEnd - AnchorStart).SafeNormalize(Vector2.Zero) * MathHelper.Clamp((AnchorEnd - AnchorStart).Length(), 0, 760);
        public Player Owner => Main.player[Ownerindex];
        public Projectile Father => Main.projectile[Projectileindex];
        public int Ownerindex => VFXInstance.AiInt[1];
        public int Projectileindex => VFXInstance.AiInt[2];
        public bool FirstFrame = true;
        public List<BloomLine2> BloomLine = [];
        public List<SparkParticle2> Spark = [];
        public override void OnSpawn()
        {
            BloomLine = new List<BloomLine2>();
            Spark = new List<SparkParticle2>();
            VFXInstance.ExtraUpdate = 0;
            VFXInstance.Lifetime = 20;
        }
        public override bool UpdatePosition()
        {
            return false;
        }
        public override void Update()
        {
            if (FirstFrame)
            {
                float constellationColorHue = Main.rand.NextFloat();
                Color constellationColor = Main.hslToRgb(constellationColorHue, 1, 0.8f);
                Vector2 previousStar = AnchorStart;
                Vector2 offset;
                for (float i = 0 + Main.rand.NextFloat(0.2f, 0.5f); i < 1; i += Main.rand.NextFloat(0.2f, 0.5f))
                {
                    constellationColorHue = (constellationColorHue + 0.16f) % 1;
                    constellationColor = Main.hslToRgb(constellationColorHue, 1, 0.8f);

                    offset = Main.rand.NextFloat(-50f, 50f) * SizeVector.RotatedBy(MathHelper.PiOver2).SafeNormalize(Vector2.Zero);
                    SparkParticle2 Star = new SparkParticle2(AnchorStart + SizeVector * i + offset, constellationColor, Color.White, Main.rand.NextFloat(1.5f, 2f), 3f, VFXInstance.Lifetime);
                    SpawnStar(Star);
                    // 先变换到屏幕坐标，再根据玩家坐标变换到世界坐标
                    BloomLine2 Line = new BloomLine2(previousStar, AnchorStart + SizeVector * i + offset, constellationColor * 0.75f, 0.8f, VFXInstance.Lifetime, true);
                    SpawnLine(Line);

                    if (Main.rand.NextBool(3))
                    {
                        constellationColorHue = (constellationColorHue + 0.16f) % 1;
                        constellationColor = Main.hslToRgb(constellationColorHue, 1, 0.8f);

                        offset = Main.rand.NextFloat(-50f, 50f) * SizeVector.RotatedBy(MathHelper.PiOver2).SafeNormalize(Vector2.Zero);
                        Star = new SparkParticle2(AnchorStart + SizeVector * i + offset, constellationColor, Color.White, Main.rand.NextFloat(1.5f, 2f), 3f, VFXInstance.Lifetime);
                        SpawnStar(Star);
                        Line = new BloomLine2(previousStar, AnchorStart + SizeVector * i + offset, constellationColor, 0.8f, 20, true);
                        SpawnLine(Line);
                    }
                    previousStar = AnchorStart + SizeVector * i + offset;
                }
                SparkParticle2 Star3 = new SparkParticle2(Owner.Center, constellationColor, Color.White, Main.rand.NextFloat(1.5f, 2f), 3f, VFXInstance.Lifetime);
                SpawnStar(Star3);
                SparkParticle2 Star2 = new SparkParticle2(AnchorEnd, constellationColor, Color.White, Main.rand.NextFloat(1.5f, 2f), 3f, VFXInstance.Lifetime);
                SpawnStar(Star2);
                BloomLine2 Line2 = new BloomLine2(previousStar, AnchorEnd, constellationColor, 0.8f, VFXInstance.Lifetime, true);
                SpawnLine(Line2);
                FirstFrame = false;
            }
            for (int i = 0; i < BloomLine.Count; i++)
            {
                BloomLine2 particle = BloomLine[i];
                if (particle == null)
                    continue;
                particle.EndPos += particle.Velocity + Owner.velocity;
                particle.Position += particle.Velocity + Owner.velocity;
                particle.Time++;
                particle.Update();
                particle.DrawColor = Main.hslToRgb(Main.rgbToHsl(particle.DrawColor).X + 0.02f, Main.rgbToHsl(particle.DrawColor).Y, Main.rgbToHsl(particle.DrawColor).Z);
            }
            BloomLine.RemoveAll(particle => particle.Time >= particle.Lifetime);
            for (int i = 0; i < Spark.Count; i++)
            {
                SparkParticle2 particle = Spark[i];
                if (particle == null)
                    continue;
                particle.Position += particle.Velocity + Owner.velocity;
                particle.Time++;
                particle.Update();
                particle.DrawColor = Main.hslToRgb(Main.rgbToHsl(particle.DrawColor).X + 0.02f, Main.rgbToHsl(particle.DrawColor).Y, Main.rgbToHsl(particle.DrawColor).Z);
            }
            Spark.RemoveAll(particle => particle.Time >= particle.Lifetime);
        }
        public override void Draw()
        {
            if (BloomLine != null)
            {
                foreach (BloomLine2 particle in BloomLine)
                    particle.Draw(Main.spriteBatch);
            }
            if (Spark != null)
            {
                foreach (SparkParticle2 particle in Spark)
                    particle.Draw(Main.spriteBatch);
            }
        }
        public void SpawnLine(BloomLine2 particle)
        {
            if (!Main.dedServ)
                BloomLine.Add(particle);
        }
        public void SpawnStar(SparkParticle2 particle)
        {
            if (!Main.dedServ)
                Spark.Add(particle);
        }
    }
}
