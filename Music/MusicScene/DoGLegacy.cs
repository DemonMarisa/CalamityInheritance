using Terraria;
using Terraria.ModLoader;
using static CalamityInheritance.Music.CrossMod.CalamityCompatibility;

namespace CalamityInheritance.Music.MusicScene
{
    public class DoGLegacy: ModSceneEffect
    {
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Music/DoGLegacy");
        public override SceneEffectPriority Priority => (SceneEffectPriority)13;
        public override float GetWeight(Player player) => 0.500f;
        public override bool IsSceneEffectActive(Player player)
        {

            // Check if any of the Exos is nearby
            bool isAnyBossNearby = IsNpcNearby(DoGHead, player , 8500f);

            return !BossRushActive &&  isAnyBossNearby && CalamityInheritanceConfig.Instance.DoGLegacyMusic;
        }

        /// <summary>
        /// Determines if an NPC of a given type is near the player within a specified range.
        /// </summary>
        /// <param name="npcType">The type ID of the NPC to check.</param>
        /// <param name="player">The player to measure distance from.</param>
        /// <param name="range">The maximum range within which the NPC is considered "nearby".</param>
        /// <returns>True if the NPC is within the specified range; otherwise, false.</returns>
        private static bool IsNpcNearby(int npcType, Player player, float range)
        {
            if (npcType <= 0)
                return false;
            int npcIndex = NPC.FindFirstNPC(npcType);
            if (npcIndex != -1)
            {
                NPC npc = Main.npc[npcIndex];
                return npc.active && npc.Distance(player.Center) <= range;
            }
            return false;
        }
    }
}
