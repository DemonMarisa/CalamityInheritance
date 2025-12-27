using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityMod.Items.Materials;
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
        public static void DConsumeMatter(Recipe recipe, int type, ref int amount, bool isDecrafting)
        {
            if (type == ModContent.ItemType<AncientMiracleMatter>())
                amount = 0;
        }
        public static void DontConsumeExoWeapons(Recipe recipe, int type, ref int amount, bool isDecrafting)
        {
            if (type == ModContent.ItemType<HeavenlyGaleold>())
            {
                amount = 0;
            }
        }
        public static void DontConsumePostDOGMaterials(Recipe recipe, int type, ref int amount, bool isDecrafting)
        {
            if (type == ModContent.ItemType<YharonSoulFragment>())
            {
                amount = 0;
            }

            if (type == ModContent.ItemType<AscendantSpiritEssence>())
            {
                amount = 0;
            }
        }

        public static void IfDragonBow(Recipe recipe, int type, ref int amount, bool isDecrafting)
        {
            if(type == ModContent.ItemType<HeavenlyGaleold>()) amount = 0;
            if(type == ModContent.ItemType<AscendantSpiritEssence>()) amount = 0;
        }
    }
}