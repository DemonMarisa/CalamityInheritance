using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.NPCs.Boss.SCAL.Sky
{
    public class SCalBackgroundScene3 : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

        public override bool IsSceneEffectActive(Player player)
        {
            if (!Main.npc.IndexInRange(CIGlobalNPC.LegacySCal) || Main.npc[CIGlobalNPC.LegacySCal].type != ModContent.NPCType<SupremeCalamitasLegacy>())
                return false;

            NPC scal = Main.npc[CIGlobalNPC.LegacySCal];
            float lifeRatio = scal.life / (float)scal.lifeMax;
            bool active = (NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()) || SCalSkyLegacy.OverridingIntensity > 0f) && lifeRatio > 0.01f && lifeRatio < 0.3f;
            return active;
        }
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("CalamityInheritance:SupremeCalamitasLegacy3", isActive);
        }
    }
}
