using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CalamityMod.Projectiles;
using CalamityMod;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.CalAmmoConversionWeapons.Gun
{
    public class SomaPrimeChange : GlobalItem
    {
        private static readonly float XYInaccuracy = 0.32f;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ModContent.ItemType<SomaPrime>();
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CalamityInheritanceConfig.Instance.AmmoConversion == true)
            {

                type = ProjectileID.BulletHighVelocity;
                damage += 4; // in 1.4, HVBs deal 11 damage and Musket Balls deal 7
                

                velocity.X += Main.rand.NextFloat(-XYInaccuracy, XYInaccuracy);
                velocity.Y += Main.rand.NextFloat(-XYInaccuracy, XYInaccuracy);
                Vector2 vel = velocity;
                Projectile shot = Projectile.NewProjectileDirect(source, position, vel, type, damage, knockback, player.whoAmI);
                CalamityGlobalProjectile cgp = shot.Calamity();
                cgp.supercritHits = -1;
                cgp.appliesSomaShred = true;
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
