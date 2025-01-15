using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Accessories;
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

            if (astralArcanum)
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
            if (darkSunRingold)
            {
                if (Main.eclipse || Main.dayTime)
                    Player.lifeRegen += Main.eclipse ? 6 : 6;
            }
        }
        #endregion
    }
}
