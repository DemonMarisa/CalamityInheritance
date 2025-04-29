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
using Terraria.Localization;

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
        public float expRate = 1.22f;
        // 最大等级
        public int maxLevel = 15;
        // 每次攻击获得的经验值
        public short exp = 1;
        // 经验获取CD
        public short expCD = 0;
        // 经验获取CD
        public short levelUpCD = 0;
        #endregion
        // 计算每一级的经验需求
        public int CalculateRequiredExp(int currentLevel)
        {
            int requiredExp = baseExp;
            for(int i = 0; i < currentLevel; i++)
                requiredExp += baseExp + baseExp * i;
            if (currentLevel == 15)
                requiredExp += 500;
            return requiredExp;
        }

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
                // 先减去对应经验值再加等级
                Celebration(meleeLevel == 14 ? ProjectileID.RocketFireworkRed : ProjectileID.RocketFireworksBoxRed);
                meleePool -= CalculateRequiredExp(meleeLevel);
                meleeLevel++;
                levelUpCD = 60;
                SendMessageOnPlayer(meleeLevel, 0);
                return;
            }
            if (rangePool >= CalculateRequiredExp(rangeLevel) && rangeLevel < maxLevel)
            {
                Celebration(rangeLevel == 14 ? ProjectileID.RocketFireworkGreen : ProjectileID.RocketFireworksBoxGreen);
                rangePool -= CalculateRequiredExp(rangeLevel);
                rangeLevel++;
                levelUpCD = 60;
                SendMessageOnPlayer(rangeLevel, 1);
                return;
            }
            if (magicPool >= CalculateRequiredExp(magicLevel) && magicLevel < maxLevel)
            {
                Celebration(magicLevel == 14 ? ProjectileID.RocketFireworkBlue : ProjectileID.RocketFireworksBoxBlue);
                magicPool -= CalculateRequiredExp(magicLevel);
                magicLevel++;
                levelUpCD = 60;
                SendMessageOnPlayer(magicLevel, 2);
                return;
            }
            if (summonPool >= CalculateRequiredExp(summonLevel) && summonLevel < maxLevel)
            {
                Celebration(summonLevel == 14 ? ModContent.ProjectileType<SummonLevelFirework_Final>() : ModContent.ProjectileType<SummonLevelFirework>());
                summonPool -= CalculateRequiredExp(summonLevel);
                summonLevel++;
                levelUpCD = 60;
                SendMessageOnPlayer(summonLevel, 3);
                return;
            }
            if (roguePool >= CalculateRequiredExp(rogueLevel) && rogueLevel < maxLevel)
            {
                Celebration(rogueLevel == 14 ? ModContent.ProjectileType<RogueLevelFirework_Final>() : ModContent.ProjectileType<RogueLevelFirework>());
                roguePool -= CalculateRequiredExp(rogueLevel);
                rogueLevel++;
                levelUpCD = 60;
                SendMessageOnPlayer(rogueLevel, 4);
                return;
            }
            if (meleePool > 12500)
                meleePool = 12500;
            if (rangePool > 12500)
                rangePool = 12500;
            if (magicPool > 12500)
                magicPool = 12500;
            if (summonPool > 12500)
                summonPool = 12500;
            if (roguePool > 12500)
                roguePool = 12500;
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
        public string Local = "Mods.CalamityInheritance.Status.";
        public void SendMessageOnPlayer(int currentLevel, int SwitchClass)
        {
            string Melee = Language.GetTextValue($"{Local}MeleeLevel");
            string Ranged = Language.GetTextValue($"{Local}RangedLevel");
            string Magic = Language.GetTextValue($"{Local}MagicLevel");
            string Summon = Language.GetTextValue($"{Local}SummonLevel");
            string Rogue = Language.GetTextValue($"{Local}RogueLevel");

            string MeleeMax = Language.GetTextValue($"{Local}MeleeLevelMax");
            string RangedMax = Language.GetTextValue($"{Local}RangedLevelMax");
            string MagicMax = Language.GetTextValue($"{Local}MagicLevelMax");
            string SummonMax = Language.GetTextValue($"{Local}SummonLevelMax");
            string RogueMax = Language.GetTextValue($"{Local}RogueLevelMax");

            if (SwitchClass == 0)
                CIFunction.SendTextOnPlayer(currentLevel == 14 ? MeleeMax : Melee, Color.Red);
            if (SwitchClass == 1)
                CIFunction.SendTextOnPlayer(currentLevel == 14 ? RangedMax : Ranged, Color.Green);
            if (SwitchClass == 2)
                CIFunction.SendTextOnPlayer(currentLevel == 14 ? MagicMax : Magic, Color.DeepSkyBlue);
            if (SwitchClass == 3)
                CIFunction.SendTextOnPlayer(currentLevel == 14 ? SummonMax : Summon, Color.MediumPurple);
            if (SwitchClass == 4)
                CIFunction.SendTextOnPlayer(currentLevel == 14 ? RogueMax : Rogue, Color.Purple);
        }
        #endregion
        public void GiveBoost()
        {
            var modPlayer = Player.Calamity();
            #region 战士增幅
            Player.GetDamage<MeleeDamageClass>() += meleeLevel * 0.01f;
            Player.GetCritChance<MeleeDamageClass>() += meleeLevel;
            #endregion
            #region 射手
            Player.GetDamage<RangedDamageClass>() += rangeLevel * 0.01f;
            Player.GetCritChance<RangedDamageClass>() += rangeLevel;
            #endregion
            #region 法师
            Player.GetDamage<MagicDamageClass>() += magicLevel * 0.01f;
            Player.GetCritChance<MagicDamageClass>() += magicLevel;
            #endregion
            #region 召唤
            Player.GetDamage<SummonDamageClass>() += summonLevel * 0.02f;
            if (summonLevel > 14)
                Player.maxMinions += 1;
            #endregion
            #region 盗贼
            Player.GetDamage<RogueDamageClass>() += rogueLevel * 0.01f;
            Player.GetCritChance<RogueDamageClass>() += rogueLevel;
            #endregion
        }
        public void GiveExpMelee(NPC target, bool isTrueMelee, bool isMelee, bool isCrit)
        {
            if (expCD > 0)
                return;

            const short CD = 60;
            // 暴击经验点+1
            int points = isCrit ? (exp + 1) : exp;
            // 都是Item击中了，统一按真近战判吧
            meleePool += points * 5;
            expCD = CD;
        }

        public void GiveExp(NPC target, NPC.HitInfo hit, Projectile proj)
        {
            if (expCD > 0)
                return;

            const short CD = 60;

            #region 每个职业的判定
            // 战士
            bool isTrueMelee = hit.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>() || hit.DamageType == ModContent.GetInstance<TrueMeleeNoSpeedDamageClass>();
            bool isMelee = hit.DamageType == DamageClass.Melee || hit.DamageType == DamageClass.MeleeNoSpeed || isTrueMelee;
            // 射手
            bool isRanged = proj.CountsAsClass<RangedDamageClass>();
            // 法师
            bool isMagic = proj.CountsAsClass<MagicDamageClass>();
            // 召唤
            bool isWhip = proj.CountsAsClass<SummonMeleeSpeedDamageClass>();
            bool isSummon = proj.CountsAsClass<SummonDamageClass>() || isWhip;
            // 盗贼
            bool isRogueStealth = proj.Calamity().stealthStrike;
            bool isRogue = proj.CountsAsClass<RogueDamageClass>() && (!proj.Calamity().stealthStrike || isRogueStealth);
            #endregion
            int points = exp;
            // 攻击成功暴击都会让Points基础值 +1
            if (hit.Crit)
                points += 1;
            // 真近战五倍经验
            if (isMelee && meleeLevel < maxLevel)
                meleePool += points * (isTrueMelee ? 5 : 1);
            // 射手
            if (isRanged && rangeLevel < maxLevel)
                rangePool += points;
            // 法师
            if (isMagic && magicLevel < maxLevel)
                magicPool += points;
            // 召唤
            if (isSummon && summonLevel < maxLevel)//鞭子也能吃真近战增幅
                summonPool += isWhip ? points * 5 : points;
            // 盗贼
            if (isRogue && rogueLevel < maxLevel)// 潜伏攻击经验更多
                roguePool += isRogueStealth ? points * 10 : points;

            expCD = CD;
        }
    }
}
