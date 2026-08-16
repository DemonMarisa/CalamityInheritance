using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Assets.Effects
{
    public class CIShaders : ModSystem
    {
        public static Asset<Effect> RoverDriveShield { get; private set; }
        public override void Load()
        {
            if (Main.dedServ)
                return;
            RoverDriveShield = Request<Effect>("CalamityInheritance/Assets/Effects/Shaders/RoverDriveShield");
        }
        public override void Unload()
        {
            if (Main.dedServ)
                return;
            RoverDriveShield = null;
        }
    }
}
