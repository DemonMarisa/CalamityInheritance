using System.Data;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Sounds.Custom;
using CalamityMod;
using log4net.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityInheritance.CIPlayer
{
    /*
    *这一文件专门用于管理职业熟练度机制。也就是原灾的旧机制
    *与原灾的熟练度机制有一定的区别是，这里的机制会比较容易升级
    *但会有一的区分：会有一个类似于表现分的玩意（？），如果玩家做出不同的行为则可以获得额外的经验点数
    */
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        const int MeleeType = 1;
        const int RangedType = 2;
        const int MagicType = 3;
        const int SummonType = 4;
        const int RogueType = 5;
        const int MaxLevel = 15;
        //每次获得的熟练度经验值
        public int ProficiencyPoints = 10;
        //每个熟练度的获取至少需要的伤害量
        public int ProficiencyDamageNeeded = 5;
        //每个大阶段的升级所需的经验。固定的值
        //我不想打表啊哇哇哇哇哇哇哇[Evil哭哭.jpg]
        //1~5级的每一级间隔, 第五级5000经验
        public static readonly int GapBeforeLevel6 = 1000;
        //6~10级的每二级间隔, 第11级50000经验
        public static readonly int GapBeforeLevel11 = 10000;
        //11~15级的每二级间隔, 满级十万经验
        public static readonly int GapBeforeLevelMax = 100000;
        //获得熟练度经验值的CD
        public int ProficiencyGainCD = 0;
        //默认CD
        public int DefualtCD = 60;
        //每个职业等级的数组，一个数组位代表一个职业，注意这个要用SaveData去存储
        //顺序为近战、远程、魔法、召唤与盗贼
        public int MeleeLevel = 0;
        public int MeleePool = 0;
        public int RangedLevel = 0;
        public int RangedPool = 0;
        public int MagicLevel = 0;
        public int MagicPool = 0;
        public int SummonLevel = 0;
        public int SummonPool = 0;
        public int RogueLevel = 0;
        public int RoguePool = 0;
        //大阶段的标准。肉前最高五级->1500经验, 月前最高10级->5000经验, 月后最高15级->15000经验
        public int TopProficiencyPreHardmode = 1500;
        public int TopProficiencyHardmode = 5000;
        public int TopProficiencyPostML = 15000;
    

        public void ProficiencySaveData(ref TagCompound tag)
        {
            tag.Add("MeleeLevel", MeleeLevel);
            tag.Add("RangedLevel", RangedLevel);
            tag.Add("MagicLevel", MagicLevel);
            tag.Add("SummonLevel", SummonLevel);
            tag.Add("RogueLevel", RogueLevel);
            tag.Add("MeleePool", MeleePool);
            tag.Add("RangedPool", RangedPool);
            tag.Add("MagicPool", MagicPool);
            tag.Add("SummonPool", SummonPool);
            tag.Add("RoguePool", RoguePool);
        }
        public void ProficiencyLoadData(ref TagCompound tag)
        {
            tag.TryGet("MeleeLevel", out MeleeLevel);
            tag.TryGet("RangedLevel", out RangedLevel);
            tag.TryGet("MagicLevel", out MagicLevel);
            tag.TryGet("SummonLevel", out SummonLevel);
            tag.TryGet("RogueLevel", out RogueLevel);
            tag.TryGet("MeleePool", out MeleePool);
            tag.TryGet("RangedPool", out RangedPool);
            tag.TryGet("MagicPool", out MagicPool);
            tag.TryGet("SummonPool", out SummonPool);
            tag.TryGet("RoguePool", out RoguePool);
        }
        //根据熟练度给予相应的数值奖励, 这个注意要塞给PostUpdate
        public void GiveBoost()
        {
            //熟练度备注：所有职业升到第15级的时候不出意外都是15%伤害与15%暴击。
            //我只能打表了哇啊娃娃啊啊啊啊啊啊啊
            Player.GetDamage<MeleeDamageClass>() += MeleeLevel * 0.01f;
            Player.GetCritChance<MeleeDamageClass>() += MeleeLevel;
            Player.GetDamage<RangedDamageClass>() += RangedLevel * 0.01f;
            Player.GetCritChance<RangedDamageClass>() += RangedLevel;
            Player.GetDamage<MagicDamageClass>() += MagicLevel * 0.01f;
            Player.GetCritChance<MagicDamageClass>() += MagicLevel;
            //不，召唤没有暴击加成的, 因此这里是直接……转化成额外的15%伤害
            Player.GetDamage<SummonDamageClass>() += SummonLevel * 0.02f;
            Player.GetDamage<RogueDamageClass>() += RogueLevel * 0.01f;
            Player.GetCritChance<MeleeDamageClass>() += RogueLevel;

            //部分特殊待遇：
            //近战职业：获得攻速加成
            Player.GetAttackSpeed<MeleeDamageClass>() += MeleeLevel * 0.01f;
            //远程职业: 获得额外的暴击率加成
            Player.GetCritChance<RangedDamageClass>() += RangedLevel / 15 * 10;
            //魔法职业：最大魔力加成
            Player.statManaMax2 += MagicLevel / 15 * 30;
            //召唤职业: 鞭子, 甚至是栏位
            Player.whipRangeMultiplier += SummonLevel * 0.01f;
            Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += SummonLevel * 0.01f;
            Player.maxMinions += SummonLevel / 10;
            //盗贼: 潜伏值, 最大+30
            
        }
        //准备给玩家升级, 注意，这里要置于postupdate内
        public void UpdateLevel()
        {
            //遍历你个头，这好像只能打表，我草
            if (MeleeLevel < MaxLevel && JudgeIfCanLevelUp(MeleeLevel, MeleeType))
            {
                //可以升级就直接+= 1
                MeleeLevel += 1;
                Celebration();
            }
            if (RangedLevel < MaxLevel && JudgeIfCanLevelUp(RangedLevel, RangedType))
            {
                RangedLevel += 1;
                Celebration();
            }
            if (JudgeIfCanLevelUp(MagicLevel, MagicType))
            {
                MagicLevel += 1;
                Celebration();
            }
            if (JudgeIfCanLevelUp(SummonLevel, SummonType))
            {
                SummonLevel += 1;
                Celebration();
            }
            if (JudgeIfCanLevelUp(RogueLevel, RogueType))
            {
                RogueLevel += 1;
                Celebration();
            }
            
        }
        public void Celebration()
        {
            int fireRocket = Utils.SelectRandom(Main.rand, new int[]{ProjectileID.RocketFireworkBlue, ProjectileID.RocketFireworkGreen, ProjectileID.RocketFireworkRed, ProjectileID.RocketFireworkYellow});
            SoundEngine.PlaySound(CISoundMenu.FireworkLauncher, Player.Center);
            int damage = Player.ApplyArmorAccDamageBonusesTo(5000);
            //天顶世界下，熟练度成功后发射的火箭将会获得5000基础伤害，且将会根据玩家当前的增伤来获得伤害，并且以1/2的概率成为敌对
            //到底是哪位能成为第一个被火箭炸死的幸运儿呢 :)
            if (Main.zenithWorld)
            {
                damage = Player.ApplyArmorAccDamageBonusesTo(50000);
            }
            //在玩家位置往头上发射就完事了
            Vector2 firePos = new (0f, 16f);
            int p =Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, firePos, fireRocket, damage, 0f, Player.whoAmI);
            if (Main.zenithWorld && Main.rand.NextBool())
                Main.projectile[p].friendly = false;
        }
        /// <summary>
        /// 判断玩家当前职业是否可以升级
        /// </summary>
        /// <param name="level">职业ID. 1:近战, 2:远程, 3:魔法, 4:召唤, 5:盗贼</param>
        /// <returns>返回为真</returns>
        public bool JudgeIfCanLevelUp(int level, int type)
        {
            bool canLevelUp = false;
            //just in case...
            if (level > 15)
                level = 15;
            //这是一个存放了15个数的数组
            int[] LevelArray =
            [
                GapBeforeLevel6, GapBeforeLevel6 * 2, GapBeforeLevel6 * 3, GapBeforeLevel6 * 4, GapBeforeLevel6 * 5,
                GapBeforeLevel11, GapBeforeLevel11 * 2, GapBeforeLevel11 * 3, GapBeforeLevel11 * 4, GapBeforeLevel11 * 5,
                GapBeforeLevelMax, GapBeforeLevelMax * 2, GapBeforeLevelMax * 3, GapBeforeLevelMax * 4, GapBeforeLevelMax * 5
            ];
            //遍历式查看到底谁能他妈的升级
            switch (type)
            {
                case MeleeType:
                    if (MeleePool > LevelArray[level - 1])
                        canLevelUp = true;
                    return canLevelUp;
                case RangedType:
                    if (RangedPool > LevelArray[level - 1])
                        canLevelUp = true;
                    return canLevelUp;
                case MagicType:
                    if (MagicPool > LevelArray[level - 1])
                        canLevelUp = true;
                    return canLevelUp;
                case SummonType:
                    if (SummonPool > LevelArray[level - 1])
                        canLevelUp = true;
                    return canLevelUp;
                case RogueType:
                    if (RoguePool > LevelArray[level-1])
                        canLevelUp = true;
                    return canLevelUp;
            }
            return canLevelUp;
        }
        //专门用于处理近战物品的
        public void MeleePoints(NPC.HitInfo hit, Item item)
        {
            bool isTrueMelee = item.CountsAsClass<TrueMeleeDamageClass>() || item.CountsAsClass<TrueMeleeNoSpeedDamageClass>();
            bool isMelee = item.CountsAsClass<MeleeDamageClass>() || item.CountsAsClass<MeleeNoSpeedDamageClass>() || isTrueMelee;
            int points = ProficiencyPoints;
            if (hit.Crit)
                points += 1;
            if (ProficiencyGainCD == 0 && isMelee)
            {
                //如果是真近战的话，每次获得经验值的CD会更短，经验的获得量也更多，这是故意这么做的
                MeleePool += isTrueMelee ? points * 5 : points;
                ProficiencyGainCD = isTrueMelee ? (int)(DefualtCD * 0.75f) : DefualtCD;
            }
        }
        /// <summary>
        /// 射弹攻击时获得经验点数
        /// </summary>
        /// <param name="proj">射弹</param>
        /// <param name="hit">npc受击信息</param>
        /// <returns>返回获得的点数</returns>
        public void EarnPoints(NPC.HitInfo hit, Projectile proj)
        {
            //每个职业的判定
            bool isTrueMelee = proj.CountsAsClass<TrueMeleeDamageClass>() || proj.CountsAsClass<TrueMeleeNoSpeedDamageClass>();
            bool isMelee = proj.CountsAsClass<MeleeDamageClass>() || proj.CountsAsClass<MeleeNoSpeedDamageClass>() || isTrueMelee;
            
            //一般情况下应该是不会有……物品本身能造成远程职业伤害的情况的，但……以防万一？
            bool isRanged = proj.CountsAsClass<RangedDamageClass>();
            //理由同上，下也同
            bool isMagic = proj.CountsAsClass<MagicDamageClass>();
            bool isWhip = proj.CountsAsClass<SummonMeleeSpeedDamageClass>();
            bool isSummon = proj.CountsAsClass<SummonDamageClass>()|| isWhip;
            //盗贼这里需要划分射弹是否为潜伏攻击
            bool isRogueStealth = proj.Calamity().stealthStrike;
            bool isRogue = proj.CountsAsClass<RogueDamageClass>() && !proj.Calamity().stealthStrike || isRogueStealth;
            int points = ProficiencyPoints;
            //攻击成功暴击都会让Points +1
            if (hit.Crit)
                points += 1;
            
            //现在我们需要开始获取经验值。
            if (ProficiencyGainCD == 0)
            {
                if (isMelee)
                {
                    //如果是真近战的话，每次获得经验值的CD会更短，经验的获得量也更多，这是故意这么做的
                    MeleePool += isTrueMelee ? points * 5 : points;
                    ProficiencyGainCD = isTrueMelee ? (int)(DefualtCD * 0.75f) : DefualtCD;
                }
                if (isRanged)
                {
                    RangedPool += points;
                    ProficiencyGainCD = DefualtCD;
                }
                if (isMagic)
                {
                    MagicPool += points;
                    ProficiencyGainCD = DefualtCD;
                }
                if (isSummon)
                {
                    //鞭子也是真近战!
                    SummonPool += isWhip ? points * 5 : points;
                    ProficiencyGainCD = isWhip ? (int)(DefualtCD * 0.75f) : DefualtCD;
                }
                if (isRogue)   
                {
                    //潜伏贼一次会获得比预期更多的经验值，但是会进入更长的CD
                    RoguePool += isRogueStealth ? points * 7 : points / 2;
                    //普攻会获得预期更少的CD同时更少的经验值
                    ProficiencyGainCD = isRogueStealth ? (int)(DefualtCD * 2.25f) : (int)(DefualtCD * 0.75f);
                }
            }
        }
    }
}