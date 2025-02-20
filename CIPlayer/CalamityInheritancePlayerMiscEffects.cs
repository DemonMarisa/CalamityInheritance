using System;
using CalamityInheritance.Content.Items.Accessories;
using CalamityMod;
using CalamityMod.Items.Accessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using CalamityInheritance.CICooldowns;
using CalamityMod.CalPlayer;
using CalamityInheritance.Utilities;
using CalamityMod.Buffs.StatBuffs;
using Terraria.DataStructures;
using Mono.Cecil;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.NPCs.Abyss;
using CalamityInheritance.Buffs;
using Terraria.ID;
using CalamityMod.Cooldowns;
using CalamityInheritance.Content.Items.Potions;
using CalamityInheritance.Buffs.Statbuffs;
using CalamityMod.Dusts;
using CalamityMod.Items.Armor.Silva;
using Terraria.Graphics.Shaders;
using CalamityInheritance.Content.Items.Weapons.Melee;
using Microsoft.Xna.Framework.Audio;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Content.Items.Accessories.Rogue;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CalamityInheritance.Content.Projectiles.Magic;


//Scarlet:将全部灾厄的Player与CI的Player的变量名统一修改，byd modPlayer和modPlayer1飞来飞去的到底在整啥😡
//灾厄Player的变量名现在统一为calPlayer。本模组player的变量名统一为usPlayer
namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public static int darkSunRingDayRegen = 6;
        public static int darkSunRingNightDefense = 20;
        public override void PostUpdateMiscEffects()
        {
            CalamityPlayer calPlayer = Player.Calamity();
            //海绵的护盾
            CIEnergyShields();

            //非常冗余的其他效果
            MiscEffects();

            //纳米技术堆叠UI
            NanoTechUI();

            //Buff效果
            OtherBuffEffects();

            //饰品数值
            AccessoriesStatsFunc();

            //站立不动时玩家可以获得的效果
            StandingStillEffects();

            //😡
            ElysianAegisEffects();

            //各种套装效果的封装
            ArmorSetBonusEffects();

            //克希洛克套装效果的封装(因为太长了所以单独封装起来了)
            AncientXerocEffect();

            if (Player.statLifeMax2 > 800 && !calPlayer.chaliceOfTheBloodGod)
                ShieldDurabilityMax = Player.statLifeMax2;
            else
                ShieldDurabilityMax = 800;

            if (calPlayer.chaliceOfTheBloodGod)
                ShieldDurabilityMax = 15;
        }
        public void OtherBuffEffects()
        {
            CalamityPlayer calPlayer = Player.Calamity();
            var usPlayer = Player.CalamityInheritance();
            Player player = Main.player[Main.myPlayer];
            Item item = player.HeldItem;
            if (armorShattering)
            {
                Player.GetDamage<ThrowingDamageClass>() += 0.08f;
                Player.GetDamage<MeleeDamageClass>() += 0.08f;
                Player.GetCritChance<RogueDamageClass>() += 8;
                Player.GetCritChance<MeleeDamageClass>() += 8;
            }

            if (cadence)
            {
                Player.lifeMagnet = true;
                Player.lifeRegen += 10;
                Player.statLifeMax2 += Player.statLifeMax / 5 / 20 * 25;
            }

            if (draconicSurge)
            {
                if (Player.wingTimeMax > 0)
                {
                    Player.wingTimeMax = (int)(Player.wingTimeMax * 1.25);
                }
                Player.statDefense += 16;
                Player.wingAccRunSpeed += 0.1f;
                Player.accRunSpeed += 0.1f;
                if(yharonLore)
                {
                    Player.GetDamage<GenericDamageClass>() += 0.15f;
                }

                if (Player.HasCooldown(DraconicElixirCooldown.ID))
                {
                    Player.statDefense -= 32;
                    Player.wingAccRunSpeed -= 0.2f;
                    Player.accRunSpeed -= 0.2f;
                    Player.GetDamage<GenericDamageClass>() -= 0.15f;
                }
            }

            if (penumbra)
            {
                calPlayer.stealthGenStandstill += 0.15f;
                calPlayer.stealthGenMoving += 0.1f;
            }

            if (profanedRage)
            {
                Player.GetCritChance<GenericDamageClass>() += ProfanedRagePotion.CritBoost;
            }

            if (holyWrath)
            {
                Player.GetDamage<GenericDamageClass>() += 0.12f;
            }

            if (tScale)
            {
                Player.endurance += 0.05f;
                Player.statDefense += 5;
                Player.kbBuff = true;
                if (titanBoost > 0)
                {
                    Player.statDefense += 20;
                    Player.endurance += 0.05f;
                    titanBoost--;
                }
            }
            else
            {
                titanBoost = 0;
            }

            if (yPower)
            {
                Player.endurance += 0.04f;
                Player.statDefense += 10;
                Player.pickSpeed -= 0.1f;
                Player.GetDamage<GenericDamageClass>() += 0.05f;
                Player.GetCritChance<GenericDamageClass>() += 2;
                Player.GetKnockback<SummonDamageClass>() += 1f;
                Player.moveSpeed += 0.075f;
            }

            
            if (animusBoost > 1f)
            {
                if (Player.ActiveItem().type != ModContent.ItemType<Animus>())
                    animusBoost = 1f;
            }

            if (yharimOfPerunBuff)
            {
                Player.GetAttackSpeed<MeleeDamageClass>() += 0.35f;
                Player.GetAttackSpeed<RangedDamageClass>() += 0.10f;
                Player.GetAttackSpeed<MagicDamageClass>() += 0.15f;
                Player.manaCost *= 0.35f;
                Player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += 3.5f;
            }
            
           if(bloodPactBoost)
           {
                calPlayer.healingPotionMultiplier += 0.5f;
                Player.GetDamage<GenericDamageClass>() += 0.05f;
                Player.statDefense += 20;
                Player.endurance += 0.1f;
                Player.longInvince = true;
                Player.crimsonRegen = true;
            }
        }
        #region AccessoriesStats
        private void AccessoriesStatsFunc()
        {
            CalamityPlayer calPlayer = Player.Calamity();
            var usPlayer = Player.CalamityInheritance();
            if (YharimsInsignia)
            {
                Player.GetDamage<MeleeDamageClass>() += 0.15f;
                if (Player.statLife <= (int)(Player.statLifeMax2 * 0.5))
                    Player.GetDamage<GenericDamageClass>() += 0.1f;
            }

            if (darkSunRingold)
            {
                Player.lifeRegen += 2;
                Player.maxMinions += 2;
                Player.GetDamage<GenericDamageClass>() += 0.12f;
                Player.GetKnockback<SummonDamageClass>() += 1.2f;
                Player.GetAttackSpeed<MeleeDamageClass>() += 0.12f;
                Player.pickSpeed -= 0.12f;
                // if (Main.eclipse || !Main.dayTime)
                //     Player.statDefense += Main.eclipse ? 30 : 20;
                // if (Main.eclipse || Main.dayTime)
                //     Player.lifeRegen += 6;
                if(Main.eclipse || !Main.dayTime)
                    Player.statDefense += darkSunRingNightDefense;
            }
            
            if (badgeofBravery) //如果启用
            {
                if(calPlayer.tarraMelee) //金源套不再能吃到勇气勋章的效果
                {
                    if(calPlayer.auricSet)
                    {
                        return;
                    }
                    Player.GetCritChance<MeleeDamageClass>() += 10;
                    Player.GetDamage<MeleeDamageClass>() += 0.10f;
                    Player.GetArmorPenetration<MeleeDamageClass>() += 15; 
                }
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
            if (usPlayer.ElementalQuiver)
                Player.magicQuiver = true;
            
            if(ancientReaperToothNeclace)
            {
                Player.GetArmorPenetration<GenericDamageClass>() += 250;
                Player.GetDamage<GenericDamageClass>() += 0.50f;
                Player.GetCritChance<GenericDamageClass>() += 50;
                Player.endurance *= 0.1f;
                Player.statDefense /= 10;
            }
            if(ancientCoreofTheBloodGod)
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
                Player.statLifeMax2 += (int)(Player.statLifeMax * 0.1f);
                Player.endurance += 0.05f;
                Player.GetDamage<GenericDamageClass>() += 0.5f;
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

            if(bloodflareCoreLegacy)
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
            if(hotEStats)
            {
                //Scarlet：改了。
                //在召唤物开着的时候这buff怎么可能会给这么多
                //而且尤其是元素之心的基础伤害是150的情况下？
                Player.statLifeMax2 += 15;
                Player.statManaMax2 += 15;
                Player.lifeRegen += 2;
                Player.moveSpeed += 0.05f;
                Player.endurance += 0.05f;
                Player.GetDamage<GenericDamageClass>() += 0.05f;
                Player.GetCritChance<GenericDamageClass>() += 5;
                Player.jumpSpeedBoost += 0.5f;
                Player.manaCost *=0.95f;
                if(buffEStats) //关闭元素之心的召唤物的情况下
                {
                    Player.statLifeMax2 += 25;  //40(15+25)HP
                    Player.statManaMax2 += 25;  //40(15+25)魔力
                    Player.lifeRegen += 8;      //5(1+4)HP/s
                    Player.moveSpeed += 0.05f;   //10(5+5)%移速
                    Player.endurance += 0.05f;  //10(5+5)%免伤
                    Player.GetDamage<GenericDamageClass>() += 0.05f; //10(5+5)%伤害
                    Player.GetCritChance<GenericDamageClass>() += 5; //10(5+5)%暴击
                    Player.jumpSpeedBoost += 1.0f;  //150(50+100)%跳跃速度
                    Player.manaCost *= 0.90f;       //10(5%→10%)%不耗魔
                    //由于返回值的原因导致Buff数值反而不能乱写。
                    //所以现在这些个的buff值都是5的系数了。
                }
            }
        }
        private void NanoTechUI()
        {
            if(nanotechold)
            {
                CalamityPlayer modPlayer = Player.Calamity();
                Player.AddCooldown(NanotechUI.ID, NanotechOld.nanotechDMGStack);
                
                if (nanoTechStackDurability >= 0 && nanoTechStackDurability < 150)
                {
                    //储存了攻击的积攒数量。
                    nanoTechStackDurability = raiderStack;

                    if (modPlayer.cooldowns.TryGetValue(NanotechUI.ID, out var nanoDurability))
                        nanoDurability.timeLeft = nanoTechStackDurability;
                }
                
            }
        }

        #endregion
        #region Energy Shields
        private void CIEnergyShields()
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
                    int pointsActuallyRecharged = (int)MathF.Floor(CIspongeShieldPartialRechargeProgress);

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
        #endregion
        #region ArmorSetBonusEffect
        public void ArmorSetBonusEffects()
        {
            if (reaverMagePower)
            {
                Player.manaCost *= 0.80f;
                Player.GetDamage<MagicDamageClass>() += 0.1f;
            }
            /*
            * 原动不封地把战士永恒套提供的“增加10%伤害”并不能契合当前版本的强度，因此此处直接进行了比较超量的数值加强
            * 但永恒套的怒气Buff本身只能通过受击获得，考虑到其触发条件我并不特别认为这会导致数值能多爆破（吧）
            * 速览: 永恒套的怒气buff现在触发不再有任何条件，但提供10点防御力与10%近战攻速与伤害，不提供暴击概率
            */
            if (reaverMeleeRage)
            {
                Player.GetDamage<MeleeDamageClass>() += 0.10f;
                Player.GetAttackSpeed<MeleeDamageClass>() += 0.10f;
                Player.statDefense += 10;
            }

        }
        #endregion
        #region Misc Effects
        public void MiscEffects()
        {
            CalamityInheritancePlayer usPlayer = Player.CalamityInheritance();
            CalamityPlayer calPlayer = Player.Calamity();
            Player player = Main.player[Main.myPlayer];
            Item item = player.HeldItem;

            #region Lore
            if (usPlayer.kingSlimeLore)
            {
                Player.moveSpeed += 0.05f;
                Player.jumpSpeedBoost += Player.autoJump ? 0f : 0.1f;
                Player.statDefense -= 3;
            }

            if (usPlayer.desertScourgeLore)
            {
                if (Player.ZoneDesert || Player.Calamity().ZoneSunkenSea)
                {
                    Player.statDefense += 5;
                    Player.GetDamage<GenericDamageClass>() -= 0.025f;
                }
            }

            if (usPlayer.crabulonLore)
            {
                if (Player.ZoneGlowshroom || Player.ZoneDirtLayerHeight || Player.ZoneRockLayerHeight)
                {
                    if (Main.myPlayer == Player.whoAmI)
                        Player.AddBuff(ModContent.BuffType<Mushy>(), 2);

                    Player.moveSpeed -= 0.1f;
                }
            }

            if (usPlayer.eaterOfWorldsLore)
            {
                int damage = (int)(15 * Player.GetBestClassDamage().ApplyTo(1));
                damage = Player.ApplyArmorAccDamageBonusesTo(damage);
                float knockBack = 1f;

                if (Main.rand.NextBool(15))
                {
                    int num = 0;

                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == Player.whoAmI && Main.projectile[i].type == ModContent.ProjectileType<TheDeadlyMicrobeProjectile>())
                            num++;
                    }

                    if (Main.rand.Next(15) >= num && num < 6)
                    {
                        int num2 = 50;
                        int num3 = 24;
                        int num4 = 90;

                        for (int j = 0; j < num2; j++)
                        {
                            int num5 = Main.rand.Next(200 - j * 2, 400 + j * 2);
                            Vector2 center = Player.Center;
                            center.X += Main.rand.NextFloat(-num5, num5 + 1);
                            center.Y += Main.rand.NextFloat(-num5, num5 + 1);

                            if (!Collision.SolidCollision(center, num3, num3) && !Collision.WetCollision(center, num3, num3))
                            {
                                center.X += num3 / 2;
                                center.Y += num3 / 2;

                                if (Collision.CanHit(Player.Center, 1, 1, center, 1, 1) || Collision.CanHit(new Vector2(Player.Center.X, Player.position.Y - 50f), 1, 1, center, 1, 1))
                                {
                                    int num6 = (int)center.X / 16;
                                    int num7 = (int)center.Y / 16;
                                    bool flag = false;

                                    if (Main.rand.NextBool(3) && Main.tile[num6, num7] != null && Main.tile[num6, num7].WallType > 0)
                                        flag = true;
                                    else
                                    {
                                        center.X -= num4 / 2;
                                        center.Y -= num4 / 2;

                                        if (Collision.SolidCollision(center, num4, num4))
                                        {
                                            center.X += num4 / 2;
                                            center.Y += num4 / 2;
                                            flag = true;
                                        }
                                    }

                                    if (flag)
                                    {
                                        for (int k = 0; k < Main.maxProjectiles; k++)
                                        {
                                            if (Main.projectile[k].active && Main.projectile[k].owner == Player.whoAmI && Main.projectile[k].type == ModContent.ProjectileType<TheDeadlyMicrobeProjectile>() && (center - Main.projectile[k].Center).Length() < 48f)
                                            {
                                                flag = false;
                                                break;
                                            }
                                        }

                                        if (flag && Main.myPlayer == Player.whoAmI)
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
            if (usPlayer.skeletronLore)
            {
                Player.GetDamage<GenericDamageClass>() += 0.1f;
                Player.GetCritChance<GenericDamageClass>() += 5;
                Player.statLifeMax2 = (int)(Player.statLifeMax2 * 0.90);
            }

            if (usPlayer.destroyerLore)
            {
                Player.pickSpeed -= 0.05f;
            }

            if (usPlayer.aquaticScourgeLore)
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

            if (usPlayer.skeletronPrimeLore)
            {
                Player.GetArmorPenetration(DamageClass.Generic) += 10;
            }

            if (usPlayer.leviathanAndSirenLore)
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
                        int num65 = 30;
                        int num66 = (int)Player.Center.X / 16;
                        int num67 = (int)Player.Center.Y / 16;

                        for (int num68 = num66 - num65; num68 <= num66 + num65; num68++)
                        {
                            for (int num69 = num67 - num65; num69 <= num67 + num65; num69++)
                            {
                                if (Main.rand.NextBool(4))
                                {
                                    Vector2 vector = new Vector2(num66 - num68, num67 - num69);
                                    if (vector.Length() < num65 && num68 > 0 && num68 < Main.maxTilesX - 1 && num69 > 0 && num69 < Main.maxTilesY - 1 && Main.tile[num68, num69] != null && Main.tile[num68, num69].HasTile)
                                    {
                                        bool flag7 = false;
                                        if (Main.tile[num68, num69].TileType == 185 && Main.tile[num68, num69].TileFrameY == 18)
                                        {
                                            if (Main.tile[num68, num69].TileFrameX >= 576 && Main.tile[num68, num69].TileFrameX <= 882)
                                                flag7 = true;
                                        }
                                        else if (Main.tile[num68, num69].TileType == 186 && Main.tile[num68, num69].TileFrameX >= 864 && Main.tile[num68, num69].TileFrameX <= 1170)
                                            flag7 = true;

                                        if (flag7 || Main.tileSpelunker[Main.tile[num68, num69].TileType] || (Main.tileAlch[Main.tile[num68, num69].TileType] && Main.tile[num68, num69].TileType != 82))
                                        {
                                            int num70 = Dust.NewDust(new Vector2(num68 * 16, num69 * 16), 16, 16, DustID.TreasureSparkle, 0f, 0f, 150, default, 0.3f);
                                            Main.dust[num70].fadeIn = 0.75f;
                                            Main.dust[num70].velocity *= 0.1f;
                                            Main.dust[num70].noLight = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (Player.ZoneSkyHeight)
            {
                if (usPlayer.astrumDeusLore)
                    Player.moveSpeed += 0.2f;
                if (usPlayer.astrumAureusLore)
                    Player.jumpSpeedBoost += 0.5f;
            }

            if (usPlayer.golemLore)
            {
                if (Math.Abs(Player.velocity.X) < 0.05f && Math.Abs(Player.velocity.Y) < 0.05f && Player.itemAnimation == 0)
                    Player.statDefense += 30;
            }

            if (usPlayer.dukeFishronLore)
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
                    usPlayer.Player.GetCritChance<GenericDamageClass>() -= 2;
                    Player.moveSpeed -= 0.04f;
                }
            }

            if (usPlayer.lunaticCultistLore)
            {
                Player.blind = true;
                Player.endurance += 0.04f;
                Player.statDefense += 4;
                Player.GetDamage(DamageClass.Generic) += 0.04f;
                Player.GetCritChance<GenericDamageClass>() += 4;
                Player.GetKnockback(DamageClass.Summon).Base += 0.5f;
                Player.moveSpeed += 0.1f;
            }

            if (usPlayer.moonLordLore)
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

            if (usPlayer.twinsLore)
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

            if (usPlayer.wallOfFleshLore)
            {
                Player.GetDamage<GenericDamageClass>() -= 0.03f;
            }

            if (usPlayer.planteraLore)
            {
                if (Player.statLife >= (int)(Player.statLifeMax2 * 0.5))
                {
                    Player.GetDamage<GenericDamageClass>() -= 0.05f;
                    Player.statDefense += 10;
                }
                if (Player.statLife <= (int)(Player.statLifeMax2 * 0.5))
                {
                    Player.GetDamage<GenericDamageClass>() += 0.1f;
                    Player.statDefense -= 10;
                }
            }

            if (usPlayer.polterghastLore)
            {
                Player.GetDamage<GenericDamageClass>() -= 0.1f;
            }
            // Brimstone Elemental lore inferno potion boost
            if ((usPlayer.brimstoneElementalLore || calPlayer.ataxiaBlaze) && Player.inferno)
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

            if (usPlayer.calamitasCloneLore)
            {
                Player.maxMinions += 2;
                Player.statLifeMax2 = (int)(Player.statLifeMax2 * 0.75);
            }
            if (usPlayer.plaguebringerGoliathLore)
            {
                if (Player.wingTimeMax > 0)
                    Player.wingTimeMax = (int)(Player.wingTimeMax * 1.25);
            }

            if (usPlayer.boomerDukeLore)
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

            if (usPlayer.ravagerLore)
            {
                calPlayer.weakPetrification = true;
                if (Player.wingTimeMax > 0)
                    Player.wingTimeMax = (int)(Player.wingTimeMax * 0.5);
                Player.GetDamage<GenericDamageClass>() += 0.5f;
                Player.ClearBuff(BuffID.Featherfall);
            }

            if (usPlayer.providenceLore)
            {
                Player.statLifeMax2 = (int)(Player.statLifeMax2 * 0.8);
                Player.GetDamage<GenericDamageClass>() += 0.25f;
            }

            if (usPlayer.DoGLore)
            {
                Player.GetDamage<TrueMeleeDamageClass>() += 0.25f;
            }
            if (usPlayer.yharonLore)
            {
                calPlayer.infiniteFlight = true;
                Player.GetDamage<GenericDamageClass>() -= 0.25f;
            }

            #endregion
            #region ArmorSet
            if (usPlayer.invincible)
            {
                foreach (int debuff in CalamityLists.debuffList)
                    Player.buffImmune[debuff] = true;
            }

            if (silvaMageCooldownold > 0)
                silvaMageCooldownold--;

            if (silvaStunCooldownold > 0)
                silvaStunCooldownold--;

            if (reaverBlastCooldown > 0)
                reaverBlastCooldown--; //战士永恒套cd

            if (reaverBurstCooldown > 0)
                reaverBurstCooldown--; //法师永恒套CD

            if (StepToolShadowChairSmallCD > 0)
                StepToolShadowChairSmallCD--;

            if (StepToolShadowChairSmallFireCD > 0)
                StepToolShadowChairSmallFireCD--; 
            if (auricYharimHealCooldown > 0)
                auricYharimHealCooldown--;
            
            if (yharimOfPerunStrikesCooldown > 0)
                yharimOfPerunStrikesCooldown--;

            if (statisTimerOld > 0 && CIDashDelay >= 0)
                statisTimerOld = 0;//斯塔提斯CD

            if (usPlayer.silvaMageold && Player.HasCooldown(SilvaRevive.ID))
            {
                Player.GetDamage<MagicDamageClass>() += 0.60f;
            }

            if (usPlayer.silvaMelee && Player.HasCooldown(SilvaRevive.ID))
            {
                calPlayer.contactDamageReduction += 0.2f;
            }

            if (silvaMelee)
            {
                double multiplier = Player.statLife / (double)Player.statLifeMax2;
                Player.GetDamage<MeleeDamageClass>() += (float)(multiplier * 0.2);

                if (calPlayer.auricSet && silvaMelee)
                {
                    double multiplier1 = Player.statLife / (double)Player.statLifeMax2;
                    Player.GetDamage<MeleeDamageClass>() += (float)(multiplier1 * 0.2);
                }
            }

            if (usPlayer.silvaRanged && Player.HasCooldown(SilvaRevive.ID))
            {
                Player.GetDamage<RangedDamageClass>() += 0.40f;
            }

            if (usPlayer.silvaSummonEx && Player.HasCooldown(SilvaRevive.ID))
            {
                Player.GetCritChance<SummonDamageClass>() += 10;
                Player.maxMinions += 2;
            }

            if (usPlayer.silvaRogue && Player.HasCooldown(SilvaRevive.ID))
            {
                Player.GetDamage<RogueDamageClass>() += 0.40f;
            }

            if (usPlayer.AuricDebuffImmune)
            {
                foreach (int debuff in CalamityLists.debuffList)
                    Player.buffImmune[debuff] = true;
            }

            // Silva invincibility effects
            if (auricsilvaCountdown > 0 && aurichasSilvaEffect)
            {
                if(auricsilvaset && !silvaRebornMark)
                {
                    foreach (int debuff in CalamityLists.debuffList)
                        Player.buffImmune[debuff] = true;

                    auricsilvaCountdown -= 1;
                    if (auricsilvaCountdown <= 0)
                    {
                        SoundEngine.PlaySound(SilvaHeadSummon.DispelSound, Player.Center);
                        Player.AddCooldown(SilvaRevive.ID, CalamityUtils.SecondsToFrames(3 * 60));
                    }

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
                    if (!Player.HasCooldown(SilvaRevive.ID) && aurichasSilvaEffect && auricsilvaCountdown <= 0)
                    {
                        auricsilvaCountdown = 600;
                        aurichasSilvaEffect = false;
                    }
                }
            }

            if (CIsilvaCountdown > 0 && aurichasSilvaEffect)
            {
                if (auricsilvaset && silvaRebornMark)
                {
                    foreach (int debuff in CalamityLists.debuffList)
                        Player.buffImmune[debuff] = true;

                    CIsilvaCountdown -= 1;
                    if (CIsilvaCountdown <= 0)
                    {
                        SoundEngine.PlaySound(SilvaHeadSummon.DispelSound, Player.Center);
                        Player.AddCooldown(SilvaRevive.ID, CalamityUtils.SecondsToFrames(3 * 60));
                    }

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
                    if (!Player.HasCooldown(SilvaRevive.ID) && aurichasSilvaEffect && CIsilvaCountdown <= 0)
                    {
                        CIsilvaCountdown = 900;
                        aurichasSilvaEffect = false;
                    }
                }
            }
            #endregion
            if (Player.miscCounter % 150 == 0)
            {
                canFireReaverRangedRocket = true;
            }
            //纳米技术
            if (nanotechold)
            {
                float damageMult =  0.15f;
                Player.GetDamage<GenericDamageClass>() *= 1 + raiderStack / 150f * damageMult;
            }
            if(auricYharimSet)
            {
                Player.statLifeMax2 += (int)(Player.statLifeMax * 1.05f);
                calPlayer.healingPotionMultiplier += 0.70f; //将血药恢复提高至70%，这样能让300的大血药在不依靠血神核心的情况下能直接恢复500以上的血量
                Player.noKnockback = true;
                Player.lifeRegen += 60;
                Player.shinyStone = true;
                Player.lifeRegenTime = 1800f;
                if(calPlayer.purity == true) //与灾厄的纯净饰品进行联动
                {
                    Player.lifeRegenTime = 1200f; //之前是在一半的基础上再减了一半然后发现我受击也能回血了
                }

                if(Player.statLife <= Player.statLifeMax2 * 0.5f)
                {
                    Player.lifeRegen += 120;
                    Player.statDefense += 60;
                }
            }
            if(ancientBloodFact)
            {
                Player.statLifeMax2 +=(int)(player.statLifeMax * 2);
            }
            if(backFireDebuff)
            {
                //获得淬火Debuff后，玩家的伤害将被0.5倍率乘算
                Player.GetDamage<GenericDamageClass>() *= 0.5f;
                //生命将会高速流失。直到低于生命值上线的1/3为止
                if(Player.statLife > Player.statLifeMax2/3)
                   Player.statLife -= 5;
                //直接减少玩家20%的免伤，也就是可以让玩家免伤变成负数(有可能)
                Player.endurance -= 0.2f;
                //玩家的防御力取50%
                Player.statDefense *= 0.5f;
                //玩家的翅膀飞行时间将会被设置为0
            }
        }

        #endregion

        #region Standing Still Effects
        private void StandingStillEffects()
        {
            CalamityInheritancePlayer usPlayer = Player.CalamityInheritance();
            CalamityPlayer calPlayer = Player.Calamity();

            // Auric bonus
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
        #endregion

        #region Elysian Aegis Effects
        public void ElysianAegisEffects()
        {
            if (elysianAegis)
            {
                bool spawnDust = false;

                // Activate buff
                if (elysianGuard)
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

                    if (Player.mount.Active)
                    {
                        elysianGuard = false;
                    }
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
                elysianGuard = false;
        }
        #endregion

        public void AncientXerocEffect()
        {
            CalamityPlayer calPlayer= Player.Calamity();
            if(ancientXerocSet)
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
                    ancientXerocWrath = true;
                }
            }
        }
    }
}
