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
        public static Tex2DWithPath TornadoProj { get; set; }

        public static Tex2DWithPath MidnightSunBeamEnd { get; set; }
        public static Tex2DWithPath MidnightSunBeamBegin { get; set; }
        public static Tex2DWithPath MidnightSunBeamMid { get; set; }
        public override void Load()
        {
            StarTrail = new Tex2DWithPath("CalamityInheritance/Assets/Textures/StarTrail");
            LaserProj = new Tex2DWithPath("CalamityInheritance/Assets/Textures/LaserProj");
            TornadoProj = new Tex2DWithPath("CalamityInheritance/Assets/Textures/TornadoProj");

            MidnightSunBeamEnd = new Tex2DWithPath("CalamityInheritance/Assets/Textures/MidnightSunBeamEnd");
            MidnightSunBeamBegin = new Tex2DWithPath("CalamityInheritance/Assets/Textures/MidnightSunBeamBegin");
            MidnightSunBeamMid = new Tex2DWithPath("CalamityInheritance/Assets/Textures/MidnightSunBeamMid");
        }
        public override void Unload()
        {
            StarTrail = null;
            LaserProj = null;

            MidnightSunBeamEnd = null;
            MidnightSunBeamBegin = null;
            MidnightSunBeamMid = null;
        }
    }
}
