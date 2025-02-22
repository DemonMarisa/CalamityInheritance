using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Rarity
{
    public class BlueGreen: ModRarity
    {
        //Scarlet:BlueGreen这一稀有度应与灾厄(当前)的Turqoise稀有度同级，即Rarity12
        public override Color RarityColor => new(0,255,200); //#03E4E0
        public override int GetPrefixedRarity(int offset, float valueMult) => offset switch
        {
            -2 => ItemRarityID.Red,
            -1 => ItemRarityID.Purple,
            +1 => ModContent.RarityType<AbsoluteGreen>(),
            +2 => ModContent.RarityType<DeepBlue>     (),
            _  => Type,
        };
    }
} 