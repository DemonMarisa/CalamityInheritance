using CalamityInheritance.Utilities;
using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace CalamityInheritance.CICooldowns
{
    public class CotbgTotem : CooldownHandler
    {
        public static new string ID => "CotbgTotem";
        public override LocalizedText DisplayName => CIFunction.GetText($"UI.Cooldowns.{ID}");
        public override bool ShouldDisplay => true;
        public override string Texture => "CalamityInheritance/CICooldowns/CotbgTotem";
        public override Color OutlineColor =>  new(255, 162, 205);
        public override Color CooldownStartColor => new(193, 205, 255);
        public override Color CooldownEndColor => new(255, 193, 219);
    }
}