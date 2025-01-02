using System;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Cooldowns;
using CalamityMod.Items.Accessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using static CalamityMod.CalamityUtils;
using static Terraria.ModLoader.ModContent;

namespace CalamityInheritance.CICooldowns
{
    public class DraconicElixirCooldown : CooldownHandler
    {
        public static new string ID => "DraconicElixirCooldown";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => CalamityInheritanceUtils.GetText($"UI.Cooldowns.{ID}");
        public override string Texture => "CalamityInheritance/CICooldowns/DraconicElixirCooldown";
        public override string OutlineTexture => "CalamityInheritance/CICooldowns/DraconicElixirCooldownOutline";
        public override string OverlayTexture => "CalamityInheritance/CICooldowns/DraconicElixirCooldownOverlay";
        public override Color OutlineColor => Color.Lerp(new Color(141, 199, 90), new Color(221, 187, 106), (float)Math.Sin(Main.GlobalTimeWrappedHourly) * 0.5f + 0.5f);
        public override Color CooldownStartColor => new Color(165, 22, 46);
        public override Color CooldownEndColor => new Color(216, 103, 43);
    }
}
