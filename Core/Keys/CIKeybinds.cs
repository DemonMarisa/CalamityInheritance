using Terraria.ModLoader;

namespace CalamityInheritance.Core.Keys
{
    public class CIKeybinds : ModSystem
    {
        public static ModKeybind BoCLoreTeleportation { get; private set; }
        public static ModKeybind AegisHotKey { get; private set; }
        public static ModKeybind AstralArcanumUIHotkey { get; private set; }
        public static ModKeybind QOLUIHotKey { get; private set; }
        public static ModKeybind GodSlayerDash { get; private set; }
        public override void Load()
        {
            BoCLoreTeleportation = KeybindLoader.RegisterKeybind(Mod, "BoCLoreTeleportation", "Z");
            AegisHotKey = KeybindLoader.RegisterKeybind(Mod, "AegisHotKey", "N");
            AstralArcanumUIHotkey = KeybindLoader.RegisterKeybind(Mod, "Astral Arcanum UI Toggle", "O");
            QOLUIHotKey = KeybindLoader.RegisterKeybind(Mod, "Qol Panel UI Toggle", "L");
            GodSlayerDash = KeybindLoader.RegisterKeybind(Mod, "God Slayer Dash Toggle", "L");
        }
        public override void Unload()
        {
            BoCLoreTeleportation = null;
            AegisHotKey = null;
            AstralArcanumUIHotkey = null;
            QOLUIHotKey = null;
            GodSlayerDash = null;
        }
    }
}
