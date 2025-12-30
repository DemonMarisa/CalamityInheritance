using LAP.Assets.TextureRegister;
using Terraria.ModLoader;

namespace CalamityInheritance.Texture
{
    public partial class CITextureRegistry : ModSystem
    {
        public static Tex2DWithPath AMRShot { get; set; }
        public static void LoadRangedProjTex()
        {
            AMRShot = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Range/AMRShot");
        }
        public static void UnLoadRangedProjTex()
        {
            AMRShot = null;
        }
    }
}
