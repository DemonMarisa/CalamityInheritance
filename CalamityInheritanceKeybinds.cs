using Terraria.ModLoader;

namespace CalamityInheritance
{
    public class CalamityInheritanceKeybinds : ModSystem
    {
        public static ModKeybind BoCLoreTeleportation { get; private set; }
        public static ModKeybind AegisHotKey { get; private set; }
        public static ModKeybind AstralArcanumUIHotkey { get; private set; }
        public static ModKeybind QOLUIHotKey { get; private set; }
        public override void Load()
        {
            BoCLoreTeleportation = KeybindLoader.RegisterKeybind(Mod, "BoCLoreTeleportation", "Z");
            AegisHotKey = KeybindLoader.RegisterKeybind(Mod, "AegisHotKey", "N");
            AstralArcanumUIHotkey = KeybindLoader.RegisterKeybind(Mod, "Astral Arcanum UI Toggle", "O");
            QOLUIHotKey = KeybindLoader.RegisterKeybind(Mod, "Qol Panel UI Toggle", "L");
        }
        public override void Unload()
        {
            BoCLoreTeleportation = null;
            AegisHotKey = null;
            AstralArcanumUIHotkey = null;
            QOLUIHotKey = null;
        }
    }
}
