using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalamityInheritance.Rarity
{
    public class PureRed : ModRarity
    {
        //Scarlet:LegacyRed这一稀有度应与灾厄(当前)的CalamityRed稀有度同级，即Rarity17
        public override Color RarityColor => new(163,25,26); //#A3191A
    
        //Scarlet:下方用于带词缀的武器之后的稀有度切换（或者说，品质切换）
        public override int GetPrefixedRarity(int offset, float valueMult) => offset switch
        {
            -2 => ModContent.RarityType<CatalystViolet> (),
            -1 => ModContent.RarityType<DonatorPink>    (),
            _  => Type,
        };
    }
}
 