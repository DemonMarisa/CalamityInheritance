using CalamityInheritance.Content.Projectiles.Melee.Swords;
using LAP.Assets.TextureRegister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static Tex2DWithPath DepthOrbLegacy { get; private set; }
        public static Tex2DWithPath EntropicFlechette1 { get; private set; }
        public static Tex2DWithPath EntropicFlechette2 { get; private set; }
        public static Tex2DWithPath EntropicFlechette3 { get; private set; }
        public static void LoadMeleeProjTex()
        {
            ExcelsusBlue = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Melee/ExcelsusBlue");
            ExcelsusBlueGlow = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Melee/ExcelsusBlueGlow");
            ExcelsusMain = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Melee/ExcelsusMain");
            ExcelsusMainGlow = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Melee/ExcelsusMainGlow");
            ExcelsusPink = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Melee/ExcelsusPink");
            ExcelsusPinkGlow = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Melee/ExcelsusPinkGlow");
            DepthOrbLegacy = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Melee/DepthOrbLegacy");
            EntropicFlechette1 = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Melee/EntropicFlechette1");
            EntropicFlechette2 = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Melee/EntropicFlechette2");
            EntropicFlechette3 = new Tex2DWithPath("CalamityInheritance/Texture/Projectiles/Melee/EntropicFlechette3");
        }
        public static void UnLoadMeleeProjTex()
        {
            ExcelsusBlue = null;
            ExcelsusBlueGlow = null;
            ExcelsusMain = null;
            ExcelsusMainGlow = null;
            ExcelsusPink = null;
            ExcelsusPinkGlow = null;
            DepthOrbLegacy = null;
            EntropicFlechette1 = null;
            EntropicFlechette2 = null;
            EntropicFlechette3 = null;
        }
    }
}
