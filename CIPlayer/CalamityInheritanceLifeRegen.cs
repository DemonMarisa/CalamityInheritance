using CalamityInheritance.Utilities;
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

        }
        #endregion
        #region Update Life Regen
        public override void UpdateLifeRegen()
        {
            if (darkSunRingold)
            {
                if (Main.eclipse || Main.dayTime)
                    Player.lifeRegen += Main.eclipse ? 3 : 3;
            }
        }
        #endregion
    }
}
