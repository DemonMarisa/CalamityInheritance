using CalamityInheritance.Sounds.Custom;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Typeless.LevelFirework;
using CalamityInheritance.Utilities;
using CalamityMod.Projectiles.Typeless;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        #region 基础信息
        // 储存等级
        public int meleeLevel = 0;
        public int rangeLevel = 0;
        public int summonLevel = 0;
        public int magicLevel = 0;
        public int rogueLevel = 0;
        // 储存进度
        public int meleePool = 0;
        public int rangePool = 0;
        public int summonPool = 0;
        public int magicPool = 0;
        public int roguePool = 0;
        //bossPool, 不会退出游戏的时候进行存储
        public int MeleePoolBoss = 0;
        public int RangedPoolBoss = 0;
        public int MagicPoolBoss = 0;
        public int SummonPoolBoss = 0;
        public int RoguePoolBoss = 0;
        // 基础经验需求
        public int baseExp = 100;
        // 基础经验倍率
        public float expRate = 1.4f;
        // 最大等级
        const short maxLevel = 15;
        // 每次攻击获得的经验值
        public short exp = 1;
        // 经验获取CD
        public short expCD = 0;
        // 经验获取CD
        public short levelUpCD = 0;
        #endregion
        // 计算每一级的经验需求
        public int CalculateRequiredExp(int currentLevel) => (int)(baseExp + baseExp * MathF.Pow(expRate, currentLevel));
        #region 保存数据
        public void LevelSaveData(TagCompound tag)
        {
            tag.Add("CIMeleeLevelNew", meleeLevel);
            tag.Add("CIRangedLevelNew", rangeLevel);
            tag.Add("CIsummonLevelNew", summonLevel);
            tag.Add("CImagicLevelNew", magicLevel);
            tag.Add("CIrogueLevelNew", rogueLevel);

            tag.Add("CIMeleePoolNew", meleePool);
            tag.Add("CIRangedPoolNew", rangePool);
            tag.Add("CISummonPoolNew", summonPool);
            tag.Add("CIMagicPoolNew", magicPool);
            tag.Add("CIRoguePoolNew", roguePool);
        }
        public void LevelLoadData(TagCompound tag)
        {
            tag.TryGet("CIMeleeLevelNew", out meleeLevel);
            tag.TryGet("CIRangedLevelNew", out rangeLevel);
            tag.TryGet("CIsummonLevelNew", out summonLevel);
            tag.TryGet("CImagicLevelNew", out magicLevel);
            tag.TryGet("CIrogueLevelNew", out rogueLevel);

            tag.TryGet("CIMeleePoolNew", out meleePool);
            tag.TryGet("CIRangedPoolNew", out rangePool);
            tag.TryGet("CIMagicPoolNew", out magicPool);
            tag.TryGet("CISummonPoolNew", out summonPool);
            tag.TryGet("CIRoguePoolNew", out roguePool);
        }
        #endregion
        #region 等级升级与特效
        public void LevelUp()
        {
            if (levelUpCD > 0)
            {
                levelUpCD--;
                return;
            }
            if (expCD > 0)
                expCD--;
            //Main.NewText($"magicLevel : {magicLevel}");
            if (meleePool >= CalculateRequiredExp(meleeLevel) && meleeLevel < maxLevel)
            {
                Celebration(meleeLevel == 14 ? ProjectileID.RocketFireworkRed : ProjectileID.RocketFireworksBoxRed);
                meleeLevel++;
                levelUpCD = 60;
                meleePool -= CalculateRequiredExp(meleeLevel);
                return;
            }
            if (rangePool >= CalculateRequiredExp(rangeLevel) && rangeLevel < maxLevel)
            {
                Celebration(rangeLevel == 14 ? ProjectileID.RocketFireworkGreen : ProjectileID.RocketFireworksBoxGreen);
                rangeLevel++;
                levelUpCD = 60;
                rangePool -= CalculateRequiredExp(rangeLevel);
                return;
            }
            if (magicPool >= CalculateRequiredExp(magicLevel) && magicLevel < maxLevel)
            {
                Celebration(magicLevel == 14 ? ProjectileID.RocketFireworkBlue : ProjectileID.RocketFireworksBoxBlue);
                magicLevel++;
                levelUpCD = 60;
                magicPool -= CalculateRequiredExp(magicLevel);
                return;
            }
            if (summonPool >= CalculateRequiredExp(summonLevel) && summonLevel < maxLevel)
            {
                Celebration(summonLevel == 14 ? ModContent.ProjectileType<SummonLevelFirework_Final>() : ModContent.ProjectileType<SummonLevelFirework>());
                summonLevel++;
                levelUpCD = 60;
                summonPool -= CalculateRequiredExp(summonLevel);
                return;
            }
            if (roguePool >= CalculateRequiredExp(rogueLevel) && rogueLevel < maxLevel)
            {
                Celebration(rogueLevel == 14 ? ModContent.ProjectileType<RogueLevelFirework_Final>() : ModContent.ProjectileType<RogueLevelFirework>());
                rogueLevel++;
                levelUpCD = 60;
                roguePool -= CalculateRequiredExp(rogueLevel);
                return;
            }
        }
        public void Celebration(int fireType)
        {
            // 蓝色法师 绿色射手 红色战士 粉色召唤 深粉色盗贼
            SoundEngine.PlaySound(CISoundMenu.FireworkLauncher, Player.Center);
            int damage = 0;
            //天顶世界下，熟练度成功后发射的火箭将会获得5000基础伤害，且将会根据玩家当前的增伤来获得伤害，并且以1/2的概率成为敌对
            if (Main.zenithWorld && Main.rand.NextBool())
            {
                damage = Player.ApplyArmorAccDamageBonusesTo(5000);
            }
            Vector2 firePos = new(0f, -4f);
            int p = Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, firePos, fireType, damage, 0f, Player.whoAmI);
            Main.projectile[p].timeLeft = 30;
            if (Main.zenithWorld && Main.rand.NextBool())
                Main.projectile[p].friendly = false;
        }
        #endregion
        public void GiveBoost()
        {
            var modPlayer = Player.Calamity();
            #region 战士增幅
            // 30伤 15爆 30攻速 防击退
            Player.GetDamage<MeleeDamageClass>() += meleeLevel * 0.02f;
            Player.GetCritChance<MeleeDamageClass>() += meleeLevel;
            Player.GetAttackSpeed<MeleeDamageClass>() += meleeLevel * 0.02f;
            if (meleeLevel > 14)
                Player.noKnockback = true;
            #endregion
            #region 射手
            // 30伤 30爆 15攻速 30穿 常态狙击镜
            Player.GetDamage<RangedDamageClass>() += rangeLevel * 0.02f;
            Player.GetCritChance<RangedDamageClass>() += rangeLevel * 2;
            // 我草，谁家好人给远程攻速
            // 只有ut大于3才会给攻速
            if (Player.HeldItem.useTime > 3 && Player.HeldItem.DamageType == DamageClass.Ranged)
                Player.GetAttackSpeed<RangedDamageClass>() += rangeLevel * 0.01f;
            //移除狙击镜效果
            //不是，哥们，这个狙击镜他会影响某些右键
            //比如星火右键喷不出来
            /*
            if (rangeLevel > 14)
                Player.scope = true;
            */
            #endregion
            #region 法师
            // 45伤 15爆 150法力 15%法力消耗降低 获得魔力花的效果 每秒恢复15点魔力
            Player.GetDamage<MagicDamageClass>() += magicLevel * 0.03f;
            Player.GetCritChance<MagicDamageClass>() += magicLevel;
            Player.statManaMax2 += magicLevel * 10;
            Player.manaCost *= 1 - magicLevel * 0.01f;
            if (magicLevel > 0)
                if (Player.miscCounter % (60 / magicLevel) == 0 && Player.statMana < Player.statManaMax2)
                    Player.statMana += 1;
            if (magicLevel > 14)
                Player.manaFlower = true;
            #endregion
            #region 召唤
            // 15%外围增伤 2召唤栏 15%鞭子范围与攻速速度加成
            Player.GetDamage<SummonDamageClass>() *= 1 + summonLevel * 0.01f;
            Player.whipRangeMultiplier += summonLevel * 0.01f;
            Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += summonLevel * 0.01f;
            if (summonLevel > 14)
                Player.maxMinions += 2;
            #endregion
            #region 盗贼
            // 30%伤 15%爆 30最大潜伏值 满级后无需穿戴盗贼套装也可以进行潜伏攻击
            Player.GetDamage<RogueDamageClass>() += rogueLevel * 0.02f;
            Player.GetCritChance<RogueDamageClass>() += rogueLevel;
            // 这一段应该写在reseteffects里
            // modPlayer.rogueStealthMax += rogueLevel * 0.02f;
            if (rogueLevel > 14)
                modPlayer.wearingRogueArmor = true;
            #endregion
        }
        public void GiveExpMelee(NPC target, bool isTrueMelee, bool isCrit)
        {
            if (expCD > 0 || target.immortal)
                return;

            const short CD = 60;
            //暴击经验点+1
            int points = isCrit ? (exp + 1) : exp;
            //boss倍率加成。
            //这里的计算无法检测别的boss血量膨胀的方法，比如fargo的，但……我也不是很想管了
            //倍率计算: boss单位最大血量/ KS血量。这里的计算会使玩家打史莱姆王的时候获得双倍加成，不过这里是有意为之的。
            float bossMultipler = 1f + (target.IsRealBossWeNeed(false) ? target.lifeMax / RecoredKSHealth() : 0f);
            points += isTrueMelee ? points * 6 : points;
            //boss倍率是最后执行的
            meleePool += (int)(points * bossMultipler);
            expCD = CD;
        }
        public static int RecoredKSHealth()
        {
            //大师死亡的KS血量
            float KSHealth = 5536;
            //考虑boss倍率增幅。
            KSHealth *= 1f + CalamityConfig.Instance.BossHealthBoost;
            //返回。
            return (int)KSHealth;
        }
        public void GiveExp(NPC target, NPC.HitInfo hit, Projectile proj)
        {
            if (expCD > 0)
                return;

            const short CD = 60;
            //boss倍率加成。
            //这里的计算无法检测别的boss血量膨胀的方法，比如fargo的，但……我也不是很想管了
            //倍率计算: boss单位最大血量/ KS血量。这里的计算会使玩家打史莱姆王的时候获得双倍加成，不过这里是有意为之的。
            float bossMultipler = 1f + (target.IsRealBossWeNeed(false) ? target.lifeMax / RecoredKSHealth() : 0f);
            //每个职业的判定
            //一般情况下应该是不会有……物品本身能造成远程职业伤害的情况的，但……以防万一？
            bool isRanged = proj.CountsAsClass<RangedDamageClass>();
            //理由同上，下也同
            bool isMagic = proj.CountsAsClass<MagicDamageClass>();
            bool isWhip = proj.CountsAsClass<SummonMeleeSpeedDamageClass>();
            bool isSummon = proj.CountsAsClass<SummonDamageClass>() || isWhip;
            //盗贼这里需要划分射弹是否为潜伏攻击
            bool isRogueStealth = proj.Calamity().stealthStrike;
            bool isRogue = proj.CountsAsClass<RogueDamageClass>() && (!proj.Calamity().stealthStrike || isRogueStealth);
            int points = exp;
            int i = 0;
            //攻击成功暴击都会让Points +1
            if (hit.Crit)
                points += 1;
            points *= (int)bossMultipler;
            if (isRanged)
                rangePool += points;
            if (isMagic)
                magicPool += points;
            if (isSummon)
                //鞭子也是真近战!
                summonPool += isWhip ? points * 6 : points;
            if (isRogue)
            {
                //潜伏攻击经验更多
                roguePool += isRogueStealth ? points * 7 : points;
                //普攻会获得预期更少的CD同时更少的经验值
                i = isRogueStealth ? 30 : -30; 
            }
            expCD = (short)(CD + i);
        }
    }
}
