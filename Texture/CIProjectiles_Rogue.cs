using LAP.Assets.TextureRegister;
using Terraria.ModLoader;

namespace CalamityInheritance.Texture
{
    public partial class CITextureRegistry : ModSystem
    {
        public static Tex2DWithPath AlphaVirusAura { get; set; }
        public static void LoadRogueProjTex()
        {
            AlphaVirusAura = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Rogue/AlphaVirusAura");
        }
        public static void UnLoadRogueProjTex()
        {
            AlphaVirusAura = null;
        }
    }
}
