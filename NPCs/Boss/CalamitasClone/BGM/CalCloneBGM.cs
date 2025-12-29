using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityMod.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.CalamitasClone.BGM
{
    public class CalCloneBGM
    {
        public class CalClonePhase1MusicScene : BaseMusicSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

            public override int NPCType => NPCType<CalamitasCloneLegacy>();
            public override int? MusicModMusic => MusicID.Boss2;
            public override int VanillaMusic => MusicID.Boss2;
            public override int OtherworldMusic => MusicID.OtherworldlyBoss2;
            public override bool AdditionalCheck()
            {
                return CIGlobalNPC.LegacyCalamitasClone != -1 && CIGlobalNPC.LegacyCalamitasCloneP2 == -1;
            }
        }
        public class CalClonePhase2MusicScene : BaseMusicSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

            public override int NPCType => NPCType<CalamitasCloneLegacy>();
            public override int? MusicModMusic => CalamityInheritance.Instance.GetMusicFromMusicMod("CalamitasClone");
            public override int VanillaMusic => MusicID.Boss2;
            public override int OtherworldMusic => MusicID.OtherworldlyBoss2;
            public override bool AdditionalCheck() => CIGlobalNPC.LegacyCalamitasCloneP2 != -1;
        }
    }
}
