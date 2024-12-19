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
    }
}
