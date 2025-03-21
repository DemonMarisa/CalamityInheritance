using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.System.Configs;
using CalamityInheritance.System;

namespace CalamityInheritance.SceneEffect
{
    public class BlessingoftheMoon : ModSceneEffect
    {
        public override SceneEffectPriority Priority
        {
            get
            {
                return SceneEffectPriority.Environment;
            }
        }
        public override int Music =>new int?(MusicLoader.GetMusicSlot(Mod,"Music/BlessingoftheMoon")).Value;
        public override bool IsSceneEffectActive(Player player)
        {
            return (double)(Main.LocalPlayer.position.Y / 16f) <= Main.worldSurface * 0.35 && PlanetoidsCounts.Planetoids && CIConfig.Instance.BlessingoftheMoon;
        }
    }
}
