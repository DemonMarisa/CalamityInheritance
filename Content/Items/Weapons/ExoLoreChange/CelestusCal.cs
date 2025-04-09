using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Rogue;
using System.Collections.Generic;
using Terraria.Localization;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class CelestusCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation) => item.type == ModContent.ItemType<Celestus>();
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            var usPlayer = player.CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
            {
                damage.Base = 620;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string t = Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo ? Language.GetTextValue($"{Generic.GetWeaponLocal}.Rogue.CelestusChange") : null;
            if (t != null) tooltips.Add(new TooltipLine(Mod, "Name", t));
        }
    }
    //金源的小镰刀
    public class ZScythe : GlobalProjectile
    {
        const int Timer = 1;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ModContent.ProjectileType<CelestusMiniScythe>();
        //我直接干掉了原本的AI，自己写一个
        public override bool PreAI(Projectile projectile)
        {

            var usPlayer = Main.player[projectile.owner].CIMod();
            if (usPlayer.PanelsLoreExo || usPlayer.LoreExo)
            {
                projectile.ai[Timer] += 1f; 
                if (projectile.ai[Timer] > 60f)
                    CIFunction.HomeInOnNPC(projectile, true, 1800f, 10f, 0f);
            }
            return true;
        }
    }
}
