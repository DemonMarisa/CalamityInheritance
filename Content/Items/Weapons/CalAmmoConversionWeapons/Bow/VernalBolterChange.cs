using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
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
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Items.Weapons.CalAmmoConversionWeapons.Bow
{
    public class VernalBolterChange : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ModContent.ItemType<VernalBolter>();
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo spawnSource, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CalamityInheritanceConfig.Instance.AmmoConversion == true)
            {
                Vector2 source = player.RotatedRelativePoint(player.MountedCenter, true);
                float piOver10 = MathHelper.Pi * 0.1f;
                int projAmt = 3;

                velocity.Normalize();
                velocity *= 30f;
                bool canHit = Collision.CanHit(source, 0, 0, source + velocity, 0, 0);
                for (int i = 0; i < projAmt; i++)
                {
                    float offsetAmt = i - (projAmt - 1f) / 2f;
                    Vector2 offset = velocity.RotatedBy(piOver10 * offsetAmt, default);
                    if (!canHit)
                        offset -= velocity;

                    Projectile.NewProjectile(spawnSource, source + offset, velocity, ModContent.ProjectileType<VernalBolt>(), (int)(damage * 1.2), knockback * 1.2f, player.whoAmI);
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
