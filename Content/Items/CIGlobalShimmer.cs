using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items
{
    public class CIGlobalShimmer: GlobalItem
    {
        public override void SetStaticDefaults()
        {
        }
        public static void ShimmerEach<I>(int result) where I : ModItem
        {
            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<I>()] = result;
            ItemID.Sets.ShimmerTransformToItem[result] = ModContent.ItemType<I>();
        }
    }
}