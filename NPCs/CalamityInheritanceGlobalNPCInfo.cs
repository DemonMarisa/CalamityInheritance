using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityInheritance.NPCs.Boss.SCAL.Brother;
using Microsoft.Xna.Framework;
using CalamityInheritance.NPCs.Boss.SCAL.ScalWorm;
using CalamityInheritance.NPCs.Boss.CalamitasClone;
using CalamityInheritance.NPCs.Boss.CalamitasClone.Brothers;
using CalamityInheritance.NPCs.Boss.Yharon;
using CalamityMod.NPCs.DevourerofGods;
using CalamityInheritance.System;

namespace CalamityInheritance.NPCs
{
    public partial class CIGlobalNPC : GlobalNPC
    {
        // 场地
        public static Rectangle Arena = default;

        //一百个栏位
        internal const int MaxAIMode = 100;
        public float[] BossNewAI = new float[MaxAIMode];
        public int BossAITimer = 0;
        #region 普灾
        public static int LegacyCalamitasClone = -1;
        public static int LegacyCalamitasCloneP2 = -1;
        public static int LegacyCatalysmClone = -1;
        public static int LegacyCatastropheClone = -1;
        #endregion
        #region 丛林龙
        public static int LegacyYharon = -1;
        public static int LegacyYharonStage2FadeIn = -1;
        public static int LegacyYharonStage2 = -1;
        #endregion
        #region 終灾
        public static int LegacySCalWorm = -1;
        public static int LegacySCal = -1;
        public static int LegacySCalCataclysm = -1;
        public static int LegacySCalCatastrophe = -1;
        public static int LegacySCalGrief = -1;
        public static int LegacySCalLament = -1;
        public static int LegacySCalEpiphany = -1;
        public static int LegacySCalAcceptance = -1;
        #endregion
        #region Reset Effects
        public static void BossResetEffects(NPC npc)
        {
            static void ResetSavedIndex(ref int type, int type1, int type2 = -1)
            {
                if (type >= 0)
                {
                    if (!Main.npc[type].active)
                    {
                        type = -1;
                    }
                    else if (type2 == -1)
                    {
                        if (Main.npc[type].type != type1)
                            type = -1;
                    }
                    else
                    {
                        if (Main.npc[type].type != type1 && Main.npc[type].type != type2)
                            type = -1;
                    }
                }
            }
            ResetSavedIndex(ref LegacyCalamitasClone, NPCType<CalamitasCloneLegacy>());
            ResetSavedIndex(ref LegacyCalamitasCloneP2, NPCType<CalamitasCloneLegacy>());
            ResetSavedIndex(ref LegacyCatalysmClone, NPCType<CataclysmLegacy>());
            ResetSavedIndex(ref LegacyCatastropheClone, NPCType<CatastropheLegacy>());

            ResetSavedIndex(ref LegacySCalWorm, NPCType<SCalWormHead>());
            ResetSavedIndex(ref LegacySCalCataclysm, NPCType<SupremeCataclysmLegacy>());
            ResetSavedIndex(ref LegacySCalCatastrophe, NPCType<SupremeCatastropheLegacy>());
            ResetSavedIndex(ref LegacySCal, NPCType<SupremeCalamitasLegacy>());
            ResetSavedIndex(ref LegacySCalGrief, NPCType<SupremeCalamitasLegacy>());
            ResetSavedIndex(ref LegacySCalLament, NPCType<SupremeCalamitasLegacy>());
            ResetSavedIndex(ref LegacySCalEpiphany, NPCType<SupremeCalamitasLegacy>());
            ResetSavedIndex(ref LegacySCalAcceptance, NPCType<SupremeCalamitasLegacy>());

            ResetSavedIndex(ref LegacyYharon, NPCType<YharonLegacy>());
            ResetSavedIndex(ref LegacyYharonStage2FadeIn, NPCType<YharonLegacy>());
            ResetSavedIndex(ref LegacyYharonStage2, NPCType<YharonLegacy>());
        }
        #endregion

        public override void PostAI(NPC npc)
        {
            if (npc.type == NPCType<DevourerofGodsHead>())
                MiscFlagReset.PlayDogLegacyMusic = true;
        }
    }
}
