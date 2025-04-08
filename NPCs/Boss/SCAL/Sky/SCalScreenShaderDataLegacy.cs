using System;
using Microsoft.Xna.Framework;
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

        private void UpdateSCalIndex()
        {
            int SCalType = ModContent.NPCType<SupremeCalamitasLegacy>();
            if (SCalIndex >= 0 && Main.npc[SCalIndex].active && Main.npc[SCalIndex].type == SCalType)
            {
                return;
            }
            SCalIndex = NPC.FindFirstNPC(SCalType);
        }


        public override void Update(GameTime gameTime)
        {
            if (SCalIndex == -1)
            {
                UpdateSCalIndex();
                if (SCalIndex == -1)
                {
                    Filters.Scene["CalamityInheritance:SupremeCalamitasLegacy1"].Deactivate(Array.Empty<object>());
                    Filters.Scene["CalamityInheritance:SupremeCalamitasLegacy2"].Deactivate(Array.Empty<object>());
                    Filters.Scene["CalamityInheritance:SupremeCalamitasLegacy3"].Deactivate(Array.Empty<object>());
                    Filters.Scene["CalamityInheritance:SupremeCalamitasLegacy4"].Deactivate(Array.Empty<object>());
                }
            }
        }

        public override void Apply()
        {
            UpdateSCalIndex();
            if (SCalIndex != -1)
            {
                UseTargetPosition(Main.npc[SCalIndex].Center);
            }
            base.Apply();
        }
    }
}
