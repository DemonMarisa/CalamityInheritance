using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public abstract class FlamethrowerSpecial: ModItem
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Ranged";
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