using CalamityInheritance.Core.GlobalInstance.Players;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Armor.ArmorBonus
{
    public class AncientBloodflare
    {
        public static void AncientBloodFlareArmorHitNPC(CIPlayer player, NPC target)
        {
            if (player.Player.whoAmI != Main.myPlayer)
                return;
            if (target.lifeMax > 5)
            {
                if (!player.HasCount("AncientBloodflareArmorFlightCD"))
                {
                    Item.NewItem(target.GetSource_FromThis(), target.Hitbox, ItemID.Heart);
                    Item.NewItem(target.GetSource_FromThis(), target.Hitbox, ItemID.Star);
                    player.AddCount("AncientBloodflareArmorFlightCD", 45);
                }
            }
        }
        public static void AncientBloodFlareArmorHurt(CIPlayer player, Player.HurtInfo info)
        {
            if (!player.HasCount("AncientBloodflareArmorFlightCD"))
            {
                player.Player.NCHeal((int)(info.Damage * 1.5f));
                player.AddCount("AncientBloodflareArmorFlightCD", 20 * 60);
            }
        }
    }
}
