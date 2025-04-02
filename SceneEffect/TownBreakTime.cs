using CalamityInheritance.Core;
using CalamityInheritance.System.Configs;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.SceneEffect
{
    public class TownBreakTime : ModSceneEffect
    {
        public override SceneEffectPriority Priority
        {
            get
            {
                return SceneEffectPriority.Environment;
            }
        }
        public override int Music =>new int?(MusicLoader.GetMusicSlot(Mod,"Music/Kunojihousing")).Value;
        public override bool IsSceneEffectActive(Player player)
        {
            return Main.LocalPlayer.townNPCs > 3f && Main.dayTime && Main.LocalPlayer.ZoneForest && CIConfig.Instance.Kunoji;
        }
    }
}