using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items
{
    public partial class CalamityInheritanceGlobalItem : GlobalItem
    {
        #region GrabChanges
        public override void GrabRange(Item item, Player player, ref int grabRange)
        {
            CalamityInheritancePlayer modPlayer = player.CalamityInheritance();
            int itemGrabRangeBoost = 0 +
                (modPlayer.wallOfFleshLore ? 100 : 0) +
                (modPlayer.planteraLore ? 150 : 0) +
                (modPlayer.polterghastLore ? 300 : 0);

            grabRange += itemGrabRangeBoost;
        }
        #endregion
        #region Shoot
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockBack)
        {
            CalamityInheritancePlayer modPlayer = player.CalamityInheritance();

            if (modPlayer.wallOfFleshLore)
                velocity *= 1.25f;
            if (modPlayer.planteraLore)
                velocity *= 1.5f;
            if (modPlayer.polterghastLore)
                velocity *= 2;
        }
        #endregion
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            CalamityInheritancePlayer modPlayer = player.CalamityInheritance();
            CalamityPlayer modPlayer1 = player.Calamity();
            if (modPlayer.godSlayerRangedold && modPlayer1.canFireGodSlayerRangedProjectile)
            {
                if (item.CountsAsClass<RangedDamageClass>() && !item.channel)
                {
                    modPlayer1.canFireGodSlayerRangedProjectile = false;
                    if (player.whoAmI == Main.myPlayer)
                    {
                        // God Slayer Ranged Shrapnel: 100%, soft cap starts at 800 base damage
                        int shrapnelRoundDamage = damage * 2;
                        shrapnelRoundDamage = player.ApplyArmorAccDamageBonusesTo(shrapnelRoundDamage);

                        Projectile.NewProjectile(source, position, velocity * 1.25f, ModContent.ProjectileType<GodSlayerShrapnelRound>(), shrapnelRoundDamage, 2f, player.whoAmI);
                    }
                }
            }
            return true;
        }
    }
}
