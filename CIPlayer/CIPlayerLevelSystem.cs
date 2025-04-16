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
        // 基础经验需求
        public int baseExp = 100;
        // 基础经验倍率
        public float expRate = 1.4f;
        // 最大等级
        public int maxLevel = 15;
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
            //Main.NewText($"magicLevel : {magicLevel}");
            if (meleePool >= CalculateRequiredExp(meleeLevel) && meleeLevel < maxLevel)
            {
                Celebration(meleeLevel == 14 ? ProjectileID.RocketFireworkRed : ProjectileID.RocketFireworksBoxRed);
                meleeLevel++;
                return;
            }
            if (rangePool >= CalculateRequiredExp(rangeLevel) && rangeLevel < maxLevel)
            {
                Celebration(rangeLevel == 14 ? ProjectileID.RocketFireworkGreen : ProjectileID.RocketFireworksBoxGreen);
                rangeLevel++;
                return;
            }
            if (magicPool >= CalculateRequiredExp(magicLevel) && magicLevel < maxLevel)
            {
                Celebration(magicLevel == 14 ? ProjectileID.RocketFireworkBlue : ProjectileID.RocketFireworksBoxBlue);
                magicLevel++;
                return;
            }
            if (summonPool >= CalculateRequiredExp(summonLevel) && summonLevel < maxLevel)
            {
                Celebration(summonLevel == 14 ? ModContent.ProjectileType<SummonLevelFirework_Final>() : ModContent.ProjectileType<SummonLevelFirework>());
                summonLevel++;
                return;
            }
            if (roguePool >= CalculateRequiredExp(rogueLevel) && rogueLevel < maxLevel)
            {
                Celebration(rogueLevel == 14 ? ModContent.ProjectileType<RogueLevelFirework_Final>() : ModContent.ProjectileType<RogueLevelFirework>());
                rogueLevel++;
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
            if (Main.zenithWorld && Main.rand.NextBool())
            {
                Main.projectile[p].friendly = false;
                Main.projectile[p].timeLeft = 30;
            }
        }
        #endregion
        public void GiveBoost()
        {
            var modPlayer = Player.Calamity();
            #region 战士增幅
            float playerLifemultiplier1 = Player.statLife / Player.statLifeMax2;
            // 30伤 15爆 30攻速
            // 150最大生命值 7.5生命恢复
            Player.GetDamage<MeleeDamageClass>() += meleeLevel * 0.02f;
            Player.GetCritChance<MeleeDamageClass>() += meleeLevel;
            Player.GetAttackSpeed<MeleeDamageClass>() += meleeLevel * 0.02f;
            Player.statLifeMax2 += meleeLevel * 10;
            Player.lifeRegen += meleeLevel;
            #endregion
            #region 射手
            // 30伤 30爆 15攻速 30穿 常态狙击镜 45sCD闪避
            Player.GetDamage<RangedDamageClass>() += rangeLevel * 0.02f;
            Player.GetCritChance<RangedDamageClass>() += rangeLevel * 2;
            Player.GetAttackSpeed<RangedDamageClass>() += rangeLevel * 0.01f;
            if (rangeLevel > 14)
                Player.scope = true;
            #endregion
            #region 法师
            // 45伤 15爆 150法力 15%法力消耗降低 获得魔力花的效果 每秒恢复15点魔力
            Player.GetDamage<MagicDamageClass>() += magicLevel * 0.03f;
            Player.GetCritChance<MagicDamageClass>() += magicLevel;
            Player.statManaMax2 += magicLevel * 10;
            Player.manaCost *= 1 - magicLevel * 0.01f;
            if (summonLevel > 4)
                Player.manaFlower = true;
            if(magicLevel > 0)
                if(Player.miscCounter % (60 / magicLevel) == 0 && Player.statMana < Player.statManaMax2)
                    Player.statMana += 1;
            #endregion
            #region 召唤
            // 15%外围增伤 2召唤栏 15%鞭子范围与攻速速度加成
            Player.GetDamage<SummonDamageClass>() *= 1 + summonLevel * 0.01f;
            Player.whipRangeMultiplier += summonLevel * 0.01f;
            Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += summonLevel * 0.01f;
            if(summonLevel > 14)
                Player.maxMinions += 2;
            #endregion
            #region 盗贼
            // 30%伤 15%爆 30最大潜伏值 满级后无需穿戴盗贼套装也可以进行潜伏攻击
            Player.GetDamage<RogueDamageClass>() += rogueLevel * 0.02f;
            Player.GetCritChance<RogueDamageClass>() += rogueLevel;
            modPlayer.rogueStealthMax += rogueLevel * 0.02f;
            if (rogueLevel > 15)
                modPlayer.wearingRogueArmor = true;
            #endregion
        }
    }
}
