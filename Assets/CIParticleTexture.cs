using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalamityInheritance.Assets
{
    public class CIParticleTexture : ModSystem
    {
        public static Asset<Texture2D> Sparkle2 { get; set; }
        public static Asset<Texture2D> DrainLineBloom { get; set; }
        public static Asset<Texture2D> DrainLineBloom2 { get; set; }
        public static Asset<Texture2D> HeavySmoke { get; set; }
        public static Asset<Texture2D> Light { get; set; }
        public static Asset<Texture2D> CritSpark { get; set; }
        public static Asset<Texture2D> ThinSparkle { get; set; }
        public static Asset<Texture2D> ThinEndedLine { get; set; }
        public static Asset<Texture2D> PearlParticle { get; set; }
        public static Asset<Texture2D> PearlParticleGlow { get; set; }
        public static Asset<Texture2D> MediumMist { get; set; }
        public override void Load()
        {
            Sparkle2 = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/Sparkle2");
            DrainLineBloom = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/DrainLineBloom");
            DrainLineBloom2 = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/DrainLineBloom2");
            HeavySmoke = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/HeavySmoke");
            Light = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/Light");
            CritSpark = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/CritSpark");
            ThinSparkle = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/ThinSparkle");
            ThinEndedLine = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/ThinEndedLine");
            PearlParticle = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/PearlParticle");
            PearlParticleGlow = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/PearlParticleGlow");
            MediumMist = Request<Texture2D>("CalamityInheritance/Assets/ParticleTextures/MediumMist");
        }
        public override void Unload()
        {
            Sparkle2 = null;
            DrainLineBloom = null;
            DrainLineBloom2 = null;
            HeavySmoke = null;
            Light = null;
            CritSpark = null;
            ThinSparkle = null;
            ThinEndedLine = null;
            PearlParticle = null;
            PearlParticleGlow = null;
            MediumMist = null;
        }
    }
}
