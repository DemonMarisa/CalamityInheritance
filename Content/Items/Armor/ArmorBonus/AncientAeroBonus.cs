using CalamityInheritance.Core.GlobalInstance.Players;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Armor.ArmorBonus
{
    public class AncientAeroBonus
    {
        public static void AncientAeroArmorBonus_PostUpdate(CIPlayer ciplayer, Player player)
        {
            if (ciplayer.AncientAeroWings)
            {
                if (ciplayer.HasCount("AncientAeroArmorFlightCD"))
                {
                    player.LAP().PostWingTimeMaxMult = 0.5f;
                    player.LAP().InfiniteFlight = false;
                }
                else
                {
                    player.LAP().InfiniteFlight = true;
                    player.AddBuff(BuffID.Featherfall, 2);
                }
            }
        }
        public static void AncientAeroArmorHurt(CIPlayer player)
        {
            if (player.Player.whoAmI != Main.myPlayer)
                return;
            if (player.AncientAeroWings)
            {
                if (!player.HasCount("AncientAeroArmorFlightCD"))
                    player.AddCount("AncientAeroArmorFlightCD", 450);
                else if (player.CICD.TryGetValue("AncientAeroArmorFlightCD", out _))
                    player.CICD["AncientAeroArmorFlightCD"] = 450;
            }
        }
    }
}
