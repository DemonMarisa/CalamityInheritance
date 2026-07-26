using Terraria;

namespace CalamityInheritance.Content.Rarity.ShopValue
{
    public static class CIShopValue
    {
        private static readonly int RarityPrice0 = Item.buyPrice(0, 0, 50, 0);
        private static readonly int RarityPrice1 = Item.buyPrice(0, 1, 0, 0);
        private static readonly int RarityPrice2 = Item.buyPrice(0, 2, 0, 0);
        private static readonly int RarityPrice3 = Item.buyPrice(0, 4, 0, 0);
        private static readonly int RarityPrice4 = Item.buyPrice(0, 12, 0, 0);
        private static readonly int RarityPrice5 = Item.buyPrice(0, 24, 0, 0);
        private static readonly int RarityPrice6 = Item.buyPrice(0, 36, 0, 0);
        private static readonly int RarityPrice7 = Item.buyPrice(0, 48, 0, 0);
        private static readonly int RarityPrice8 = Item.buyPrice(0, 60, 0, 0);
        private static readonly int RarityPrice9 = Item.buyPrice(0, 80, 0, 0);
        private static readonly int RarityPrice10 = Item.buyPrice(1, 0, 0, 0);
        private static readonly int RarityPrice11 = Item.buyPrice(1, 0, 0, 0); //原版最高的稀有度
        private static readonly int RarityPrice12 = Item.buyPrice(1, 50, 0, 0);
        private static readonly int RarityPrice13 = Item.buyPrice(1, 75, 0, 0);
        private static readonly int RarityPrice14 = Item.buyPrice(2, 0, 0, 0);
        private static readonly int RarityPrice15 = Item.buyPrice(2, 40, 0, 0);
        private static readonly int RarityPrice16 = Item.buyPrice(2, 80, 0, 0);
        private static readonly int RarityPrice17 = Item.buyPrice(3, 20, 0, 0);
        private static readonly int RarityPrice18 = Item.buyPrice(5, 0, 0, 0);
        public static int RarityPriceWhite => RarityPrice0; //白色
        public static int RarityPriceBlue => RarityPrice1; //蓝色
        public static int RarityPriceGreen => RarityPrice2; //绿色
        public static int RarityPriceOrange => RarityPrice3; //橙色
        public static int RarityPriceLightRed => RarityPrice4; //淡红色
        public static int RarityPricePink => RarityPrice5; //粉色
        public static int RarityPriceLightPurple => RarityPrice6; //淡紫色
        public static int RarityPriceLime => RarityPrice7; //淡绿色
        public static int RarityPriceYellow => RarityPrice8; //黄色
        public static int RarityPriceCyan => RarityPrice9; //淡蓝色
        public static int RarityPriceRed => RarityPrice10; //红色
        public static int RarityPricePurple => RarityPrice11; //紫色
        public static int RarityPriceBlueGreen => RarityPrice12; //蓝绿
        public static int RarityPriceAbsoluteGreen => RarityPrice13; //纯绿
        public static int RarityPriceDeepBlue => RarityPrice14; //深蓝
        public static int RarityPriceCatalystViolet => RarityPrice15; //紫罗兰
        public static int RarityPriceDonatorPink => RarityPrice16; //捐赠者
        public static int RarityPricePureRed => RarityPrice17; //灾厄红
        public static int RarityMaliceDrop => RarityPrice18; //恶意掉落
    }
}
