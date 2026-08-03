using CalamityInheritance.Content.Projectiles.Armor.Ranged;
using CalamityInheritance.Core.GlobalInstance.Players;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorBonus
{
    public class DesertProwlerBonus
    {
        public static void DesertProwlerArmorBonus_OnHitNPCProj(CIPlayer ciplayer, Player player, Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (ciplayer.DesertProwler)
            {
                if (Main.myPlayer == player.whoAmI && Main.rand.NextBool(10) && proj.type != ProjectileType<DesertMark>() && proj.type != ProjectileType<DesertTornado>())
                {
                    int mark = ProjectileType<DesertMark>();
                    bool noTornado = player.ownedProjectileCounts[mark] < 1 && player.ownedProjectileCounts[ProjectileType<DesertTornado>()] < 1;
                    if (noTornado)
                        Projectile.NewProjectile(player.GetSource_FromThis(), proj.Center, Vector2.Zero, mark, CIUtils.DamageSoftCap(hit.SourceDamage * 2, 120), 0f, player.whoAmI);
                }
            }
        }
    }
}
