using CalamityInheritance.Core.Utils;
using CalamityMod;
using CalamityMod.CalPlayer;
using Terraria;
using Terraria.ModLoader;

#pragma warning disable RS0030

namespace CalamityInheritance.Common.CalamityModCross
{
    public class CalPlayerInfo : ModPlayer
    {
        public bool ZoneAstral = false;
        public bool astralInjection = false;
        public override void ResetEffects()
        {
            ZoneAstral = false;
            if (CIUtils.HasCalamity())
                CheckZone();
        }
        [JITWhenModsEnabled("CalamityMod")]
        public void CheckZone()
        {
            CalamityPlayer modPlayer = Player.Calamity();
            ZoneAstral = modPlayer.ZoneAstral;
            astralInjection = modPlayer.astralInjection;
        }
    }
}
