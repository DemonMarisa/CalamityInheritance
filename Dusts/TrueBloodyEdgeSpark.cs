using System;
using Terraria.Graphics.Renderers;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Dusts
{
    public static class TrueBloodyEdgeSpark
    {
        private static PrettySparkleParticle GetNewPrettySparkleParticle()
        {
            return new PrettySparkleParticle();
        }

        private static ParticlePool<PrettySparkleParticle> _poolPrettySparkle = new ParticlePool<PrettySparkleParticle>(200, GetNewPrettySparkleParticle);
        public static void GeneratePrettySparkles(Vector2 position, float num, float num2, float scaleModifier = 1f)
        {
            for (float num3 = 0f; num3 < 3f; num3 += 1f)
            {
                PrettySparkleParticle prettySparkleParticle = _poolPrettySparkle.RequestParticle();
                Vector2 vector = ((float)Math.PI / 4f + (float)Math.PI / 4f * num3 + num2).ToRotationVector2() * 4f;
                prettySparkleParticle.ColorTint = new Color(1f, 0.25f, 0.1f, 1f);
                prettySparkleParticle.LocalPosition = position;
                prettySparkleParticle.Rotation = vector.ToRotation();
                prettySparkleParticle.Scale = new Vector2(2f, 1f) * 1.1f;
                prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
                prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
                prettySparkleParticle.TimeToLive = num;
                prettySparkleParticle.FadeOutEnd = num;
                prettySparkleParticle.FadeInEnd = num / 2f;
                prettySparkleParticle.FadeOutStart = num / 2f;
                prettySparkleParticle.AdditiveAmount = 0.35f;
                prettySparkleParticle.LocalPosition -= vector * num * 0.25f;
                prettySparkleParticle.Velocity = vector;
                prettySparkleParticle.DrawVerticalAxis = false;
                if (num3 == 1f)
                {
                    prettySparkleParticle.Scale *= 1.5f;
                    prettySparkleParticle.Velocity *= 1.5f;
                    prettySparkleParticle.LocalPosition -= prettySparkleParticle.Velocity * 4f;
                }
                Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
            }
            for (float num4 = 0f; num4 < 3f; num4 += 1f)
            {
                PrettySparkleParticle prettySparkleParticle2 = _poolPrettySparkle.RequestParticle();
                Vector2 vector2 = ((float)Math.PI / 4f + (float)Math.PI / 4f * num4 + num2).ToRotationVector2() * 4f;
                prettySparkleParticle2.ColorTint = new Color(1f, 0f, 0f, 1f);
                prettySparkleParticle2.LocalPosition = position;
                prettySparkleParticle2.Rotation = vector2.ToRotation();
                prettySparkleParticle2.Scale = new Vector2(2f, 1f) * 0.7f;
                prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
                prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
                prettySparkleParticle2.TimeToLive = num;
                prettySparkleParticle2.FadeOutEnd = num;
                prettySparkleParticle2.FadeInEnd = num / 2f;
                prettySparkleParticle2.FadeOutStart = num / 2f;
                prettySparkleParticle2.LocalPosition -= vector2 * num * 0.25f;
                prettySparkleParticle2.Velocity = vector2;
                prettySparkleParticle2.DrawVerticalAxis = false;
                if (num4 == 1f)
                {
                    prettySparkleParticle2.Scale *= 1.5f;
                    prettySparkleParticle2.Velocity *= 1.5f;
                    prettySparkleParticle2.LocalPosition -= prettySparkleParticle2.Velocity * 4f;
                }
                Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustPerfect(position, 115, vector2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
                    dust.noGravity = true;
                    dust.scale = 1.4f;
                    Dust dust2 = Dust.NewDustPerfect(position, 115, -vector2.RotatedBy(Main.rand.NextFloatDirection() * ((float)Math.PI * 2f) * 0.025f) * Main.rand.NextFloat());
                    dust2.noGravity = true;
                    dust2.scale = 1.4f;
                }
            }
        }
    }
}
