using CalamityInheritance.Core;
using CalamityMod;
using CalamityMod.NPCs.DevourerofGods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CIHook
{
    public class DropHelperHook
    {
        public static void Load(Mod mod)
        {
            MethodInfo originalMethod = typeof(DropHelper).GetMethod(nameof(DropHelper.PostLevi));
            MonoModHooks.Add(originalMethod, PostLevi_Hook);
        }

        public static IItemDropRuleCondition PostLevi_Hook(bool ui = true) => CIConditions.PostLeviOrCalClone.ToDropCondition(ui ? ShowItemDropInUI.Always : ShowItemDropInUI.Never);
    }
}
