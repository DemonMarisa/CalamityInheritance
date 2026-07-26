using LAP.Assets.TextureRegister;
using Terraria.ModLoader;

namespace CalamityInheritance.Assets
{
    public class CIProjectiles_Range : ModSystem
    {
        public static Tex2DWithPath AMRShot { get; set; }
        public override void Load()
        {
            AMRShot = new Tex2DWithPath("CalamityInheritance/Assets/Projectiles/Range/AMRShot");
        }
        public override void Unload()
        {
            AMRShot = null;
        }
    }
}
