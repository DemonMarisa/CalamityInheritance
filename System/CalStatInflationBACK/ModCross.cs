using CalamityInheritance.System.Configs;
using LAP.Common.CIModCross;
using Terraria.ModLoader;

namespace CalamityInheritance.System.CalStatInflationBACK
{
    public class ModCross : ModSystem
    {
        public override void PreUpdateTime()
        {
            CIMainDate.UseCICalStatInflation = CIServerConfig.Instance.CalStatInflationBACK;
        }
    }
}
