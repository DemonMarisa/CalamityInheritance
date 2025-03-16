using CalamityMod;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;

namespace CalamityInheritance.NPCs
{
    //I don't know how to code.
    public class GetDarksunFragmentDrop : IItemDropRuleCondition
    {
        private static LocalizedText GetDescrip;
        public GetDarksunFragmentDrop()
        {
            GetDescrip ??= Language.GetOrRegister("Mods.CalamityInheritance.DropConditions.DarksunFragmentDrop");
        }
        public bool CanDrop(DropAttemptInfo info)
        {
            NPC npc = info.npc;
            return Main.eclipse && CalamityConditions.DownedDevourerOfGods.IsMet() && !npc.boss &&
                   !npc.friendly && npc.lifeMax > 5 && npc.value >= 1f;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return GetDescrip.Value;
        }
    }
}