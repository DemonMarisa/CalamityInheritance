using CalamityInheritance.Core.Path;
using LAP.Core.BaseClass;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Buff
{
    public abstract class CISummonBuff : BaseSummonBuff,ILocalizedModType
    {
        public new string LocalizationCategory => LocalizationPath.CISummonBuff;
    }
}
