using System;
using CalamityInheritance.Buffs;
using CalamityInheritance.CICooldowns;
using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Potions;
using CalamityInheritance.Utilities;
using CalamityMod;
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
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Mono.Cecil;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

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

                            float scale = Main.rand.NextFloat(2.5f, 3f);
                            Color particleColor = Main.rand.NextBool() ? new Color(99, 255, 229) : new Color(25, 132, 247);
                            int lifetime = 25;


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
            if(invincible)
            {
                CalamityPlayer modPlayer = Player.Calamity();
                modPlayer.freeDodgeFromShieldAbsorption = true;
            }
        }

        #region On Hurt
        public override void OnHurt(Player.HurtInfo hurtInfo)
        {
            CalamityPlayer modPlayer = Player.Calamity();
            CalamityInheritancePlayer Modplayer1 = Player.CalamityInheritance();
            if (Modplayer1.CIsponge)
            {
                int healAmt = (int)(hurtInfo.Damage / (CIsponge ? 16D : 20D));
                Player.statLife += healAmt;
                Player.HealEffect(healAmt);
            }
            if (Modplayer1.Revivify)
            {
                int healAmt = (int)(hurtInfo.Damage / 15D);
                Player.statLife += healAmt;
                Player.HealEffect(healAmt);
            }

            if (CalamityWorld.armageddon || SCalLore || (BossRushEvent.BossRushActive))
            {
                if (CalamityPlayer.areThereAnyDamnBosses || SCalLore || (BossRushEvent.BossRushActive))
                {
                    if (SCalLore)
                    {
                        string key = "Mods.CalamityMod.SupremeBossText2";
                        Color messageColor = Color.Orange;
                        if (Main.netMode == NetmodeID.SinglePlayer)
                        {
                            Main.NewText(Language.GetTextValue(key), messageColor);
                        }
                        else if (Main.netMode == NetmodeID.Server)
                        {
                            ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                        }
                    }
                    modPlayer.KillPlayer();
                }
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

                modPlayer.hasSilvaEffect = true;

                if (Player.statLife < 1)
                    Player.statLife = 1;

                return false;
            }
            return true;
        }
        #endregion
        #region Modify Hit By NPC
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (triumph)
                contactDamageReduction += 0.15 * (1D - (npc.life / (double)npc.lifeMax));
        }
        #endregion
    }
}