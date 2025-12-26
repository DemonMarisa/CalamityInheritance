using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.CIPlayer;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ID;
using CalamityInheritance.Content.Projectiles.ExoLore;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class MagnomalyCannonCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation) => item.type == ModContent.ItemType<MagnomalyCannon>();
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (player.CheckExoLore())
            {
                item.shoot = ModContent.ProjectileType<MagnomalyRocketExoLore>();
                item.useAnimation = item.useTime = 67;
                item.UseSound = CISoundMenu.MagnomalyShootSound.WithVolumeScale(0.8f);
            }
            else
            {
                item.shoot = ModContent.ProjectileType<MagnomalyRocket>();
                item.useAnimation = item.useTime = 15;
                item.UseSound = SoundID.Item11;
            }
            return base.CanUseItem(item , player);
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer usPlayer = player.CIMod();

            if (usPlayer.LoreExo == true || usPlayer.PanelsLoreExo)
            {
                string ExoLoreOn = Language.GetTextValue($"{Generic.WeaponTextPath}Ranged.MagnomalyCannon.ExoLoreOn");

                tooltips.Add(new TooltipLine(Mod, "ExoLore", ExoLoreOn));
            }
        }
    }
}
