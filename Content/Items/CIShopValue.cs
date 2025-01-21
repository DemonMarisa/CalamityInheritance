using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items
{
    public partial class CIShopValue: GlobalItem
    {
       #region CustomRarityPrices
        //Scarlet:
        //当前该Mod使用的稀有度都是直接调用的灾厄提供的API。
        //不是很确定如果哪天灾厄重写了一遍稀有度相关的东西的话会导致什么后果。
        //因此由于这个目的我就在这里打了一轮稀有度的价格表，可能会方便之后的维护
        private static readonly int RarityPrice0  = Item.buyPrice(0,0,50,0);
        private static readonly int RarityPrice1  = Item.buyPrice(0,1,0,0) ;
        private static readonly int RarityPrice2  = Item.buyPrice(0,2,0,0) ;
        private static readonly int RarityPrice3  = Item.buyPrice(0,4,0,0) ;
        private static readonly int RarityPrice4  = Item.buyPrice(0,12,0,0);
        private static readonly int RarityPrice5  = Item.buyPrice(0,24,0,0);
        private static readonly int RarityPrice6  = Item.buyPrice(0,36,0,0);
        private static readonly int RarityPrice7  = Item.buyPrice(0,48,0,0);
        private static readonly int RarityPrice8  = Item.buyPrice(0,60,0,0);
        private static readonly int RarityPrice9  = Item.buyPrice(0,80,0,0);
        private static readonly int RarityPrice10 = Item.buyPrice(1,0,0,0) ;
        private static readonly int RarityPrice11 = Item.buyPrice(1,0,0,0) ; //原版最高的稀有度（紫）

        #region RarityFreeSlot
        //下面的几个稀有度都是灾厄的
        private static readonly int RarityPrice12 = Item.buyPrice (1,50,0,0);
        private static readonly int RarityPrice13 = Item.buyPrice (1,75,0,0);
        private static readonly int RarityPrice14 = Item.buyPrice (2,0,0,0) ;
        private static readonly int RarityPrice15 = Item.buyPrice (2,40,0,0);
        private static readonly int RarityPrice16 = Item.buyPrice (2,80,0,0);
        private static readonly int RarityPrice17 = Item.buyPrice (3,20,0,0);
        #endregion

        private static readonly int RarityPrice18 = Item.buyPrice (0,50,0,0);

        private static readonly int[] RarityPriceTrain = [
            RarityPrice0 ,
            RarityPrice1 ,
            RarityPrice2 ,
            RarityPrice3 ,
            RarityPrice4 ,
            RarityPrice5 ,
            RarityPrice6 ,
            RarityPrice7 , 
            RarityPrice8 ,
            RarityPrice9 ,
            RarityPrice10,
            RarityPrice11,
            RarityPrice12,
            RarityPrice13,
            RarityPrice14,
            RarityPrice15,
            RarityPrice16,
            RarityPrice17,
            RarityPrice18,
        ];
        //Scarlet:下面才是能够用来调用的稀有度价格清单。如果后续有新物品（如果有）的话直接用这里的稀有度就行了
        public static int RarityPriceWhite       =>  RarityPrice0; //白色
        public static int RarityPriceBlue        =>  RarityPrice1; //蓝色
        public static int RarityPriceGreen       =>  RarityPrice2; //绿色
        public static int RarityPriceOrange      =>  RarityPrice3; //橙色
        public static int RarityPriceLightRed    =>  RarityPrice4; //淡红色
        public static int RarityPricePink        =>  RarityPrice5; //粉色
        public static int RarityPriceLightPurple =>  RarityPrice6; //淡紫色
        public static int RarityPriceLime        =>  RarityPrice7; //淡绿色
        public static int RarityPriceYellow      =>  RarityPrice8; //黄色
        public static int RarityPriceCyan        =>  RarityPrice9; //淡蓝色
        public static int RarityPriceRed         => RarityPrice10; //红色
        public static int RarityPricePurple      => RarityPrice11; //紫色
        //往下都是灾厄的品质价目表
        public static int RarityPriceBlueGreen     => RarityPrice12; //蓝绿
        public static int RarityPriceAbsoluteGreen => RarityPrice13; //纯绿
        public static int RarityPriceDeepBlue      => RarityPrice14; //深蓝
        public static int RarityPriceCatalystViolet  => RarityPrice15; //紫罗兰
        public static int RarityPriceDonatorPink   => RarityPrice16; //捐赠者
        public static int RarityPricePureRed       => RarityPrice17; //灾厄红
        public static int RarityMaliceDrop         => RarityPrice18; //恶意掉落
        #endregion 
    }
}