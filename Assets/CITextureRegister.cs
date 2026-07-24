using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalamityInheritance.Assets
{
    public class CITextureRegister : ModSystem
    {
        public static Asset<Texture2D> StarTrail { get; set; }
        public override void Load()
        {
            StarTrail = Request<Texture2D>("CalamityInheritance/Assets/Textures/StarTrail");
        }
        public override void Unload()
        {
            StarTrail = null;
        }
    }
}
