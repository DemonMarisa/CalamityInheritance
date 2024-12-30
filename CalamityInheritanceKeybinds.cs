using Terraria.ModLoader;

namespace CalamityInheritance
{
    public class CalamityInheritanceKeybinds : ModSystem
    {
        public static ModKeybind BoCLoreTeleportation { get; private set; }
        public static ModKeybind AegisHotKey { get; private set; }
        public override void Load()
        {
            BoCLoreTeleportation = KeybindLoader.RegisterKeybind(Mod, "BoCLoreTeleportation", "Z");
            AegisHotKey = KeybindLoader.RegisterKeybind(Mod, "AegisHotKey", "N");
        }
        public override void Unload()
        {
            BoCLoreTeleportation = null;
            AegisHotKey = null;
        }
    }
}
