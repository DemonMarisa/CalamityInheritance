using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Rarity
{
    public class AbsoluteGreen: ModRarity
    {
        //Scarlet:AbsoluteGreen这一稀有度应与灾厄(当前)的PureGreen稀有度同级，即Rarity13
        public override Color RarityColor => new(0,255,0); //#00FF00
        public override int GetPrefixedRarity(int offset, float valueMult) => offset switch
        {
            -2 => ItemRarityID.Purple,
            -1 => ModContent.RarityType<BlueGreen>      (),
            +1 => ModContent.RarityType<DeepBlue>       (),
            +2 => ModContent.RarityType<CatalystViolet> (),
            _  => Type,
        };
    }
} 