using CalamityInheritance.NPCs.Boss.SCAL;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria;
using Terraria.Graphics.Effects;

namespace CalamityInheritance.NPCs.Boss.CalamitasClone.Sky
{
    public class CalCloneScreenShaderDataLegacy : ScreenShaderData
    {
        private int CalCloneIndex;

        public CalCloneScreenShaderDataLegacy(string passName) : base(passName)
        {
        }

        private void UpdateIndex()
        {
            int SCalType = NPCType<CalamitasCloneLegacy>();
            if (CalCloneIndex >= 0 && Main.npc[CalCloneIndex].active && Main.npc[CalCloneIndex].type == SCalType)
            {
                return;
            }
            CalCloneIndex = NPC.FindFirstNPC(SCalType);
        }


        public override void Update(GameTime gameTime)
        {
            if (CalCloneIndex == -1)
            {
                UpdateIndex();
                if (CalCloneIndex == -1)
                {
                    Filters.Scene["CalamityInheritance:CalClone"].Deactivate(Array.Empty<object>());
                }
            }
        }

        public override void Apply()
        {
            UpdateIndex();
            if (CalCloneIndex != -1)
            {
                UseTargetPosition(Main.npc[CalCloneIndex].Center);
            }
            base.Apply();
        }
    }
}
