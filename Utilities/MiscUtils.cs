namespace CalamityInheritance.Utilities
{
    public static partial class CalamityInheritanceUtils
    {
        public static int SecondsToFrames(int seconds) => seconds * 60;
        public static int SecondsToFrames(float seconds) => (int)(seconds * 60);
    }
}
