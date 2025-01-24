using System;
using CalamityInheritance.Buffs.Melee;
using CalamityInheritance.Buffs.Potions;
using CalamityInheritance.Buffs.StatDebuffs;
using CalamityInheritance.CICooldowns;
using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Potions;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Balancing;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.CalPlayer;
using CalamityMod.Cooldowns;
using CalamityMod.Dusts;
using CalamityMod.Enums;
using CalamityMod.Events;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Armor.Silva;
using CalamityMod.NPCs.Abyss;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Healing;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Mono.Cecil;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static CalamityMod.World.CustomConditions;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            CalamityPlayer modPlayer = Player.Calamity();
            // Handles energy shields and Boss Rush, in that order
            modifiers.ModifyHurtInfo += ModifyHurtInfo_Calamity;
            #region Custom Hurt Sounds
            if (modPlayer.hurtSoundTimer == 0)
            {
                if (CIsponge && CISpongeShieldDurability > 0)
                {
                    modifiers.DisableSound();
                    SoundEngine.PlaySound(TheSponge.ShieldHurtSound, Player.Center);
                    modPlayer.hurtSoundTimer = 20;
                }
            }
            #endregion

            #region Player Incoming Damage Multiplier (Increases)
            double damageMult = 1D;
            CalamityInheritancePlayer modPlayer1 = Player.CalamityInheritance();
            if (modPlayer1.desertScourgeLore) // Dimensional Soul Artifact increases incoming damage by 15%.
                damageMult += 0.05;

            modifiers.SourceDamage *= (float)damageMult;
            #endregion

        }
        private void ModifyHurtInfo_Calamity(ref Player.HurtInfo info)
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
                    CalamityPlayer modPlayer = Player.Calamity();

                    // 如果任何护盾受到了伤害，则显示文本以指示护盾受到了伤害
                    string shieldDamageText = (-totalDamageBlocked).ToString();
                    Rectangle location = new Rectangle((int)Player.position.X, (int)Player.position.Y - 16, Player.width, Player.height);
                    CombatText.NewText(location, Color.LightBlue, Language.GetTextValue(shieldDamageText));

                    // 不论护盾是否被打破，都给玩家提供无敌帧（iframes）以应对护盾被击中的情况。
                    int shieldHitIFrames = Player.ComputeHitIFrames(info);
                    Player.GiveIFrames(info.CooldownCounter, shieldHitIFrames, true);

                    // 当护盾处于激活状态时受到攻击时会产生粒子效果，不论护盾是否被打破。
                    // 如果护盾被打破，则会产生更多的粒子。
                    if (modPlayer.pSoulArtifact)
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
                        int numParticles = Main.rand.Next(2, 6) + (anyShieldBroke ? 6 : 0);
                        for (int i = 0; i < numParticles; i++)
                        {
                            float maxVelocity = modPlayer.roverDrive ? 14f : 7f;
                            Vector2 velocity = Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(3f, maxVelocity);
                            velocity.X += 5f * info.HitDirection;

                            float scale = Main.rand.NextFloat(2.5f, 3f);
                            Color particleColor = Main.rand.NextBool() ? new Color(99, 255, 229) : new Color(25, 132, 247);
                            int lifetime = 25;

                            var shieldParticle = new TechyHoloysquareParticle(Player.Center, velocity, scale, particleColor, lifetime);
                            GeneralParticleHandler.SpawnParticle(shieldParticle);
                        }
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
                    if (!Player.HasCooldown(CISpongeRecharge.ID))
                    {
                        Player.RemoveCooldown(CISpongeRechargeRelay.ID);
                        Player.AddCooldown(CISpongeRecharge.ID, TheSpongetest.CIShieldRechargeDelay, true);
                        //Main.NewText($"已添加冷却", 0, 255, 0);
                    }
                }

                // 如果护盾吸收了攻击，则使用反射删除此次攻击。
                if (shieldsTookHit)
                {
                    CalamityPlayer modPlayer = Player.Calamity();
                    modPlayer.freeDodgeFromShieldAbsorption = true;
                }

            }
            #endregion
            //史神无敌
            if (invincible)
            {
                CalamityPlayer modPlayer = Player.Calamity();
                modPlayer.freeDodgeFromShieldAbsorption = true;
            }

            if (godSlayerReflect && Main.rand.NextBool(50))
            {
                CalamityPlayer modPlayer = Player.Calamity();
                modPlayer.freeDodgeFromShieldAbsorption = true;
                Player.immune = true;
                Player.immuneTime = 60;
            }
        }

        #region On Hurt
        public override void OnHurt(Player.HurtInfo hurtInfo)
        {
            CalamityPlayer modPlayer = Player.Calamity();
            CalamityInheritancePlayer Modplayer1 = Player.CalamityInheritance();
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
                Player.statLife += healAmt;
                Player.HealEffect(healAmt);
            }
            //阴阳石
            if (Modplayer1.TheAbsorberOld)
            {
                int healAmt = (int)(hurtInfo.Damage / 20D);
                Player.statLife += healAmt;
                Player.HealEffect(healAmt);
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
            if (CalamityWorld.armageddon || SCalLore || (BossRushEvent.BossRushActive))
            {
                if (CalamityPlayer.areThereAnyDamnBosses || SCalLore || (BossRushEvent.BossRushActive))
                {
                    if (SCalLore)
                    {
                        string key = "Mods.CalamityInheritance.Status.SCAL";
                        Color messageColor = Color.DarkRed;
                        CalamityUtils.DisplayLocalizedText(key, messageColor);
                        modPlayer.KillPlayer();
                    }
                }
            }

            if (godSlayerMagic)
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

            if (reaverMeleeBlast) //受伤后提供战士永恒套怒气buff
            {
                Player.AddBuff(ModContent.BuffType<ReaverMeleeRage>(), 180);
            }
        }
        #endregion
        #region Kill Player
        public void KillPlayer()
        {
            PlayerDeathReason damageSource = PlayerDeathReason.ByOther(Player.Male ? 14 : 15);
            if (SCalLore)
            {
                damageSource = PlayerDeathReason.ByCustomReason(Player.Male ? Player.name + " was consumed by his inner hatred." : Player.name + " was consumed by her inner hatred.");
            }
        }
        #endregion
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            CalamityPlayer modPlayer = Player.Calamity();
            CalamityInheritancePlayer modPlayer1 = Player.CalamityInheritance();

            if (Player.name == "TrueScarlet" || Player.name == "FakeAqua")
            {
                if (modPlayer1.SCalLore && target.type == ModContent.NPCType<ReaperShark>())
                {
                    modifiers.SetInstantKill();
                }
            }

            modifiers.ModifyHitInfo += (ref NPC.HitInfo hitInfo) =>
            {
                if (godSlayerRangedold && hitInfo.Crit && proj.DamageType == DamageClass.Ranged)
                {
                    int randomChance = (int)(Player.GetTotalCritChance(DamageClass.Ranged) - 100);
                    if (randomChance >= 1)
                    {
                        if(Main.rand.NextBool(100/randomChance))
                        {
                            hitInfo.Damage *= 2;
                        }
                    }
                    else
                    {
                        if (Main.rand.NextBool(20))
                        {
                            hitInfo.Damage *= 4;
                        }
                    }
                }
            };
            if (silvaMelee)
            {
                //Main.NewText($"触发判定", 255, 255, 255);
                if (Main.rand.NextBool(4) && proj.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>())
                {
                    //Main.NewText($"造成伤害", 255, 255, 255);
                    modifiers.ModifyHitInfo += (ref NPC.HitInfo hitInfo) =>
                    {
                        hitInfo.Damage *= 5;
                    };
                }
            }

            if (CalamityInheritanceConfig.Instance.silvastun == true)
            {
                if (proj.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>() && silvaStunCooldownold <= 0 && silvaMelee && Main.rand.NextBool(4))
                {
                    //Main.NewText($"触发眩晕TMp", 255, 255, 255);
                    target.AddBuff(ModContent.BuffType<SilvaStun>(), 20);
                    silvaStunCooldownold = 1800;
                }
                if (proj.DamageType == DamageClass.Melee && silvaStunCooldownold <= 0 && silvaMelee && Main.rand.NextBool(4))
                {
                    //Main.NewText($"触发眩晕mp", 255, 255, 255);
                    target.AddBuff(ModContent.BuffType<SilvaStun>(), 20);
                    silvaStunCooldownold = 1800;
                }
            }
        }
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            CalamityPlayer modPlayer = Player.Calamity();
            CalamityInheritancePlayer modPlayer1 = Player.CalamityInheritance();

            if (Player.name == "TrueScarlet" || Player.name == "FakeAqua")
            {
                if (modPlayer1.SCalLore && target.type == ModContent.NPCType<ReaperShark>())
                {
                    modifiers.SetInstantKill();
                }
            }

            if (silvaMelee)
            {
                //Main.NewText($"触发判定", 255, 255, 255);
                if (Main.rand.NextBool(4) && item.DamageType == DamageClass.Melee || item.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>())
                {
                    //Main.NewText($"造成伤害", 255, 255, 255);
                    modifiers.ModifyHitInfo += (ref NPC.HitInfo hitInfo) =>
                    {
                        hitInfo.Damage *= 5;
                    };
                }
            }

            if (CalamityInheritanceConfig.Instance.silvastun == true)
            {
                if (item.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>() && silvaStunCooldownold <= 0 && silvaMelee && Main.rand.NextBool(4))
                {
                    //Main.NewText($"触发眩晕im", 255, 255, 255);
                    target.AddBuff(ModContent.BuffType<SilvaStun>(), 20);
                    silvaStunCooldownold = 1800;
                }
            }

            var source = Player.GetSource_ItemUse(item);

            if (item.DamageType == DamageClass.Melee)
            {
                titanBoost = 600;
            }
        }
        public void ModifyHitNPCBoth(Projectile proj, NPC target, ref NPC.HitModifiers modifiers, DamageClass damageClass)
        {
            CalamityInheritancePlayer modPlayer = Player.CalamityInheritance();
            if (Player.name == "TrueScarlet" || Player.name == "FakeAqua")
            {
                if (modPlayer.SCalLore && target.type == ModContent.NPCType<ReaperShark>())
                {
                    modifiers.SetInstantKill();
                }
            }
        }
        #region Pre Kill
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            CalamityPlayer modPlayer = Player.Calamity();
            
            if (GodSlayerReborn && !Player.HasCooldown(GodSlayerCooldown.ID))
            {
                SoundEngine.PlaySound(SoundID.Item67, Player.Center);

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

                if (draconicSurge)
                {
                    Player.statLife += Player.statLifeMax2;
                    Player.HealEffect(Player.statLifeMax2);

                    if (Player.FindBuffIndex(ModContent.BuffType<DraconicSurgeBuff>()) > -1)
                    {
                        Player.AddCooldown(DraconicElixirCooldown.ID, CalamityUtils.SecondsToFrames(30));
                    }
                }
                Player.AddCooldown(GodSlayerCooldown.ID, CalamityUtils.SecondsToFrames(45));
                return false;
            }
            //金源套的林海复活，或者说本mod的林海复活
            if (auricsilvaset && CIsilvaCountdown > 0)
            {
                if (silvaRebornMark)
                {
                    if (CIsilvaCountdown == CIsilvaReviveDuration && !aurichasSilvaEffect)
                    {
                        SoundEngine.PlaySound(SilvaHeadSummon.ActivationSound, Player.Center);

                        Player.AddBuff(ModContent.BuffType<SilvaRevival>(), CIsilvaReviveDuration);

                        if (modPlayer.silvaWings)
                        {
                            Player.statLife += Player.statLifeMax2 / 2;
                            Player.HealEffect(Player.statLifeMax2 / 2);

                            if (Player.statLife > Player.statLifeMax2)
                                Player.statLife = Player.statLifeMax2;
                        }
                    }

                    aurichasSilvaEffect = true;

                    if (Player.statLife < 1)
                        Player.statLife = 1;

                    // Silva revive clears Chalice of the Blood God's bleedout buffer every frame while active
                    // Can we please remove this from the game
                    if (modPlayer.chaliceOfTheBloodGod)
                    {
                        modPlayer.chaliceBleedoutBuffer = 0D;
                        modPlayer.chaliceDamagePointPartialProgress = 0D;
                    }

                    return false;
                }
            }
                //金源套的林海复活，或者说本mod的林海复活
            if (auricsilvaset && auricsilvaCountdown > 0 )
            {
                if (Player.HasCooldown(GodSlayerCooldown.ID))
                {
                    if (auricsilvaCountdown == auricsilvaReviveDuration && !aurichasSilvaEffect)
                    {
                        SoundEngine.PlaySound(SilvaHeadSummon.ActivationSound, Player.Center);

                        Player.AddBuff(ModContent.BuffType<SilvaRevival>(), auricsilvaReviveDuration);

                        if (modPlayer.silvaWings)
                        {
                            Player.statLife += Player.statLifeMax2 / 2;
                            Player.HealEffect(Player.statLifeMax2 / 2);

                            if (Player.statLife > Player.statLifeMax2)
                                Player.statLife = Player.statLifeMax2;
                        }
                    }

                    aurichasSilvaEffect = true;

                    if (Player.statLife < 1)
                        Player.statLife = 1;

                    // Silva revive clears Chalice of the Blood God's bleedout buffer every frame while active
                    // Can we please remove this from the game
                    if (modPlayer.chaliceOfTheBloodGod)
                    {
                        modPlayer.chaliceBleedoutBuffer = 0D;
                        modPlayer.chaliceDamagePointPartialProgress = 0D;
                    }

                    return false;
                }
            }

            //目前用于龙魂与原灾金源和复活效果的互动
            if (modPlayer.silvaSet && modPlayer.silvaCountdown > 0)
            {
                SoundEngine.PlaySound(SilvaHeadSummon.ActivationSound, Player.position);

                if (Player.HasCooldown(DraconicElixirCooldown.ID))
                {
                    Player.statLife += Player.statLifeMax2;
                    Player.HealEffect(Player.statLifeMax2);

                    if (Player.statLife > Player.statLifeMax2)
                        Player.statLife = Player.statLifeMax2;
                }

                if (draconicSurge)
                {

                    Player.statLife += Player.statLifeMax2;
                    Player.HealEffect(Player.statLifeMax2);

                    if (Player.statLife > Player.statLifeMax2)
                        Player.statLife = Player.statLifeMax2;

                    if (Player.FindBuffIndex(ModContent.BuffType<DraconicSurgeBuff>()) > -1)
                    {

                        Player.AddCooldown(DraconicElixirCooldown.ID, CalamityUtils.SecondsToFrames(30));

                        // Additional potion sickness time
                        int additionalTime = 0;
                        for (int i = 0; i < Player.MaxBuffs; i++)
                        {
                            if (Player.buffType[i] == BuffID.PotionSickness)
                                additionalTime = Player.buffTime[i];
                        }

                        float potionSicknessTime = 30f + (float)Math.Ceiling(additionalTime / 60D);
                        Player.AddBuff(BuffID.PotionSickness, CalamityUtils.SecondsToFrames(potionSicknessTime));
                    }
                }
                return false;
            }
            return true;
        }
        #endregion
        #region Modify Hit By NPC
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            CalamityPlayer modPlayer = Player.Calamity();
            if (triumph)
                modPlayer.contactDamageReduction += 0.15 * (1D - (npc.life / (double)npc.lifeMax));
            if (beeResist)
            {
                if (CalamityInheritanceLists.beeEnemyList.Contains(npc.type))
                    modPlayer.contactDamageReduction += 0.25;
            }

        }
        #endregion
        #region Free and Consumable Dodge Hooks
        public override bool FreeDodge(Player.HurtInfo info)
        {
            Player player = Main.player[Main.myPlayer];
            CalamityPlayer modPlayer1 = player.Calamity();
            // 22AUG2023: Ozzatron: god slayer damage resistance removed due to it being strong enough to godmode rev yharon
            // If the incoming damage is somehow less than 1 (TML doesn't allow this, but...), the hit is completely ignored.
            if (GodSlayerDMGprotect && info.Damage <= 80 && !modPlayer1.chaliceOfTheBloodGod)

                return true;
            // If no other effects occurred, run vanilla code
            return base.FreeDodge(info);
        }
        #endregion
        #region Modify Hit By Proj
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            CalamityPlayer modPlayer = Player.Calamity();
            // TODO -- Evolution dodge isn't actually a dodge and you'll still get hit for 1.
            // This should probably be changed so that when the evolution reflects it gives you 1 frame of guaranteed free dodging everything.
            if (CalamityLists.projectileDestroyExceptionList.TrueForAll(x => proj.type != x) && proj.active && !proj.friendly && proj.hostile && proj.damage > 0)
            {
                double dodgeDamageGateValuePercent = 0.05;
                int dodgeDamageGateValue = (int)Math.Round(Player.statLifeMax2 * dodgeDamageGateValuePercent);

                // Reflects count as dodges. They share the timer and can be disabled by Armageddon right click.
                if (!modPlayer.disableAllDodges && !Player.HasCooldown(GlobalDodge.ID) && proj.damage >= dodgeDamageGateValue)
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
                        modPlayer.evolutionLifeRegenCounter = 300;
                        modPlayer.projTypeJustHitBy = proj.type;

                        int cooldownDuration = (int)MathHelper.Lerp(900, 5400 , cooldownDurationScalar);
                        Player.AddCooldown(GlobalDodge.ID, cooldownDuration);

                        return;
                    }
                }
            }

            if (beeResist)
            {
                if (CalamityInheritanceLists.beeProjectileList.Contains(proj.type))
                    modPlayer.projectileDamageReduction += 0.25;
            }
        }
        #endregion
    }
}