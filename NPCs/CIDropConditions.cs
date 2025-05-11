using CalamityInheritance.Core;
using CalamityMod;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;

namespace CalamityInheritance.NPCs
{
    //I don't know how to code.
    public static class CIDropHelper
    {
        public static IItemDropRuleCondition ArmageddonNoNor => CIConditions.ArmageddonNoNor.ToDropCondition(ShowItemDropInUI.WhenConditionSatisfied);
        public static IItemDropRuleCondition MADRule => CIConditions.MAD.ToDropCondition(ShowItemDropInUI.WhenConditionSatisfied);
        public static IItemDropRuleCondition MasterDeath => CIConditions.MasterDeath.ToDropCondition(ShowItemDropInUI.WhenConditionSatisfied);

        public static IItemDropRuleCondition CIPostCalClone(bool ui = true) => CIConditions.DownedCalClone.ToDropCondition(ui ? ShowItemDropInUI.Always : ShowItemDropInUI.Never);
    }

}