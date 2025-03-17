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
                LifePercentMax += 0.25f;
            if (AncientBloodPact)
                LifePercentMax += 2f;
            if (AncientCotbg)
                LifePercentMax += 0.1f;
            if (AncientTarragonSet)
                LifePercentMax += 0.45f;
            if (AncientBloodflareStat)
                LifePercentMax += 0.35f;
            if (AncientGodSlayerStat)
                LifePercentMax += 0.85f;
            if (AncientSilvaStat)
                LifePercentMax += 0.65f;
            if (AncientAuricSet)
                LifePercentMax += Main.zenithWorld ? 25f : 1.20f; 
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
            if (LoreProvidence)
                LifePercentMax -= 0.15f;
            if (BuffStatsCadence)
                LifePercentMax += 0.25f;
            int StatLifeInt = 0;
            if (EHeartStats)
            {
                StatLifeInt += 15;
                if (EHeartStatsBoost)
                StatLifeInt += 25;
            }
            //避免血量倒扣
            if (LifePercentMax < 0) LifePercentMax = 0;
            //先乘算计算完，在考虑加算
            Player.statLifeMax2 += (int)(Player.statLifeMax * LifePercentMax) + StatLifeInt;



        }
    }
}