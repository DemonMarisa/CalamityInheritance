using CalamityInheritance.Core.Utils;
using CalamityMod;
using System;
using Terraria;
using Terraria.ModLoader;

#pragma warning disable RS0030

namespace CalamityInheritance.Common.CalamityModCross.RogueCheck
{
    public static class CalRogueCheck
    {
        [JITWhenModsEnabled("CalamityMod")]
        public static bool CheckStealth(this Player player)
        {
            return player.Calamity().StealthStrikeAvailable();
        }
        [JITWhenModsEnabled("CalamityMod")]
        public static float GetStealthFocuseMult(this Player player)
        {
            float num = 1f;
            if (player.Calamity().stealthStrikeHalfCost)
            {
                num = 0.5f;
            }
            else if (player.Calamity().stealthStrike75Cost)
            {
                num = 0.75f;
            }
            else if (player.Calamity().stealthStrike90Cost)
            {
                num = 0.9f;
            }
            return num;
        }
        public static void SetStealthAttack(this Projectile proj)
        {
            proj.CI().Stealth = true;
            if (CIUtils.HasCalamity())
                proj.SetCalamityStealth();

        }
        [JITWhenModsEnabled("CalamityMod")]
        internal static void SetCalamityStealth(this Projectile proj)
        {
            proj.Calamity().stealthStrike = true;
        }
    }
}
