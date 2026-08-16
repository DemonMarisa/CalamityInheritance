using CalamityInheritance.Core.GlobalInstance.Players;

namespace CalamityInheritance.Content.Items.Armor.ArmorBonus
{
    public class AncientGodSlayerBonus
    {
        public static void AncientGodSlayerArmor_Hurt(CIPlayer player)
        {
            player.InvincibleTimer = 60;
        }
    }
}
