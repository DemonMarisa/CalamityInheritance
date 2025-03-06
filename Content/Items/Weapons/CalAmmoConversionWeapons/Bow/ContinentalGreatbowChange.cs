using CalamityMod.Items.Weapons.Ranged;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Weapons.CalAmmoConversionWeapons.Bow
{
    public class ContinentalGreatbowChange : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ModContent.ItemType<ContinentalGreatbow>();
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo spawnSource, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CIConfig.Instance.AmmoConversion == true)
            {
                Vector2 source = player.RotatedRelativePoint(player.MountedCenter, true);
                float piOverTen = MathHelper.Pi * 0.1f;
                int arrowAmt = 3;

                velocity.Normalize();
                velocity *= 40f;
                bool canHit = Collision.CanHit(source, 0, 0, source + velocity, 0, 0);
                for (int projIndex = 0; projIndex < arrowAmt; projIndex++)
                {
                    float offsetAmt = projIndex - (arrowAmt - 1f) / 2f;
                    Vector2 offset = velocity.RotatedBy((double)(piOverTen * offsetAmt), default);
                    if (!canHit)
                    {
                        offset -= velocity;
                    }
                    type = ProjectileID.FireArrow;
                    
                    int baseArrow = Projectile.NewProjectile(spawnSource, source + offset, velocity, type, damage, knockback, player.whoAmI);
                    Main.projectile[baseArrow].noDropItem = true;
                }
                for (int i = 0; i < 2; i++)
                {
                    float SpeedX = velocity.X + (float)Main.rand.Next(-10, 11) * 0.05f;
                    float SpeedY = velocity.Y + (float)Main.rand.Next(-10, 11) * 0.05f;
                    type = Utils.SelectRandom(Main.rand, new int[]
                    {
                    ProjectileID.HellfireArrow,
                    ProjectileID.IchorArrow
                    });
                    int index = Projectile.NewProjectile(spawnSource, position, new Vector2(SpeedX, SpeedY), type, (int)(damage * 0.5f), knockback, player.whoAmI);
                    Main.projectile[index].noDropItem = true;
                    Main.projectile[index].usesLocalNPCImmunity = true;
                    Main.projectile[index].localNPCHitCooldown = 10;
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
