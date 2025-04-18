﻿using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityInheritance.NPCs.Boss.SCAL.Brother;
using Microsoft.Xna.Framework;
using CalamityInheritance.NPCs.Boss.SCAL.ScalWorm;

namespace CalamityInheritance.NPCs
{
    public partial class CIGlobalNPC : GlobalNPC
    {
        // 场地
        public Rectangle Arena = default;

        //一百个栏位
        internal const int MaxAIMode = 100;
        public float[] BossNewAI = new float[MaxAIMode];
        public int BossAITimer = 0;
        //获取whoami
        public static int ThisCalamitasReborn = -1;
        public static int ThisCalamitasRebornP2 = -1;
        public static int CatalysmCloneWhoAmI = -1;
        public static int CatastropheCloneWhoAmI = -1;

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
            void ResetSavedIndex(ref int type, int type1, int type2 = -1)
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

            ResetSavedIndex(ref LegacySCalWorm, ModContent.NPCType<SCalWormHead>());
            ResetSavedIndex(ref LegacySCalCataclysm, ModContent.NPCType<SupremeCataclysmLegacy>());
            ResetSavedIndex(ref LegacySCalCatastrophe, ModContent.NPCType<SupremeCatastropheLegacy>());
            ResetSavedIndex(ref LegacySCal, ModContent.NPCType<SupremeCalamitasLegacy>());
            ResetSavedIndex(ref LegacySCalGrief, ModContent.NPCType<SupremeCalamitasLegacy>());
            ResetSavedIndex(ref LegacySCalLament, ModContent.NPCType<SupremeCalamitasLegacy>());
            ResetSavedIndex(ref LegacySCalEpiphany, ModContent.NPCType<SupremeCalamitasLegacy>());
            ResetSavedIndex(ref LegacySCalAcceptance, ModContent.NPCType<SupremeCalamitasLegacy>());
            /*
            // Reset the enraged state every frame. The expectation is that bosses will continuously set it back to true if necessary.
            CurrentlyEnraged = false;
            CurrentlyIncreasingDefenseOrDR = false;
            CanHaveBossHealthBar = false;
            ShouldCloseHPBar = false;
            if (arcZapCooldown > 0) { arcZapCooldown--; }
            */
        }
        #endregion
    }
}
