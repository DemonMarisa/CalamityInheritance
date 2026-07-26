using LAP.Assets.TextureRegister;
using Terraria.ModLoader;

namespace CalamityInheritance.Assets
{
    public class CIProjectiles_Summon : ModSystem
    {
        public static Tex2DWithPath DOGworm_Body { get; private set; }
        public static Tex2DWithPath DOGworm_Head { get; private set; }
        public static Tex2DWithPath DOGworm_Tail { get; private set; }
        public override void Load()
        {
            DOGworm_Body = new Tex2DWithPath("CalamityInheritance/Assets/Projectiles/Summon/DOGworm_Body");
            DOGworm_Head = new Tex2DWithPath("CalamityInheritance/Assets/Projectiles/Summon/DOGworm_Head");
            DOGworm_Tail = new Tex2DWithPath("CalamityInheritance/Assets/Projectiles/Summon/DOGworm_Tail");
        }
        public override void Unload()
        {
            DOGworm_Body = null;
            DOGworm_Head = null;
            DOGworm_Tail = null;
        }
    }
}
