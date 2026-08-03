using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Core.Utils;
using CalamityMod;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

#pragma warning disable RS0030

namespace CalamityInheritance.Common.CalamityModCross
{
    public static class CalCrossUtils
    {
        [JITWhenModsEnabled("CalamityMod")]
        public static void ChargeCalamityItem(Player player)
        {
            SoundEngine.PlaySound(CISounds.AuricQuantumCoolingCellInstallNew, Main.player[Main.myPlayer].Center);
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i].type > ItemID.Count && player.inventory[i].Calamity().UsesCharge)
                    player.inventory[i].Calamity().Charge = player.inventory[i].Calamity().MaxCharge;
            }
        }
        public static void SetRogueArmor(this Player player, float stealthMax, bool halfCost = false)
        {
            if (CIUtils.HasCalamity())
            {
                player.RogueArmor_Jit(stealthMax, halfCost);
            }
        }

        [JITWhenModsEnabled("CalamityMod")]
        internal static void RogueArmor_Jit(this Player player, float stealthMax, bool halfCost = false)
        {
            player.Calamity().wearingRogueArmor = true;
            player.Calamity().rogueStealthMax += stealthMax;
            if (halfCost)
                player.Calamity().stealthStrikeHalfCost = true;
        }
    }
}
