using LAP.Assets.TextureRegister;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalamityInheritance.Texture
{
    public partial class CITextureRegistry : ModSystem
    {
        public static string ParticlesTexturesPath => "CalamityInheritance/Texture/Particles";
        public static Tex2DWithPath TrientCircularSmear { get; private set; }
        public static void LoadPTexture()
        {
            TrientCircularSmear = new Tex2DWithPath($"{ParticlesTexturesPath}/TrientCircularSmear");
        }
        public static void UnloadPTexture()
        {
            TrientCircularSmear = null;
        }
    }
}
