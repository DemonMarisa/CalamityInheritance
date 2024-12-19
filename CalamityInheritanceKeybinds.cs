using Terraria.ModLoader;

namespace CalamityInheritance
{
    public class CalamityInheritanceKeybinds : ModSystem
    {
        public static ModKeybind BoCLoreTeleportation { get; private set; }
        public override void Load()
        {
            BoCLoreTeleportation = KeybindLoader.RegisterKeybind(Mod, "BoCLoreTeleportation", "Z");
        }
        public override void Unload()
        {
            BoCLoreTeleportation = null;
        }
    }
}
