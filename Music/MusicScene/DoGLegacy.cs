using CalamityInheritance.System;
using CalamityInheritance.System.Configs;
using CalamityMod.Events;
using CalamityMod.Items.Accessories;
using CalamityMod.World;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Music.MusicScene
{
    public class DoGLegacy: ModSceneEffect
    {
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Music/DoGLegacy");
        public override SceneEffectPriority Priority => (SceneEffectPriority)13;
        public override float GetWeight(Player player) => 0.500f;
        public override bool IsSceneEffectActive(Player player)
        {
            return !BossRushEvent.BossRushActive && MiscFlagReset.PlayDogLegacyMusic && CIConfig.Instance.DoGLegacyMusic;
        }

    }
}
