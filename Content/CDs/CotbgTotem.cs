using LAP.Core.LAPUI.CustomCD;
using Terraria.Localization;

namespace CalamityInheritance.Content.CDs
{
    public class CotbgTotem : BaseCD
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
            return GetText($"UI.Cooldowns.CotbgTotem");
        }
    }
}