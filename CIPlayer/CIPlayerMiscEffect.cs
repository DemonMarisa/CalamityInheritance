using System;
using CalamityInheritance.Content.Items.Accessories;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using CalamityInheritance.CICooldowns;
using CalamityMod.CalPlayer;
using CalamityInheritance.Utilities;
using CalamityMod.Buffs.StatBuffs;
using Terraria.DataStructures;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Typeless;
using Terraria.ID;
using CalamityMod.Cooldowns;
using CalamityInheritance.Content.Items.Potions;
using CalamityInheritance.Buffs.Statbuffs;
using CalamityMod.Dusts;
using CalamityMod.Items.Armor.Silva;
using Terraria.Graphics.Shaders;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Projectiles.Summon;
using CalamityInheritance.System.Configs;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.CalPlayer.Dashes;
using CalamityInheritance.Buffs.Legendary;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Buffs.StatDebuffs;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.NPCs;
using CalamityMod.Items.Weapons.Magic;
using CalamityInheritance.Buffs.Summon;
using CalamityInheritance.Content.Items.Weapons.Summon;
using CalamityInheritance.Content.Items.Weapons.Typeless;


//Scarlet:将全部灾厄的Player与CI的Player的变量名统一修改，byd modPlayer和modPlayer1飞来飞去的到底在整啥😡
//灾厄Player的变量名现在统一为calPlayer。本模组player的变量名统一为usPlayer

/*
*3/6:“玩家”类内的各种……对象现在更加严格地分类整理
*此处的一些分类标准:
*ArmorSetbonus()现在存放不会通过提供buff来间接修改玩家数值的套装效果
*Buffs()所有的buff都应该转到这里面曲
*Accessories()饰品的数值都应该跑到这里来
*/
namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public static readonly int darkSunRingDayRegen = 6;
        public static readonly int darkSunRingNightDefense = 20;

        public override void PostUpdateMiscEffects()
        {
            CalamityPlayer calPlayer = Player.Calamity();
            CalamityInheritancePlayer clPlayer = Player.CIMod();
            //海绵的护盾
            Sponge();

            //非常冗余的其他效果
            MiscEffects();

            //纳米技术堆叠UI
            Nanotechs();

            //lore效果
            LoreEffects();

            //Buff效果
            Buffs();

            //饰品数值
            Accessories();

            //站立不动时玩家可以获得的效果
            StandingStill();

            //盾冲饰品的一些数值效果(e.g:阿斯加德 )
            RamShield();

            //各种套装效果的封装
            ArmorSetbonus();

            //克希洛克套装效果的封装(因为太长了所以单独封装起来了)
            XerocSetbouns();
            
            //冷却变动
            ResetCD();

            //Qol面板相关
            Panels();

            //直接向玩家生成物品
            CISpawnItem();

            // Debuff的效果
            DebuffEffect();

            // 正面Buff
            BuffEffect();

            //升级
            LevelUp();

            // 护盾的综合效果
            ShieldEffect();

            // 杂项条件与重置
            // ReSet();

            // 神殇
            Defiled();

            // 恶意
            Malice();

            // 铁心
            IronHeartChange();
        }

        public void Buffs()
        {
            Player player = Main.player[Main.myPlayer];
            //庇护之刃T3: 你的防御力将会被转化为伤害加成
            if (player.ActiveItem().type == ModContent.ItemType<DefenseBlade>() && DefendTier3)
            {
                //获取当前
                int getDef = Player.statDefense;
                //以300防为例，这一计算会变成 300 * 0.001 = 0.3 / 3 = 0.1 * 4 =0.4, 最后转float变成0.4f, 即40%伤害
                //我是Cerber
                double getRatio = getDef * 0.001 / 3 * 4;
                Player.GetDamage<MeleeDamageClass>() += (float)getRatio;

            }
            if (Player.ActiveItem().type == ModContent.ItemType<SubsumingVortex>() && Player.altFunctionUse != 2 && BuffSubsumingVortexFireRate > 0)
            {
                Player.GetAttackSpeed<MagicDamageClass>() += 0.10f;
                Player.manaCost -= 0.10f;
            }
            if (CryoDrainPlayer)
            {
                Player.lifeRegen += CIGlobalNPC.CryoDrainDotDamage / 10;
                Player.whipRangeMultiplier += 0.2f;
            }

            if (BuffPolarisBoost)
            {
                Player.lifeRegen += 1;
                Player.lifeRegenTime += 1;
            }
        
            if (BuffStatsDraconicSurge)
            {
                if (Player.wingTimeMax > 0)
                {
                    Player.wingTimeMax = (int)(Player.wingTimeMax * 1.25);
                }
                Player.statDefense += 16;
                Player.wingAccRunSpeed += 0.1f;
                Player.accRunSpeed += 0.1f;
                if(LoreJungleDragon || PanelsLoreJungleDragon)
                {
                    Player.GetDamage<GenericDamageClass>() += 0.25f;
                }

                if (Player.HasCooldown(DraconicElixirCooldown.ID))
                {
                    Player.statDefense -= 16;
                    Player.wingAccRunSpeed -= 0.1f;
                    Player.accRunSpeed -= 0.1f;
                    Player.GetDamage<GenericDamageClass>() -= 0.15f;
                }
            }

            if (BuffStatsTitanScale)
            {
                Player.endurance += 0.05f;
                Player.statDefense += 5;
                Player.kbBuff = true;
                if (BuffStatsTitanScaleTrueMelee > 0 || Main.zenithWorld)
                {
                    Player.statDefense += 20;
                    Player.endurance += 0.05f;
                    BuffStatsTitanScaleTrueMelee--;
                }
            }
            else
            {
                BuffStatsTitanScaleTrueMelee = 0;
            }
            
            if (AnimusDamage > 1f)
            {
                if (Player.ActiveItem().type != ModContent.ItemType<Animus>())
                    AnimusDamage = 1f;
            }

            if (PerunofYharimStats)
            {
                Player.GetAttackSpeed<MeleeDamageClass>() += 0.35f; 
                Player.GetAttackSpeed<RangedDamageClass>() += 0.35f; 
                Player.GetAttackSpeed<MagicDamageClass>() += 0.35f;
                Player.GetCritChance<GenericDamageClass>() += 100; //所有职业获得100暴击概率
                Player.manaCost *= 0.20f;
                Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += 3.5f;
            }
            
            /*
            *2/25:
            *移除淬火debuff的伤害削减, 因为龙弓承伤后的resueDelay惩罚已经足够高了 
            *使拥有龙魂秘药效果的玩家免疫淬火debuff的超高速烧血效果, 但以削减生命恢复作为代价
            *下调玩家的防御数据惩罚, 防御力乘算从0.3 -> 0.7, 免伤降低从0.3→0.2
            */
            if(BuffStatsBackfire)
            {
                if(Player.statLife > Player.statLifeMax2/3)
                {
                    if(BuffStatsDraconicSurge)
                    player.lifeRegen -= 10; //龙魂秘药使烧血转化为削减5HP/s的生命恢复
                    else
                    Player.statLife -= 5;
                }

                Player.endurance -= 0.2f;  //直接减少玩家20%的免伤，也就是可以让玩家免伤变成负数(有可能)
                Player.statDefense *= 0.7f; //玩家的防御力取70%
            }
        }
        private void Accessories()
        {
            CalamityPlayer calPlayer = Player.Calamity();
            var usPlayer = Player.CIMod();
            if (YharimsInsignia)
            {
                if (Player.statLife <= (int)(Player.statLifeMax2 * 0.5))
                    Player.GetDamage<GenericDamageClass>() += 0.1f;
            }
            if (AeroStonePower)
            {
                Player.jumpSpeedBoost +=0.2f;
                Player.moveSpeed += 0.1f;
                Player.wingTime += 0.1f;
            }
            if (AncientAeroWingsPower && AeroFlightPower == 0)
                calPlayer.infiniteFlight = true;

            if (GodlySons)
            {
                Player.maxMinions += 10;
				if (Player.whoAmI == Main.myPlayer)
				{
					if (Player.FindBuffIndex(ModContent.BuffType<SonYharonBuff>()) == -1)
						Player.AddBuff(ModContent.BuffType<SonYharonBuff>(), 3600, true);
					if (Player.ownedProjectileCounts[ModContent.ProjectileType<SonYharon>()] < 2)
						Projectile.NewProjectile(Player.GetSource_FromThis(),Player.Center, Vector2.Zero, ModContent.ProjectileType<SonYharon>(), (int)Player.GetTotalDamage<SummonDamageClass>().ApplyTo(YharonSonStaff.WeaponDamage), 2f, Main.myPlayer, 0f, 0f);
				}
            }
            if (DarkSunRings)
            {
                Player.maxMinions += 2;
                Player.GetDamage<GenericDamageClass>() += 0.12f;
                Player.GetKnockback<SummonDamageClass>() += 1.2f;
                Player.GetAttackSpeed<MeleeDamageClass>() += 0.12f;
                Player.pickSpeed -= 0.12f;
                if(Main.eclipse || !Main.dayTime)
                    Player.statDefense += darkSunRingNightDefense;
            }
            
            if (BraveBadge && calPlayer.tarraMelee && !calPlayer.auricSet) //如果启用
            {
                Player.GetCritChance<MeleeDamageClass>() += 10;
                Player.GetDamage<MeleeDamageClass>() += 0.10f;
                Player.GetArmorPenetration<MeleeDamageClass>() += 15; 
            }
            
            if (deificAmuletEffect)
            {
                Player.lifeRegen += 1; //生命恢复
            }
            if (RoDPaladianShieldActive) //如果佩戴壁垒
            {
                // 符合条件就启用圣骑士盾效果
                if (Player.statLife > Player.statLifeMax2 * 0.25f)
                {
                    Player.hasPaladinShield = true;
                    if (Player.whoAmI != Main.myPlayer && Player.miscCounter % 10 == 0)
                    {
                        int myPlayer = Main.myPlayer;
                        if (Main.player[myPlayer].team == Player.team && Player.team != 0)
                        {
                            float teamPlayerXDist = Player.position.X - Main.player[myPlayer].position.X;
                            float teamPlayerYDist = Player.position.Y - Main.player[myPlayer].position.Y;
                            if ((float)Math.Sqrt(teamPlayerXDist * teamPlayerXDist + teamPlayerYDist * teamPlayerYDist) < 800f)
                                Main.player[myPlayer].AddBuff(BuffID.PaladinsShield, 20);
                        }
                    }
                }
            }
            if(SpeedrunNecklace)
            {
                Player.GetArmorPenetration<GenericDamageClass>() += 300;
                Player.GetDamage<GenericDamageClass>() += 0.5f;
                Player.GetCritChance<GenericDamageClass>() += 50;
                Player.endurance *= 0.01f;
                Player.statDefense /= 100;
                if (Player.lifeRegen > 0)
                    Player.lifeRegen /= 100;
            }
            if(AncientCotbg)
            /*
            远古血神加强：
            ·+10%血上限
            ·10%常驻增伤和5%免伤与血肉图腾效果
            ·少于50%血量5%免伤，10%增伤
            ·少于15%血量10%免伤，20%增伤
            ·低于100防御20%增伤
            ·低于100防御追加一个10%的全局攻速加成
            ↑上述效果可以与返厂的旧血炎叠加.
            */
            {
                calPlayer.fleshTotem = true;
                // Player.statLifeMax2 += (int)(Player.statLifeMax * 0.1f);
                Player.endurance += 0.05f;
                Player.GetDamage<GenericDamageClass>() += 0.05f;
                if(Player.statLife <= (int)(Player.statLifeMax2 * 0.5f))
                {
                    Player.endurance += 0.05f;
                    Player.GetDamage<GenericDamageClass>() += 0.1f;
                    if(Player.statLife <= (int)(Player.statLifeMax2 * 0.15f))
                    {
                        Player.endurance += 0.10f;
                        Player.GetDamage<GenericDamageClass>() += 0.20f;
                    }
                }
                if(Player.statDefense <= 100)
                {
                    Player.GetDamage<GenericDamageClass>() += 0.20f;
                    Player.GetAttackSpeed<GenericDamageClass>() += 0.1f;
                }
            }

            if(BloodflareCoreStat)
            /*旧血炎：低于50%血量5%免伤与10%增伤，低于15%血量10免伤与20增伤。低于100防御力20增伤*/
            {
                if(Player.statLife <= (int)(Player.statLifeMax2 * 0.5f))
                {
                    Player.endurance += 0.05f;
                    Player.GetDamage<GenericDamageClass>() += 0.1f;
                    if(Player.statLife <= (int)(Player.statLifeMax2 * 0.15f))
                    {
                        Player.endurance += 0.10f;
                        Player.GetDamage<GenericDamageClass>() += 0.20f;
                    }
                }
                if(Player.statDefense <= 100)
                {
                    Player.GetDamage<GenericDamageClass>() += 0.15f;
                }
            }
            if(EHeartStats)
            {
                //Scarlet：改了。
                //在召唤物开着的时候这buff怎么可能会给这么多
                //而且尤其是元素之心的基础伤害是150的情况下？
                //跳跃速度砍了一刀，影响到实际用途了
                // Player.statLifeMax2 += 15;
                Player.statManaMax2 += 15;
                Player.moveSpeed += 0.05f;
                Player.endurance += 0.05f;
                Player.GetDamage<GenericDamageClass>() += 0.05f;
                Player.GetCritChance<GenericDamageClass>() += 5;
                Player.jumpSpeedBoost += 0.6f;
                Player.manaCost *=0.95f;
                if(EHeartStatsBoost) //关闭元素之心的召唤物的情况下
                {
                    // Player.statLifeMax2 += 25;  //40(15+25)HP
                    Player.statManaMax2 += 25;  //40(15+25)魔力
                    Player.moveSpeed += 0.05f;   //10(5+5)%移速
                    Player.endurance += 0.05f;  //10(5+5)%免伤
                    Player.GetDamage<GenericDamageClass>() += 0.05f; //10(5+5)%伤害
                    Player.GetCritChance<GenericDamageClass>() += 5; //10(5+5)%暴击
                    Player.jumpSpeedBoost += 1.0f;  //32(12+20)%跳跃速度
                    Player.manaCost *= 0.90f;       //10(5%→10%)%不耗魔
                    //由于返回值的原因导致Buff数值反而不能乱写。
                    //所以现在这些个的buff值都是5的系数了。
                }
            }

            if(AmbrosialImmnue)
            {
                Player.buffImmune[BuffID.Venom] = true;
                Player.buffImmune[BuffID.Frozen] = true;
                Player.buffImmune[BuffID.Chilled] = true;
                Player.buffImmune[BuffID.Frostburn] = true;
                Player.buffImmune[BuffID.Frostburn2] = true; //加了一个霜冻
                calPlayer.alwaysHoneyRegen = true;
                calPlayer.honeyDewHalveDebuffs = true;
                calPlayer.livingDewHalveDebuffs = true;
            }
            if(AmbrosialStats)
            {
                Player.pickSpeed -= 0.5f; //这样会使挖矿速度上下位不能叠加, 但是有一说一都到四柱/神后了, 挖矿速度又不缺这点
            }
        }
        private void Nanotechs()
        {
            CalamityPlayer modPlayer = Player.Calamity();
            if(nanotechold)
            {
                Player.AddCooldown(NanotechUI.ID, Content.Items.Accessories.Rogue.NanotechOld.nanotechDMGStack);
                
                if (nanoTechStackDurability >= 0 && nanoTechStackDurability < 150)
                {
                    //储存了攻击的积攒数量。
                    nanoTechStackDurability = RaiderStacks;
                    //修改了纳米核心的RaiderBuff，现在RaiderBuff只会在达到最大值的时候播报特殊音效。    

                    if (modPlayer.cooldowns.TryGetValue(NanotechUI.ID, out var nanoDurability))
                        nanoDurability.timeLeft = nanoTechStackDurability;
                }
            }
            //现在纳米核心应该能正确地禁用1.5倍伤害加成(在没有佩戴饰品的时候)
            else
            {
                if (RaiderStacks > 0)
                {
                    RaiderStacks = 0;
                    nanoTechStackDurability = RaiderStacks;
                    if (modPlayer.cooldowns.TryGetValue(NanotechUI.ID, out var nanoDurability))
                        nanoDurability.timeLeft = nanoTechStackDurability;
                    InitNanotechSound = 0;
                }
            }
        }
        public void Sponge()
        {
            // 因为较高等级的护盾更亮，所以这里从最高等级到最低等级处理护盾。
            bool shieldAddedLight = false;
            // 如果“海绵”装备没有装备，则消除其耐久冷却时间。
            // 故意保留充电冷却时间以防止快速切换来重新充电护盾。
            if (!CIsponge)
            {
                CalamityPlayer calPlayer = Player.Calamity();
                if (calPlayer.cooldowns.TryGetValue(CISpongeDurability.ID, out var cdDurability))
                    cdDurability.timeLeft = 0;

                // 由于“海绵”的护盾可能处于部分充电状态，这里是为了安全起见。
                // 如果玩家哪怕只有一帧没有装备这个配件，就会完全耗尽所有护盾。

                CISpongeShieldDurability = 0;

            }
            else
            {
                CalamityPlayer calPlayer = Player.Calamity();
                // 如果“海绵”的护盾已经耗尽且还没有开始其充电延迟，则开始充电延迟。
                if (CISpongeShieldDurability == 0 && !calPlayer.cooldowns.ContainsKey(CISpongeRecharge.ID))
                    Player.AddCooldown(CISpongeRecharge.ID, TheSpongetest.CIShieldRechargeDelay);

                // 如果护盾的耐久度大于0但耐久度冷却时间不在冷却时间字典中，则将其添加到冷却时间字典中。
                if (CISpongeShieldDurability > 0 && !calPlayer.cooldowns.ContainsKey(CISpongeDurability.ID))
                {
                    var durabilityCooldown = Player.AddCooldown(CISpongeDurability.ID, TheSpongetest.CIShieldRechargeDelay);
                    durabilityCooldown.timeLeft = CISpongeShieldDurability;
                }

                // 如果护盾的耐久度大于0且不在充电延迟中，则主动补充护盾点数。
                // 在第一次发生这种情况时播放声音。
                if (CISpongeShieldDurability > 0 && !calPlayer.cooldowns.ContainsKey(CISpongeRechargeRelay.ID))
                {
                    if (!CIplayedSpongeShieldSound)
                        SoundEngine.PlaySound(TheSpongetest.ActivationSound, Player.Center);
                    CIplayedSpongeShieldSound = true;

                    // 这个数不是一个整数，并存储了每帧的确切充电进度。
                    CIspongeShieldPartialRechargeProgress += TheSpongetest.CIShieldDurabilityMax / (float)TheSpongetest.CITotalShieldRechargeTime;

                    // 向下取整以获取本帧实际充电的护盾点数。
                    int pointsActuallyRecharged = (int)MathF.Ceiling(CIspongeShieldPartialRechargeProgress);

                    // 将这些点数加到真实的护盾耐久度上，并限制结果。然后从充电进度中减去这些点数。
                    CISpongeShieldDurability = Math.Min(CISpongeShieldDurability + pointsActuallyRecharged, TheSpongetest.CIShieldDurabilityMax);
                    CIspongeShieldPartialRechargeProgress -= pointsActuallyRecharged;

                    // 更新冷却时间字典中的耐久度指示器。
                    if (calPlayer.cooldowns.TryGetValue(CISpongeDurability.ID, out var cdDurability))
                        cdDurability.timeLeft = CISpongeShieldDurability;
                }

                // Add light if this shield is currently active
                if (CISpongeShieldDurability > 0 && !shieldAddedLight)
                {
                    // The Sponge is much brigher than other shields
                    Lighting.AddLight(Player.Center, Color.White.ToVector3() * 0.75f);
                    shieldAddedLight = true;
                }
            }
        }
        public void ArmorSetbonus()
        {
            CalamityPlayer calPlayer = Player.Calamity();
            var usPlayer = Player.CIMod();
            
            if (GodSlayerRangedSet) 
            {
                float getCrits = Player.GetWeaponCrit(Player.ActiveItem());
                
                if (getCrits > 95)
                    Player.GetCritChance<RangedDamageClass>() += 20;
            }
            #region 远古套装系列
            if (AncientTarragonSet)
            {
                calPlayer.defenseDamageRatio *= 0.45f; //防损减免
                if(Player.statLife <= Player.statLifeMax2 * 0.5f)
                {
                    int getDef = Player.GetCurrentDefense();
                    int buffDef = (int)(getDef * 0.2f);
                    Player.statDefense += buffDef;
                    Player.endurance += 0.2f;
                }
                calPlayer.healingPotionMultiplier += 0.45f; 
                Player.crimsonRegen = true;
                Player.lifeRegen += 8; //+4HP/s
                
            }

            if (AncientBloodflareStat)
            {
                calPlayer.healingPotionMultiplier += 0.35f; 
                Player.lifeRegen += 10; //+10HP/s
                if(Player.statLife <= Player.statLifeMax2/2)
                Player.lifeRegen += 16; //+8HP/s
            }

            if (AncientGodSlayerStat)
            {
                //旧套装通用新增；血上限，血药，回血
                calPlayer.healingPotionMultiplier += 0.70f;
                Player.lifeRegen += 8; //+4HP/s
                RefreshGodSlayerDash(calPlayer);
            }
            if (AncientSilvaStat)
            {
                calPlayer.healingPotionMultiplier += 0.30f; 
                Player.lifeRegen += 24; //+12HP/s
                Player.lifeRegenTime = 2000;
            }
            
            if(AncientAuricSet)
            {
                Player.noKnockback = true;
                RefreshGodSlayerDash(calPlayer);
                if(Player.statLife <= Player.statLifeMax2 * 0.5f)
                {
                    int getDef = Player.GetCurrentDefense();
                    int buffDef = (int)(getDef * 0.3f);
                    Player.statDefense += 60 + buffDef;
                    Player.endurance += 0.3f;
                }
            }
            #endregion
            if (Player.vortexStealthActive) //回调星璇数值
            {
                Player.GetDamage<RangedDamageClass>() += (1f - Player.stealth) * 0.4f;
                Player.GetCritChance<RangedDamageClass>() += (int)((1f - Player.stealth) * 5f);
            }
            #region 林海效果
            bool triggerSilvaFakeDeath = Player.HasCooldown(SilvaRevive.ID) || Player.HasBuff(ModContent.BuffType<SilvaRevival>());
            //林海自起后的效果
            if (triggerSilvaFakeDeath)
                DoSilvaFakeDeathEffect();

            // 林海无敌的视觉效果。与其他的。
            if (SilvaRebornTimer > 0)
            {
                SilvaRebornEffect();
            }
            if (SilvaMeleeSetLegacy)
            {
                double multiplier = Player.statLife / (double)Player.statLifeMax2;
                Player.GetDamage<MeleeDamageClass>() += (float)(multiplier * 0.1);
                if (calPlayer.auricSet && SilvaMeleeSetLegacy)
                {
                    double multiplier1 = Player.statLife / (double)Player.statLifeMax2;
                    Player.GetDamage<MeleeDamageClass>() += (float)(multiplier1 * 0.1);
                }
            }
            #endregion

            if (AuricDebuffImmune)
            {
                // 好你个小子免疫这游戏所有Debuff了
                // foreach (int debuff in CalamityLists.debuffList)
                foreach (int debuff in CalamityInheritanceLists.AuricdebuffList)
                    Player.buffImmune[debuff] = true;
            } 
        }

        public void RefreshGodSlayerDash(CalamityPlayer calPlayer)
        {
            if (Player.HasCooldown(GodSlayerDash.ID))
            {
                if (calPlayer.rogueStealth == calPlayer.rogueStealthMax)
                {
                    Player.RemoveCooldown(GodSlayerDash.ID);
                    Player.AddCooldown(GodSlayerDash.ID, 0);
                    AncinetGodSlayerDashReset = true;
                }
            }
        }

        private void DoSilvaFakeDeathEffect()
        {
            var calPlayer = Player.Calamity();
            if (SilvaMagicSetLegacy)
                Player.GetDamage<MagicDamageClass>() += 0.60f;

            if (SilvaMeleeSetLegacy)
                calPlayer.contactDamageReduction += 0.4f;

            if (SilvaRangedSetLegacy)
                Player.GetDamage<RangedDamageClass>() += 0.40f;

            if (SilvaSummonSetLegacy)
            {
                Player.GetDamage<SummonDamageClass>() *= 1.05f;
                Player.maxMinions += 2;
            }

            if (SilvaRougeSetLegacy)
                Player.GetDamage<RogueDamageClass>() += 0.40f;
        }
        public void SilvaRebornEffect()
        {
            // 计时器
            SilvaRebornTimer -= 1;
            // 粒子
            DoSilvaDust();
            // CD
            if (SilvaRebornTimer == 1)
            {
                SoundEngine.PlaySound(SilvaHeadSummon.DispelSound, Player.Center);
                // 45秒
                Player.AddCooldown(SilvaRevive.ID, CalamityUtils.SecondsToFrames(90));
            }
            // 重置到100血防止反复触发复活
            if (Player.statLife < 100)
                Player.statLife = 100;
        }
        public void DoSilvaDust()
        {
            for (int j = 0; j < 2; j++)
            {
                int green = Dust.NewDust(Player.position, Player.width, Player.height, DustID.ChlorophyteWeapon, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 2f);
                Main.dust[green].position.X += Main.rand.Next(-20, 21);
                Main.dust[green].position.Y += Main.rand.Next(-20, 21);
                Main.dust[green].velocity *= 0.9f;
                Main.dust[green].noGravity = true;
                Main.dust[green].scale *= 1f + Main.rand.Next(40) * 0.01f;
                Main.dust[green].shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
                if (Main.rand.NextBool())
                    Main.dust[green].scale *= 1f + Main.rand.Next(40) * 0.01f;
            }
        }

        public void MiscEffects()
        {
            CalamityInheritancePlayer usPlayer = Player.CIMod();
            CalamityPlayer calPlayer = Player.Calamity();
            Player player = Main.player[Main.myPlayer];
            Item item = player.HeldItem;
            if (ShroomiteFlameBooster && item.useAmmo == AmmoID.Gel)
            {
                Player.GetDamage<RangedDamageClass>() += 0.30f;
                Player.GetCritChance<RangedDamageClass>() += 5;
                if (Main.zenithWorld)
                    Player.GetDamage<RangedDamageClass>() *= 3f;
            }
            if (EmpressBooster)
            {
                Player.jumpSpeedBoost += 1.80f;
                Player.runAcceleration *= 1.20f;
                Player.moveSpeed += 0.12f;
                calPlayer.infiniteFlight = true; //再次准许无限飞行
            }
            if (CIConfig.Instance.ReduceMoveSpeed && CalamityConditions.DownedDevourerOfGods.IsMet())
            {
                player.moveSpeed -= 0.40f;
                player.runAcceleration *= 0.80f;
                player.accRunSpeed -= 0.40f;
            }

            //T3维苏威阿斯：使用时为自己提供+2HP/s生命恢复速度，并提高10%伤害。
            if (Player.ActiveItem().type == ModContent.ItemType<RavagerLegendary>() && BetsyTier3)
            {
                Player.AddBuff(ModContent.BuffType<VolcanoBuff>(), 120);
            }

            if (Player.ActiveItem().type != ModContent.ItemType<DefenseBlade>())
            {
                if (DefenseBoost > 0f || DefendTier1Timer > 0)
                {
                    DefenseBoost = 0f;
                    DefendTier1Timer = 0;
                }
            }
            if (DefendTier1)
            {
                int b = Player.GetCurrentDefense();
                int realDefense = (int)(b * DefenseBoost);
                Player.statDefense += realDefense;  
            }
            if (!BuffPolarisBoost || Player.ActiveItem().type != ModContent.ItemType<PolarisParrotfishLegacy>())
            {
                BuffPolarisBoost = false;
                if (Player.FindBuffIndex(ModContent.BuffType<PolarisBuffLegacy>()) > -1)
                    Player.ClearBuff(ModContent.BuffType<PolarisBuffLegacy>());

                PolarisBoostCounter = 0;
                PolarisPhase2 = false;
                PolarisPhase3 = false;
            }
            if (PolarisBoostCounter >= 20 /*|| CIFunction.IsThereNpcNearby(ModContent.NPCType<CalamitasRebornPhase2>(), Player, 3000f)*/)
            {
                PolarisPhase2 = false;
                PolarisPhase3 = true;
            }
            else if (PolarisBoostCounter >= 10)
                PolarisPhase2 = true;
            if (InvincibleJam)
            {
                foreach (int debuff in CalamityLists.debuffList)
                    Player.buffImmune[debuff] = true;
            }

            //龙弓左键伤害倍率计算
            if (Player.ownedProjectileCounts[ModContent.ProjectileType<DragonBow>()] != 0)
            {
                float armorPeneBoost = Player.GetTotalArmorPenetration<RangedDamageClass>();
                int totalArmor = Player.GetCurrentDefense();
                float totalArmorBoost = (1 - totalArmor/100) < 0f ? 0f : (1 - totalArmor/100);
                float damageBoost = armorPeneBoost / 100f + totalArmorBoost;
                float actualDamage = 1.0f + damageBoost; 
                //避免伤害变成0乃至负数倍, 不惜一切代价
                if(actualDamage <= 0f) actualDamage = 1.0f;
                player.GetDamage<RangedDamageClass>() *= actualDamage;
            }           
            
            //玩家佩戴创造之手，挥舞板凳时，提供30%伤害与暴击概率
            if (Player.ActiveItem().type == ModContent.ItemType<StepToolShadow>() && IfGodHand)
            {
                player.GetDamage<MagicDamageClass>() += 0.30f;
                player.GetCritChance<MagicDamageClass>() += 30;
            }

            if(AncientAstralSet && AncientAstralStealthGap == 0 && AncientAstralStealth > 0)
            {
                AncientAstralStealth = 0; //置零就行了 
            }

            
            if (nanotechold)
            {
                float damageMult =  0.15f;
                Player.GetDamage<GenericDamageClass>() *= 1 + RaiderStacks / 150f * damageMult;
            }
            if (Player.ownedProjectileCounts[ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>()] == 1 && 
                Player.ActiveItem().type == ModContent.ItemType<ExoTheApostle>()) 
            {
                player.GetDamage<RogueDamageClass>() *= 2;
            }
        }
        private void StandingStill()
        {
            CalamityInheritancePlayer usPlayer = Player.CIMod();
            CalamityPlayer calPlayer = Player.Calamity();

            //气功念珠
            if (SForest)
            {
                if (SForestBuff)
                {
                    Player.GetDamage<GenericDamageClass>() += 0.5f;
                    if (Player.itemAnimation > 0)
                        SForestBuffTimer = 0;
                }
                if (Player.StandingStill(0.1f) && !Player.mount.Active)
                {
                    if (SForestBuffTimer < 120)
                        SForestBuffTimer++;
                    else
                        Player.AddBuff(ModContent.BuffType<ShrineForestBuff>(), 6);
                }
                else SForestBuffTimer -= 1;
            }
            else SForestBuffTimer = 0;

            if(DraedonsHeartLegacyStats) //嘉登之心的站立不动提供的效果
            {
                if(Player.StandingStill(0.1f) && !Player.mount.Active)
                {
                    int getDefense = Player.GetCurrentDefense();
                    Player.GetDamage<GenericDamageClass>() *= DraedonsHeartLegacy.DamageReduceRatio;
                    int buffDefense = (int)(getDefense * DraedonsHeartLegacy.DefenseMultipler);
                    Player.statDefense += buffDefense;
                    Player.lifeRegen += DraedonsHeartLegacy.LifeRegenSpeed;
                }

            }
            // Auric bonus
            //哇还有大缩进哦
            if (auricBoostold)
            {
                if (Player.StandingStill(0.1f) && !Player.mount.Active)
                {
                    if (modStealth > 0)
                    {
                        modStealth -= 20;
                        if (modStealth <= 0)
                        {
                            modStealth = 0;
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                                NetMessage.SendData(MessageID.PlayerStealth, -1, -1, null, Player.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                        }
                    }
                }
                else
                {
                    float playerVel = Math.Abs(Player.velocity.X) + Math.Abs(Player.velocity.Y);
                    modStealth += (int)(playerVel * 5);
                    if (modStealth > 1000)
                        modStealth = 1000;
                }

                Player.GetDamage<GenericDamageClass>() += (1000 - modStealth) * 0.0003f;
                Player.GetCritChance<GenericDamageClass>() += (int)((1000 -modStealth) * 0.015f);
            }
            
            if (PsychoticAmulet)
            {
                if (Player.StandingStill(0.1f) && !Player.mount.Active)
                {
                    if (modStealth > 0)
                    {
                        modStealth -= 20;
                        if (modStealth <= 0)
                        {
                            modStealth = 0;
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                                NetMessage.SendData(MessageID.PlayerStealth, -1, -1, null, Player.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                        }
                    }
                }
                else
                {
                    float playerVel = Math.Abs(Player.velocity.X) + Math.Abs(Player.velocity.Y);
                    modStealth += (int)(playerVel * 5);
                    if (modStealth > 1000)
                        modStealth = 1000;
                }

                Player.GetDamage<RogueDamageClass>() += (1000 - modStealth) * 0.0005f;
                Player.GetCritChance<RogueDamageClass>() += (int)((1000 - modStealth) * 0.015f);
                Player.aggro -= ((1000 - modStealth) * 750);
            }
        }
        public void RamShield()
        {
            if(ElysianAegisImmnue)
            {
                Player.buffImmune[BuffID.CursedInferno] = true; //是的, 就是这么少
                Player.buffImmune[BuffID.ShadowFlame] = true;
                Player.buffImmune[ModContent.BuffType<Nightwither>()] = true;
                Player.buffImmune[BuffID.Daybreak] = true;
                Player.buffImmune[ModContent.BuffType<WhisperingDeath>()] = true;
                Player.buffImmune[ModContent.BuffType<WeakPetrification>()] = true;
            }
            if(AsgardsValorImmnue)
            {
                Player.buffImmune[BuffID.Chilled] = true;
                Player.buffImmune[BuffID.Frostburn] = true;
                Player.buffImmune[BuffID.Frostburn2] = true;
                Player.buffImmune[BuffID.Frozen] = true;
                Player.buffImmune[BuffID.Weak] = true;
                Player.buffImmune[BuffID.BrokenArmor] = true;
                Player.buffImmune[BuffID.Bleeding] = true;
                Player.buffImmune[BuffID.Poisoned] = true;
                Player.buffImmune[BuffID.Slow] = true;
                Player.buffImmune[BuffID.Confused] = true;
                Player.buffImmune[BuffID.Silenced] = true;
                Player.buffImmune[BuffID.Cursed] = true;
                Player.buffImmune[BuffID.Darkness] = true;
                Player.buffImmune[BuffID.WindPushed] = true;
                Player.buffImmune[BuffID.Stoned] = true;
                Player.buffImmune[BuffID.Daybreak] = true;
                Player.buffImmune[ModContent.BuffType<SearingLava>()] = true;
            }
            if (ElysianAegis)
            {
                bool spawnDust = false;

                // Activate buff
                if (ElysianGuard)
                {
                    if (Player.whoAmI == Main.myPlayer)
                        Player.AddBuff(ModContent.BuffType<ElysianGuard>(), 2, false);

                    float shieldBoostInitial = shieldInvinc;
                    shieldInvinc -= 0.08f;
                    if (shieldInvinc < 0f)
                        shieldInvinc = 0f;
                    else
                        spawnDust = true;

                    if (shieldInvinc == 0f && shieldBoostInitial != shieldInvinc && Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.PlayerStealth, -1, -1, null, Player.whoAmI, 0f, 0f, 0f, 0, 0, 0);

                    float damageBoost = (5f - shieldInvinc) * 0.03f;
                    Player.GetDamage<GenericDamageClass>() += damageBoost;

                    int critBoost = (int)((5f - shieldInvinc) * 2f);
                    Player.GetCritChance<GenericDamageClass>() += critBoost;

                    Player.aggro += (int)((5f - shieldInvinc) * 220f);
                    Player.statDefense += (int)((5f - shieldInvinc) * 8f);
                    Player.moveSpeed *= 0.85f;
                }

                // Remove buff
                else
                {
                    float shieldBoostInitial = shieldInvinc;
                    shieldInvinc += 0.08f;
                    if (shieldInvinc > 5f)
                        shieldInvinc = 5f;
                    else
                        spawnDust = true;

                    if (shieldInvinc == 5f && shieldBoostInitial != shieldInvinc && Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.PlayerStealth, -1, -1, null, Player.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                }

                // Emit dust
                if (spawnDust)
                {
                    if (Main.rand.NextBool(2))
                    {
                        Vector2 vector = Vector2.UnitY.RotatedByRandom(Math.PI * 2D);
                        Dust dust = Main.dust[Dust.NewDust(Player.Center - vector * 30f, 0, 0, (int)CalamityDusts.ProfanedFire, 0f, 0f, 0, default, 1f)];
                        dust.noGravity = true;
                        dust.position = Player.Center - vector * Main.rand.Next(5, 11);
                        dust.velocity = vector.RotatedBy(Math.PI / 2D, default) * 4f;
                        dust.scale = 0.5f + Main.rand.NextFloat();
                        dust.fadeIn = 0.5f;
                    }

                    if (Main.rand.NextBool(2))
                    {
                        Vector2 vector2 = Vector2.UnitY.RotatedByRandom(Math.PI * 2D);
                        Dust dust2 = Main.dust[Dust.NewDust(Player.Center - vector2 * 30f, 0, 0, DustID.GoldCoin, 0f, 0f, 0, default, 1f)];
                        dust2.noGravity = true;
                        dust2.position = Player.Center - vector2 * 12f;
                        dust2.velocity = vector2.RotatedBy(-Math.PI / 2D, default) * 2f;
                        dust2.scale = 0.5f + Main.rand.NextFloat();
                        dust2.fadeIn = 0.5f;
                    }
                }
            }
            else
                ElysianGuard = false;
        }
        public void XerocSetbouns()
        {
            CalamityPlayer calPlayer= Player.Calamity();
            if(AncientXerocSet)
            {
                calPlayer.stealthStrikeHalfCost = true; //使盗贼的潜伏值只消耗一半
     
                if(Player.statLife<=(Player.statLifeMax2 * 0.8f) && Player.statLife > (Player.statLifeMax2 * 0.6f))
                {
                    Player.GetDamage<GenericDamageClass>() +=0.10f;
                    Player.GetCritChance<GenericDamageClass>() += 10;
                }

                else if(Player.statLife<=(Player.statLifeMax2 * 0.6f) && Player.statLife > (Player.statLifeMax2 * 0.25f))
                {
                    Player.GetDamage<GenericDamageClass>() +=0.15f;
                    Player.GetCritChance<GenericDamageClass>() += 15;
                }
                
                else if(Player.statLife<=(Player.statLifeMax2 * 0.25f) && Player.statLife > (Player.statLifeMax2 * 0.15f))
                {
                    //进一步压缩血量 阈值。现在最高收益需要的血量区间为最大生命值的25%到15%.（此前为35%）
                    Player.AddBuff(ModContent.BuffType<AncientXerocMadness>(), 2);
                    Player.GetDamage<GenericDamageClass>() += 0.40f; //玩家血量30%下的数值加成：50%伤害与50%暴击率
                    Player.GetCritChance<GenericDamageClass>() += 40;
                    Player.manaCost *= 0.10f; //魔法武器几乎不耗魔力
                    calPlayer.healingPotionMultiplier += 0.10f;
                    //Scarlet:追加了10%治疗量加成，这一效果会使150血药的治疗变成165治疗，保证使用150血治疗后不会让玩家继续停留在这个增伤区间
                    //附：我并不是很喜欢这种卖血换输出的设计，但原作如此。
                }
                else if(Player.statLife<=(Player.statLifeMax2 *0.15f))
                {
                    Player.AddBuff(ModContent.BuffType<AncientXerocShame>(), 2);
                    Player.GetDamage<GenericDamageClass>() -= 0.40f; //低于15%血量时-40%伤害与暴击率 - 这一效果可以通过搭配克希洛克翅膀免疫
                    Player.GetCritChance<GenericDamageClass>() -= 40;
                    // Player.statDefense -= 50; //削减其防御力，使损失的防御力几乎足以致死
                    //25.2.11:移除防御力削减的负面效果，我也不知道我是出于什么心态才加的
                    AncientXerocWrath = true;
                }
            }
        }
        public void LoreEffects()
        {
            CalamityInheritancePlayer usPlayer = Player.CIMod();
            CalamityPlayer calPlayer = Player.Calamity();
            #region Lore
            if(LoreEOC || PanelsLoreEoC)
            {
                if (!Main.dayTime)
                    Player.nightVision = true;
                else
                    Player.blind = true;
            }
            if (LoreKingSlime || PanelsLoreKingSlime)
            {
                Player.moveSpeed += 0.05f;
                Player.jumpSpeedBoost += Player.autoJump ? 0f : 0.1f;
                Player.statDefense -= 3;
            }

            if (LoreDesertScourge || PanelsLoreDesertScourge)
            {
                if (Player.ZoneDesert || Player.Calamity().ZoneSunkenSea)
                {
                    Player.statDefense += 15;
                    Player.GetDamage<GenericDamageClass>() -= 0.025f;
                }
            }

            if (LoreCrabulon || PanelsLoreCrabulon)
            {
                if (Player.ZoneGlowshroom || Player.ZoneDirtLayerHeight || Player.ZoneRockLayerHeight)
                {
                    if (Main.myPlayer == Player.whoAmI)
                        Player.AddBuff(ModContent.BuffType<Mushy>(), 2);

                    Player.moveSpeed -= 0.1f;
                }
            }

            if (LoreEaterofWorld || PanelsLoreEaterofWorld)
            {
                int damage = (int)(15 * Player.GetBestClassDamage().ApplyTo(1));
                damage = Player.ApplyArmorAccDamageBonusesTo(damage);
                float knockBack = 1f;

                if (Main.rand.NextBool(15))
                {
                    int defualtProj = 0;

                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == Player.whoAmI && Main.projectile[i].type == ModContent.ProjectileType<TheDeadlyMicrobeProjectile>())
                            defualtProj++;
                    }

                    if (Main.rand.Next(15) >= defualtProj && defualtProj < 6)
                    {
                        int projFirstStack = 50;
                        int projSecStack = 24;
                        int projThirdStack = 90;

                        for (int j = 0; j < projFirstStack; j++)
                        {
                            int projPos = Main.rand.Next(200 - j * 2, 400 + j * 2);
                            Vector2 center = Player.Center;
                            center.X += Main.rand.NextFloat(-projPos, projPos + 1);
                            center.Y += Main.rand.NextFloat(-projPos, projPos + 1);

                            if (!Collision.SolidCollision(center, projSecStack, projSecStack) && !Collision.WetCollision(center, projSecStack, projSecStack))
                            {
                                center.X += projSecStack / 2;
                                center.Y += projSecStack / 2;

                                if (Collision.CanHit(Player.Center, 1, 1, center, 1, 1) || Collision.CanHit(new Vector2(Player.Center.X, Player.position.Y - 50f), 1, 1, center, 1, 1))
                                {
                                    int tileX = (int)center.X / 16;
                                    int tileY = (int)center.Y / 16;
                                    bool ifBounce = false;

                                    if (Main.rand.NextBool(3) && Main.tile[tileX, tileY] != null && Main.tile[tileX, tileY].WallType > 0)
                                        ifBounce = true;
                                    else
                                    {
                                        center.X -= projThirdStack / 2;
                                        center.Y -= projThirdStack / 2;

                                        if (Collision.SolidCollision(center, projThirdStack, projThirdStack))
                                        {
                                            center.X += projThirdStack / 2;
                                            center.Y += projThirdStack / 2;
                                            ifBounce = true;
                                        }
                                    }

                                    if (ifBounce)
                                    {
                                        for (int k = 0; k < Main.maxProjectiles; k++)
                                        {
                                            if (Main.projectile[k].active && Main.projectile[k].owner == Player.whoAmI && Main.projectile[k].type == ModContent.ProjectileType<TheDeadlyMicrobeProjectile>() && (center - Main.projectile[k].Center).Length() < 48f)
                                            {
                                                ifBounce = false;
                                                break;
                                            }
                                        }

                                        if (ifBounce && Main.myPlayer == Player.whoAmI)
                                        {
                                            IEntitySource entitySource = Player.GetSource_ItemUse(Player.HeldItem);
                                            Projectile.NewProjectile(entitySource, center.X, center.Y, 0f, 0f, ModContent.ProjectileType<TheDeadlyMicrobeProjectile>(), damage, knockBack, Player.whoAmI, 0f, 0f);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (LoreSkeletron || PanelsLoreSkeletron)
            {
                Player.GetDamage<GenericDamageClass>() += 0.1f;
                Player.GetCritChance<GenericDamageClass>() += 5;
                Player.statLifeMax2 = (int)(Player.statLifeMax2 * 0.90);
            }

            if (LoreDestroyer || PanelsLoreDestroyer)
            {
                Player.pickSpeed -= 0.15f;
            }

            if (LoreAquaticScourge || PanelsLoreAquaticScourge)
            {
                if (Player.wellFed)
                {
                    Player.Calamity().decayEffigy = true;
                    Player.statDefense += 1;
                    Player.GetDamage(DamageClass.Generic) += 0.025f;
                    Player.GetCritChance<GenericDamageClass>() += 1;
                    Player.GetKnockback(DamageClass.Summon).Base += 0.25f;
                    Player.moveSpeed += 0.1f;
                }
                else
                {
                    Player.statDefense -= 1;
                    Player.GetDamage(DamageClass.Generic) -= 0.025f;
                    Player.GetCritChance<GenericDamageClass>() -= 1;
                    Player.GetKnockback(DamageClass.Summon).Base -= 0.25f;
                    Player.moveSpeed -= 0.1f;
                }
            }

            if (LorePrime || PanelsLorePrime)
            {
                Player.GetArmorPenetration(DamageClass.Generic) += 10;
            }

            if (LoreLeviAnahita || PanelsLoreLeviAnahita)
            {
                CalamityPlayer modplayer = Player.Calamity();
                if (Player.IsUnderwater())
                {
                    if (modplayer.aquaticHeart || modplayer.aquaticHeartPrevious)
                        Player.statLifeMax2 += Player.statLifeMax2 / 20;
                }

                if (!Player.IsUnderwater())
                {
                    Player.statDefense -= 8;
                    Player.endurance -= 0.05f;
                }

                if (calPlayer.sirenPet)
                {
                    Player.spelunkerTimer += 1;
                    if (Player.spelunkerTimer >= 10)
                    {
                        Player.spelunkerTimer = 0;
                        int offset = 30;
                        int playerSpriteX = (int)Player.Center.X / 16;
                        int playerSpriteY = (int)Player.Center.Y / 16;

                        for (int i = playerSpriteX - offset; i <= playerSpriteX + offset; i++)
                        {
                            for (int j = playerSpriteY - offset; j <= playerSpriteY + offset; j++)
                            {
                                if (Main.rand.NextBool(4))
                                {
                                    Vector2 vector = new(playerSpriteX - i, playerSpriteY - j);
                                    if (vector.Length() < offset && i > 0 && i < Main.maxTilesX - 1 && j > 0 && j < Main.maxTilesY - 1 && Main.tile[i, j] != null && Main.tile[i, j].HasTile)
                                    {
                                        bool ifSubm = false;
                                        if (Main.tile[i, j].TileType == 185 && Main.tile[i, j].TileFrameY == 18)
                                        {
                                            if (Main.tile[i, j].TileFrameX >= 576 && Main.tile[i, j].TileFrameX <= 882)
                                                ifSubm = true;
                                        }
                                        else if (Main.tile[i, j].TileType == 186 && Main.tile[i, j].TileFrameX >= 864 && Main.tile[i, j].TileFrameX <= 1170)
                                            ifSubm = true;

                                        if (ifSubm || Main.tileSpelunker[Main.tile[i, j].TileType] || (Main.tileAlch[Main.tile[i, j].TileType] && Main.tile[i, j].TileType != 82))
                                        {
                                            int dType = Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustID.TreasureSparkle, 0f, 0f, 150, default, 0.3f);
                                            Main.dust[dType].fadeIn = 0.75f;
                                            Main.dust[dType].velocity *= 0.1f;
                                            Main.dust[dType].noLight = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (LoreDeus || PanelsLoreDeus)
            {
                Player.moveSpeed += 0.2f;
                Player.Calamity().gravityNormalizer = true;
            }

            if (LoreAureus || PanelsLoreAureus)
               if (Player.ZoneSkyHeight)
                    Player.jumpSpeedBoost += 0.5f;

            if (LoreGolem || PanelsLoreGolem)
            {
                if (Math.Abs(Player.velocity.X) < 0.05f && Math.Abs(Player.velocity.Y) < 0.05f && Player.itemAnimation == 0)
                    Player.statDefense += 30;
            }

            if (LoreDuke || PanelsLoreDuke)
            {
                if (Player.IsUnderwater())
                {
                    Player.GetDamage(DamageClass.Generic) += 0.05f;
                    Player.GetCritChance<GenericDamageClass>() += 5;
                    Player.moveSpeed += 0.1f;
                }
                else
                {
                    Player.GetDamage(DamageClass.Generic) -= 0.02f;
                    Player.GetCritChance<GenericDamageClass>() -= 2;
                    Player.moveSpeed -= 0.04f;
                }
            }

            if (LoreCultist || PanelsLoreCultist)
            {
                Player.blind = true;
                Player.endurance += 0.04f;
                Player.statDefense += 4;
                Player.GetDamage(DamageClass.Generic) += 0.04f;
                Player.GetCritChance<GenericDamageClass>() += 4;
                Player.GetKnockback(DamageClass.Summon).Base += 0.5f;
                Player.moveSpeed += 0.1f;
            }

            if (LoreLunarBoss || PanelsLoreLunarBoss)
            {
                if (Player.gravDir == -1f && Player.gravControl2)
                {
                    Player.endurance += 0.05f;
                    Player.statDefense += 10;
                    Player.GetDamage(DamageClass.Generic) += 0.1f;
                    Player.GetCritChance<GenericDamageClass>() += 10;
                    Player.GetKnockback(DamageClass.Summon).Base += 1.5f;
                    Player.moveSpeed += 0.15f;
                }
                else
                    Player.slowFall = true;
            }

            if (LoreTwins || PanelsLoreTwins)
            {
                if (!Main.dayTime)
                {
                    Player.invis = true;
                    Player.GetCritChance<ThrowingDamageClass>() += 5;
                    Player.GetDamage<ThrowingDamageClass>() += 0.05f;
                }

                if (Player.statLife >= (int)(Player.statLifeMax2 * 0.5))
                    Player.statDefense -= 10;
            }

            if (LorePlantera || PanelsLorePlantera)
            {
                if (Player.statLife >= (int)(Player.statLifeMax2 * 0.5))
                {
                    Player.GetDamage<GenericDamageClass>() -= 0.05f;
                    Player.statDefense += 15;
                }
                if (Player.statLife <= (int)(Player.statLifeMax2 * 0.5))
                {
                    Player.GetDamage<GenericDamageClass>() += 0.1f;
                    Player.statDefense -= 10;
                }
            }

            // Brimstone Elemental lore inferno potion boost
            if ((LoreBrimstoneElement || PanelsLoreBrimstoneElement ||calPlayer.ataxiaBlaze) && Player.inferno)
            {
                const int FramesPerHit = 30;

                // Constantly increment the timer every frame.
                calPlayer.brimLoreInfernoTimer = (calPlayer.brimLoreInfernoTimer + 1) % FramesPerHit;

                // Only run this code for the client which is wearing the armor.
                // Brimstone flames is applied every single frame, but direct damage is only dealt twice per second.
                if (Player.whoAmI == Main.myPlayer)
                {
                    const int BaseDamage = 50;
                    int damage = (int)(BaseDamage * Player.GetBestClassDamage().ApplyTo(1));
                    damage = Player.ApplyArmorAccDamageBonusesTo(damage);
                    float range = 300f;
                    IEntitySource entitySource = Player.GetSource_Accessory(Player.HeldItem);
                    for (int i = 0; i < Main.maxNPCs; ++i)
                    {
                        NPC Npc = Main.npc[i];
                        if (!Npc.active || Npc.friendly || Npc.damage <= 0 || Npc.dontTakeDamage)
                            continue;

                        if (Vector2.Distance(Player.Center, Npc.Center) <= range)
                        {
                            Npc.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
                            if (calPlayer.brimLoreInfernoTimer == 0)
                                Projectile.NewProjectileDirect(entitySource, Npc.Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), damage, 0f, Player.whoAmI, i);
                        }
                    }
                }
            }

            if (LoreCalamitasClone || PanelsLoreCalamitasClone)
            {
                Player.maxMinions += 2;
                Player.statLifeMax2 = (int)(Player.statLifeMax2 * 0.75);
            }
            if (LoreGoliath || PanelsLoreGoliath)
            {
                if (Player.wingTimeMax > 0)
                    Player.wingTimeMax = (int)(Player.wingTimeMax * 1.25);
                Player.lifeRegen -= 8;
            }

            if (LoreDukeElder || PanelsLoreDukeElder)
            {
                if (calPlayer.ZoneAbyss || calPlayer.ZoneSulphur)
                {
                    Player.breath = Player.breathMax + 91;
                    Player.endurance += 0.2f;
                    Player.statDefense += 30;
                    Player.Calamity().decayEffigy = true;
                    Player.buffImmune[ModContent.BuffType<SulphuricPoisoning>()] = true;
                    Player.buffImmune[ModContent.BuffType<CrushDepth>()] = true;
                    Player.lifeRegen += 3;
                }
                if (!calPlayer.ZoneAbyss || !calPlayer.ZoneSulphur)
                {
                    Player.endurance -= 0.1f;
                    Player.statDefense -= 15;
                    Player.accRunSpeed -= 0.5f;
                    Player.lifeRegen -= 3;
                }
            }

            if (LoreRavager || PanelsLoreRavager)
            {
                calPlayer.weakPetrification = true;
                if (Player.wingTimeMax > 0)
                    Player.wingTimeMax = (int)(Player.wingTimeMax * 0.5);
                Player.GetDamage<GenericDamageClass>() += 0.5f;
                Player.ClearBuff(BuffID.Featherfall);
            }

            if (LoreProvidence || PanelsLoreProvidence)
            {
                Player.statLifeMax2 = (int)(Player.statLifeMax2 * 0.8);
                Player.GetDamage<GenericDamageClass>() *= 1.1f;
                Player.buffImmune[ModContent.BuffType<IcarusFolly>()] = true;
            }

            if (LoreDevourer || PanelsLoreDevourer)
            {
                Player.GetDamage<TrueMeleeDamageClass>() += 0.5f;
            }
            if (LoreJungleDragon || PanelsLoreJungleDragon)
            {
                calPlayer.infiniteFlight = true;
                Player.wingAccRunSpeed += 0.2f;
                Player.accRunSpeed += 0.2f;
                Player.GetDamage<GenericDamageClass>() -= 0.25f;
            }
            if(LoreCryoDash || PanelsLoreCryoDash)
            {
                Player.Calamity().DashID = OrnateShieldDash.ID;
                Player.dashType = 0;
                Player.statDefense -= 10;
            }
            if(LoreSG ||PanelsLoreSG)
            {
                if (Player.dashDelay < 0)
                    Player.velocity.X *= 0.9f;

                Player.slippy2 = true;

                if (Main.myPlayer == Player.whoAmI)
                    Player.AddBuff(BuffID.Slimed, 2);

                Player.statDefense -= 10;
            }
            #endregion
            
        }
        public void ShieldEffect()
        {
            CalamityInheritancePlayer usPlayer = Player.CIMod();
            CalamityPlayer calPlayer = Player.Calamity();
            // 海绵的保底
            if (Player.statLifeMax2 > 800)
                ShieldDurabilityMax = Player.statLifeMax2;
            else
                ShieldDurabilityMax = 800;

            if (calPlayer.chaliceOfTheBloodGod)
                ShieldDurabilityMax = Main.zenithWorld ? Player.statLifeMax2 : 20;
        }
    }
}
