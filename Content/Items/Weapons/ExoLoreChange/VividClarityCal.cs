using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using CalamityMod;
using System;
using CalamityMod.Projectiles.Rogue;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityInheritance.Content.Projectiles.ExoLore;
using CalamityMod.Projectiles.Melee;
using CalamityMod.CalPlayer;
using System.Reflection;
using CalamityMod.Buffs.DamageOverTime;
using CalamityInheritance.CIPlayer;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class VividClarityCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation) => item.type == ModContent.ItemType<VividClarity>();
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            var usPlayer = player.CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
                damage.Base *= 0.5f;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string t = Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo ? Language.GetTextValue($"{Generic.GetWeaponLocal}.Magic.VividChange") : null;
            if (t != null) tooltips.Add(new TooltipLine(Mod, "Name", t));
        }
        public override bool CanUseItem(Item item, Player player)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
                item.shoot = ModContent.ProjectileType<VividClarityBeamCal>();
            else
                item.shoot = ModContent.ProjectileType<VividBeam>();
            return true;
        }
    }
}
