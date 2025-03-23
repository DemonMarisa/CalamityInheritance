using CalamityMod.CalPlayer;
using Terraria;
using static Terraria.Player;
using CalamityMod;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Projectiles;
using System.Reflection;
using System;
using Terraria.ModLoader;
using CalamityMod.Balancing;
using Microsoft.Xna.Framework.Graphics;

namespace CalamityInheritance.Utilities
{
    public static partial class CIFunction
    {
        #region Cooldowns
        // 移除冷却
        public static void RemoveCooldown(this Player player, string id)
        {
            CalamityPlayer calamityPlayer = player.Calamity();
            CalamityInheritancePlayer inheritancePlayer = player.CIMod();

            RemoveCooldownFromModPlayer(calamityPlayer, id);
            RemoveCooldownFromModPlayer(inheritancePlayer, id);
        }
        private static void RemoveCooldownFromModPlayer(dynamic player, string id)
        {
            player.cooldowns.Remove(id);
        }
        #endregion
        public static CalamityInheritanceGlobalProjectile CalamityInheritance(this Projectile proj)
        {
            return proj.GetGlobalProjectile<CalamityInheritanceGlobalProjectile>();
        }

        /// <summary>
        /// Gets the total amount of extra immunity frames from a hit granted by various Calamity effects.
        /// </summary>
        /// <param name="player">The player whose extra immunity frames are being computed.</param>
        /// <returns>The amount of extra immunity frames to grant.</returns>
        public static int GetExtraHitIFrames(this Player player, HurtInfo hurtInfo)
        {
            CalamityInheritancePlayer modPlayer = player.CIMod();
            int extraIFrames = 0;
            // Ozzatron 20FEB2024: Moved extra iframes from Seraph Tracers to Rampart of Deities to counteract its loss of Charm of Myths
            // This stacks with the above Deific Amulet effect
            if (modPlayer.AuricTracersFrames && hurtInfo.Damage > 200)
                extraIFrames += 30;
            if (modPlayer.RoDPaladianShieldActive)
                extraIFrames += 30;
            if (modPlayer.YharimsInsignia)
                extraIFrames += 40;
            return extraIFrames;
        }
    }
}
