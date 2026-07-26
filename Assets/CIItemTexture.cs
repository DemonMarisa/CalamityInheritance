using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CalamityInheritance.Assets
{
    public class CIItemTexture : ModSystem
    {
        public static Asset<Texture2D> MurasamaSheathed { get; set; }
        public static Asset<Texture2D> SunderingScissorsRight { get; set; }
        public static Asset<Texture2D> SunderingScissorsLeft { get; set; }
        public override void Load()
        {
            MurasamaSheathed = Request<Texture2D>("CalamityInheritance/Assets/Items/Melee/Katanas/MurasamaSheathed");
            SunderingScissorsRight = Request<Texture2D>("CalamityInheritance/Assets/Items/Melee/AOTC/SunderingScissorsRight");
            SunderingScissorsLeft = Request<Texture2D>("CalamityInheritance/Assets/Items/Melee/AOTC/SunderingScissorsLeft");
        }
        public override void Unload()
        {
            MurasamaSheathed = null;
            SunderingScissorsRight = null;
            SunderingScissorsLeft = null;
        }
    }
}
