using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items
{
    public abstract class CIAccessories: ModItem
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Accessories;
        }
    }
    public static class AccessoriesMethods
    {
        public static bool SetConflictMod<T>(this int self, Item equipped, Item incoming) where T : ModItem => SetConflict(self, equipped, incoming, ModContent.ItemType<T>());
        public static bool SetConflict(this int self, Item equipped, Item incoming, int alter) => (equipped.type == self && incoming.type == alter) || (equipped.type == alter && incoming.type == self);
    }
}