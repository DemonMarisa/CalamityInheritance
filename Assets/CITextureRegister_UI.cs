using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalamityInheritance.Assets
{
    public class CITextureRegister_UI : ModSystem
    {
        public static Asset<Texture2D> AstralArcanumCircles { get; private set; }
        public override void Load()
        {
            AstralArcanumCircles = Request<Texture2D>("CalamityInheritance/Assets/UI/AstralArcanum/AstralArcanumCircles");
        }
        public override void Unload()
        {
            AstralArcanumCircles = null;
        }
    }
}
