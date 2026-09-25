using CalamityInheritance.Core.Keys;
using CalamityInheritance.Core.UI;
using LAP.Core.MiscDate;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (CIKeybinds.AstralArcanumUIHotkey.JustPressed && AstralArcanumRegen && !LAPInfo.AnyBossHere)
            {
                AstralArcanumUI.Toggle();
            }
            if (CIKeybinds.AegisHotKey.JustPressed)
            {
                if (ElysianAegis)
                {
                    ElysianGuard = !ElysianGuard;
                }
            }
        }
    }
}
