using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        #region Update Bad Life Regen
        public override void UpdateBadLifeRegen()
        {
            CalamityInheritancePlayer modPlayer = Player.CalamityInheritance();

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
        }
        #endregion
        #region Update Life Regen
        public override void UpdateLifeRegen()
        {
            CalamityPlayer calPlayer = Player.Calamity();
            if (darkSunRingold) //日食指环
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
            if (hotEStats)
            {
                Player.lifeRegen += 2;
                if(buffEStats)
                Player.lifeRegen += 8;      //5(1+4)HP/s
            }
            //魔君套
            if (auricYharimSet)
            {
                Player.lifeRegen += 60;

                calPlayer.healingPotionMultiplier += 0.70f; //将血药恢复提高至70%，这样能让300的大血药在不依靠血神核心的情况下能直接恢复500以上的血量
                Player.shinyStone = true;
                Player.lifeRegenTime = 1800f;
                if(calPlayer.purity == true) //与灾厄的纯净饰品进行联动
                    Player.lifeRegenTime = 1200f; //之前是在一半的基础上再减了一半然后发现我受击也能回血了
                if(Player.statLife <= Player.statLifeMax2 * 0.5f)
                    Player.lifeRegen += 120;
            }
        }
        #endregion
    }
}
