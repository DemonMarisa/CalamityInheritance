using CalamityInheritance.Core.CIWorlds;
using LAP.Assets.Effects;
using LAP.Core.Graphics.LAPPostProgress;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;

namespace CalamityInheritance.Content.NPCs.Boss.Scal.Sky
{
    public class ScalPP : LAPPostProgress
    {
        public float Strength;
        public Color DrawColor = new Color(1.1f, 0.3f, 0.3f);
        public override void Update()
        {
            if (CIBossAliveFlag.LegacySCal == -1)
            {
                Strength = MathHelper.Lerp(Strength, 0f, 0.03f);
            }
            else
            {
                Strength = MathHelper.Lerp(Strength, 1f, 0.03f);
            }
            Main.lightning = Strength;
            if (CIBossAliveFlag.LegacySCalBGM == SupremeCalamitasLegacy.Acceptance)
                DrawColor = Color.Lerp(DrawColor, new Color(0f, 0f, 0f, 1f) * Strength, 0.03f);
            else if (CIBossAliveFlag.LegacySCalBGM == SupremeCalamitasLegacy.Epiphany)
                DrawColor = Color.Lerp(DrawColor, new Color(1.1f, 0.4f, 0f) * Strength, 0.03f);
            else if (CIBossAliveFlag.LegacySCalBGM == SupremeCalamitasLegacy.Lament)
                DrawColor = Color.Lerp(DrawColor, new Color(0.2f, 0.8f, 1f) * Strength, 0.03f);
            else if (CIBossAliveFlag.LegacySCalBGM == SupremeCalamitasLegacy.Grief)
                DrawColor = Color.Lerp(DrawColor, new Color(1.1f, 0.3f, 0.3f) * Strength, 0.03f);
        }
        public override void Draw(RenderTarget2D finalTexture, RenderTarget2D screenTarget, RenderTarget2D screenTargetSwap, Color clearColor)
        {
            Main.spriteBatch.End();

            screenTargetSwap.SwapToTarget();

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null);
            Main.spriteBatch.Draw(screenTarget, Vector2.Zero, Color.White);
            Main.spriteBatch.End();

            screenTarget.SwapToTarget();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, Main.Rasterizer, null);
            if (CIBossAliveFlag.LegacySCalBGM == SupremeCalamitasLegacy.Acceptance)
            {
                Main.screenShader.Parameters["uColor"]?.SetValue(DrawColor.ToVector4());
                Main.screenShader.Parameters["uOpacity"]?.SetValue(Strength);
                Main.screenShader.CurrentTechnique.Passes["FilterMiniTower"].Apply();

                Main.spriteBatch.Draw(screenTargetSwap, Vector2.Zero, Color.White);
            }
            else
            {
                Effect effect = LAPShaderRegister.OverlayPass.Value;
                effect.Parameters["uIntensity"].SetValue(Strength * 0.52f);
                effect.CurrentTechnique.Passes[0].Apply();

                Main.spriteBatch.Draw(screenTargetSwap, Vector2.Zero, DrawColor * Strength);

                LAPUtilities.ApplyDefaultShader();

                Main.spriteBatch.Draw(screenTargetSwap, Vector2.Zero, DrawColor with { A = 50 } * Strength * 0.1f);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null);

        }
        public override bool DeActiveCondition()
        {
            return Strength < 0.001f && CIBossAliveFlag.LegacySCal == -1;
        }
    }
}
