using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Weapons.CalAmmoConversionWeapons.Gun
{
    public class UniversalGenesisChange : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ModContent.ItemType<UniversalGenesis>();
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CIConfig.Instance.AmmoConversion == true)
            {
                Vector2 shootVelocity = velocity;
                Vector2 shootDirection = shootVelocity.SafeNormalize(Vector2.UnitX * player.direction);
                Vector2 gunTip = position + shootDirection * item.scale * 100f;
                gunTip.Y -= 10f;
                float tightness = 1f;
                type = ModContent.ProjectileType<UniversalGenesisStarcaller>();
                for (float i = -tightness * 5f; i <= tightness * 5f; i += tightness * 2f)
                {
                    Vector2 perturbedSpeed = shootVelocity.RotatedBy(MathHelper.ToRadians(i));
                    Projectile.NewProjectile(source, gunTip, perturbedSpeed, type, damage, knockback, player.whoAmI);
                }

                // Stars from above
                float speed = item.shootSpeed;
                Vector2 spawnPos = player.RotatedRelativePoint(player.MountedCenter, true);
                int starAmt = 6;
                int starDmg = (int)(damage * 0.4);
                for (int i = 0; i < starAmt; i++)
                {
                    spawnPos = new Vector2(player.Center.X + (Main.rand.Next(201) * -(float)player.direction) + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
                    spawnPos.X = (spawnPos.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
                    spawnPos.Y -= 100 + i;
                    float xDist = Main.mouseX + Main.screenPosition.X - spawnPos.X;
                    float yDist = Main.mouseY + Main.screenPosition.Y - spawnPos.Y;
                    if (yDist < 0f)
                    {
                        yDist *= -1f;
                    }
                    if (yDist < 20f)
                    {
                        yDist = 20f;
                    }
                    float travelDist = (float)Math.Sqrt(xDist * xDist + yDist * yDist);
                    travelDist = speed / travelDist;
                    xDist *= travelDist;
                    yDist *= travelDist;
                    float xVel = xDist + Main.rand.NextFloat(-0.6f, 0.6f);
                    float yVel = yDist + Main.rand.NextFloat(-0.6f, 0.6f);
                    int star = Projectile.NewProjectile(source, spawnPos.X, spawnPos.Y, xVel, yVel, ModContent.ProjectileType<UniversalGenesisStar>(), starDmg, knockback, player.whoAmI, i, 1f);
                    Main.projectile[star].extraUpdates = 2;
                    Main.projectile[star].localNPCHitCooldown = 30;
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
