using CalamityInheritance.Core.Utils;
using LAP.Core.LAPUI.CustomCD;
using Terraria.Localization;

namespace CalamityInheritance.Content.CDs
{
    public class BloodflareRanged : BaseCD
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
            return CIUtils.GetText($"UI.Cooldowns.BloodflareRanged");
        }
    }
}
