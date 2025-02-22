using CalamityMod.Cooldowns;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.CICooldowns
{
    public class GodSlayerCooldown : CooldownHandler
    {
        public static new string ID => "GodSlayerCooldown";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => CalamityInheritanceUtils.GetText($"UI.Cooldowns.{ID}");
        public override string Texture => "CalamityInheritance/CICooldowns/GodSlayerCooldown";
        public override Color OutlineColor => Color.Lerp(new Color(252, 109, 203), new Color(58, 91, 146), instance.Completion);
        public override Color CooldownStartColor => new Color(148, 62, 216);
        public override Color CooldownEndColor => new Color(255, 187, 207);
    }
}
