using CalamityInheritance.System.Configs;
using LAP.Core.CrossModSupports;
using Terraria.ModLoader;

namespace CalamityInheritance.System.CalStatInflationBACK
{
    public class ModCross : ModSystem
    {
        public override void PreUpdateTime()
        {
            CrossModSupport.UseCICalStatInflation = CIServerConfig.Instance.CalStatInflationBACK;
        }
    }
}
