using CalamityInheritance.Buffs.Statbuffs;
using CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.Alcohol;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using rail;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public float vulnerabilityHexLegacyProgress = 0f;
        #region Update Bad Life Regen
        public override void UpdateBadLifeRegen()
        {
            CalamityInheritancePlayer modPlayer = Player.CIMod();
            CalamityPlayer calPlayer = Player.Calamity();

            if (AstralArcanumEffect)
            {
                bool lesserEffect = false;

                int defenseBoost = 15;
                if (lesserEffect)
                {
                    Player.lifeRegen += 2;
                    Player.statDefense += defenseBoost;
                }
                else
                {
                    if (Player.lifeRegen < 0)
                    {
                        if (Player.lifeRegenTime < 1800)
                            Player.lifeRegenTime = 1800;

                        Player.lifeRegen += 6;
                        Player.statDefense += defenseBoost;
                    }
                    else
                        Player.lifeRegen += 3;
                }
            }
            TotalDebuff();
            IronHeartChange();
        }
        #endregion
        public void TotalDebuff()
        {
            // 死亡模式debuff伤害+25%
            // 移除死亡增幅
            float calamityDebuffMultiplier = 1f;
            // 总共降低的生命值
            float totalPowerfulNegativeLifeRegen = 0;

            // 走原版掉血的Debuff
            void ApplyDoTDebuff(bool hasDebuff, int negativeLifeRegenToApply, bool immuneCondition = false)
            {
                if (!hasDebuff || immuneCondition)
                    return;

                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;

                Player.lifeRegenTime = 0;
                totalPowerfulNegativeLifeRegen += negativeLifeRegenToApply * calamityDebuffMultiplier;
            }

            //150 -> 90 （75HP/s -> 50HP/s) 比超位崩解(40HP/s)强一些
            // 削弱，50HP/s - 25P/s
            ApplyDoTDebuff(abyssalFlames, 50);
            // 一秒1点，剩余的是真实伤害
            ApplyDoTDebuff(vulnerabilityHexLegacy, 10);

            Player.lifeRegen -= (int)totalPowerfulNegativeLifeRegen;

            // 孱弱巫咒的真伤，因为应该只有这两个，所以不写系统了
            if(vulnerabilityHexLegacy)
            {
                // 每秒30点真实伤害
                if (Player.miscCounter % 2 == 0)
                {
                    Player.statLife -= 1;
                }
            }
            // 深渊之火低一点
            if (abyssalFlames)
            {
                // 每秒10点真实伤害
                if (Player.miscCounter % 6 == 0)
                {
                    Player.statLife -= 1;
                }
            }
        }
        #region Update Life Regen
        public override void UpdateLifeRegen()
        {
            CalamityPlayer calPlayer = Player.Calamity();
            if (DarkSunRings) //日食指环
            {
                Player.lifeRegen += 2;
                if (Main.eclipse || Main.dayTime)
                    Player.lifeRegen += darkSunRingDayRegen;
            }
            if (AmbrosialAmpouleOld)
            {
                if (!Player.honey && Player.lifeRegen < 0)
                {
                    Player.lifeRegen += 2;
                    if (Player.lifeRegen > 0)
                        Player.lifeRegen = 0;
                }
                Player.lifeRegenTime += 1;
                Player.lifeRegen += 2;
            }
            //魔君套
            if (AncientAuricSet)
            {
                Player.lifeRegen += 60;
                //提升整合套的回血强度: 由0.7f->1.20f
                calPlayer.healingPotionMultiplier += 1.20f;
                Player.shinyStone = true;
                Player.lifeRegenTime = 1800f;
                if(calPlayer.purity) //与灾厄的纯净饰品进行联动
                    Player.lifeRegenTime = 1200f; //之前是在一半的基础上再减了一半然后发现我受击也能回血了
                if(Player.statLife <= Player.statLifeMax2 * 0.5f)
                    Player.lifeRegen += 120;
            }
            if (AncientAstralSet && Player.lifeRegen < 0 && !Player.HasBuff<AlcoholPoisoning>())
            {
                Player.lifeRegen = 4;
                Player.lifeRegenTime = 0;
            }
            if (Player.HeldItem.type == ModContent.ItemType<ShizukuSword>())
            {
                if (Player.lifeRegen < 0)
                    Player.lifeRegen = 1;
            }
            if (AncientSilvaForceRegen)
            {
                int lifeRegen = 1;
                if (Player.lifeRegen < 0 && !Player.HasBuff<AlcoholPoisoning>())
                {
                    // 储存具体的回血进度
                    // 因为回血的数值1 = 0.5HP/s，所以除120
                    int Source = Player.lifeRegen;
                    if (Source < -20)
                    {
                        Source = -20;
                    }
                    float lifeRegenTimer = Math.Abs((float)(Source / 120f));
                    AncientSilvaRegenCounter += lifeRegenTimer;
                    int RengeCount = 0;
                    while (AncientSilvaRegenCounter > 1f)
                    {
                        RengeCount++;
                        AncientSilvaRegenCounter -= 1f;
                        Player.Heal(lifeRegen);
                    }

                    if(Player.miscCounter % 15 == 0)
                        Player.Heal(lifeRegen);

                    if (AncientSilvaRegenCounter < 0f)
                        AncientSilvaRegenCounter = 0f;

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
                
                if (AncientSilvaRegenTimer > 0 && Player.statLife < Player.statLifeMax2)
                {
                    //粒子
                    for(int i = 0; i< 15; i++)
                    {
                        if (Main.rand.NextBool())
                        {
                            Vector2 offset = new Vector2(16f, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                            Vector2 velOffset = new Vector2(8f, 0).RotatedBy(offset.ToRotation());
                            float dFlyVelX = Player.velocity.X * 0.8f + velOffset.X;
                            float dFlyVelY = Player.velocity.Y * 0.8f + velOffset.Y;
                            float dScale =  1.2f;
                            Dust dust = Dust.NewDustPerfect(new Vector2(Player.Center.X, Player.Center.Y) + offset, DustID.GemEmerald, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
                            dust.noGravity = true;
                        }

                        if (Main.rand.NextBool(6))
                        {
                            Vector2 offset = new Vector2(16f, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                            Vector2 velOffset = new Vector2(8f, 0).RotatedBy(offset.ToRotation());
                            float dFlyVelX = Player.velocity.X * 0.7f + velOffset.X;
                            float dFlyVelY = Player.velocity.Y * 0.7f + velOffset.Y;
                            float dScale =  1.2f;
                            Dust dust = Dust.NewDustPerfect(new Vector2(Player.Center.X, Player.Center.Y) + offset, DustID.Vortex, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
                            dust.noGravity = true;
                        }
                    }
                    Player.AddBuff(ModContent.BuffType<SilvaPrice>(), 2);
                    int healAmt = AncientAuricSet ? 50 : 3;
                    int minCD = AncientAuricSet ? 1800 : 2700; //魔君套30sCD

                    if(Main.zenithWorld) healAmt = 10;  //林海强回血在天顶下一次回10
                    if(Main.zenithWorld) minCD = 30; //林海强回血在天顶世界下只有半秒CD
                    
                    Player.Heal(healAmt); //直接操作血量条进行回血
                    int cd = Main.rand.Next(minCD, minCD + 201);
                    AncientSilvaRegenCD = cd;
                    AncientSilvaRegenTimer--;
                }
                if (AncientSilvaRegenTimer == 0 && AncientSilvaRegenCD == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item4, Player.Center); //林海强回血准备好的时候播报这个音效
                    int minTimer = AncientAuricSet ? 60 : 30;

                    if(Main.zenithWorld) minTimer = 3600; //林海强回血在天顶世界下会强行回1分钟

                    int timer = 50; // 现在固定50帧
                    AncientSilvaRegenTimer = timer;
                }
            }
            if (RegenatorLegacy)
            {
                Player.lifeRegenTime += 8;
                Player.lifeRegen *= 2;
            }
        }
        #endregion
    }
}
