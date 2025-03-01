namespace CalamityInheritance.Utilities
{
    public static partial class CIFunction
    {
        public static int SecondsToFrames(int seconds) => seconds * 60;
        public static int SecondsToFrames(float seconds) => (int)(seconds * 60);
        /// <summary>
        /// 输入一个>0的整形数, 返回对应的刻度, 一般而言60刻 = 1秒, 推荐使用时尽可能不要取<0的值
        /// </summary>
        /// <param name="Sec">需要转化的秒数</param>
        /// <returns>返回一个浮点数，记录刻度</returns>
        public static float SecConvertTicks(int Sec)
        {
            return (float)(sec * 60);
        }
    }
}
