using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;
using CalamityMod.NPCs.Cryogen;
using CalamityInheritance.Content.Projectiles.Summon;
using CalamityMod;
using CalamityInheritance.Content.Items.Weapons.Summon;
using CalamityInheritance.Buffs.Legendary;

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
        public bool CryoDrainDoT = false;
        internal object newAI;

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
            if (CryoDrainDoT)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                //dot伤害:800
                npc.lifeRegen -= 800;
            }
        }
    
        #region Reset Effects
        public override void ResetEffects(NPC npc)
        {
            SilvaStunDebuff = false;
            CryoDrainDoT = false;
            BossResetEffects(npc);
        }
        #endregion
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            Player plr = Main.player[projectile.owner];
            var mplr = plr.CIMod();
            //拥有失温虹吸的敌怪将会被汲取大部分的数值
            if (projectile.type == ModContent.ProjectileType<CryogenPtr>() && mplr.ColdDivityTier3 && npc.HasBuff(ModContent.BuffType<CryoDrain>()))
            {
                //伤害、dr和防御 
                npc.damage /= 2;
                npc.Calamity().DR /= 2;
                npc.defense /= 2;
                //敌对单位的……血量
                npc.life *= (int)0.99f;
                int buffDef = npc.defense;
                float buffDR = npc.Calamity().DR;
                if (plr.HasBuff(ModContent.BuffType<CryoDrain>()) && plr.ActiveItem().type == ModContent.ItemType<CyrogenLegendary>())
                {
                    plr.statDefense += buffDef;
                    plr.endurance += buffDR;
                    //固定只给1.1f乘算
                    plr.GetDamage<SummonDamageClass>() *= 1.1f; 
                }
                //这些效果只有手持冰寒神性且玩家本身具备失温虹吸效果时才会提供
            }
        }
        #region Pre AI
        public override bool PreAI(NPC npc)
        {
            if (npc.type == NPCID.Bee || npc.type == NPCID.BeeSmall || npc.type == NPCID.Hornet || npc.type == NPCID.HornetFatty || npc.type == NPCID.HornetHoney ||
                npc.type == NPCID.HornetLeafy || npc.type == NPCID.HornetSpikey || npc.type == NPCID.HornetStingy || npc.type == NPCID.BigHornetStingy || npc.type == NPCID.LittleHornetStingy ||
                npc.type == NPCID.BigHornetSpikey || npc.type == NPCID.LittleHornetSpikey || npc.type == NPCID.BigHornetLeafy || npc.type == NPCID.LittleHornetLeafy ||
                npc.type == NPCID.BigHornetHoney || npc.type == NPCID.LittleHornetHoney || npc.type == NPCID.BigHornetFatty || npc.type == NPCID.LittleHornetFatty)
            {
                if (Main.player[npc.target].CIMod().LoreQueenBee)
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
            if (player.CIMod().LoreHive)
            {
                spawnRate = (int)(spawnRate * 1.3);
                maxSpawns = (int)(maxSpawns * 0.6f);
            }
            if (player.CIMod().LorePerforator)
            {
                spawnRate = (int)(spawnRate * 0.7);
                maxSpawns = (int)(maxSpawns * 1.8f);
            }
        }
        #endregion
    }
}
