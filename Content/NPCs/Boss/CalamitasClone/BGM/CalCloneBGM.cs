using CalamityInheritance.Core.CIWorlds;
using CalamityInheritance.Core.Conditions;
using CalamityInheritance.Music;
using CalamityMod.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.CalamitasClone.BGM
{
    public class CalCloneBGM
    {
        public class CalClonePhase1MusicScene : ModSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
            public override int Music => MusicLoader.GetMusicSlot(Mod, CIMusicRegister.CalamitasClone);
            public override bool IsSceneEffectActive(Player player)
            {
                return CIBossAliveFlag.ActiveCalCloneP1Flag < 0 && CIBossAliveFlag.ActiveCalCloneFlag > 0;
            }
        }
        public class CalClonePhase2MusicScene : ModSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
            public override int Music => MusicID.Boss2;
            public override bool IsSceneEffectActive(Player player)
            {
                return CIBossAliveFlag.ActiveCalCloneP1Flag > 0 && CIBossAliveFlag.ActiveCalCloneFlag < 0;
            }
        }
    }
}
