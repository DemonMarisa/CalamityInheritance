using CalamityMod.Systems;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.SCAL.BGM
{
    public class ScalMusicScene
    {
        public class ScalPhase1MusicScene : BaseMusicSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

            public override int NPCType => NPCType<SupremeCalamitasLegacy>();
            public override int? MusicModMusic => CalamityInheritance.Instance.GetMusicFromMusicMod("CalamitasPhase1");
            public override int VanillaMusic => MusicID.Boss2;
            public override int OtherworldMusic => MusicID.OtherworldlyBoss2;
            public override bool AdditionalCheck() => CIGlobalNPC.LegacySCalGrief != -1;
        }
        public class ScalPhase2MusicScene : BaseMusicSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

            public override int NPCType => NPCType<SupremeCalamitasLegacy>();
            public override int? MusicModMusic => CalamityInheritance.Instance.GetMusicFromMusicMod("CalamitasPhase2");
            public override int VanillaMusic => MusicID.Boss2;
            public override int OtherworldMusic => MusicID.OtherworldlyBoss2;
            public override bool AdditionalCheck() => CIGlobalNPC.LegacySCalLament != -1;
        }
        public class ScalPhase3MusicScene : BaseMusicSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

            public override int NPCType => NPCType<SupremeCalamitasLegacy>();
            public override int? MusicModMusic => CalamityInheritance.Instance.GetMusicFromMusicMod("CalamitasPhase3");
            public override int VanillaMusic => MusicID.Boss2;
            public override int OtherworldMusic => MusicID.OtherworldlyBoss2;
            public override bool AdditionalCheck() => CIGlobalNPC.LegacySCalEpiphany != -1;
        }
        public class ScalPhase4MusicScene : BaseMusicSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

            public override int NPCType => NPCType<SupremeCalamitasLegacy>();
            public override int? MusicModMusic => CalamityInheritance.Instance.GetMusicFromMusicMod("CalamitasDefeat_LongFade");
            public override int VanillaMusic => MusicID.Boss2;
            public override int OtherworldMusic => MusicID.OtherworldlyBoss2;
            public override bool AdditionalCheck() => CIGlobalNPC.LegacySCalAcceptance != -1;
        }
    }
}
