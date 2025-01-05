using CalamityMod.CalPlayer;
using CalamityMod.Cooldowns;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.CICooldowns
{
    public class AuricSilvaRevive : CooldownHandler
    {
        public static new string ID => "AuricSilvaRevive";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => CalamityInheritanceUtils.GetText($"UI.Cooldowns.{ID}");
        public override string Texture => "CalamityInheritance/CICooldowns/SilvaRevive";
        public override Color OutlineColor => new Color(151, 211, 152);
        public override Color CooldownStartColor => new Color(226, 188, 74);
        public override Color CooldownEndColor => new Color(151, 211, 152);
    }
}
