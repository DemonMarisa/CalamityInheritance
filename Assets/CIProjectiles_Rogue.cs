using LAP.Assets.TextureRegister;
using Terraria.ModLoader;

namespace CalamityInheritance.Assets
{
    public class CIProjectiles_Rogue : ModSystem
    {
        public static Tex2DWithPath AlphaVirusAura { get; set; }
        public override void Load()
        {
            AlphaVirusAura = new Tex2DWithPath("CalamityInheritance/Assets/Projectiles/Rogue/AlphaVirusAura");
        }
        public override void Unload()
        {
            AlphaVirusAura = null;
        }
    }
}
