using CalamityInheritance.Content.Items.Materials;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items
{
    public static class CIRecipesCallback
    {
        public static void DConsumeMatter(Recipe recipe, int type, ref int amount)
        {
            if (type == ModContent.ItemType<AncientMiracleMatter>())
            {
                amount = 0;
            }
        }
    }
}