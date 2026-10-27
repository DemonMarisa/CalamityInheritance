using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorBonus
{
    public class AuricBonus
    {
        public static void Main_PostUpdate(Player player)
        {
            if (player.CI().Stealth != 0)
            {
                player.GetDamage<GenericDamageClass>() += player.CI().Stealth * 0.3f;
                player.GetCritChance<GenericDamageClass>() += player.CI().Stealth * 15;
            }
        }
    }
}
