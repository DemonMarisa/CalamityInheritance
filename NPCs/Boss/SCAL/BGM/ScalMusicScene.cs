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
            public override bool IsSceneEffectActive(Player player)
            {
                if (!NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()))
                    return false;

                NPC scal = Main.npc[CIGlobalNPC.LegacySCal];
                float lifeRatio = scal.life / (float)scal.lifeMax;
                bool active = lifeRatio > 0.5f;
                return active;
            }
        }
        public class ScalPhase2MusicScene : BaseMusicSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

            public override int NPCType => ModContent.NPCType<SupremeCalamitasLegacy>();
            public override int? MusicModMusic => CalamityInheritance.Instance.GetMusicFromMusicMod("CalamitasPhase2");
            public override int VanillaMusic => MusicID.Boss2;
            public override int OtherworldMusic => MusicID.OtherworldlyBoss2;
            public override bool IsSceneEffectActive(Player player)
            {
                if (!NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()))
                    return false;

                NPC scal = Main.npc[CIGlobalNPC.LegacySCal];
                float lifeRatio = scal.life / (float)scal.lifeMax;
                bool active = lifeRatio > 0.3f && lifeRatio < 0.5f;
                return active;
            }
        }
        public class ScalPhase3MusicScene : BaseMusicSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

            public override int NPCType => ModContent.NPCType<SupremeCalamitasLegacy>();
            public override int? MusicModMusic => CalamityInheritance.Instance.GetMusicFromMusicMod("CalamitasPhase3");
            public override int VanillaMusic => MusicID.Boss2;
            public override int OtherworldMusic => MusicID.OtherworldlyBoss2;
            public override bool IsSceneEffectActive(Player player)
            {
                if (!NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()))
                    return false;

                NPC scal = Main.npc[CIGlobalNPC.LegacySCal];
                float lifeRatio = scal.life / (float)scal.lifeMax;
                bool active = lifeRatio < 0.3f && lifeRatio > 0.01f;
                return active;
            }
        }
        public class ScalPhase4MusicScene : BaseMusicSceneEffect
        {
            public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

            public override int NPCType => ModContent.NPCType<SupremeCalamitasLegacy>();
            public override int? MusicModMusic => CalamityInheritance.Instance.GetMusicFromMusicMod("CalamitasDefeat_LongFade");
            public override int VanillaMusic => MusicID.Boss2;
            public override int OtherworldMusic => MusicID.OtherworldlyBoss2;
            public override bool IsSceneEffectActive(Player player)
            {
                if (!NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()))
                    return false;

                NPC scal = Main.npc[CIGlobalNPC.LegacySCal];
                float lifeRatio = scal.life / (float)scal.lifeMax;
                bool active = lifeRatio < 0.01;
                return active;
            }
        }
    }
}
