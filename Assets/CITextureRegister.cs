using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalamityInheritance.Assets
{
    public class CITextureRegister : ModSystem
    {
        public static Tex2DWithPath StarTrail { get; set; }
        public static Tex2DWithPath LaserProj { get; set; }
        public override void Load()
        {
            StarTrail = new Tex2DWithPath("CalamityInheritance/Assets/Textures/StarTrail");
            LaserProj = new Tex2DWithPath("CalamityInheritance/Assets/Textures/LaserProj");
        }
        public override void Unload()
        {
            StarTrail = null;
            LaserProj = null;
        }
    }
}
