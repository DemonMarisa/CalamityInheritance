using System.Collections.Generic;
using CalamityInheritance.Content.Projectiles.CalProjChange;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Projectiles.Summon;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class HowToSaveMiracleSummonWeapon : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ModContent.ItemType<CosmicImmaterializer>();
        public override bool CanUseItem(Item item, Player player)
        {
            if (Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo)
            {
                //目前这里的改法是……直接复制了一份射弹。
                //我不清楚到底怎么改。 
                item.shoot = ModContent.ProjectileType<SaveCosmic>();
                if (player.ownedProjectileCounts[item.shoot] <= 0)
                    return true;
            }
            else
            {
                item.shoot = ModContent.ProjectileType<CosmicEnergySpiral>();
                if (player.maxMinions > 10f && player.ownedProjectileCounts[item.shoot] <= 0)
                    return true;
            }
            return false;
        }
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo)
                damage.Base = 600;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string t = Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo ? Language.GetTextValue($"{Generic.GetWeaponLocal}.Summon.SaveCosmic") : null;
            if (t != null) tooltips.Add(new TooltipLine(Mod, "Name", t));
        }
    }
}
