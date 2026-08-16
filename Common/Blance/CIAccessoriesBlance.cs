namespace CalamityInheritance.Common.Blance
{
    public class CIAccessoriesBlance
    {
        public static int CIShieldRechargeDelay = SecondsToFrames(12); // 完全破碎后要求的充能时间
        public static int CIShieldRechargeRelay = SecondsToFrames(6); // 受击后的充能时间
        public static int CITotalShieldRechargeTime = SecondsToFrames(12);  // 恢复至满的时间
    }
}
