using CalamityInheritance.Core.Utils;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Potions;
using Terraria;
using Terraria.ModLoader;

#pragma warning disable RS0030

namespace CalamityInheritance.Common.CalamityModCross
{
    public class CalPlayerInfo : ModPlayer
    {
        public bool ZoneAstral = false;
        public bool ZoneAbyss = false;
        public bool ZoneSunkenSea = false;
        public bool astralInjection = false;
        public bool ZoneSulphur = false;
        public override void ResetEffects()
        {
            ZoneAstral = false;
            ZoneAbyss = false;
            ZoneSunkenSea = false;
            astralInjection = false;
            ZoneSulphur = false;
            if (CIUtils.HasCalamity())
                CheckZone();
        }
        [JITWhenModsEnabled("CalamityMod")]
        public void CheckZone()
        {
            CalamityPlayer modPlayer = Player.Calamity();
            ZoneAstral = modPlayer.ZoneAstral;
            astralInjection = modPlayer.astralInjection;
            ZoneAbyss = modPlayer.ZoneAbyss;
            ZoneSunkenSea = modPlayer.ZoneSunkenSea;
            ZoneSulphur = modPlayer.ZoneSulphur;
        }
    }
}
