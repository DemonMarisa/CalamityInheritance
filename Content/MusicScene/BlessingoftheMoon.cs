using CalamityInheritance.Core.Conditions;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.MusicScene
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
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Music/BlessingoftheMoon");
        public override bool IsSceneEffectActive(Player player)
        {
            return (double)(Main.LocalPlayer.position.Y / 16f) <= Main.worldSurface * 0.35 && !PlanetoidsCounts.Planetoids;
        }
    }
}
