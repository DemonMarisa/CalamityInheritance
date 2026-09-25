using CalamityInheritance.Core.CIWorlds;
using LAP.Assets.Effects;
using LAP.Core.Graphics.LAPPostProgress;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace CalamityInheritance.Content.NPCs.Boss.CalamitasClone.Sky
{
    public class CalClonePostProgress : LAPPostProgress
    {
        public float Strength;
        public override void Update()
        {
            if (CIBossAliveFlag.ActiveCalCloneFlag == -1)
            {
                Strength = MathHelper.Lerp(Strength, 0f, 0.03f);
            }
            else
            {
                Strength = MathHelper.Lerp(Strength, 1f, 0.03f);
            }
            Main.lightning = Strength;
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

            Main.spriteBatch.Draw(screenTarget, Vector2.Zero, Color.White);

            Effect effect = LAPShaderRegister.OverlayPass.Value;
            effect.Parameters["uIntensity"].SetValue(Strength * 0.52f);
            effect.CurrentTechnique.Passes[0].Apply();

            Main.spriteBatch.Draw(screenTargetSwap, Vector2.Zero, new Color(1.1f, 0.3f, 0.3f) * Strength);

            LAPUtilities.ApplyDefaultShader();

            Main.spriteBatch.Draw(screenTargetSwap, Vector2.Zero, new Color(1.1f, 0.3f, 0.3f, 0.2f) * Strength * 0.2f);
            Main.spriteBatch.End();

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null);

        }
        public override bool DeActiveCondition()
        {
            return Strength < 0.001f && CIBossAliveFlag.ActiveCalCloneFlag == -1;
        }
    }
}
