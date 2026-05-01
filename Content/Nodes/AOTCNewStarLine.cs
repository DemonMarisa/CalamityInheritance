using CalamityInheritance.Content.Projectiles.HeldProj.Melee.AOTCNew;
using CalamityInheritance.Particles;
using CalamityMod;
using CalamityMod.Particles;
using LAP.Core.Enums;
using LAP.Core.Graphics.DrawNode;
using LAP.Core.ParticleSystem;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;

namespace CalamityInheritance.Content.Nodes
{
    public class AOTCNewStarLine : DrawNode
    {
        public AOTCNewStarLine(int TimeLeft, int owner, int projindex)
        {
            Lifetime = TimeLeft;
            ownerindex = owner;
            projectileindex = projindex;
        }
        Vector2 AnchorStart => Owner.Center;
        // 14NOV2024: Ozzatron: I have no idea what this does so I clamped it
        Vector2 AnchorEnd => Father.Center;
        public Vector2 SizeVector => Utils.SafeNormalize(AnchorEnd - AnchorStart, Vector2.Zero) * MathHelper.Clamp((AnchorEnd - AnchorStart).Length(), 0, 760);
        public override DrawLayer Layer => DrawLayer.BeforeProjectiles;
        public override int BlendState => BlendStateID.Additive;
        public Player Owner => Main.player[ownerindex];
        public override bool UseShader => false;
        public Projectile Father => Main.projectile[projectileindex];
        public bool FirstFrame = true;
        public int ownerindex;
        public int projectileindex;
        public List<BloomLine2> BloomLine = [];
        public List<SparkParticle2> Spark = [];
        public override void OnSpawn()
        {
            base.OnSpawn();
        }
        public override bool UpDatePos()
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

                    offset = Main.rand.NextFloat(-50f, 50f) * Utils.SafeNormalize(SizeVector.RotatedBy(MathHelper.PiOver2), Vector2.Zero);
                    SparkParticle2 Star = new SparkParticle2(AnchorStart + SizeVector * i + offset, constellationColor, Color.White, Main.rand.NextFloat(1.5f, 2f), 3f, 20);
                    SpawnStar(Star);
                    // 先变换到屏幕坐标，再根据玩家坐标变换到世界坐标
                    BloomLine2 Line = new BloomLine2(previousStar, (AnchorStart + SizeVector * i + offset), constellationColor * 0.75f, 0.8f, 20, true);
                    SpawnLine(Line);

                    if (Main.rand.NextBool(3))
                    {
                        constellationColorHue = (constellationColorHue + 0.16f) % 1;
                        constellationColor = Main.hslToRgb(constellationColorHue, 1, 0.8f);

                        offset = Main.rand.NextFloat(-50f, 50f) * Utils.SafeNormalize(SizeVector.RotatedBy(MathHelper.PiOver2), Vector2.Zero);
                        Star = new SparkParticle2(AnchorStart + SizeVector * i + offset, constellationColor, Color.White, Main.rand.NextFloat(1.5f, 2f), 3f, 20);
                        SpawnStar(Star);
                        Line = new BloomLine2(previousStar, (AnchorStart + SizeVector * i + offset), constellationColor, 0.8f, 20, true);
                        SpawnLine(Line);
                    }
                    previousStar = AnchorStart + SizeVector * i + offset;
                }
                SparkParticle2 Star3 = new SparkParticle2(Owner.Center, constellationColor, Color.White, Main.rand.NextFloat(1.5f, 2f), 3f, 20);
                SpawnStar(Star3);
                SparkParticle2 Star2 = new SparkParticle2(AnchorEnd, constellationColor, Color.White, Main.rand.NextFloat(1.5f, 2f), 3f, 20);
                SpawnStar(Star2);
                BloomLine2 Line2 = new BloomLine2(previousStar, AnchorEnd, constellationColor, 0.8f, 20, true);
                SpawnLine(Line2);
                FirstFrame = false;
            }
            foreach (BloomLine2 particle in BloomLine)
            {
                if (particle == null)
                    continue;
                particle.EndPos += particle.Velocity + Owner.velocity;
                particle.Position += particle.Velocity + Owner.velocity;
                particle.Time++;
                particle.Update();
                particle.DrawColor = Main.hslToRgb(Main.rgbToHsl(particle.DrawColor).X + 0.02f, Main.rgbToHsl(particle.DrawColor).Y, Main.rgbToHsl(particle.DrawColor).Z);
            }
            BloomLine.RemoveAll(particle => (particle.Time >= particle.Lifetime));
            foreach (SparkParticle2 particle in Spark)
            {
                if (particle == null)
                    continue;
                particle.Position += particle.Velocity + Owner.velocity;
                particle.Time++;
                particle.Update();
                particle.DrawColor = Main.hslToRgb(Main.rgbToHsl(particle.DrawColor).X + 0.02f, Main.rgbToHsl(particle.DrawColor).Y, Main.rgbToHsl(particle.DrawColor).Z);
            }
            Spark.RemoveAll(particle => (particle.Time >= particle.Lifetime));
        }
        public override void Draw(SpriteBatch sb)
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
