using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalamityInheritance.Rarity.Special
{
    public class AlgtPink: ModRarity
    {
        //给予给ACT的特殊稀有度颜色(DB53A0), 仅在config内开启特殊稀有度生效
        public override Color RarityColor => new(219,83,160); //#DB53A0
    }
    public class PlantareGreen: ModRarity
    {
        //给予给灾厄魂CalamitySoul的特殊稀有度颜色，仅在config内开启特殊稀有度时生效
        public override Color RarityColor => new(85,210,28);//#55D21C
    }
    public class SeraphPurple: ModRarity
    {
        //给予给灾厄维度的特殊稀有度颜色，仅在config内开启特殊稀有度时生效
        public override Color RarityColor => new(147,112,219);//#9370db
    }
    public class MurasamRed: ModRarity
    {
        //给予给旧鬼妖的特殊稀有度颜色，默认生效
        public override Color RarityColor => new(210,1,3);//#9370db
    }
}