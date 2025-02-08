using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace CalamityInheritance.Content.Items.Weapons.CalAmmoConversionWeapons.Bow
{
    public class FlarewingBowChange : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ModContent.ItemType<FlarewingBow>();
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CalamityInheritanceConfig.Instance.AmmoConversion == true)
            {
                Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
                float tenthPi = 0.314159274f;
                Vector2 arrowVel = velocity;
                arrowVel.Normalize();
                arrowVel *= 50f;
                bool arrowHitsTiles = Collision.CanHit(vector2, 0, 0, vector2 + arrowVel, 0, 0);
                for (int i = 0; i < 3; i++)
                {
                    float piOffsetValue = (float)i - 1f;
                    Vector2 offsetSpawn = arrowVel.RotatedBy((double)(tenthPi * piOffsetValue), default);
                    if (!arrowHitsTiles)
                    {
                        offsetSpawn -= arrowVel;
                    }
                    int arrowSpawn = Projectile.NewProjectile(source, vector2.X + offsetSpawn.X, vector2.Y + offsetSpawn.Y, velocity.X, velocity.Y, ModContent.ProjectileType<FlareBat>(), damage, knockback, player.whoAmI);
                    Main.projectile[arrowSpawn].noDropItem = true;
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
