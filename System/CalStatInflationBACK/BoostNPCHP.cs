using CalamityInheritance.System.Configs;
using CalamityMod.NPCs.Yharon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityInheritance.System.CalStatInflationBACK
{
    public class BoostNPCHP : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        #region 弱引用
        public static Mod FuckGoozmaMod { get; set; }
        public static int GoozmaBoss { get; private set; }
        public bool FirstFrame = true;
        #endregion
        public override void Load()
        {
            if (ModLoader.TryGetMod("CalamityHunt", out Mod hunt))
            {
                FuckGoozmaMod = hunt;
                if (FuckGoozmaMod.TryFind("Goozma", out ModNPC FuckGoozma))
                    GoozmaBoss = FuckGoozma.Type;
            }
        }
        public override bool PreAI(NPC npc)
        {
            if (CIServerConfig.Instance.CalStatInflationBACK && FirstFrame)
            {
                SetWarthofGods(npc);
                if (CalamityInheritanceLists.PostMLBoss.Contains(npc.type))
                {
                    npc.lifeMax = (int)(npc.lifeMax * 1.2f);
                    npc.life = (int)(npc.life * 1.2f);
                    npc.defense = (int)(npc.defense * 1.2f);
                }
                if (CalamityInheritanceLists.PostProfanedBoss.Contains(npc.type))
                {
                    npc.lifeMax = (int)(npc.lifeMax * 1.5f);
                    npc.life = (int)(npc.life * 1.5f);
                    npc.defense = (int)(npc.defense * 1.5f);
                }
                if (CalamityInheritanceLists.PostPolterghastBoss.Contains(npc.type))
                {
                    npc.lifeMax = (int)(npc.lifeMax * 1.7f);
                    npc.life = (int)(npc.life * 1.7f);
                }
                if (CalamityInheritanceLists.DOG.Contains(npc.type))
                {
                    npc.lifeMax = (int)(npc.lifeMax * 2.2f);
                    npc.life = (int)(npc.life * 2.2f);
                }
                if (npc.type == ModContent.NPCType<Yharon>())
                {
                    npc.lifeMax = (int)(npc.lifeMax * 2.8f);
                    npc.life = (int)(npc.life * 2.8f);
                }
                if (CalamityInheritanceLists.ExoMech.Contains(npc.type))
                {
                    npc.lifeMax = (int)(npc.lifeMax * 5f);
                    npc.life = (int)(npc.life * 5f);
                    npc.defense = (int)(npc.defense * 1.2f);
                }
                if (CalamityInheritanceLists.Scal.Contains(npc.type))
                {
                    npc.lifeMax = (int)(npc.lifeMax * 6.6f);
                    npc.life = (int)(npc.life * 6.6f);
                    npc.defense = (int)(npc.defense * 1.2f);
                }
                if (CalamityMod.Events.BossRushEvent.BossRushActive)
                {
                    npc.lifeMax = (int)(npc.lifeMax * 10f);
                    npc.life = (int)(npc.life * 10f);
                    npc.defense = (int)(npc.defense * 10f);
                }
                //如果为空不准执行。
                if (npc.type == GoozmaBoss)
                {
                    npc.lifeMax = (int)(npc.lifeMax * 6.6f);
                    npc.life = (int)(npc.life * 6.6f);
                    npc.defense = (int)(npc.defense * 1.2f);
                }
                npc.netUpdate = true;
                FirstFrame = false;
            }
            return false;
        }

        private void SetWarthofGods(NPC npc)
        {
            //暗神的两个阶段血量与npc独立
            string noxusP1 = "AvatarRift";
            string noxusP2 = "AvatarOfEmptiness";
            string xeroc = "NamelessDeityBoss";
            //暗神一阶段8500w，看看得了，打一半会自己进二阶段的
            int noxusP1HP = 85000000;
            //暗神二阶段2.5亿
            int noxuseP2HP = (int)(25 * Math.Pow(10, 7));
            //光神暂时采用3亿的数值
            int namelessHP = (int)(5 * Math.Pow(10, 8));
            Mod mod = CalamityInheritance.WrathoftheGods;
            if (mod is null)
                return;
            ModNPC avatarRift = mod.Find<ModNPC>(noxusP1);
            ModNPC avatarOfEmptiness = mod.Find<ModNPC>(noxusP2);
            ModNPC namelessDeity = mod.Find<ModNPC>(xeroc);
            if (npc.type == avatarRift.Type)
            {
                npc.lifeMax = npc.life = noxusP1HP;
            }
            if (npc.type == avatarOfEmptiness.Type)
            {
                npc.lifeMax = npc.life = noxuseP2HP;
            }
            if (npc.type == namelessDeity.Type)
            {
                npc.lifeMax = npc.life = namelessHP;
            }
        }
    }
}
