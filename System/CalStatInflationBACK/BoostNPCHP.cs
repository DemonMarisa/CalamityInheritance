using CalamityInheritance.System.Configs;
using CalamityMod.NPCs.Yharon;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Net;

namespace CalamityInheritance.System.CalStatInflationBACK
{
    public class BoostNPCHP : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        #region 弱引用
        public static Mod FuckGoozmaMod { get; set; }
        public static int GoozmaBoss { get; private set; }
        public static int AvatarRift { get; private set; }
        public static int AvatarOfEmptiness { get; private set; }
        public static int NamelessDeityBoss { get; private set; }
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
            if (CalamityInheritance.WrathoftheGods is not null)
            {
                if (CalamityInheritance.WrathoftheGods.TryFind("AvatarRift", out ModNPC avatarRift))
                    AvatarRift = avatarRift.Type;
                if (CalamityInheritance.WrathoftheGods.TryFind("AvatarOfEmptiness", out ModNPC avatarOfEmptiness))
                    AvatarOfEmptiness = avatarOfEmptiness.Type;
                if (CalamityInheritance.WrathoftheGods.TryFind("NamelessDeityBoss", out ModNPC namelessDeity))
                    NamelessDeityBoss = namelessDeity.Type;
            }
        }
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            if (!CIServerConfig.Instance.CalStatInflationBACK)
                return;
            binaryWriter.Write(npc.lifeMax);
            binaryWriter.Write(npc.life);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
        {
            if (!CIServerConfig.Instance.CalStatInflationBACK)
                return;
            npc.lifeMax = binaryReader.ReadInt32();
            npc.life = binaryReader.ReadInt32();
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (CIServerConfig.Instance.CalStatInflationBACK)
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
                if (npc.type == NPCType<Yharon>())
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
            }
        }
        public void SetWarthofGods(NPC npc)
        {
            Mod mod = CalamityInheritance.WrathoftheGods;
            if (mod is null)
                return;
            int noxusP1HP = 85000000;
            int noxuseP2HP = (int)(25 * Math.Pow(10, 7));
            int namelessHP = (int)(5 * Math.Pow(10, 8));
            if (npc.type == AvatarRift)
            {
                npc.lifeMax = npc.life = noxusP1HP;
            }
            if (npc.type == AvatarOfEmptiness)
            {
                npc.lifeMax = npc.life = noxuseP2HP;
            }
            if (npc.type == NamelessDeityBoss)
            {
                npc.lifeMax = npc.life = namelessHP;
            }
        }
    }
}
