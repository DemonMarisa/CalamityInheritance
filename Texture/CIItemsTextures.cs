using LAP.Assets.TextureRegister;
using Terraria.ModLoader;

namespace CalamityInheritance.Texture
{
    public partial class CITextureRegistry : ModSystem
    {
        public static string TexturesPath => "CalamityInheritance/Texture/Items/Weapons";
        public static string RoguePath => $"{TexturesPath}/Rogue";
        public static string MagicPath => $"{TexturesPath}/Magic";
        public static string MeleePath => $"{TexturesPath}/Melees";
        public static Tex2DWithPath NightsGaze { get; private set; }
        public static Tex2DWithPath SunderingScissorsLeft { get; private set; }
        public static Tex2DWithPath SunderingScissorsRight { get; private set; }
        public static void LoadItemTextures()
        {
            NightsGaze = new Tex2DWithPath($"{RoguePath}/NightsGaze");
            SunderingScissorsLeft = new Tex2DWithPath($"{MeleePath}/SunderingScissorsLeft");
            SunderingScissorsRight = new Tex2DWithPath($"{MeleePath}/SunderingScissorsRight");
        }
        public static void UnLoadItemTextures()
        {
            NightsGaze = null;
            SunderingScissorsLeft = null;
            SunderingScissorsRight = null;
        }
    }
}
