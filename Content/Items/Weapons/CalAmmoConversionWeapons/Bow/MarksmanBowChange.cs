using CalamityMod.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Weapons.CalAmmoConversionWeapons.Bow
{
    public class MarksmanBowChange : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ModContent.ItemType<MarksmanBow>();
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CIConfig.Instance.AmmoConversion == true)
            {
                type = ProjectileID.JestersArrow;

                for (int i = 0; i < 3; i++)
                {
                    int randomExtraUpdates = Main.rand.Next(3);
                    float SpeedX = velocity.X + Main.rand.NextFloat(-10f, 10f) * 0.05f;
                    float SpeedY = velocity.Y + Main.rand.NextFloat(-10f, 10f) * 0.05f;
                    int arrow = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI);
                    Main.projectile[arrow].noDropItem = true;
                    Main.projectile[arrow].extraUpdates += randomExtraUpdates; //0 to 2 extra updates
                    if (type == ProjectileID.JestersArrow)
                    {
                        Main.projectile[arrow].localNPCHitCooldown = 10 * (randomExtraUpdates + 1);
                        Main.projectile[arrow].usesLocalNPCImmunity = true;
                        Main.projectile[arrow].tileCollide = false;
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

            if (CIConfig.Instance.AmmoConversion == true)
            {
                string AmmoConversionOn = Language.GetTextValue("Mods.CalamityInheritance.ConfigsMessage.AmmoConversion");

                tooltips.Add(new TooltipLine(Mod, "AmmoConversion", AmmoConversionOn));
            }
        }
    }
}
