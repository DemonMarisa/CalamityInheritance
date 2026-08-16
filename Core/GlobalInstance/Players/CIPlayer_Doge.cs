using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public int InvincibleTimer;
        public bool FreeDodgeThisDamage;
        public void ResetDodge()
        {
            if (InvincibleTimer > 0)
                InvincibleTimer--;
            FreeDodgeThisDamage = false;
        }
        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (FreeDodgeThisDamage)
                return true;
            if (InvincibleTimer > 0)
                return true;
            if (GodSlayerSet && info.Damage < 80)
                return true;
            return false;
        }
    }
}
