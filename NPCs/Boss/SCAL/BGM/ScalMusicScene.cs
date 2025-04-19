using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs;
using CalamityMod.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.NPCs.Boss.SCAL.Sky;
using Terraria;
using CalamityInheritance.NPCs.Boss.SCAL.SoulSeeker;

namespace CalamityInheritance.NPCs.Boss.SCAL.BGM
{
    public class ScalMusicScene
    {
        public class ScalPhase1MusicScene : BaseMusicSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

            public override int NPCType => ModContent.NPCType<SupremeCalamitasLegacy>();
            public override int? MusicModMusic => CalamityInheritance.Instance.GetMusicFromMusicMod("CalamitasPhase1");
            public override int VanillaMusic => MusicID.Boss2;
            public override int OtherworldMusic => MusicID.OtherworldlyBoss2;
            public override bool AdditionalCheck() => CIGlobalNPC.LegacySCalGrief != -1;
        }
        public class ScalPhase2MusicScene : BaseMusicSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

            public override int NPCType => ModContent.NPCType<SupremeCalamitasLegacy>();
            public override int? MusicModMusic => CalamityInheritance.Instance.GetMusicFromMusicMod("CalamitasPhase2");
            public override int VanillaMusic => MusicID.Boss2;
            public override int OtherworldMusic => MusicID.OtherworldlyBoss2;
            public override bool AdditionalCheck() => CIGlobalNPC.LegacySCalLament != -1;
        }
        public class ScalPhase3MusicScene : BaseMusicSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

            public override int NPCType => ModContent.NPCType<SupremeCalamitasLegacy>();
            public override int? MusicModMusic => CalamityInheritance.Instance.GetMusicFromMusicMod("CalamitasPhase3");
            public override int VanillaMusic => MusicID.Boss2;
            public override int OtherworldMusic => MusicID.OtherworldlyBoss2;
            public override bool AdditionalCheck() => CIGlobalNPC.LegacySCalEpiphany != -1;
        }
        public class ScalPhase4MusicScene : BaseMusicSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

            public override int NPCType => ModContent.NPCType<SupremeCalamitasLegacy>();
            public override int? MusicModMusic => CalamityInheritance.Instance.GetMusicFromMusicMod("CalamitasDefeat_LongFade");
            public override int VanillaMusic => MusicID.Boss2;
            public override int OtherworldMusic => MusicID.OtherworldlyBoss2;
            public override bool AdditionalCheck() => CIGlobalNPC.LegacySCalAcceptance != -1;
        }
    }
}
