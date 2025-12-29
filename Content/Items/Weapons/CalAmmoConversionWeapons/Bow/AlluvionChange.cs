using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Weapons.CalAmmoConversionWeapons.Bow
{
    public class AlluvionChange : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemType<Alluvion>();
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo spawnSource, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CIConfig.Instance.AmmoConversion == true)
            {
                Vector2 source = player.RotatedRelativePoint(player.MountedCenter);
                float tenthPi = MathHelper.Pi * 0.1f;
                int totalProjectiles = 6;

                velocity.Normalize();
                velocity *= 35f;
                bool canHit = Collision.CanHit(source, 0, 0, source + velocity, 0, 0);
                for (int i = 0; i < totalProjectiles; i++)
                {
                    float arrowOffset = i - (totalProjectiles - 1f) / 2f;
                    Vector2 offset = velocity.RotatedBy(tenthPi * arrowOffset);
                    if (!canHit)
                        offset -= velocity;

                        int newType = type;
                        switch (i)
                        {
                            case 0:
                            case 5:
                                newType = ProjectileType<TyphoonArrow>();
                                break;
                            case 1:
                            case 4:
                                newType = ProjectileType<MiniSharkron>();
                                break;
                            case 2:
                            case 3:
                                newType = ProjectileType<TorrentialArrow>();
                                break;
                        }
                        int proj = Projectile.NewProjectile(spawnSource, source.X + offset.X, source.Y + offset.Y, velocity.X, velocity.Y, newType, (int)(damage * 2.3f), knockback, player.whoAmI);
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

            if (CIConfig.Instance.AmmoConversion == true)
            {
                string AmmoConversionOn = Language.GetTextValue("Mods.CalamityInheritance.ConfigsMessage.AmmoConversion");

                tooltips.Add(new TooltipLine(Mod, "AmmoConversion", AmmoConversionOn));
            }
        }
    }
}