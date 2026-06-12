using CalamityInheritance.UI.MusicUI;
using CalamityInheritance.UI.MusicUI.MusicButton;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.MusicScene
{
    public class CalTitleScene
    {
        public class CalTitleNor : ModSceneEffect
        {
            public override int Music => MusicLoader.GetMusicSlot(Mod, "Music/TheTaleofaCruelWorld/TheTaleofaCruelWorldNor");

            public override SceneEffectPriority Priority => (SceneEffectPriority)15;

            public override bool IsSceneEffectActive(Player player)
            {
                return NorVerBehavior.NorVerOpen && !MusicChoiceUI.turnOffAll;
            }
        }
        public class CalTitleMusicBox : ModSceneEffect
        {
            public override int Music => MusicLoader.GetMusicSlot(Mod, "Music/TheTaleofaCruelWorld/TheTaleofaCruelWorldMusicBox");

            public override SceneEffectPriority Priority => (SceneEffectPriority)15;

            public override bool IsSceneEffectActive(Player player)
            {
                return MusicBoxVerBehavior.MusicBoxVerOpen && !MusicChoiceUI.turnOffAll;
            }
        }
        public class CalTitlePiano : ModSceneEffect
        {
            public override int Music => MusicLoader.GetMusicSlot(Mod, "Music/TheTaleofaCruelWorld/TheTaleofaCruelWorldPiano");

            public override SceneEffectPriority Priority => (SceneEffectPriority)15;

            public override bool IsSceneEffectActive(Player player)
            {
                return PianoVerBehavior.PianoVerOpen && !MusicChoiceUI.turnOffAll;
            }
        }
    }
}
