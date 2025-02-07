using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CalamityMod.Balancing;
using CalamityMod.CalPlayer;
using CalamityMod.Cooldowns;
using CalamityMod.Events;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Enums;
using static Terraria.Player;
using CalamityMod;
using CalamityInheritance.CIPlayer;
using CalamityMod.Projectiles;
using CalamityInheritance.Content.Projectiles;

namespace CalamityInheritance.Utilities
{
    public static partial class CalamityInheritanceUtils
    {
        #region Cooldowns

        // 移除冷却从
        public static void RemoveCooldown(this Player player, string id)
        {
                CalamityPlayer calamityPlayer = player.Calamity();
                CalamityInheritancePlayer inheritancePlayer = player.CalamityInheritance();

                if (calamityPlayer != null)
                {
                    RemoveCooldownFromModPlayer(calamityPlayer, id);
                }

                if (inheritancePlayer != null)
                {
                    RemoveCooldownFromModPlayer(inheritancePlayer, id);
                }
        }
        private static void RemoveCooldownFromModPlayer(dynamic player, string id)
        {
            if (player.cooldowns.ContainsKey(id))
            {
                player.cooldowns.Remove(id);
                //Main.NewText($"成功移除冷却: {id}", 0, 255, 0);
            }
            else
            {
                //Main.NewText($"冷却 {id} 不存在，无法移除", 255, 0, 0);
            }
        }
        #endregion
    }
    public static class ProjectileExtensions
    {
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
            CalamityInheritancePlayer modPlayer = player.CalamityInheritance();
            int extraIFrames = 0;
            // Ozzatron 20FEB2024: Moved extra iframes from Seraph Tracers to Rampart of Deities to counteract its loss of Charm of Myths
            // This stacks with the above Deific Amulet effect
            if (modPlayer.fasterAuricTracers && hurtInfo.Damage > 200)
                extraIFrames += 30;
            if (modPlayer.RoDPaladianShieldActive)
                extraIFrames += 30;
            if(modPlayer.YharimsInsignia)
                extraIFrames += 40;
            return extraIFrames;
        }
    }
}
