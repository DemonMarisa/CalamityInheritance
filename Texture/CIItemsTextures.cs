using LAP.Assets.TextureRegister;
using Terraria.ModLoader;

namespace CalamityInheritance.Texture
{
    public partial class CITextureRegistry : ModSystem
    {
        public static string TexturesPath => "CalamityInheritance/Texture/Items/Weapons";
        public static string RoguePath => $"{TexturesPath}/Rogue";
        public static Tex2DWithPath NightsGaze { get; private set; }
        public static void LoadItemTextures()
        {
            NightsGaze = new Tex2DWithPath($"{TexturesPath}/NightsGaze");
        }
        public static void UnLoadItemTextures()
        {
            NightsGaze = null;
        }
    }
}
