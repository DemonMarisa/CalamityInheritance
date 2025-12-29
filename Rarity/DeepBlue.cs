using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalamityInheritance.Rarity
{
    public class DeepBlue: ModRarity
    {
        //Scarlet:DeepBlue这一稀有度应与灾厄(当前)的DarkBlue稀有度同级，即Rarity14
        public override Color RarityColor => new(43,96,222); //#4314BE

        public override int GetPrefixedRarity(int offset, float valueMult) => offset switch
        {
            -2 => RarityType<BlueGreen>(),
            -1 => RarityType<AbsoluteGreen>(),
            +1 => RarityType<CatalystViolet>(),
            +2 => RarityType<DonatorPink>(),
            _  => Type,
        };
    }
} 