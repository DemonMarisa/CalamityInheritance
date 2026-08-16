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
        public override void Load()
        {
            TrientCircularSmear = Request<Texture2D>("CalamityInheritance/Assets/ExtraTextures/TrientCircularSmear");
            PearlGodAimLaser = Request<Texture2D>("CalamityInheritance/Assets/ExtraTextures/PearlGodAimLaser");
            TechyNoise = Request<Texture2D>("CalamityInheritance/Assets/ExtraTextures/TechyNoise");
        }
        public override void Unload()
        {
            TrientCircularSmear = null;
            PearlGodAimLaser = null;
            TechyNoise = null;
        }
    }
}
