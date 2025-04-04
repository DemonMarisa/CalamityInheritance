using CalamityInheritance.NPCs.Boss.Calamitas;
using CalamityMod.NPCs.CalClone;
using CalamityMod.Systems;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.System.Scenes.MusicScenes
{
    public class CalamitasCloneMusicScene : BaseMusicSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;

        public override int NPCType => ModContent.NPCType<CalamitasReborn>();
        public override int? MusicModMusic => CalamityInheritance.Instance.GetMusicFromMusicMod("CalamitasClone");
        public override int VanillaMusic => MusicID.Boss2;
        public override int OtherworldMusic => MusicID.OtherworldlyBoss2;
        public override int[] AdditionalNPCs => new int[]
        {
            ModContent.NPCType<Cataclysm>(),
            ModContent.NPCType<Catastrophe>()
        };
    }

}
