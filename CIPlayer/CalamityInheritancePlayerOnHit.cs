using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.NPCs;
using Terraria.ID;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Healing;
using System;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityMod.Projectiles;
using CalamityMod.Projectiles.Magic;
using Terraria.Audio;
using CalamityInheritance.Buffs.Mage;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Buffs.Statbuffs;
using CalamityMod.Projectiles.Summon;
using System.Collections.Generic;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityMod.NPCs.Yharon;
using CalamityInheritance.Content.Items.MiscItem;
using CalamityInheritance.Content.Items.Potions;
using CalamityMod.Items.Armor.Vanity;
using CalamityMod.NPCs.DevourerofGods;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod.NPCs.Polterghast;
using CalamityMod.NPCs.OldDuke;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.NPCs.Abyss;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityMod.Items.Accessories;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.CalamityAIs.CalamityBossAIs;
using CalamityMod.NPCs.Bumblebirb;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        #region On Hit NPC With Item
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            if (Player.whoAmI != Main.myPlayer)
                return;
            NPCDebuffs(target, item.CountsAsClass<MeleeDamageClass>(), item.CountsAsClass<RangedDamageClass>(), item.CountsAsClass<MagicDamageClass>(), item.CountsAsClass<SummonDamageClass>(), item.CountsAsClass<ThrowingDamageClass>(), item.CountsAsClass<SummonMeleeSpeedDamageClass>());
        }
        #endregion

        public override void OnHitNPCWithProj(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            CalamityPlayer modPlayer = Player.Calamity();
            CalamityGlobalNPC cgn = target.Calamity();
            Player player = Main.player[projectile.owner];
            var usPlayer = player.CIMod();
            var heldingItem = player.ActiveItem();
            if (Player.whoAmI != Main.myPlayer)
                return;
            if (projectile.type == ModContent.ProjectileType<PolarStarLegacy>())
            {
                PolarisBoostCounter += 1;
            }
            #region Lore
            if (LorePerforator)
            {
                target.AddBuff(BuffID.Ichor, 90);
            }
            if (LoreHive)
            {
                target.AddBuff(BuffID.CursedInferno, 90);
            }
            if (LoreProvidence || PanelsLoreProvidence)
            {
                target.AddBuff(ModContent.BuffType<HolyInferno>(), 180, false);
            }
            if (BuffStatsHolyWrath)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 180, false);
            }
            if (YharimsInsignia)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 120, false);
            }
            if (BuffStatsDraconicSurge && Main.zenithWorld)
            {
                target.AddBuff(ModContent.BuffType<Dragonfire>(), 360, false);
            }
            #endregion
            #region Armorset
            #region GodSlayer
            if (GodSlayerMagicSet && projectile.DamageType == DamageClass.Magic)
            {
                if (hasFiredThisFrame)
                {
                    return;
                }
                hasFiredThisFrame = true;
                int weaponDamage = player.HeldItem.damage;
                int finalDamage = 200 + weaponDamage / 2;

                int projectileTypes = ModContent.ProjectileType<GodSlayerOrb>();
                float randomAngleOffset = (float)(Main.rand.NextDouble() * 2 * MathHelper.Pi);
                Vector2 direction = new((float)Math.Cos(randomAngleOffset), (float)Math.Sin(randomAngleOffset));
                float randomSpeed = Main.rand.NextFloat(12f, 16f);
                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, direction * randomSpeed, projectileTypes, finalDamage, projectile.knockBack);
            }

            if (GodSlayerSummonSet && projectile.DamageType == DamageClass.Summon)
            {
                if (hasFiredThisFrame)
                {
                    return;
                }
                hasFiredThisFrame = true;
                player = Main.player[projectile.owner];
                int weaponDamage = player.HeldItem.damage;
                int finalDamage = 200 + weaponDamage / 2;

                int projectileTypes = ModContent.ProjectileType<GodSlayerPhantom>();
                float randomAngleOffset = (float)(Main.rand.NextDouble() * 2 * MathHelper.Pi);
                Vector2 direction = new((float)Math.Cos(randomAngleOffset), (float)Math.Sin(randomAngleOffset));
                float randomSpeed = Main.rand.NextFloat(6f, 8f);
                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, direction * randomSpeed, projectileTypes, finalDamage, projectile.knockBack);
            }
            #endregion
           

            var source = projectile.GetSource_FromThis();
            if (SilvaMagicSetLegacy && SilvaMagicSetLegacyCooldown <= 0 && (projectile.penetrate == 1 || projectile.timeLeft <= 5) && projectile.DamageType == DamageClass.Magic)
            {
                SilvaMagicSetLegacyCooldown = 300;
                SoundEngine.PlaySound(SoundID.Zombie103, projectile.Center); //So scuffed, just because zombie sounds werent ported normally
                int silvaBurstDamage = Player.ApplyArmorAccDamageBonusesTo((float)(800.0 + 0.6 * projectile.damage));
                Projectile.NewProjectile(source, projectile.Center, Vector2.Zero, ModContent.ProjectileType<SilvaBurst>(), silvaBurstDamage, 8f, Player.whoAmI);
            }

            if (projectile.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>() || projectile.type == ModContent.ProjectileType<StepToolShadowChair>())
            {
                BuffStatsTitanScaleTrueMelee = 600;
            }
            #region ReaverSets
            #region ReaverMage
            //法师永恒套的套装效果

            CalamityInheritancePlayer ReaverMagePlayer = Main.player[projectile.owner].GetModPlayer<CalamityInheritancePlayer>();
            var ReaverMage = projectile.GetSource_FromThis();
            if (Main.player[projectile.owner].CIMod().ReaverMageBurst)
            {
                if (ReaverMageBurst) //击发时提供法术增强buff
                {
                    Player.AddBuff(ModContent.BuffType<ReaverMagePower>(), 180);
                }

                if (ReaverBurstCooldown <= 0)
                {
                    int[] projectileTypes = { ModContent.ProjectileType<CISporeGas>(), ModContent.ProjectileType<CISporeGas2>(), ModContent.ProjectileType<CISporeGas3>() };
                    float baseAngleIncrement = 2 * MathHelper.Pi / 16;
                    float randomAngleOffset = (float)(Main.rand.NextDouble() * MathHelper.Pi / 4 - MathHelper.Pi / 8);
                    //好像这样伤害还挺低的但我也不知道该不该调整了
                    int newDamage = Player.ApplyArmorAccDamageBonusesTo(CalamityUtils.DamageSoftCap(15 + 0.15 * projectile.damage, 30));

                    for (int sporecounts = 0; sporecounts < 16; sporecounts++)
                    {
                        float angle = sporecounts * baseAngleIncrement + randomAngleOffset;
                        Vector2 direction = new((float)Math.Cos(angle), (float)Math.Sin(angle));
                        int randomProjectileType = projectileTypes[Main.rand.Next(projectileTypes.Length)];
                        float randomSpeed = Main.rand.NextFloat(2f, 4f);
                        Projectile.NewProjectile(source, target.Center, direction * randomSpeed, randomProjectileType, newDamage, projectile.knockBack);
                    }
                    target.AddBuff(BuffID.Poisoned, 120);
                    ReaverBurstCooldown = 90;
                }
            }
            #endregion
            #region ReaverMelee
            //永恒套的近战爆炸攻击
            var meleeReaverSrc = projectile.GetSource_FromThis();
            if (Main.player[projectile.owner].CIMod().ReaverMeleeBlast && projectile.DamageType == DamageClass.Melee)
            {
                int BlastDamage = (int)(projectile.damage * 0.4);
                if (BlastDamage > 30)
                {
                    BlastDamage = 30;
                }
                if (ReaverBlastCooldown <= 0)
                {
                    SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, projectile.Center);
                    Projectile.NewProjectile(meleeReaverSrc, projectile.Center, Vector2.Zero, ModContent.ProjectileType<ReaverBlast>(),
                                            BlastDamage, 0.15f, Player.whoAmI);
                    ReaverBlastCooldown = 10;
                }
            }
            #endregion
            #endregion
            if (!projectile.npcProj && !projectile.trap && projectile.friendly)
            {
                ProjLifesteal(target, projectile, damageDone, hit.Crit);
                ProjOnHit(projectile, target.Center, hit.Crit, target.IsAnEnemy(false));
            }
            #region AncientArmor
            if (AncientBloodflareSet && hit.Damage > 300 && target.IsAnEnemy(false) && target.lifeMax > 5 && AncientBloodflareHeartDropCD == 0) //大于300伤害才能产出红心与魔力星 
            {
                int amt = Main.rand.Next(2, 5); //2->4
                if(Main.rand.NextBool(6)) //每次攻击时1/6概率
                {
                    for(int i = 0; i < amt; i++)
                    {
                        Item.NewItem(target.GetSource_FromThis(), target.Hitbox, ItemID.Heart);
                        Item.NewItem(target.GetSource_FromThis(), target.Hitbox, ItemID.Star);
                    }
                    AncientBloodflareHeartDropCD = 180; //2s一次
                }
            }
            if (AncientGodSlayerSet && projectile.Calamity().stealthStrike && projectile.DamageType == ModContent.GetInstance<RogueDamageClass>() && PerunofYharimCooldown == 0)
            {
                //潜伏攻击成功时提供20%增伤
                player.GetDamage<GenericDamageClass>() += 0.2f;
                PerunofYharimCooldown = 2700;

            }
            #endregion
            
            #region AuricYharim
            if (AncientAuricSet)
            {
                if (hit.Damage > 300 && target.IsAnEnemy(false) && target.lifeMax > 5 && AncientBloodflareHeartDropCD == 0) //大于300伤害才能产出红心与魔力星 
                {
                    int amt = Main.rand.Next(3, 6); //3->5
                    if(Main.rand.NextBool(5)) //每次攻击时1/5概率
                    {
                        for(int i = 0; i < amt; i++)
                        {
                            Item.NewItem(target.GetSource_FromThis(), target.Hitbox, ItemID.Heart);
                            Item.NewItem(target.GetSource_FromThis(), target.Hitbox, ItemID.Star);
                        }
                        AncientBloodflareHeartDropCD = 90; //1.5秒一次
                    }
                }

                if (projectile.DamageType == ModContent.GetInstance<RogueDamageClass>()
                    && projectile.Calamity().stealthStrike
                    && PerunofYharimCooldown == 0
                    ) 
                {
                        SoundEngine.PlaySound(CISoundMenu.YharimsThuner with {Volume = 0.5f});
                        for (int j = 0; j < 50; j++)
                        {
                            int nebulousReviveDust = Dust.NewDust(Player.position, Player.width, Player.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 2f);
                            Dust dust = Main.dust[nebulousReviveDust];
                            dust.position.X += Main.rand.Next(-20, 21);
                            dust.position.Y += Main.rand.Next(-20, 21);
                            dust.velocity *= 0.9f;
                            dust.scale *= 1f + Main.rand.Next(40) * 0.01f;
                            if (Main.rand.NextBool())
                                dust.scale *= 1f + Main.rand.Next(40) * 0.01f;
                        }
                        Player.AddBuff(ModContent.BuffType<yharimOfPerun>(), 1800);
                        PerunofYharimCooldown = 1800;

                }
            }
            #endregion

            if(AncientAstralSet)
            {
                if(hit.Damage > 10 && hit.Crit && projectile.DamageType == ModContent.GetInstance<RogueDamageClass>() && AncientAstralCritsCD == 0)
                {
                    Player.Heal(20);
                    AncientAstralCritsCount += 1;// 自增
                    if(AncientAstralCritsCount == 20)
                    {
                        SoundEngine.PlaySound(CISoundID.SoundFallenStar with {Volume = 0.7f}, Player.Center);
                        Player.AddBuff(ModContent.BuffType<AncientAstralBuff>(), 300); //5秒
                        CIFunction.DustCircle(Player.Center, 18f, 1.8f, DustID.HallowedWeapons, false, 8f);
                    }
                    AncientAstralCritsCD = 60; //一个非常微弱的CD
                }
                if(projectile.DamageType == ModContent.GetInstance<RogueDamageClass>() && AncientAstralStealthCD == 0 && projectile.Calamity().stealthStrike)
                {
                    AncientAstralStealthGap = 900; //15s
                    AncientAstralStealthCD = 60; //1秒间隔
                    AncientAstralStealth++;
                    //使其不超过12，即我们需要的上限
                    if (AncientAstralStealth > 12)
                        AncientAstralStealth = 12;
                    if (AncientAstralStealth < 13)
                    {
                        player.lifeRegen += AncientAstralStealth; //(6)
                    }
                }
            }
            #endregion
            #region 传奇武器伤害任务
        
            //孔雀翎(T2,T3)
            if (heldingItem.type == ModContent.ItemType<PBGLegendary>())
            {
                PBGLegendaryDamageTask(target, hit);

                if (PBGLegendaryTier3)
                    PBGLegendaryBuff(target, hit);
            }

            //海爵剑(T1,T2,T3)
            if (heldingItem.type == ModContent.ItemType<DukeLegendary>())
            {
                DukeLegendaryDamageTask(target, hit);

                if (DukeLegendaryTier3)
                    DukeLegendaryBuff(target, hit);
            }

            //维苏威阿斯(T1, T2)
            if (heldingItem.type == ModContent.ItemType<RavagerLegendary>())
                RavagerLegendaryDamageTask(target, hit);

            //叶流(T1,T2)
            if (heldingItem.type == ModContent.ItemType<PlanteraLegendary>())
                PlanteraLegendaryDamageTask(target, hit);
            
            #endregion
            NPCDebuffs(target, projectile.CountsAsClass<MeleeDamageClass>(), projectile.CountsAsClass<RangedDamageClass>(), projectile.CountsAsClass<MagicDamageClass>(), projectile.CountsAsClass<SummonDamageClass>(), projectile.CountsAsClass<ThrowingDamageClass>(), projectile.CountsAsClass<SummonMeleeSpeedDamageClass>());
        }
        #region 传奇物品特殊效果(T3)
        private void DukeLegendaryBuff(NPC target, NPC.HitInfo hit)
        {
            //海爵剑T3：持续攻击增强防御属性，最高增强50点防御力与40%伤害减免
            if (hit.Damage > 5 && DukeDefenseCounter > 0)
            {
                //最大50层
                if (DukeDefenseCounter < 51)
                    DukeDefenseCounter++;
                //五秒
                DukeDefenseTimer = 300;
                //玩家的防御力视击中的次数提升
                Player.statDefense += DukeDefenseCounter;
                //每次攻击增加0.8%免伤，50次攻击后为40%
                Player.endurance += 0.008f * DukeDefenseCounter;
            }
            else
            {
                DukeDefenseCounter = 0;
                DukeDefenseTimer = 0;
            }
        }

        private void PBGLegendaryBuff(NPC target, NPC.HitInfo hit)
        {
            //T3孔雀翎特殊效果：一次击中敌人超过5000伤害时，自身生命值被置零以下时，以一定概率，将自己的生命值强行置0以停止掉血, 这一效果仅短暂持续1秒
            if (target.life > 5 && hit.Damage > 5000 && GlobalLegendaryT3CD == 0 && !AncientSilvaSet)
            {
                //孔雀翎攻速极快，因此1/75概率才是最合适的
                if (Player.lifeRegen < 0 && Main.rand.NextBool(75))
                {
                    Player.lifeRegen = 0;
                    GlobalLegendaryT3CD = 60;
                }
            }
        }
        #endregion
        #region 传奇物品伤害任务
        private void PlanteraLegendaryDamageTask(NPC target, NPC.HitInfo hit)
        {
            var usPlayer = Player.CIMod();
            //T1: 猪鲨50%伤
            if (target.type == NPCID.DukeFishron && !usPlayer.PlanteraLegendaryTier1)
            {
                usPlayer.DamagePool += hit.Damage;
                if (usPlayer.DamagePool > target.lifeMax * 0.5f)
                {
                    CIFunction.DustCircle(Player.Center, 32f, 1.8f, DustID.DryadsWard, true, 10f);
                    SoundEngine.PlaySound(CISoundID.SoundFallenStar with {Volume = .5f}, Player.Center);
                    usPlayer.DamagePool = 0;
                    usPlayer.PlanteraLegendaryTier1 = true;
                }
            }
            //T2: 在丛林范围内给予月球领主最后一击
            if (target.type == ModContent.NPCType<Bumblefuck>() && !usPlayer.PlanteraLegendaryTier2 && Main.LocalPlayer.ZoneJungle)
            {
                if (hit.Damage > target.life)
                {
                    CIFunction.DustCircle(Player.Center, 32f, 1.8f, DustID.DryadsWard, true, 10f);
                    SoundEngine.PlaySound(CISoundID.SoundFallenStar with {Volume = .5f}, Player.Center);
                    usPlayer.PlanteraLegendaryTier2 = true;
                }
            }
        }

        private void RavagerLegendaryDamageTask(NPC target, NPC.HitInfo hit)
        {
            var usPlayer = Player.CIMod();
            //T1: 在地狱击败一只贝特西
            if (target.type == NPCID.DD2Betsy && !RavagerLegendaryTier1 && Main.LocalPlayer.ZoneUnderworldHeight)
            {
                DamagePool += hit.Damage;
                if (DamagePool >= target.lifeMax)
                {
                    CIFunction.DustCircle(Player.Center, 32f, 1.8f, DustID.Meteorite, true, 10f);
                    SoundEngine.PlaySound(CISoundID.SoundBomb with {Volume = .5f}, Player.Center);
                    RavagerLegendaryTier1= true;
                    DamagePool = 0;
                }
            }
            //T2: 在地狱对亵渎天神造成80%伤害
            if (target.type == ModContent.NPCType<Providence>() && !RavagerLegendaryTier2 && Main.LocalPlayer.ZoneUnderworldHeight)
            {
                DamagePool += hit.Damage;
                if (usPlayer.DamagePool >= target.lifeMax * 0.8f)
                {
                    CIFunction.DustCircle(Player.Center, 32f, 1.8f, DustID.Meteorite, true, 10f);
                    SoundEngine.PlaySound(CISoundID.SoundBomb with {Volume = .5f}, Player.Center);
                    RavagerLegendaryTier2 = true;
                    DamagePool = 0;
                }
            }
        }

        private void DukeLegendaryDamageTask(NPC target, NPC.HitInfo hit)
        {
            var usPlayer = Player.CIMod();
            //T2:
            if (target.type == ModContent.NPCType<ReaperShark>() && !usPlayer.DukeLegendaryTier2)
            {
                usPlayer.DamagePool += hit.Damage;
                if (usPlayer.DamagePool > target.lifeMax * 0.8f)
                {
                        CIFunction.DustCircle(Player.Center, 32f, 1.8f, DustID.Water, true, 10f);
                        SoundEngine.PlaySound(SoundID.NPCDeath19 with {Volume = .5f}, Player.Center);
                        //记得清空伤害池子，因为这个是共用的
                        usPlayer.DamagePool = 0;
                        usPlayer.DukeLegendaryTier2 = true;
                }
            }
            // T3
            if (NPC.AnyNPCs(ModContent.NPCType<OldDuke>()) && NPC.AnyNPCs(NPCID.DukeFishron) && !usPlayer.DukeLegendaryTier3)
            {   
                //对猪鲨和老猪造成伤害都会增加伤害池
                if (target.type == ModContent.NPCType<OldDuke>() || target.type == NPCID.DukeFishron)
                {
                    usPlayer.DamagePool += hit.Damage;
                    //只判老猪的血量是否被打掉了20%，如果是直接场内升级
                    if ( target.type == ModContent.NPCType<OldDuke>() && usPlayer.DamagePool > target.lifeMax * 0.2f)
                    {
                        //场内直接升级发送提示音效
                        CIFunction.DustCircle(Player.Center, 32f, 1.8f, DustID.Water, true, 10f);
                        SoundEngine.PlaySound(SoundID.NPCDeath19 with {Volume = .5f}, Player.Center);
                        //将老猪血量舍去60%压缩战斗时长
                        target.life -= (int)(target.lifeMax * 0.6f);
                        //记得清空伤害池子，因为这个是共用的
                        usPlayer.DamagePool = 0;
                        usPlayer.DukeLegendaryTier3 = true;
                    }
                }
            }
        }

        private void PBGLegendaryDamageTask(NPC target, NPC.HitInfo hit)
        {
            var usPlayer = Player.CIMod();
            //T2: 使用孔雀翎对噬魂幽花造成最后一击
            //T3: 对神明吞噬者造成50%伤害
            if (target.type == ModContent.NPCType<DevourerofGodsHead>() || target.type == ModContent.NPCType<DevourerofGodsBody>() || target.type == ModContent.NPCType<DevourerofGodsTail>())
            {
                usPlayer.DamagePool += hit.Damage;
                //T3样式需要的伤害量降低至50%而非88%
                if (usPlayer.DamagePool > target.lifeMax * 0.50f && !usPlayer.PBGLegendaryTier3)
                {
                    //不考虑特殊物品而是直接在场间直接升级
                    usPlayer.PBGLegendaryTier3 = true;
                    //叮!
                    CIFunction.DustCircle(Player.Center, 32f, 1.8f, DustID.TerraBlade, true, 10f);
                    SoundEngine.PlaySound(CISoundID.SoundFallenStar with {Volume = .5f}, Player.Center);
                    usPlayer.DamagePool = 0;
                }
            }
        }
        #endregion
        
        #region Debuffs
        public void NPCDebuffs(NPC target, bool melee, bool ranged, bool magic, bool summon, bool rogue, bool whip, bool proj = false, bool noFlask = false)
        {
            if ((melee || rogue || whip) && !noFlask)
            {
                if (BuffStatsArmorShatter)
                {
                    CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<Crumbling>());
                }
            }
            if (melee && !noFlask)
            {
                if (ElemGauntlet)
                {
                    target.AddBuff(ModContent.BuffType<ElementalMix>(), 300, false);
                    target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300, false);
                    target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300, false);
                    target.AddBuff(BuffID.Frostburn2, 300);
                    target.AddBuff(BuffID.CursedInferno, 300);
                    target.AddBuff(BuffID.Inferno, 300);
                    target.AddBuff(BuffID.Venom, 300);
                }
            }
            if (summon)
            {
                if (NucleogenesisLegacy)
                {
                    target.AddBuff(BuffID.Electrified, 120);
                    target.AddBuff(ModContent.BuffType<HolyFlames>(), 300, false);
                    target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300, false);
                    target.AddBuff(ModContent.BuffType<Irradiated>(), 300, false);
                    target.AddBuff(ModContent.BuffType<Shadowflame>(), 300, false);
                }
            }
        }
        #endregion
        public override void ModifyWeaponKnockback(Item item, ref StatModifier knockback)
        {
            if (BuffStatsYharimsStin)
                knockback += item.knockBack * 0.25f;
        }
        public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
        {
            healValue = (int)(healValue * ManaHealMutipler);
        }
        public override void OnConsumeMana(Item item, int manaConsumed)
        {
            if (AncientAuricSet && PerunofYharimStats)
            Player.Heal(manaConsumed);
        }
        #region Lifesteal
        private void ProjLifesteal(NPC target, Projectile proj, int damage, bool crit)
        {
            CalamityGlobalProjectile modProj = proj.Calamity();

            if (Main.player[Main.myPlayer].lifeSteal > 0f && !Player.moonLeech && target.lifeMax > 5)
            {
                if (AuricSilvaSet)
                {
                    double healMult = 0.1;
                    int heal = Main.rand.Next(5, 11);

                    if (CalamityGlobalProjectile.CanSpawnLifeStealProjectile(healMult, heal))
                        CalamityGlobalProjectile.SpawnLifeStealProjectile(proj, Player, heal, ModContent.ProjectileType<SilvaOrb>(), 3000f, 2f);
                }
                if (GodSlayerMagicSet)
                {
                    double healMult = 0.1;
                    int heal = Main.rand.Next(5, 11);

                    if (CalamityGlobalProjectile.CanSpawnLifeStealProjectile(healMult, heal))
                        CalamityGlobalProjectile.SpawnLifeStealProjectile(proj, Player, heal, ModContent.ProjectileType<GodSlayerHealOrb>(), 3000f, 2f);
                }
            }
        }
        #endregion
        #region Proj On Hit
        public void ProjOnHit(Projectile proj, Vector2 position, bool crit, bool npcCheck)
        {
            CalamityGlobalProjectile modProj = proj.Calamity();

            if (proj.CountsAsClass<ThrowingDamageClass>())
                RogueOnHit(proj, modProj, position, crit, npcCheck);
            if (proj.CountsAsClass<SummonDamageClass>() && !proj.CountsAsClass<SummonMeleeSpeedDamageClass>())
                SummonOnHit(proj, modProj, position, crit, npcCheck);
        }
        #endregion
        #region Rogue
        private void RogueOnHit(Projectile proj, CalamityGlobalProjectile modProj, Vector2 position, bool crit, bool npcCheck)
        {
            var spawnSource = proj.GetSource_FromThis();

            CalamityPlayer modPlayer = Player.Calamity();

            if (proj.Calamity().stealthStrike && proj.Calamity().stealthStrikeHitCount < 3)
            {
                if (nanotechold)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Vector2 source = new Vector2(position.X + Main.rand.Next(-201, 201), Main.screenPosition.Y - 600f - Main.rand.Next(50));
                        Vector2 velocity = (position - source) / 40f;

                        Projectile.NewProjectile(spawnSource, source, velocity, ModContent.ProjectileType<NanoFlare>(), (int)(proj.damage * 0.05), 3f, proj.owner);
                    }
                }
            }

        }
        #endregion
        private static void SummonOnHit(Projectile proj, CalamityGlobalProjectile modProj, Vector2 position, bool crit, bool npcCheck)
        {
            Player player = Main.player[proj.owner];

            var source = proj.GetSource_FromThis();

            CalamityInheritancePlayer CIplayer = player.CIMod();

            List<int> summonExceptionList = new List<int>()
            {
                ModContent.ProjectileType<ApparatusExplosion>(),
            };

            if (summonExceptionList.TrueForAll(x => proj.type != x))
            {
                if (CIplayer.summonProjCooldown <= 0)
                {
                    if (CIplayer.NucleogenesisLegacy)
                    {
                        Projectile.NewProjectile(source, proj.Center, Vector2.Zero, ModContent.ProjectileType<ApparatusExplosion>(), (int)(proj.damage * 0.25f), 4f, proj.owner);
                        CIplayer.summonProjCooldown = 25;
                    }
                }
            }
        }
    }
}
