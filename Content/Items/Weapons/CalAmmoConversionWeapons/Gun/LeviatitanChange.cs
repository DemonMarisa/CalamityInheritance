﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.System.Configs;

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
            if (CIConfig.Instance.AmmoConversion == true)
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

            if (CIConfig.Instance.AmmoConversion == true)
            {
                string AmmoConversionOn = Language.GetTextValue("Mods.CalamityInheritance.ConfigsMessage.AmmoConversion");

                tooltips.Add(new TooltipLine(Mod, "AmmoConversion", AmmoConversionOn));
            }
        }
    }
}
