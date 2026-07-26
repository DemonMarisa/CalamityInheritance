using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalamityInheritance.Assets
{
    public class CIParticleTexture : ModSystem
    {
        public static Asset<Texture2D> Sparkle2 { get; set; }
        public static Asset<Texture2D> DrainLineBloom2 { get; set; }
        public static Asset<Texture2D> HeavySmoke { get; set; }
        public static Asset<Texture2D> Light { get; set; }
        public static Asset<Texture2D> CritSpark { get; set; }
        public static Asset<Texture2D> ThinSparkle { get; set; }
        public static Asset<Texture2D> ThinEndedLine { get; set; }
        public override void Load()
        {
            Sparkle2 = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/Sparkle2");
            DrainLineBloom2 = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/DrainLineBloom2");
            HeavySmoke = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/HeavySmoke");
            Light = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/Light");
            CritSpark = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/CritSpark");
            ThinSparkle = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/ThinSparkle");
            ThinEndedLine = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/ThinEndedLine");
        }
        public override void Unload()
        {
            Sparkle2 = null;
            DrainLineBloom2 = null;
            HeavySmoke = null;
            Light = null;
            CritSpark = null;
            ThinSparkle = null;
            ThinEndedLine = null;
        }
    }
}
