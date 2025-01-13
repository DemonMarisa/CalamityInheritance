using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalamityInheritance.Rarity
{
    public class OrangeDraedon: ModRarity
    {
        //Scarlet:OrangeDraedon这一稀有度应与灾厄(当前)的Orange稀有度
        //同级，这一稀有度目前只用于嘉登实验室类等物品
        public override Color RarityColor => new(204,71,35); //#1F3C9F
    }
} 