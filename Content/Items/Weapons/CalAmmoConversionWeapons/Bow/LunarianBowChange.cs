using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CalamityMod.Items.Weapons.Ranged;
using Terraria.Localization;

namespace CalamityInheritance.Content.Items.Weapons.CalAmmoConversionWeapons.Bow
{
    public class LunarianBowChange : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ModContent.ItemType<LunarianBow>();
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CalamityInheritanceConfig.Instance.AmmoConversion == true)
            {
                Vector2 source1 = player.RotatedRelativePoint(player.MountedCenter, true);
                float piOver10 = MathHelper.Pi * 0.1f;
                int projAmt = 2;

                velocity.Normalize();
                velocity *= 15f;
                bool canHit = Collision.CanHit(source1, 0, 0, source1 + velocity, 0, 0);
                for (int i = 0; i < projAmt; i++)
                {
                    float offsetAmt = i - (projAmt - 1f) / 2f;
                    Vector2 offset = velocity.RotatedBy(piOver10 * offsetAmt, default);
                    if (!canHit)
                        offset -= velocity;

                    Projectile.NewProjectile(source, source1 + offset, velocity, ModContent.ProjectileType<LunarBolt>(), damage, knockback, player.whoAmI);
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
