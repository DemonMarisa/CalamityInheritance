using CalamityInheritance.System.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalamityInheritance.Common.ModSupport
{
    public class CIModCall
    {
        #region 设置
        public static bool GetConfigs(string name)
        {
            switch (name)
            {
                default:
                    return false;
                case "CalStatInflationBACK":
                    return CIServerConfig.Instance.CalStatInflationBACK;
            }
        }
        #endregion
        #region 登记Call
        public static object Call(params object[] args)
        {
            string methodName = args[0].ToString();
            switch (methodName)
            {
                case "GetConfigs":
                    if (!(args[1] is string))
                        return new ArgumentException("ERROR: The argument to \"Downed\" must be a string.");
                    return GetConfigs(args[1].ToString()); ;

                default:
                    return new ArgumentException("ERROR: Invalid method name.");
            }
        }
        #endregion
    }
}
