using CalamityInheritance.Content.Items.Accessories;
using CalamityMod;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer: ModPlayer 
    {
        public void ResetLifeMax()
        {
            float LifePercentMax = 0f;
            if (CoreOfTheBloodGod)
            {
                LifePercentMax += 0.25f;
            }
            if (AncientCotbg)
                LifePercentMax += 0.1f;
            //元素手套10%
            if (ElemGauntlet)
                LifePercentMax += 0.1f;
            if (Main.zenithWorld)
            {
                if (AncientGodSlayerSet)
                    LifePercentMax += 0.85f;
                if (AncientSilvaForceRegen)
                    LifePercentMax += 0.65f;
                if (AncientTarragonSet)
                    LifePercentMax += 0.45f;
                if (AncientBloodflareSet)
                    LifePercentMax += 0.35f;
                if (AncientAuricSet)
                {
                    //原本魔君盔甲有1.2f，加上天顶特殊给的25f
                    LifePercentMax += 300f;
                    Player.statDefense += 114514;
                }
            }
            if (LoreSkeletron)
                LifePercentMax -= 0.10f;
            if (LoreLeviAnahita)
            {
                if (Player.IsUnderwater())
                {
                    if (Player.Calamity().aquaticHeart || Player.Calamity().aquaticHeartPrevious)
                    Player.statLifeMax2 = Player.statLifeMax2 / 20;
                }
            }
            if (LoreCalamitasClone)
                Player.statLifeMax2 = (int)(Player.statLifeMax2 * 0.75f);
            if (LoreProvidence || PanelsLoreProvidence)
                LifePercentMax -= 0.1f;
            if (BuffStatsCadence)
                LifePercentMax += 0.25f;
            
            //血契在天顶世界有一定的变化：其将不会使最大生命上限+100%，相反，它会尝试把玩家获得的血量上限收益翻倍.
            if (AncientBloodPact) 
            {
                if (!Main.zenithWorld)
                    LifePercentMax += 2f;
                if (Main.zenithWorld)
                    LifePercentMax *= 2;
            }
            int StatLifeInt = 0;
            if (EHeartStats)
            {
                StatLifeInt += 15;
                if (EHeartStatsBoost)
                    StatLifeInt += 25;
            }
            //避免血量倒扣
            if (LifePercentMax < 0) 
                LifePercentMax = 0;
            //先乘算计算完，在考虑加算
            Player.statLifeMax2 += (int)(Player.statLifeMax * LifePercentMax) + StatLifeInt;
        }
    }
}