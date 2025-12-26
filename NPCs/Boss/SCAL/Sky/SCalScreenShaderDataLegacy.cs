using CalamityInheritance.System;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.SCAL.Sky
{
    public class SCalScreenShaderDataLegacy : ScreenShaderData
    {
        private int SCalIndex;
        public Color Phase1Colore = new(205, 100, 100);
        public Color Phase2Colore = new(100, 100, 205);
        public Color Phase3Colore = new(255, 155, 60);
        public Color Phase4Colore = new(50, 50, 50);

        public SCalScreenShaderDataLegacy(string passName) : base(passName)
        {
        }
        public override void Update(GameTime gameTime)
        {
            if (!MiscFlagReset.ScalSkyActive)
            {
                Filters.Scene["CalamityInheritance:SupremeCalamitasLegacy1"].Deactivate(Array.Empty<object>());
                Filters.Scene["CalamityInheritance:SupremeCalamitasLegacy2"].Deactivate(Array.Empty<object>());
                Filters.Scene["CalamityInheritance:SupremeCalamitasLegacy3"].Deactivate(Array.Empty<object>());
                Filters.Scene["CalamityInheritance:SupremeCalamitasLegacy4"].Deactivate(Array.Empty<object>());
            }
        }

        public override void Apply()
        {
            if (MiscFlagReset.ScalSkyActive)
                UseTargetPosition(Main.LocalPlayer.Center);
            base.Apply();
        }
    }
}
