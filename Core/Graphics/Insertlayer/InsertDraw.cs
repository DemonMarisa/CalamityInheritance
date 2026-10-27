using LAP.Core.Enums;
using LAP.Core.Graphics.PixelatedRender;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.Graphics.Insertlayer
{
    public class InsertDraw : ModSystem
    {
        public static Dictionary<string, Action> CIPlayerDraw_PostDraw = new Dictionary<string, Action>();
        public static bool DrawAuricTrail;
        public override void Load()
        {
            On_Main.DrawPlayers_AfterProjectiles += DrawRequest_AfterPlayer;
        }
        public override void Unload()
        {
            On_Main.DrawPlayers_AfterProjectiles -= DrawRequest_AfterPlayer;
        }
        public static void SubmitDrawRequest_APlayer(string name, Action drawAction)
        {
            if (Main.dedServ || drawAction == null)
                return;
            if (!CIPlayerDraw_PostDraw.ContainsKey(name))
                CIPlayerDraw_PostDraw.Add(name, drawAction);
        }
        public static void DrawRequest_AfterPlayer(On_Main.orig_DrawPlayers_AfterProjectiles orig, Main self)
        {
            if (DrawAuricTrail)
            {
                orig(self);
                DrawAuricTrail = false;
            }
            orig(self);
            if (CIPlayerDraw_PostDraw.Count != 0)
            {
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                foreach (var key in CIPlayerDraw_PostDraw.Values)
                {
                    key.Invoke();
                }
                Main.spriteBatch.End();
                CIPlayerDraw_PostDraw.Clear();
            }
        }
    }
}
