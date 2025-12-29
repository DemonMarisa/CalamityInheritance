using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalamityInheritance.Rarity
{
    public class DonatorPink : ModRarity
    {
        //Scarlet:DonatorPink这一稀有度应与灾厄(当前)的HotPink稀有度同级，即Rarity16
        public override Color RarityColor => new(255,0,255); //#FF00FF
        public override int GetPrefixedRarity(int offset, float valueMult) => offset switch
        {
            -2 => RarityType<DeepBlue>(),
            -1 => RarityType<CatalystViolet>(),
            +1 => RarityType<PureRed>(),
            _  => Type,
        };
    }
} 