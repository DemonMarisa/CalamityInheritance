using CalamityMod.Projectiles.Magic;
using LAP.Assets.TextureRegister;
using Terraria.ModLoader;

namespace CalamityInheritance.Texture
{
    public partial class CITextureRegistry : ModSystem
    {
        public static Tex2DWithPath ExcelsusBlue { get; private set; }
        public static Tex2DWithPath ExcelsusBlueGlow { get; private set; }
        public static Tex2DWithPath ExcelsusMain { get; private set; }
        public static Tex2DWithPath ExcelsusMainGlow { get; private set; }
        public static Tex2DWithPath ExcelsusPink { get; private set; }
        public static Tex2DWithPath ExcelsusPinkGlow { get; private set; }
        public static Tex2DWithPath DOGworm_Body { get; private set; }
        public static Tex2DWithPath DOGworm_Head { get; private set; }
        public static Tex2DWithPath DOGworm_Tail { get; private set; }
        public static Tex2DWithPath DepthOrbLegacy { get; private set; }
        private static string ProjPath = "CalamityInheritance/Texture/Projectiles";
        public static void LoadProjTex()
        {
            ExcelsusBlue = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Melee/ExcelsusBlue");
            ExcelsusBlueGlow = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Melee/ExcelsusBlueGlow");
            ExcelsusMain = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Melee/ExcelsusMain");
            ExcelsusMainGlow = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Melee/ExcelsusMainGlow");
            ExcelsusPink = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Melee/ExcelsusPink");
            ExcelsusPinkGlow = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Melee/ExcelsusPinkGlow");
            DOGworm_Body = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Summon/DOGworm_Body");
            DOGworm_Head = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Summon/DOGworm_Head");
            DOGworm_Tail = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Summon/DOGworm_Tail");
            DepthOrbLegacy = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Melee/DepthOrbLegacy");
        }
        public static void UnLoadProjTex()
        {
            ExcelsusBlue = null;
            ExcelsusBlueGlow = null;
            ExcelsusMain = null;
            ExcelsusMainGlow = null;
            ExcelsusPink = null;
            ExcelsusPinkGlow = null;
            DOGworm_Body = null;
            DOGworm_Head = null;
            DOGworm_Tail = null;
            DepthOrbLegacy = null;
        }
    }
}
