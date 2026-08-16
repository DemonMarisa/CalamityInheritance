using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Core.Utils;
using LAP.Core.LAPUI.CustomCD;
using Terraria;
using Terraria.Audio;
using Terraria.Localization;

namespace CalamityInheritance.Content.CDs
{
    public class EnergyShieldCD : BaseCD
    {
        public override void OnRegister()
        {
            Buff = false;
            DeBuff = false;
            Info = true;
            CD = true;
        }
        public override LocalizedText DisplayName()
        {
            return CIUtils.GetText($"UI.Cooldowns.EnergyShieldCD");
        }
        public override void OnComplete(Player player)
        {
            SoundEngine.PlaySound(CISounds.RoverDriveActivate, player.Center);
        }
    }
}
