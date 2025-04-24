using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CalamityInheritance.CICooldowns
{
    public class Totem: CooldownHandler
    {
        public static new string ID => "Totem";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => CIFunction.GetText($"UI.Cooldowns.{ID}");
        public override string Texture => "CalamityInheritance/CICooldowns/Totem";
        public override Color OutlineColor => new (157, 248, 234);
        public override Color CooldownStartColor => new (111, 169, 241);
        public override Color CooldownEndColor => new (111, 169, 241);
    }
}
