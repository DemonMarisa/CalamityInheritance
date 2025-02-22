using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Weapons.CalAmmoConversionWeapons.Bow
{
    public class TheStormChange : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ModContent.ItemType<TheStorm>();
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CalamityInheritanceConfig.Instance.AmmoConversion == true)
            {
                int i = Main.myPlayer;
                float arrowSpeed = Main.rand.Next(25, 30);
                player.itemTime = item.useTime;
                Vector2 realPlayerPos = player.RotatedRelativePoint(player.MountedCenter, true);
                float mouseXDist = (float)Main.mouseX + Main.screenPosition.X - realPlayerPos.X;
                float mouseYDist = (float)Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
                if (player.gravDir == -1f)
                {
                    mouseYDist = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - realPlayerPos.Y;
                }
                float mouseDistance = (float)Math.Sqrt((double)(mouseXDist * mouseXDist + mouseYDist * mouseYDist));
                if ((float.IsNaN(mouseXDist) && float.IsNaN(mouseYDist)) || (mouseXDist == 0f && mouseYDist == 0f))
                {
                    mouseXDist = (float)player.direction;
                    mouseYDist = 0f;
                    mouseDistance = arrowSpeed;
                }
                else
                {
                    mouseDistance = arrowSpeed / mouseDistance;
                }

                for (int j = 0; j < 3; j++)
                {
                    realPlayerPos = new Vector2(player.position.X + (float)player.width * 0.5f + (float)(Main.rand.Next(201) * -(float)player.direction) + ((float)Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
                    realPlayerPos.X = (realPlayerPos.X + player.Center.X) / 2f + (float)Main.rand.Next(-200, 201);
                    realPlayerPos.Y -= (float)(100 * j);
                    mouseXDist = (float)Main.mouseX + Main.screenPosition.X - realPlayerPos.X;
                    mouseYDist = (float)Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
                    if (mouseYDist < 0f)
                    {
                        mouseYDist *= -1f;
                    }
                    if (mouseYDist < 20f)
                    {
                        mouseYDist = 20f;
                    }
                    mouseDistance = (float)Math.Sqrt((double)(mouseXDist * mouseXDist + mouseYDist * mouseYDist));
                    mouseDistance = arrowSpeed / mouseDistance;
                    mouseXDist *= mouseDistance;
                    mouseYDist *= mouseDistance;
                    float speedX4 = mouseXDist + (float)Main.rand.Next(-120, 121) * 0.01f;
                    float speedY5 = mouseYDist + (float)Main.rand.Next(-120, 121) * 0.01f;
                    Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, speedX4, speedY5 * 0.9f, ModContent.ProjectileType<Bolt>(), damage, knockback, i);
                    Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, speedX4, speedY5 * 0.8f, ModContent.ProjectileType<Bolt>(), damage, knockback, i);
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
