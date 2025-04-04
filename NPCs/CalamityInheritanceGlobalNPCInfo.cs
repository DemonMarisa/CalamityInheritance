using CalamityMod.Items.Accessories;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.AquaticScourge;
using CalamityMod.NPCs.AstrumAureus;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.CalamityAIs.CalamityBossAIs;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.CeaselessVoid;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.NPCs.ExoMechs.Apollo;
using CalamityMod.NPCs.ExoMechs.Ares;
using CalamityMod.NPCs.ExoMechs.Artemis;
using CalamityMod.NPCs.ExoMechs.Thanatos;
using CalamityMod.NPCs.ExoMechs;
using CalamityMod.NPCs.HiveMind;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.Polterghast;
using CalamityMod.NPCs.PrimordialWyrm;
using CalamityMod.NPCs.ProfanedGuardians;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.Ravager;
using CalamityMod.NPCs.Signus;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Projectiles.Boss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityMod.NPCs;
using Microsoft.Xna.Framework;

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
        #endregion

        #region Reset Effects
        public void BossResetEffects(NPC npc)
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
            ResetSavedIndex(ref LegacySCal, ModContent.NPCType<SupremeCalamitasLegacy>());
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
