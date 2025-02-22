using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using CalamityMod;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Weapons.CalAmmoConversionWeapons.Bow
{
    public class MonsoonChange : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ModContent.ItemType<Monsoon>();
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo spawnSource, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CalamityInheritanceConfig.Instance.AmmoConversion == true)
            {
                Vector2 source = player.RotatedRelativePoint(player.MountedCenter);
                float piOver10 = MathHelper.Pi * 0.1f;
                int totalProjectiles = 5;

                velocity.Normalize();
                velocity *= 40f;
                bool canHit = Collision.CanHit(source, 0, 0, source + velocity, 0, 0);
                for (int p = 0; p < totalProjectiles; p++)
                {
                    float offsetAmt = p - (totalProjectiles - 1f) / 2f;
                    Vector2 offset = velocity.RotatedBy(piOver10 * offsetAmt);
                    if (!canHit)
                        offset -= velocity;

                        int newType = type;
                        switch (p)
                        {
                            case 0:
                            case 1:
                            case 3:
                            case 4:
                                newType = ModContent.ProjectileType<MiniSharkron>();
                                break;
                            case 2:
                                newType = ModContent.ProjectileType<TyphoonArrow>();
                                break;
                        }
                        int proj = Projectile.NewProjectile(spawnSource, source.X + offset.X, source.Y + offset.Y, velocity.X, velocity.Y, newType, (int)(damage * 1.1), knockback, player.whoAmI);
                        if (proj.WithinBounds(Main.maxProjectiles))
                        {
                            Main.projectile[proj].arrow = true;
                            Main.projectile[proj].extraUpdates += 1;
                        }
                    
                }
                return false;
            }
            else
                return true;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;

            if (CalamityInheritanceConfig.Instance.AmmoConversion == true)
            {
                string AmmoConversionOn = Language.GetTextValue("Mods.CalamityInheritance.ConfigsMessage.AmmoConversion");

                tooltips.Add(new TooltipLine(Mod, "AmmoConversion", AmmoConversionOn));
            }
        }
    }
}
