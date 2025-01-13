using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalamityInheritance.Rarity
{
    public class CatalystViolet: ModRarity
    {
        //Scarlet:LegacyViolet这一稀有度应与灾厄(当前)的Violet稀有度同级，即Rarity15
        public override Color RarityColor => new(108,45,199); //#A57C0F

        public override int GetPrefixedRarity(int offset, float valueMult) => offset switch
        {
            -2 => ModContent.RarityType<AbsoluteGreen>(),
            -1 => ModContent.RarityType<DeepBlue>     (),
            +1 => ModContent.RarityType<DonatorPink>  (),
            +2 => ModContent.RarityType<PureRed>      (),
            _  => Type,
        };
    }
    
} 