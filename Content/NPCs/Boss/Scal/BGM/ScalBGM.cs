using CalamityInheritance.Core.CIWorlds;
using CalamityInheritance.Music;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.NPCs.Boss.Scal.BGM
{
    public class ScalBGM : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int Music => GetMusicID();
        public int GetMusicID()
        {
            if (CIBossAliveFlag.LegacySCalBGM == SupremeCalamitasLegacy.Acceptance)
                return MusicLoader.GetMusicSlot(Mod, CIMusicRegister.CalamitasPhase4);
            if (CIBossAliveFlag.LegacySCalBGM == SupremeCalamitasLegacy.Epiphany)
                return MusicLoader.GetMusicSlot(Mod, CIMusicRegister.CalamitasPhase3);
            if (CIBossAliveFlag.LegacySCalBGM == SupremeCalamitasLegacy.Lament)
                return MusicLoader.GetMusicSlot(Mod, CIMusicRegister.CalamitasPhase2);
            if (CIBossAliveFlag.LegacySCalBGM == SupremeCalamitasLegacy.Grief)
                return MusicLoader.GetMusicSlot(Mod, CIMusicRegister.CalamitasPhase1);
            return -1;
        }
        public override bool IsSceneEffectActive(Player player)
        {
            return CIBossAliveFlag.LegacySCal > 0;
        }
    }
}
