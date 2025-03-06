using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.NPCs
{
    public partial class CIGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public bool SilvaStunDebuff = false;
        //梯凳之怒
        public bool rageOfChair = false;
        public static int rageOfChairDoTDamage = 30000;
        internal object newAI;
        internal const int MaxAIMode = 4;
        public float[] BossNewAI = new float[MaxAIMode];
        public int BossAITimer = 0; 
        //获取whoami
        public static int CalamitasCloneWhoAmI = -1;
        public static int CalamitasCloneWhoAmIP2 = -1;
        public static int CatalysmCloneWhoAmI = -1;
        public static int CatastropheCloneWhoAmI = -1;
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (SilvaStunDebuff)
            {
                npc.velocity.Y = 0f;
                npc.velocity.X = 0f;
            }
            if (rageOfChair)
            {
                if(npc.lifeRegen > 0)
                npc.lifeRegen = 0;
                //敌方有涂油的话梯凳驾到的dot伤害+20000
                if(npc.HasBuff(BuffID.Oiled))
                npc.lifeRegen -= rageOfChairDoTDamage+20000;
                npc.lifeRegen -= rageOfChairDoTDamage;
            }
        }
        #region Reset Effects
        public override void ResetEffects(NPC npc)
        {
            SilvaStunDebuff = false;
        }
        #endregion

        #region Pre AI
        public override bool PreAI(NPC npc)
        {
            if (npc.type == NPCID.Bee || npc.type == NPCID.BeeSmall || npc.type == NPCID.Hornet || npc.type == NPCID.HornetFatty || npc.type == NPCID.HornetHoney ||
                npc.type == NPCID.HornetLeafy || npc.type == NPCID.HornetSpikey || npc.type == NPCID.HornetStingy || npc.type == NPCID.BigHornetStingy || npc.type == NPCID.LittleHornetStingy ||
                npc.type == NPCID.BigHornetSpikey || npc.type == NPCID.LittleHornetSpikey || npc.type == NPCID.BigHornetLeafy || npc.type == NPCID.LittleHornetLeafy ||
                npc.type == NPCID.BigHornetHoney || npc.type == NPCID.LittleHornetHoney || npc.type == NPCID.BigHornetFatty || npc.type == NPCID.LittleHornetFatty)
            {
                if (Main.player[npc.target].CalamityInheritance().LoreQueenBee)
                {
                    CIGlobalAI.LoreQueenBeeEffect(npc);
                    return false;
                }
            }
            return true;
        }
        #endregion
        #region Edit Spawn Rate
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.CalamityInheritance().LoreHive)
            {
                spawnRate = (int)(spawnRate * 1.3);
                maxSpawns = (int)(maxSpawns * 0.6f);
            }
            if (player.CalamityInheritance().LorePerforator)
            {
                spawnRate = (int)(spawnRate * 0.7);
                maxSpawns = (int)(maxSpawns * 1.8f);
            }
        }
        #endregion
    }
}
