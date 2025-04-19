using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.NPCs.Boss.SCAL.Sky
{
    public class SCalBackgroundScene4 : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

        public override bool IsSceneEffectActive(Player player)
        {
            if (CIGlobalNPC.LegacySCalAcceptance != -1)
                return true;
            else
                return false;
        }
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("CalamityInheritance:SupremeCalamitasLegacy4", isActive);
        }
    }
}
