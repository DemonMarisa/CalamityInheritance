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
            Player.lifeRegen += LifeRegen;
            LifeRegen = 0;
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
        }
    }
}
