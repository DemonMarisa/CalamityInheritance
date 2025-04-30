using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.NPCs.Boss.CalamitasClone.Sky
{
    public class CalCloneBackgroundScene : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

        public override bool IsSceneEffectActive(Player player)
        {
            if (CIGlobalNPC.LegacyCalamitasCloneP2 != -1)
                return true;
            else
                return false;
        }
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("CalamityInheritance:CalClone", isActive);
        }
    }
}
