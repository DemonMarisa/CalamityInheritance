using LAP.Core.Enums;
using LAP.Core.Graphics.PixelatedRender;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.Graphics.Insertlayer
{
    public class InsertDraw : ModSystem
    {
        public static Queue<Action> GlowRequests_AfterPlayer = new Queue<Action>();
        public static bool BeginCheckDraw;
        public override void Load()
        {
            On_Main.DrawPlayers_AfterProjectiles += DrawRequest_AfterPlayer;
        }
        public override void Unload()
        {
            On_Main.DrawPlayers_AfterProjectiles -= DrawRequest_AfterPlayer;
        }
        public static void SubmitDrawRequest_APlayer(Action drawAction)
        {
            if (Main.dedServ || drawAction == null)
                return;
            GlowRequests_AfterPlayer.Enqueue(drawAction);
        }
        public override void PreUpdatePlayers()
        {
            BeginCheckDraw = false;
        }
        public static void DrawRequest_AfterPlayer(On_Main.orig_DrawPlayers_AfterProjectiles orig, Main self)
        {
            orig(self);
            if (GlowRequests_AfterPlayer.Count != 0)
            {
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                while (GlowRequests_AfterPlayer.Count > 0)
                {
                    Action drawAction = GlowRequests_AfterPlayer.Dequeue();
                    drawAction.Invoke();
                }
                Main.spriteBatch.End();
            }
        }
    }
}
