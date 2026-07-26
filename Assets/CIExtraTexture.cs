using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalamityInheritance.Assets
{
    public class CIExtraTexture : ModSystem
    {
        public static Asset<Texture2D> TrientCircularSmear { get; set; }
        public override void Load()
        {
            TrientCircularSmear = Request<Texture2D>("CalamityInheritance/Assets/ExtraTextures/TrientCircularSmear");
        }
        public override void Unload()
        {
            TrientCircularSmear = null;
        }
    }
}
