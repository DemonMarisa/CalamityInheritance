using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items
{
    public struct BaseSetDefault(int itemWidth, int itemHeight, int itemRare, int itemValue, int itemDefense = 0)
    {
        //所有的锤子逻辑实际上只有潜伏有区别
        public int Width = itemWidth;
        public int Height = itemHeight;
        public int Rare = itemRare;
        public int Value = itemValue;
        public int Defense = itemDefense;
    }
    public abstract class CIAccessories: ModItem, ILocalizedModType
    {
        protected virtual BaseSetDefault BaseSD{ get; }
        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ExSSD();
        }

        public override void SetDefaults()
        {
            Item.width = BaseSD.Width;
            Item.height = BaseSD.Height;
            Item.value = BaseSD.Value;
            Item.rare = BaseSD.Rare;
            Item.defense = BaseSD.Defense;
            Item.accessory = true;
            ExSD();
            base.SetDefaults();
        }
        public virtual void ExSD() {}
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Accessories;
        }
        public virtual void ExSSD(){}
    }
    public static class AccessoriesMethods
    {
        public static bool SetConflictMod<T>(this int self, Item equipped, Item incoming) where T : ModItem => SetConflict(self, equipped, incoming, ItemType<T>());
        public static bool SetConflict(this int self, Item equipped, Item incoming, int alter) => (equipped.type == self && incoming.type == alter) || (equipped.type == alter && incoming.type == self);
    }
}