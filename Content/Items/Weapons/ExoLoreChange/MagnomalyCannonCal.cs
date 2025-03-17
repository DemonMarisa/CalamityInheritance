using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Ranged;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityMod.Projectiles.Ranged;
using Terraria.Audio;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.CIPlayer;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class MagnomalyCannonCal : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {
            return item.type == ModContent.ItemType<MagnomalyCannon>();
        }
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            var usPlayer = player.CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
            {
                damage.Base = 330;
            }
        }

        public override bool CanUseItem(Item item, Player player)
        {

            var CIPlayer = player.CIMod();

            if (CIPlayer.LoreExo || CIPlayer.PanelsLoreExo)
            {
                item.shoot = ModContent.ProjectileType<MagnomalyRocket>();
                item.useAnimation = item.useTime = 67;
                item.UseSound = CISoundMenu.MagnomalyShootSound.WithVolumeScale(0.8f);
            }
            else
            {
                item.useTime = 15;
                item.useAnimation = 15;
                item.shoot = ModContent.ProjectileType<MagnomalyRocket>();
                item.UseSound = SoundID.Item11;
            }
            return base.CanUseItem(item , player);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            CalamityInheritancePlayer usPlayer = player.CIMod();

            if (usPlayer.LoreExo == true || usPlayer.PanelsLoreExo)
            {
                string ExoLoreOn = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Ranged.MagnomalyCannon.ExoLoreOn");

                tooltips.Add(new TooltipLine(Mod, "ExoLore", ExoLoreOn));
            }
        }
    }
}
