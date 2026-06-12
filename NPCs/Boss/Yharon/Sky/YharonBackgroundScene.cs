using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.Yharon.Sky
{
    public class YharonBackgroundScene : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

        public override bool IsSceneEffectActive(Player player)
        {
            if (CIGlobalNPC.LegacyYharon != -1)
                return true;
            else
                return false;
        }
        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("CalamityInheritance:Yharon", isActive);
        }
    }
}
