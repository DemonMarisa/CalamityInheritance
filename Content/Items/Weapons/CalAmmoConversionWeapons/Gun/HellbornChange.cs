using CalamityMod.Items.Weapons.Ranged;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Weapons.CalAmmoConversionWeapons.Gun
{
    public class HellbornChange : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ItemType<Hellborn>();
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CIConfig.Instance.AmmoConversion == true)
            {
                for (int index = 0; index < 3; index++)
                {
                    float SpeedX = velocity.X + Main.rand.Next(-15, 16) * 0.05f;
                    float SpeedY = velocity.Y + Main.rand.Next(-15, 16) * 0.05f;

                        int bullet = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ProjectileID.ExplosiveBullet, damage, knockback, player.whoAmI);
                        Main.projectile[bullet].usesLocalNPCImmunity = true;
                        Main.projectile[bullet].localNPCHitCooldown = 10;

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
