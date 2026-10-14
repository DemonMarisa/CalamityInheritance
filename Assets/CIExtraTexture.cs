using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalamityInheritance.Assets
{
    public class CIExtraTexture : ModSystem
    {
        public static Asset<Texture2D> TrientCircularSmear { get; set; }
        public static Asset<Texture2D> PearlGodAimLaser { get; set; }
        public static Asset<Texture2D> TechyNoise { get; set; }
        public static Asset<Texture2D> SmallGreyscaleCircle { get; set; }
        public static Asset<Texture2D> Tornade_Fire { get; set; }
        public static Tex2DWithPath LargeBloom { get; set; }
        public static Tex2DWithPath HighResHollowCircleHardEdge { get; set; }
        public static Tex2DWithPath FlameExplosion { get; set; }
        public override void Load()
        {
            TrientCircularSmear = Request<Texture2D>("CalamityInheritance/Assets/ExtraTextures/TrientCircularSmear");
            PearlGodAimLaser = Request<Texture2D>("CalamityInheritance/Assets/ExtraTextures/PearlGodAimLaser");
            TechyNoise = Request<Texture2D>("CalamityInheritance/Assets/ExtraTextures/TechyNoise");
            SmallGreyscaleCircle = Request<Texture2D>("CalamityInheritance/Assets/ExtraTextures/SmallGreyscaleCircle");
            LargeBloom = new Tex2DWithPath("CalamityInheritance/Assets/ExtraTextures/LargeBloom");
            HighResHollowCircleHardEdge = new Tex2DWithPath("CalamityInheritance/Assets/ExtraTextures/HighResHollowCircleHardEdge");
            FlameExplosion = new Tex2DWithPath("CalamityInheritance/Assets/ExtraTextures/FlameExplosion");
            Tornade_Fire = Request<Texture2D>("CalamityInheritance/Assets/ExtraTextures/Tornade_Fire");
        }
        public override void Unload()
        {
            TrientCircularSmear = null;
            PearlGodAimLaser = null;
            TechyNoise = null;
            SmallGreyscaleCircle = null;
            LargeBloom = null;
            HighResHollowCircleHardEdge = null;
            FlameExplosion = null;
            Tornade_Fire = null;
        }
    }
}
