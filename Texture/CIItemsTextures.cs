using LAP.Assets.TextureRegister;
using Terraria.ModLoader;

namespace CalamityInheritance.Texture
{
    public partial class CITextureRegistry : ModSystem
    {

        public static string TexturesPath => "CalamityInheritance/Texture/Items/Weapons";
        public static string RoguePath => $"{TexturesPath}/Rogue";
        public static string MagicPath => $"{TexturesPath}/Magic";
        public static Tex2DWithPath NightsGaze { get; private set; }
        public static void LoadItemTextures()
        {
            NightsGaze = new Tex2DWithPath($"{RoguePath}/NightsGaze");
            
        }
        public static void UnLoadItemTextures()
        {
            NightsGaze = null;
        }
    }
}
