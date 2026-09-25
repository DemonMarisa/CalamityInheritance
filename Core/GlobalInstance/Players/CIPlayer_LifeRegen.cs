using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public int LifeRegen;
        public int BadLifeRegen;
        public bool BlockLifeRegen;
        public override void UpdateLifeRegen()
        {
            ArmorLifeRegen();
            Player.lifeRegen += LifeRegen;
            LifeRegen = 0;
            if (RegenatorLegacy)
            {
                Player.lifeRegenTime += 8;
                Player.lifeRegen += 12;
                Player.lifeRegen *= 2;
            }
        }
        public override void UpdateBadLifeRegen()
        {
            if (BlockLifeRegen)
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;
                BlockLifeRegen = false;
            }
            Player.lifeRegen -= BadLifeRegen;
            BadLifeRegen = 0;

            if (AstralArcanumRegen)
            {
                if (Player.lifeRegen < 0)
                {
                    if (Player.lifeRegenTime < 1800)
                        Player.lifeRegenTime = 1800;

                    Player.lifeRegen += 6;
                    Player.statDefense += 20;
                }
                else
                    Player.lifeRegen += 3;
            }
        }
    }
}
