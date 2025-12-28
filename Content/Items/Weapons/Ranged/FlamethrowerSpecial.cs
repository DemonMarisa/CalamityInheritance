using System.Collections.Generic;
using CalamityMod;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public abstract class FlamethrowerSpecial: ModItem
    {
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.Ranged";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            
            base.SetDefaults();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string getBuff = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.Ranged.FlamethrowerSpecial");
            tooltips.Add(new TooltipLine(Mod, "GETBUFF", getBuff));
            base.ModifyTooltips(tooltips);
        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.DefenseEffectiveness *= 0.5f;
            base.ModifyHitNPC(player, target, ref modifiers);
        }
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.RangedWeapon;
        }
    }
}