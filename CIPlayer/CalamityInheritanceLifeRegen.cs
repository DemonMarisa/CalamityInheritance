using CalamityInheritance.Buffs.Statbuffs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.Alcohol;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        #region Update Bad Life Regen
        public override void UpdateBadLifeRegen()
        {
            CalamityInheritancePlayer modPlayer = Player.CIMod();

            if (AstralArcanumEffect)
            {
                bool lesserEffect = false;
                for (int l = 0; l < Player.MaxBuffs; l++)
                {
                    int hasBuff = Player.buffType[l];
                    lesserEffect = CalamityLists.alcoholList.Contains(hasBuff);
                }

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
        }
        #endregion
        public void TotalDebuff()
        {
            // 死亡模式debuff伤害+25%
            float deathNegativeRegenBonus = 0.25f;
            float calamityDebuffMultiplier = 1f + (CalamityWorld.death ? deathNegativeRegenBonus : 0f);

            // 总共降低的生命值
            float totalNegativeLifeRegen = 0;

            void ApplyDoTDebuff(bool hasDebuff, int negativeLifeRegenToApply, bool immuneCondition = false)
            {
                if (!hasDebuff || immuneCondition)
                    return;

                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;

                Player.lifeRegenTime = 0;
                totalNegativeLifeRegen += negativeLifeRegenToApply * calamityDebuffMultiplier;
            }

            ApplyDoTDebuff(abyssalFlames, 150);
            ApplyDoTDebuff(vulnerabilityHexLegacy, 32);

            Player.lifeRegen -= (int)totalNegativeLifeRegen;
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
            //元灵之心
            if (EHeartStats)
            {
                Player.lifeRegen += 2;
                if(EHeartStatsBoost)
                Player.lifeRegen += 8;      //5(1+4)HP/s
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
            }
            if (AncientSilvaSet)
            {
                //旧林海新增: 生命再生速度无法低于0
                if (Player.lifeRegen < 0 && !Player.HasBuff<AlcoholPoisoning>())
                    Player.lifeRegen = 8; //承受Debuff伤害时获得4HP/s
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
                    int healAmt = AncientAuricSet ? 5 : 3;
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

                    int timer = Main.rand.Next(minTimer, minTimer + 15); //回血刻由45 -> 55内随机
                    AncientSilvaRegenTimer = timer;
                }
            }
        }
        #endregion
    }
}
