using CalamityMod.Projectiles.Magic;
using LAP.Assets.TextureRegister;
using Terraria.ModLoader;

namespace CalamityInheritance.Texture
{
    public partial class CITextureRegistry : ModSystem
    {
        public static Tex2DWithPath DOGworm_Body { get; private set; }
        public static Tex2DWithPath DOGworm_Head { get; private set; }
        public static Tex2DWithPath DOGworm_Tail { get; private set; }
        public static void LoadProjTex()
        {
            DOGworm_Body = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Summon/DOGworm_Body");
            DOGworm_Head = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Summon/DOGworm_Head");
            DOGworm_Tail = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Summon/DOGworm_Tail");
            LoadMeleeProjTex();
            LoadRangedProjTex();
        }
        public static void UnLoadProjTex()
        {
            DOGworm_Body = null;
            DOGworm_Head = null;
            DOGworm_Tail = null;
            UnLoadMeleeProjTex();
            UnLoadRangedProjTex();
        }
    }
}
