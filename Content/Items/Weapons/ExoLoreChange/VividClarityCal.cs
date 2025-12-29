using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Projectiles.Magic;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityInheritance.CIPlayer;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class VividClarityCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation) => item.type == ItemType<VividClarity>();
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            var usPlayer = player.CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
                damage.Base *= 0.5f;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string t = Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo ? Language.GetTextValue($"{Generic.WeaponTextPath}Magic.VividChange") : null;
            if (t != null) tooltips.Add(new TooltipLine(Mod, "Name", t));
        }
        public override bool CanUseItem(Item item, Player player)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
                item.shoot = ProjectileType<VividClarityBeamCal>();
            else
                item.shoot = ProjectileType<VividBeam>();
            return true;
        }
    }
}
