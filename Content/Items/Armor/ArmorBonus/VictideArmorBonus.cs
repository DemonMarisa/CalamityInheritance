using CalamityInheritance.Content.Projectiles.Typeless.HomeIn;
using CalamityInheritance.Core.GlobalInstance.Players;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace CalamityInheritance.Content.Items.Armor.ArmorBonus
{
    public class VictideArmorBonus
    {
        public static void VicTideArmorBonus(CIPlayer ciplayer,Item item, Player player, EntitySource_ItemUse_WithAmmo source, int damage, float knock)
        {
            if (ciplayer.victideSet)
            {
                if (Main.myPlayer == player.whoAmI && Main.rand.NextBool(10))
                {
                    Vector2 vel = LAPUtilities.GetVector2(player.Center, player.LocalMouseWorld()) * 9f;
                    Projectile.NewProjectile(source, player.Center, vel, ProjectileType<VictideShell>(), CIUtils.DamageSoftCap(damage * 2, 60), knock, player.whoAmI);
                }
            }
        }
    }
}
