using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalamityInheritance.Rarity
{
    public class MaliceChallengeDrop: ModRarity
    {
        //Scarlet:MaliceChallengeDrop这一稀有度应与灾厄曾经的恶意专属掉落的稀有度同级
        //这一稀有度应当只用于目前曾经是恶意掉落的武器
        public override Color RarityColor => new(255,140,0); //#FF8C00
    }
} 