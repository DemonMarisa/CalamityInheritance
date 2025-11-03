using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityInheritance.NPCs.Boss.Yharon;
using CalamityMod.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.Yharon.BGM
{
    public class YharonBGM
    {
        public class YharonPhase1MusicScene : BaseMusicSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

            public override int NPCType => ModContent.NPCType<YharonLegacy>();
            public override int? MusicModMusic => CalamityInheritance.Instance.GetMusicFromMusicMod("YharonPhase1");
            public override int VanillaMusic => MusicID.Boss2;
            public override int OtherworldMusic => MusicID.OtherworldlyBoss2;
            public override bool AdditionalCheck()
            {
                return CIGlobalNPC.LegacyYharon != -1 && CIGlobalNPC.LegacyYharonStage2 == -1 && CIGlobalNPC.LegacyYharonStage2FadeIn == -1;
            }
        }
        public class YharonPhase2FadeInMusicScene : BaseMusicSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

            public override int NPCType => ModContent.NPCType<YharonLegacy>();
            public override int? MusicModMusic => MusicLoader.GetMusicSlot(Mod, "Music/YharonStage2FadeIn");
            public override int VanillaMusic => MusicID.Boss2;
            public override int OtherworldMusic => MusicID.OtherworldlyBoss2;
            public override bool AdditionalCheck()
            {
                return CIGlobalNPC.LegacyYharonStage2FadeIn != -1;
            }
        }
        public class YharonPhase2MusicScene : BaseMusicSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

            public override int NPCType => ModContent.NPCType<YharonLegacy>();
            public override int? MusicModMusic => MusicLoader.GetMusicSlot(Mod, "Music/YharonStage2");
            public override int VanillaMusic => MusicID.Boss2;
            public override int OtherworldMusic => MusicID.OtherworldlyBoss2;
            public override bool AdditionalCheck()
            {
                return CIGlobalNPC.LegacyYharonStage2 != -1;
            }
        }
    }
}
