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

namespace CalamityInheritance.NPCs.Boss.Yharon.Sky
{
    public class YharonScreenShaderDataLegacy : ScreenShaderData
    {
        private int YharonIndex;

        public YharonScreenShaderDataLegacy(string passName) : base(passName)
        {
        }

        private void UpdateIndex()
        {
            int YharonType = ModContent.NPCType<YharonLegacy>();
            if (YharonIndex >= 0 && Main.npc[YharonIndex].active && Main.npc[YharonIndex].type == YharonType)
            {
                return;
            }
            YharonIndex = NPC.FindFirstNPC(YharonType);
        }


        public override void Update(GameTime gameTime)
        {
            if (YharonIndex == -1)
            {
                UpdateIndex();
                if (YharonIndex == -1)
                {
                    Filters.Scene["CalamityInheritance:Yharon"].Deactivate(Array.Empty<object>());
                }
            }
        }

        public override void Apply()
        {
            UpdateIndex();
            if (YharonIndex != -1)
            {
                UseTargetPosition(Main.npc[YharonIndex].Center);
            }
            base.Apply();
        }
    }
}
