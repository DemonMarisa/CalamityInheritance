using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.ModLoader;
//特殊稀有度颜色，部分只有特定设置开启的时候才会生效
namespace CalamityInheritance.Rarity.Special
{
    public class AlgtPink: ModRarity
    {
        //给予给ACT的特殊稀有度颜色(DB53A0), 仅在config内开启特殊稀有度生效
        //淡粉
        public override Color RarityColor => new(219,83,160); //#DB53A0
    }
    public class PlantareGreen: ModRarity
    {
        //给予给灾厄魂CalamitySoul的特殊稀有度颜色，仅在config内开启特殊稀有度时生效
        //翠绿
        public override Color RarityColor => new(85,210,28);//#55D21C
    }
    public class SeraphPurple: ModRarity
    {
        //给予给灾厄维度的特殊稀有度颜色，仅在config内开启特殊稀有度时生效
        //接近紫与淡紫的区间
        public override Color RarityColor => new(147,112,219);//#9370db
    }
    public class MurasamRed: ModRarity
    {
        //给予给旧鬼妖的特殊稀有度颜色，默认生效
        //近似深红
        public override Color RarityColor => new(210,1,3);//#9370db
    }
    public class TrueScarlet: ModRarity
    {
        //给予我自己的特殊稀有度颜色. 仅在config内开启特殊稀有度时生效
        //近似鲜红
        public override Color RarityColor => new(228,1,10);//E4080A
    }
    public class IchikaBlack: ModRarity
    {
        //给予久远寺 一花的特殊稀有度颜色，仅在config开启特殊稀有度时生效, 目前用于返厂的极乐升天炮上
        //近似暗灰
        public override Color RarityColor => new(79,79,79);//#4F4F4F
    }
    public class YharonFire: ModRarity
    {
        //给予回归的丛林龙旧物特殊稀有度颜色
        //近似金黄
        public override Color RarityColor => new(255,165,0);
    }
    public class ShizukuSilver: ModRarity
    {
        //给予...的特殊稀有度颜色，仅在config开启特殊稀有度时生效
        //近似银白
        public override Color RarityColor => new(248,248,255);//#F8F8FF
    }
    public class ShizukuAqua: ModRarity
    {
        //给予..的特殊稀有度颜色，仅在config开启特殊稀有度时生效
        //近似青蓝
        public override Color RarityColor => new(152,245,255);//#98F5FF
    }
    public class ArcueidColor: ModRarity
    {
        //公主
        public override Color RarityColor => new(152, 245, 249);
    }
    public class DukeAqua: ModRarity 
    {
        //海爵剑稀有度
        public override Color RarityColor => new(53, 255, 255); //#35FFFF
    }
    public class PBGLime: ModRarity
    {
        //孔雀翎稀有度
        public override Color RarityColor => new(0, 255, 127); //#00FF7F;
    }
    public class GolemPurple: ModRarity
    {
        //庇护之刃稀有度
        public override Color RarityColor => new(145, 115, 177); //#9173B1
    }
    public class CryogenBlue: ModRarity
    {
        //寒冰神性稀有度
        public override Color RarityColor => new(5, 63, 139); //#053F8B
    }
    public class PlanteraGreen: ModRarity
    {
        //叶流稀有度
        public override Color RarityColor => new(0, 203, 103); //#00CB67
    }
    public class BetsyPink: ModRarity
    {
        //贝特西稀有度
        public override Color RarityColor => new(255, 0, 155);   //#FF009B
    }
    public class SHPCAqua: ModRarity
    {
        //SHPC稀有度
        public override Color RarityColor => new(65, 105, 225); //#4169E1
    }
}