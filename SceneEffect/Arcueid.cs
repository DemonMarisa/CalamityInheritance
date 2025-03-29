using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.System.Configs;
using CalamityInheritance.System;

namespace CalamityInheritance.SceneEffect
{
    public class Arcueid: ModSceneEffect
    {
        public override SceneEffectPriority Priority
        {
            get
            {
                return SceneEffectPriority.Environment;
            }
        }
        public override int Music =>new int?(MusicLoader.GetMusicSlot(Mod,"Music/MoonPrincess")).Value;
        public override bool IsSceneEffectActive(Player player)
        {
            return (double)(Main.LocalPlayer.position.Y / 16f) <= Main.worldSurface * 0.35 && !CIConfig.Instance.BlessingoftheMoon && !PlanetoidsCounts.Planetoids && CIConfig.Instance.Arcueid;
        }
    }
}
