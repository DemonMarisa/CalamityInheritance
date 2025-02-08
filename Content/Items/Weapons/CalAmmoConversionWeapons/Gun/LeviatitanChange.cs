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
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Items.Weapons.CalAmmoConversionWeapons.Gun
{
    public class LeviatitanChange : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ModContent.ItemType<Leviatitan>();
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CalamityInheritanceConfig.Instance.AmmoConversion == true)
            {
                float SpeedX = velocity.X + Main.rand.Next(-10, 11) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-10, 11) * 0.05f;

                    if (Main.rand.NextBool(3))
                        Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<AquaBlastToxic>(), (int)(damage * 1.5), knockback, player.whoAmI);
                    else
                        Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<AquaBlast>(), damage, knockback, player.whoAmI);
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
