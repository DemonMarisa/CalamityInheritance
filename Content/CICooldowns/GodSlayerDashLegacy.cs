using CalamityInheritance.Utilities;
using LAP.Core.LAPUI.CustomCD;
using Terraria.Localization;

namespace CalamityInheritance.Content.CICooldowns
{
    public class GodSlayerDashLegacy : BaseCD
    {
        public override void OnRegister()
        {
            Buff = false;
            DeBuff = false;
            Info = true;
        }
        public override LocalizedText DisplayName()
        {
            return CIFunction.GetText($"UI.Cooldowns.GodSlayerCooldown");
        }
    }
}
