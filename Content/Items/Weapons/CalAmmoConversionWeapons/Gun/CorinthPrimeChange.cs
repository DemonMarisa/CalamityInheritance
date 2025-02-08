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
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.CalAmmoConversionWeapons.Gun
{
    public class CorinthPrimeChange : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ModContent.ItemType<CorinthPrime>();
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CalamityInheritanceConfig.Instance.AmmoConversion == true)
            {
                if (player.altFunctionUse == 2)
                {
                    Projectile.NewProjectile(source, position + Vector2.Normalize(velocity) * 60f, velocity, ModContent.ProjectileType<CorinthPrimeAirburstGrenade>(), damage, knockback, player.whoAmI);
                }
                else
                {
                    int numBullets = 6;
                    for (int index = 0; index < numBullets; index++)
                    {
                        float SpeedX = velocity.X + Main.rand.Next(-30, 31) * 0.05f;
                        float SpeedY = velocity.Y + Main.rand.Next(-30, 31) * 0.05f;
                        int proj = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<RealmRavagerBullet>(), damage, knockback, player.whoAmI);
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
