using System;
using CalamityInheritance.Buffs.Melee;
using CalamityInheritance.Buffs.Statbuffs;
using CalamityInheritance.Buffs.StatDebuffs;
using CalamityInheritance.CICooldowns;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Accessories.Rogue;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityInheritance.NPCs.Boss.SCAL.Proj;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using CalamityInheritance.World;
using CalamityMod;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.CalPlayer;
using CalamityMod.Cooldowns;
using CalamityMod.Dusts;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Silva;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    //所有需要重载的函数现在都排在最前面
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            CalamityPlayer calPlayer = Player.Calamity();
            CalamityInheritancePlayer cIPlayer = Player.CIMod();
            // 末日模式
            if (CIWorld.Armageddon || SCalLore || PanelsSCalLore)
                KillPlayer();

            // Handles energy shields and Boss Rush, in that order
            modifiers.ModifyHurtInfo += ModifyHurtInfo_Calamity;

            if (CIWorld.IronHeart)
                ModeHit(ref modifiers);

            #region Custom Hurt Sounds
            if (calPlayer.hurtSoundTimer == 0)
            {
                if (CIsponge && CISpongeShieldDurability > 0)
                {
                    modifiers.DisableSound();
                    SoundEngine.PlaySound(TheSponge.ShieldHurtSound, Player.Center);
                    calPlayer.hurtSoundTimer = 20;
                }
            }
            #endregion
            #region Player Incoming Damage Multiplier (Increases)
            double damageMult = 1D;
            CalamityInheritancePlayer modPlayer1 = Player.CIMod();

            if (modPlayer1.LoreDevourer || PanelsLoreDevourer)
                damageMult += 0.05;

            if(AncientBloodPact && Main.rand.NextBool(4))
            {
                Player.AddBuff(ModContent.BuffType<BloodyBoost>(), 600);
                damageMult += 1.25;
            }
            // 恶意模式额外受到25%伤害
            if (CIWorld.Malice)
                damageMult += 0.25;
            if (CIWorld.Defiled)
            {
                if(Main.rand.NextBool(4))
                    damageMult += 0.5;
            }
            modifiers.SourceDamage *= (float)damageMult;
            #endregion
            #region 免伤
            double damageReduce = 1D;
            var usPlayer = Player.CIMod();
            if (usPlayer.SolarShieldEndurence)
            {
                //我需要这种方法玩家来复原日耀免伤，这个属于防前计算，而原版日耀是防后计算，因此这里实际先取15%而不取原有的20%
                damageReduce -= 0.20; //日耀盾"防前"15%免伤
            }
            if (SForestBuff)
            {
                damageReduce -= 0.15;    
            }
            //大师石巨人在场，且玩家佩戴玉金喷射器+永恒套的搭配时，获得20%乘算伤害减免
            //我草泥马的石巨人
            if (Main.masterMode && CIFunction.IsThereNpcNearby(NPCID.Golem, Player, 1600f) && Main.LocalPlayer.ZoneLihzhardTemple && FuckYouGolem)
            {
                damageReduce -= 0.20;
            }
            //寒冰神性T2: 10%独立免伤
            if (ColdDivityTier2 && IsColdDivityActiving)
            {
                damageReduce -= 0.10;
            }
            modifiers.FinalDamage *= (float)damageReduce;
            #endregion

        }
        #region 玩家处死
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (CIWorld.IronHeart)
            {
                KillPlayer();
                return true;
            }

            CalamityPlayer calPlayer = Player.Calamity();
            if (GodSlayerReborn && !Player.HasCooldown(GodSlayerCooldown.ID))
            {
                SoundEngine.PlaySound(CISoundID.SoundRainbowGun, Player.Center);
                for (int j = 0; j < 50; j++)
                {
                    int nebulousReviveDust = Dust.NewDust(Player.position, Player.width, Player.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 2f);
                    Dust dust = Main.dust[nebulousReviveDust];
                    dust.position.X += Main.rand.Next(-20, 21);
                    dust.position.Y += Main.rand.Next(-20, 21);
                    dust.velocity *= 0.9f;
                    dust.scale *= 1f + Main.rand.Next(40) * 0.01f;
                    // Change this accordingly if we have a proper equipped sprite.
                    dust.shader = GameShaders.Armor.GetSecondaryShader(Player.cBody, Player);
                    if (Main.rand.NextBool())
                        dust.scale *= 1f + Main.rand.Next(40) * 0.01f;
                }

                Player.statLife = +100;

                if (BuffStatsDraconicSurge)
                {
                    Player.Heal(Player.statLifeMax2);
                    //给耐药性
                    Player.AddCooldown(DraconicElixirCooldown.ID, CalamityUtils.SecondsToFrames(30));
                    //直接给30秒。
                    Player.AddBuff(BuffID.PotionSickness, 1800);
                }
                Player.AddCooldown(GodSlayerCooldown.ID, CalamityUtils.SecondsToFrames(45));
                return false;
            }
            //先判是否为林海套
            if (SilvaFakeDeath && DoSilvaCountDown > 0)
            {
                //赋予一次。
                if (DoSilvaCountDown == SilvaRebornDura)
                {
                    Player.AddBuff(ModContent.BuffType<SilvaRevival>(), SilvaRebornDura);
                    //我们只发送一次音效。
                    SoundEngine.PlaySound(SilvaHeadSummon.ActivationSound, Player.Center);
                }

                if (!IsUsedSilvaReborn)
                    DoSilvaHeal(calPlayer.silvaWings);

                DoSilvaHeal(calPlayer.silvaWings);
                IsUsedSilvaReborn = true;
                //如果都没有，或者已经执行过一次，我们才会执行锁1血的防处死
                if (Player.statLife < 1)
                    Player.statLife = 1;
                //血神圣杯的特判
                if (calPlayer.chaliceOfTheBloodGod)
                {
                    calPlayer.chaliceBleedoutBuffer = 0D;
                    calPlayer.chaliceDamagePointPartialProgress = 0D;
                }
                return false;
            }
            //金源套，附带弑神复活的特判, 从上方复制了一遍。
            if (AuricSilvaFakeDeath && DoAuricSilvaCountdown > 0 && Player.HasCooldown(GodSlayerCooldown.ID))
            {
                //赋予一次。
                if (DoAuricSilvaCountdown == AuricSilvaRebornDura)
                {
                    Player.AddBuff(ModContent.BuffType<SilvaRevival>(), AuricSilvaRebornDura);
                    //我们只发送一次音效
                    SoundEngine.PlaySound(SilvaHeadSummon.ActivationSound, Player.Center);
                }
                //三个检测，优先判定龙魂秘药(恢复至最大生命值)
                if (!IsUsedSilvaReborn)
                    DoSilvaHeal(calPlayer.silvaWings);
                    
                IsUsedSilvaReborn = true;               //如果都没有，或者已经执行过一次，我们才会执行锁1血的防处死
                if (Player.statLife < 1)
                    Player.statLife = 1;
                //血神圣杯的特判
                if (calPlayer.chaliceOfTheBloodGod)
                {
                    calPlayer.chaliceBleedoutBuffer = 0D;
                    calPlayer.chaliceDamagePointPartialProgress = 0D;
                }
                //防处死
                return false;
            }

            //目前用于龙魂与原灾金源和复活效果的互动
            if (calPlayer.silvaSet && calPlayer.silvaCountdown > 0)
            {
                DoSilvaHeal(calPlayer.silvaWings);
                return false;
            }
            // 終灾期间玩家死亡计数
            if (NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()))
            {
                if (LegacyScal_PlayerDeathCount < 60)
                    LegacyScal_PlayerDeathCount++;
            }

            return true;
        }
        public void DoSilvaHeal(bool isUsingWings)
        {
            //有龙魂秘药的优先判龙魂秘药。
            if (BuffStatsDraconicSurge)
            {
                Player.Heal(Player.statLifeMax2);
                //给耐药性
                Player.AddCooldown(DraconicElixirCooldown.ID, CalamityUtils.SecondsToFrames(30));
                //直接给30秒。
                Player.AddBuff(BuffID.PotionSickness, 1800);
                return;
            }
            //仅在不佩戴龙魂秘药的时候判定是否佩戴翅膀
            if (!BuffStatsDraconicSurge && isUsingWings)
            {
                Player.Heal(Player.statLifeMax2 / 2);
                return;
            }

        }
        #endregion
        #region 修改来犯的NPC体术
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            CalamityPlayer calPlayer = Player.Calamity();
            //星流短剑的Lore效果。
            if (DNAImmnue > 0)
            {
                //直接减少30%伤害
                modifiers.SourceDamage *= 0.8f;
                //不要执行下方所有的计算。
                return;
            }
            if (Triumph)
            {
                if (!Main.zenithWorld)
                calPlayer.contactDamageReduction += 0.15 * (1D - (npc.life / (double)npc.lifeMax));
                if (Main.zenithWorld)
                {
                    calPlayer.contactDamageReduction += 0.35;
                    Player.thorns += 200;
                }
            }
            //庇护刃T2: 尝试使你无视防御损伤
            if (DefenderPower)
            {
                npc.Calamity().canBreakPlayerDefense = false;;
            }
            //在血神图腾被移除游戏的时候，这里会改成血神图腾的样式
            if (CoreOfTheBloodGod)
            {
                //这个用于做准备，暂时无作用。
                string cdID = CoreOfTheBloodGod ? CotbgTotem.ID : Totem.ID;
                    //给效果
                calPlayer.contactDamageReduction += 0.15;
                if (CotbgCounter <= 0)
                {
                    //给CD
                    Player.AddCooldown(cdID, CIFunction.SecondsToFrames(20));
                    //给效果
                    calPlayer.contactDamageReduction += 0.5;
                    //共用一个counter
                    CotbgCounter = CIFunction.SecondsToFrames(20);
                    return;
                }
            }
            if (PBGPower && Player.ActiveItem().type == ModContent.ItemType<PBGLegendary>())
                calPlayer.contactDamageReduction += 0.25;
            if (FuckYouBees)
            {
                if (CalamityInheritanceLists.beeEnemyList.Contains(npc.type))
                    calPlayer.contactDamageReduction += 0.25;
            }

        }
        #endregion
        #region 闪避
        public override bool FreeDodge(Player.HurtInfo info)
        {
            Player player = Main.player[Main.myPlayer];
            CalamityPlayer modPlayer1 = player.Calamity();

            // 末日模式禁用闪避
            if (CIWorld.Armageddon)
                return false;

            //日食魔镜的闪避优于所有闪避之前执行
            if (CheckEMirror())
                return true;

            // 现在免疫触发后，会让免疫的阈值降低，随后会逐渐恢复
            if (GodSlayerDMGprotect && info.Damage <= GodSlayerDMGprotectMax && !modPlayer1.chaliceOfTheBloodGod)
            {
                GodSlayerDMGprotectMax = 20;
                Player.immune = true;
                Player.immuneTime = 15;
                return true;
            }
            if((YharimAuricSet || AncientGodSlayerSet) && yharimArmorinvincibility > 0)
                return true;

            return base.FreeDodge(info);
        }

        public bool CheckEMirror()
        {
            //如果玩家没有触i发必闪, 返回
            if (!Player.HasCooldown(GlobalDodge.ID))
                return false;
            //没有佩戴日食魔镜返回回去，不要浪费任何时间
            if (!EMirror)
                return false;
            //4/5的概率没有触发闪避，直接返回
            if (!Main.rand.NextBool(5))
                return false;
            
            //无敌帧
            int eclipseMirrorDodgeIFrames = Player.ComputeDodgeIFrames();
            Player.GiveUniversalIFrames(eclipseMirrorDodgeIFrames, true);

            //恢复所有潜伏值
            Player.Calamity().rogueStealth = Player.Calamity().rogueStealthMax;
            SoundEngine.PlaySound(SoundID.Item68, Player.Center);

            //计算闪避时提供的射弹伤。
            var source = Player.GetSource_Accessory(FindAccessory(ModContent.ItemType<EclispeMirrorLegacy>()));
            int damage = (int)Player.GetTotalDamage<RogueDamageClass>().ApplyTo(5000);
            damage = Player.ApplyArmorAccDamageBonusesTo(damage);

            int eclipse = Projectile.NewProjectile(source, Player.Center, Vector2.Zero, ModContent.ProjectileType<EclipseMirrorBurst>(), damage, 0, Player.whoAmI);
            if (eclipse.WithinBounds(Main.maxProjectiles))
                Main.projectile[eclipse].DamageType = DamageClass.Generic; 

            //允许其触发闪避
            return true;
        }
        #endregion
        #region 修改来犯的射弹
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            CalamityPlayer calPlayer = Player.Calamity();
            if (DNAImmnue > 0)
            {
                //直接减少20%伤害
                modifiers.SourceDamage *= 0.8f;
                //不要执行下方所有的计算。
                return;
            }
            if (PBGPower && Player.ActiveItem().type == ModContent.ItemType<PBGLegendary>())
                calPlayer.projectileDamageReduction += 0.25;
            //血神核心专门提供红月免伤, 要注意这个是最优先被计算的。 
            if (FUCKYOUREDMOON && proj.type == ModContent.ProjectileType<BrimstoneMonsterLegacy>())
            {
                if (CotbgCounter <= 0)
                {
                    Player.AddCooldown(CotbgTotem.ID, CalamityUtils.SecondsToFrames(20));
                    //防前免伤，直接砍50%
                    calPlayer.projectileDamageReduction += 0.5;
                    //共享20秒的CD
                    CotbgCounter = CalamityUtils.SecondsToFrames(20);
                    //直接返回，不执行下方所有的杂糅计算
                    return;
                }
                //常驻0.15f
                calPlayer.projectileDamageReduction += 0.15;
            }
            // TODO -- Evolution dodge isn't actually a dodge and you'll still get hit for 1.
            // This should probably be changed so that when the evolution reflects it gives you 1 frame of guaranteed free dodging everything.
            if (CalamityLists.projectileDestroyExceptionList.TrueForAll(x => proj.type != x) && proj.active && !proj.friendly && proj.hostile && proj.damage > 0)
            {
                double dodgeDamageGateValuePercent = 0.05;
                int dodgeDamageGateValue = (int)Math.Round(Player.statLifeMax2 * dodgeDamageGateValuePercent);

                // Reflects count as dodges. They share the timer and can be disabled by Armageddon right click.
                if (!calPlayer.disableAllDodges && !Player.HasCooldown(GlobalDodge.ID) && proj.damage >= dodgeDamageGateValue)
                {
                    double maxCooldownDurationDamagePercent = 0.5;
                    int maxCooldownDurationDamageValue = (int)Math.Round(Player.statLifeMax2 * (maxCooldownDurationDamagePercent - dodgeDamageGateValuePercent));

                    // Just in case...
                    if (maxCooldownDurationDamageValue <= 0)
                        maxCooldownDurationDamageValue = 1;

                    float cooldownDurationScalar = MathHelper.Clamp((proj.damage - dodgeDamageGateValue) / (float)maxCooldownDurationDamageValue, 0f, 1f);

                    // The Evolution
                    if (projRef)
                    {
                        proj.hostile = false;
                        proj.friendly = true;
                        proj.velocity *= -2f;
                        proj.extraUpdates += 1;
                        proj.penetrate = 1;

                        // 17APR2024: Ozzatron: The Evolution is a reflect which also functions as a dodge. It uses vanilla dodge iframes and benefits from Cross Necklace.
                        int evolutionIFrames = Player.ComputeReflectIFrames();
                        Player.GiveUniversalIFrames(evolutionIFrames, true);

                        modifiers.SetMaxDamage(1);
                        calPlayer.evolutionLifeRegenCounter = 300;
                        calPlayer.projTypeJustHitBy = proj.type;

                        int cooldownDuration = (int)MathHelper.Lerp(900, 5400 , cooldownDurationScalar);
                        Player.AddCooldown(GlobalDodge.ID, cooldownDuration);

                        return;
                    }
                }
            }
            
            
            //庇护刃T2: 尝试使你无视防御损伤
            if (DefenderPower)
            {
                proj.Calamity().DealsDefenseDamage = false;;
            }
            if (FuckYouBees)
            {
                if (CalamityInheritanceLists.beeProjectileList.Contains(proj.type))
                    calPlayer.projectileDamageReduction += 0.25;
            }
        }
        #endregion
        #region 啊♂
        public override void OnHurt(Player.HurtInfo hurtInfo)
        {
            CalamityPlayer calPlayer = Player.Calamity();
            CalamityInheritancePlayer Modplayer1 = Player.CIMod();
            
            //正常受伤，日食魔镜也会提供潜伏值的恢复(恢复50%)
            if (EMirror && calPlayer.rogueStealth < calPlayer.rogueStealthMax / 2)
                calPlayer.rogueStealth += calPlayer.rogueStealthMax / 2;

            if (Player.ActiveItem().type == ModContent.ItemType<DefenseBlade>() && !DefendTier2)
                DefenseBladeTier2Task(hurtInfo);
            //这里CD只有取0的时候才会触发cd，这是为了防止再次受击的时候被重置
            if (AncientAeroSet && AeroFlightPower == 0)
            {
                //如果穿着配套的翅膀则干掉无限飞行
                if(AncientAeroWingsPower)
                {
                    AncientAeroWingsPower = false;
                    calPlayer.infiniteFlight = false;
                    //别忘了翅膀折翼
                    Player.wingTime /= 3;
                    AeroFlightPower = 600;
                }
                //否则执行简单的折翼效果.
                else
                {
                    Player.wingTime /= 3;
                    AeroFlightPower = 300;
                }
                //给予玩家羽落效果
                Player.AddBuff(BuffID.Featherfall, 300);

            }
            if (BuffPolarisBoost)
            {
                PolarisBoostCounter -= 10;
                if (PolarisBoostCounter < 0)
                    PolarisBoostCounter = 0;

                if (PolarisBoostCounter >= 20)
                {
                    PolarisPhase2= false;
                    PolarisPhase3 = true;
                }
                else if (PolarisBoostCounter >= 10)
                {
                    PolarisPhase2 = true;
                    PolarisPhase3 = false;
                }
                else
                {
                    PolarisPhase3 = false;
                    PolarisPhase2 = false;
                }
            }
            //海绵
            if (Modplayer1.CIsponge)
            {
                int healAmt = (int)(hurtInfo.Damage / 15D);
                Player.statLife += healAmt;
                Player.HealEffect(healAmt);
            }
            //再生
            if (Modplayer1.Revivify)
            {
                int healAmt = (int)(hurtInfo.Damage / 15D);
                Player.Heal(healAmt);
            }
            //阴阳石
            if (Modplayer1.TheAbsorberOld)
            {
                int healAmt = (int)(hurtInfo.Damage / 20D);
                Player.Heal(healAmt);
            }
            //神圣护符落星
            if (deificAmuletEffect)
            {
                var source = Player.GetSource_Accessory(FindAccessory(ModContent.ItemType<DeificAmulet>()));
                for (int n = 0; n < 3; n++)
                {
                    int deificStarDamage = (int)Player.GetBestClassDamage().ApplyTo(130);
                    deificStarDamage = Player.ApplyArmorAccDamageBonusesTo(deificStarDamage);

                    Projectile star = CalamityUtils.ProjectileRain(source, Player.Center, 400f, 100f, 500f, 800f, 29f, 
                    ProjectileID.StarVeilStar, deificStarDamage, 4f, Player.whoAmI);
                    
                    if (star.whoAmI.WithinBounds(Main.maxProjectiles))
                    {
                        star.DamageType = DamageClass.Generic;
                        star.usesLocalNPCImmunity = true;
                        star.localNPCHitCooldown = 5;
                    }
                }
            }
            if(AncientAstralSet)
            {
                if(calPlayer.rogueStealth < (float)(calPlayer.rogueStealthMax * 0.75)) //尝试恢复25%潜伏值
                   calPlayer.rogueStealth += (float)(calPlayer.rogueStealthMax * 0.25);
                for (int n = 0; n < 9; n++) //生成一些落星，或者说我也不知道，反正是一些落星
                {
                    int astralStarsDMG = (int)Player.GetBestClassDamage().ApplyTo(1000);
                    astralStarsDMG = Player.ApplyArmorAccDamageBonusesTo(astralStarsDMG);

                    Projectile star = CalamityUtils.ProjectileRain(Player.GetSource_FromThis(), Player.Center, 400f, 100f, 500f, 800f, 29f, 
                    ProjectileID.StarVeilStar, astralStarsDMG, 4f, Player.whoAmI);
                    
                    if (star.whoAmI.WithinBounds(Main.maxProjectiles))
                    {
                        star.DamageType = ModContent.GetInstance<RogueDamageClass>(); //:)
                        star.usesLocalNPCImmunity = true;
                        star.localNPCHitCooldown = 5;
                    }
                }
            }
            if (SCalLore || PanelsSCalLore)
            {
                int randomMessage = Main.rand.Next(1, 5);
                Color messageColor = Color.DarkRed;
                if (randomMessage == 1)
                {
                    string key1 = "Mods.CalamityInheritance.Status.SCAL1";
                    CalamityUtils.DisplayLocalizedText(key1, messageColor);
                }
                if (randomMessage == 2)
                {
                    string key2 = "Mods.CalamityInheritance.Status.SCAL2";
                    CalamityUtils.DisplayLocalizedText(key2, messageColor);
                }
                if (randomMessage == 3)
                {
                    string key3 = "Mods.CalamityInheritance.Status.SCAL3";
                    CalamityUtils.DisplayLocalizedText(key3, messageColor);
                }
                if (randomMessage == 4)
                {
                    string key4 = "Mods.CalamityInheritance.Status.SCAL4";
                    CalamityUtils.DisplayLocalizedText(key4, messageColor);
                }
                KillPlayer();
            }

            if (GodSlayerMagicSet)
            {
                if (hurtInfo.Damage > 0)
                {
                    var source = Player.GetSource_Misc("1");
                    SoundEngine.PlaySound(SoundID.Item73, Player.Center);
                    if (Player.whoAmI == Main.myPlayer)
                    {
                        Projectile.NewProjectile(source, Player.Center.X, Player.Center.Y, 0f, 0f, ModContent.ProjectileType<GodSlayerBlaze>(), 1200, 1f, Player.whoAmI, 0f, 0f);
                    }
                }
            }

            if (ReaverMeleeBlast) //受伤后提供战士永恒套怒气buff
            {
                Player.AddBuff(ModContent.BuffType<ReaverMeleeRage>(), 180);
            }

            if (AncientBloodflareSet)
            {
                if(hurtInfo.Damage > 400 && AncientAuricHealCooldown == 0)
                //旧血炎新增效果: 承受大于400点的伤害时，恢复本次承伤的1.5倍血量，取20秒内置CD
                {
                    SoundEngine.PlaySound(CISoundMenu.YharimsSelfRepair, Player.Center, null);
                    Player.Heal((int)(hurtInfo.Damage * 1.5f));
                    AncientAuricHealCooldown =  1200;
                }
            }

            if(AncientAuricSet)
            {
                //魔君套处于天顶世界下，启用高伤保护的最低生命值只需要大于2即可
                int DamageCap = Main.zenithWorld ? 2 : 300;
                if(hurtInfo.Damage> DamageCap && AncientAuricHealCooldown == 0) 
                //承受的伤害大于600点血时直接恢复承伤的2倍血量，这一效果会有10秒的内置CD
                {
                    SoundEngine.PlaySound(CISoundMenu.YharimsSelfRepair, Player.Center, null);
                    Player.Heal((int)(hurtInfo.Damage * 2f));
                    //魔君套处于天顶世界下，高伤保护的CD只有一秒
                    AncientAuricHealCooldown = Main.zenithWorld? 1 : 600;
                }
            }

            if (Player.ownedProjectileCounts[ModContent.ProjectileType<DragonBow>()] != 0)
            {
                foreach(Projectile p in Main.ActiveProjectiles)
                {
                    if (p.type == ModContent.ProjectileType<DragonBow>() && p.owner == Player.whoAmI)
                    {
                        p.Kill();
                        break;
                    }
                }
                /*玩家受击时飞行时间直接置成50f*/
                if (Player.wingTime > 50f)
                    Player.wingTime = 50f;
                Player.AddBuff(ModContent.BuffType<Backfire>(), 180); //3秒
            }
            //魔君套受击后无敌
            if (YharimAuricSet)
                yharimArmorinvincibility = 60;
        }
        #endregion
        public void ModifyHurtInfo_Calamity(ref Player.HurtInfo info)
        {
            #region shield
            // Boss Rush的伤害下限是通过一个不规范的修正器实现的
            // TODO -- 要正确实现这一点，需要重新实现所有的伤害减免（DR）和额外伤害减免（ADR）机制

            // 能量护盾是通过一个不规范的修正器实现的
            // 这是SLR屏障所做的事情；参见
            // https://github.com/ProjectStarlight/StarlightRiver/blob/master/Core/Systems/BarrierSystem/BarrierPlayer.cs
            //
            // 目前实现的能量护盾包括：
            // - Rover Drive
            // - Lunic Corps护甲套装奖励
            // - Profaned Soul神器/水晶
            // - The Sponge
            //
            // 如果护盾完全吸收了攻击，会立即给予无敌帧，并标记此次攻击为闪避。
            // 护盾按强度顺序消耗，因此较弱的护盾会先被打破。
            // 如果需要，伤害可以被多个护盾阻挡。
            bool shieldsFullyAbsorbedHit = false;
            if (CIHasAnyEnergyShield)
            {
                bool shieldsTookHit = false;
                bool anyShieldBroke = false;
                int totalDamageBlocked = 0;


                // THE SPONGE
                if (CIsponge && CISpongeShieldDurability > 0 && !shieldsFullyAbsorbedHit)
                {
                    // 检查这个护盾是否能完全吸收即将到来的攻击（或其剩余部分）。
                    bool thisShieldCanFullyAbsorb = CISpongeShieldDurability >= info.Damage;

                    // 统计这个护盾阻挡了多少伤害。
                    int spongeDamageBlocked = Math.Min(CISpongeShieldDurability, info.Damage);
                    totalDamageBlocked += spongeDamageBlocked;

                    // 因为护盾可用，所以将所有即将到来的伤害都作用于这个护盾。
                    CISpongeShieldDurability -= info.Damage;
                    shieldsTookHit = true;

                    // 打破海绵护盾的攻击会发出声音并产生轻微的屏幕震动。
                    // 如果同时有多个护盾被打破，屏幕震动会稍微强烈一些。
                    if (CISpongeShieldDurability <= 0)
                    {
                        CISpongeShieldDurability = 0;
                        SoundEngine.PlaySound(TheSponge.BreakSound, Player.Center);
                        Player.Calamity().GeneralScreenShakePower += anyShieldBroke ? 0.5f : 2f;
                        anyShieldBroke = true;
                    }

                    // 如果这个护盾有足够的耐久度来完全吸收攻击，则标记此次攻击为已取消。
                    // 这可以防止其他护盾再次尝试吸收此次攻击。
                    if (thisShieldCanFullyAbsorb)
                        shieldsFullyAbsorbedHit = true;

                    // 从即将到来的攻击中实际移除被此护盾阻挡的伤害，以便后续的护盾面对的伤害更少。
                    info.Damage -= spongeDamageBlocked;
                }

                // 如果任何护盾受到了伤害，则必须运行一些代码。
                if (shieldsTookHit)
                {
                    CalamityPlayer calPlayer = Player.Calamity();

                    // 如果任何护盾受到了伤害，则显示文本以指示护盾受到了伤害
                    string shieldDamageText = (-totalDamageBlocked).ToString();

                    Rectangle location = new Rectangle((int)Player.position.X, (int)Player.position.Y - 16, Player.width, Player.height);
                    CombatText.NewText(location, Color.LightBlue, Language.GetTextValue(shieldDamageText));

                    // 不论护盾是否被打破，都给玩家提供无敌帧（iframes）以应对护盾被击中的情况。
                    int shieldHitIFrames = Player.ComputeHitIFrames(info);
                    Player.GiveIFrames(info.CooldownCounter, shieldHitIFrames, true);

                    // 当护盾处于激活状态时受到攻击时会产生粒子效果，不论护盾是否被打破。
                    // 如果护盾被打破，则会产生更多的粒子。
                    if (calPlayer.pSoulArtifact)
                    {
                        for (int i = 0; i < Main.rand.Next(4, 8); i++) //very light dust
                        {
                            Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, (int)CalamityDusts.ProfanedFire);
                            dust.velocity = Main.rand.NextVector2Circular(3.5f, 3.5f);
                            dust.velocity.Y -= Main.rand.NextFloat(1f, 3f);
                            dust.scale = Main.rand.NextFloat(1.15f, 1.45f);
                        }
                    }
                    else
                    {
                        int numParticles = Main.rand.Next(24, 28) + (anyShieldBroke ? 16 : 0);
                        for (int i = 0; i < numParticles; i++)
                        {
                            float maxVelocity = 36f;
                            Vector2 velocity = Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(8f, maxVelocity);
                            velocity.X += 5f * info.HitDirection;

                            float scale = Main.rand.NextFloat(4f, 7f);
                            Color particleColor = Main.rand.NextBool() ? new Color(99, 255, 229) : new Color(25, 132, 247);
                            int lifetime = Main.rand.Next(35, 60);

                            var shieldParticle = new TechyHoloysquareParticle(Player.Center, velocity, scale, particleColor, lifetime);
                            GeneralParticleHandler.SpawnParticle(shieldParticle);
                        }
                        SpongeHurtEffect();
                    }
                    CalamityPlayer modPlayer1 = Player.Calamity();
                    // 在冷却条上更新海绵的耐久度。
                    if (CIsponge && modPlayer1.cooldowns.TryGetValue(CISpongeDurability.ID, out var spongeDurabilityCD))
                        spongeDurabilityCD.timeLeft = CISpongeShieldDurability;
                }

                // 无论护盾是否受到伤害，在受到任何攻击时都会遍历并暂停所有护盾的再生。
                // 这同样适用于在护盾完全失效时受到攻击，或者解除装备任何相关物品的情况。

                if (CISpongeShieldDurability > 0)
                {
                    //第一个为护盾大于0时行为，会重置Relay计时器
                    //第二个为护盾小于等于0时行为，会移除Relay计时器，并且不会被重置
                    if (CIsponge)
                    {
                        Player.AddCooldown(CISpongeRechargeRelay.ID, TheSpongetest.CIShieldRechargeRelay, true);
                        //Main.NewText($"已添加冷却", 0, 255, 0);
                    }
                }
                if (CISpongeShieldDurability <= 0)
                {
                    Player.RemoveCooldown(CISpongeRechargeRelay.ID);
                    if (!Player.HasCooldown(CISpongeRecharge.ID))
                    {
                        Player.AddCooldown(CISpongeRecharge.ID, TheSpongetest.CIShieldRechargeDelay, true);
                        //Main.NewText($"已添加冷却", 0, 255, 0);
                    }
                }

                // 如果护盾吸收了攻击，则使用反射删除此次攻击。
                if (shieldsTookHit)
                {
                    CalamityPlayer calPlayer = Player.Calamity();
                    calPlayer.freeDodgeFromShieldAbsorption = true;
                }

            }
            #endregion
            //史神无敌
            if (InvincibleJam)
            {
                CalamityPlayer calPlayer = Player.Calamity();
                calPlayer.freeDodgeFromShieldAbsorption = true;
            }

            if (GodSlayerReflect && Main.rand.NextBool(50))
            {
                CalamityPlayer calPlayer = Player.Calamity();
                calPlayer.freeDodgeFromShieldAbsorption = true;
                Player.immune = true;
            }
        }
        public static void SpongeHurtEffect()
        {
            Player player = Main.player[Main.myPlayer];
            #region 真菌壳
            SoundEngine.PlaySound(SoundID.NPCHit45, player.position);

            float spread = 45f * 0.0174f;
            double startAngle = Math.Atan2(player.velocity.X, player.velocity.Y) - spread / 2;
            double deltaAngle = spread / 8f;
            double offsetAngle;
            int fDamage = (int)player.GetBestClassDamage().ApplyTo(70);

            if (player.whoAmI == Main.myPlayer)
            {
                for (int i = 0; i < 6; i++)
                {
                    // 确定生成位置
                    float xPos = Main.rand.NextBool() ? player.Center.X + 100 : player.Center.X - 100;
                    Vector2 spawnPos = new Vector2(xPos, player.Center.Y + Main.rand.Next(-100, 101));

                    // 计算角度
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;

                    // 创建弹幕
                    var spore1 = Projectile.NewProjectileDirect(player.GetSource_FromThis(), spawnPos, new Vector2((float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f)), ProjectileID.TruffleSpore, fDamage, 1.25f, player.whoAmI
                    );
                    var spore2 = Projectile.NewProjectileDirect(player.GetSource_FromThis(), spawnPos, new Vector2((float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f)), ProjectileID.TruffleSpore, fDamage, 1.25f, player.whoAmI
                    );
                    // 设置时间
                    spore1.timeLeft = 300;
                    spore2.timeLeft = 300;
                }
            }
            #endregion
            SoundEngine.PlaySound(SoundID.Item93, player.Center);
            float spread1 = 45f * 0.0174f;
            double startAngle1 = Math.Atan2(player.velocity.X, player.velocity.Y) - spread1 / 2;
            double deltaAngle1 = spread / 8f;
            double offsetAngle1;

            // Start with base damage, then apply the best damage class you can
            int sDamage = 50;
            sDamage = (int)player.GetBestClassDamage().ApplyTo(sDamage);
            sDamage = player.ApplyArmorAccDamageBonusesTo(sDamage);

            if (player.whoAmI == Main.myPlayer)
            {
                for (int i = 0; i < 4; i++)
                {
                    offsetAngle1 = startAngle1 + deltaAngle1 * (i + i * i) / 2f + 32f * i;
                    int spark1 = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center.X, player.Center.Y, (float)(Math.Sin(offsetAngle1) * 5f), (float)(Math.Cos(offsetAngle1) * 5f), ModContent.ProjectileType<Spark>(), sDamage, 1.25f, player.whoAmI, 0f, 0f);
                    int spark2 = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center.X, player.Center.Y, (float)(-Math.Sin(offsetAngle1) * 5f), (float)(-Math.Cos(offsetAngle1) * 5f), ModContent.ProjectileType<Spark>(), sDamage, 1.25f, player.whoAmI, 0f, 0f);
                    if (spark1.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[spark1].timeLeft = 120;
                        Main.projectile[spark1].DamageType = DamageClass.Generic;
                    }
                    if (spark2.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[spark2].timeLeft = 120;
                        Main.projectile[spark2].DamageType = DamageClass.Generic;
                    }
                }
            }
        }
        
        #region Kill Player
        public void KillPlayer()
        {
            var source = Player.GetSource_Death();
            Player.lastDeathPostion = Player.Center;
            Player.lastDeathTime = DateTime.Now;
            Player.showLastDeath = true;
            int coinsOwned = (int)Utils.CoinsCount(out bool flag, Player.inventory, new int[0]);
            if (Main.myPlayer == Player.whoAmI)
            {
                Player.lostCoins = coinsOwned;
                Player.lostCoinString = Main.ValueToCoins(Player.lostCoins);
            }
            if (Main.myPlayer == Player.whoAmI)
            {
                Main.mapFullscreen = false;
            }
            if (Main.myPlayer == Player.whoAmI)
            {
                Player.trashItem.SetDefaults(0, false);
                if (Player.difficulty == PlayerDifficultyID.SoftCore || Player.difficulty == PlayerDifficultyID.Creative)
                {
                    for (int i = 0; i < 59; i++)
                    {
                        if (Player.inventory[i].stack > 0 && ((Player.inventory[i].type >= ItemID.LargeAmethyst && Player.inventory[i].type <= ItemID.LargeDiamond) || Player.inventory[i].type == ItemID.LargeAmber))
                        {
                            int droppedLargeGem = Item.NewItem(source, (int)Player.position.X, (int)Player.position.Y, Player.width, Player.height, Player.inventory[i].type, 1, false, 0, false, false);
                            Main.item[droppedLargeGem].netDefaults(Player.inventory[i].netID);
                            Main.item[droppedLargeGem].Prefix(Player.inventory[i].prefix);
                            Main.item[droppedLargeGem].stack = Player.inventory[i].stack;
                            Main.item[droppedLargeGem].velocity.Y = Main.rand.Next(-20, 1) * 0.2f;
                            Main.item[droppedLargeGem].velocity.X = Main.rand.Next(-20, 21) * 0.2f;
                            Main.item[droppedLargeGem].noGrabDelay = 100;
                            Main.item[droppedLargeGem].favorited = false;
                            Main.item[droppedLargeGem].newAndShiny = false;
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                            {
                                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, droppedLargeGem, 0f, 0f, 0f, 0, 0, 0);
                            }
                            Player.inventory[i].SetDefaults(0, false);
                        }
                    }
                }
                else if (Player.difficulty == PlayerDifficultyID.MediumCore)
                {
                    Player.DropItems();
                }
                else if (Player.difficulty == PlayerDifficultyID.Hardcore)
                {
                    Player.DropItems();
                    Player.KillMeForGood();
                }
            }

            if(CIWorld.IronHeart)
                SoundEngine.PlaySound(CISoundMenu.IronHeartDeath, Player.Center);
            else
                SoundEngine.PlaySound(SoundID.PlayerKilled, Player.Center);
            Player.headVelocity.Y = Main.rand.Next(-40, -10) * 0.1f;
            Player.bodyVelocity.Y = Main.rand.Next(-40, -10) * 0.1f;
            Player.legVelocity.Y = Main.rand.Next(-40, -10) * 0.1f;
            Player.headVelocity.X = Main.rand.Next(-20, 21) * 0.1f + 2 * 0;
            Player.bodyVelocity.X = Main.rand.Next(-20, 21) * 0.1f + 2 * 0;
            Player.legVelocity.X = Main.rand.Next(-20, 21) * 0.1f + 2 * 0;
            if (Player.stoned)
            {
                Player.headPosition = Vector2.Zero;
                Player.bodyPosition = Vector2.Zero;
                Player.legPosition = Vector2.Zero;
            }
            for (int j = 0; j < 100; j++)
            {
                Dust.NewDust(Player.position, Player.width, Player.height, DustID.LifeDrain, 2 * 0, -2f, 0, default, 1f);
            }
            Player.mount.Dismount(Player);
            Player.dead = true;
            Player.respawnTimer = 600;
            if (Main.expertMode)
            {
                Player.respawnTimer = (int)(Player.respawnTimer * 1.5);
            }
            Player.immuneAlpha = 0;
            Player.palladiumRegen = false;
            Player.iceBarrier = false;
            Player.crystalLeaf = false;

            if (Player.whoAmI == Main.myPlayer)
            {
                try
                {
                    WorldGen.saveToonWhilePlaying();
                }
                catch
                {
                }
            }
        }
        #endregion
    }
}