using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.Utils
{
    public static partial class CIUtils
    {
        public static Recipe RegisterCal(this Recipe rec)
        {
            if (HasCalamity() && rec is not null)
                return rec.Register();
            else
                return null;
        }
        public static Recipe CreateCalRecipe(int Type, int amount = 1)
        {
            if (HasCalamity())
                return Recipe.Create(Type, amount);
            else
                return null;
        }
        public static Recipe AddCalTile(this Recipe rec, int item)
        {
            if (HasCalamity() && rec is not null)
                return rec.AddTile(item);
            else
                return null;
        }
        public static Recipe AddCalIngredient(this Recipe rec, int item, int stack = 1)
        {
            if (HasCalamity() && rec is not null)
                return rec.AddIngredient(item, stack);
            else
                return null;
        }
        public static Recipe AddCalIngredient(this Recipe rec, ModItem item, int stack = 1)
        {
            if (HasCalamity() && rec is not null)
                return rec.AddIngredient(item.Type, stack);
            else
                return null;
        }
        public static Recipe AddCalIngredient<T>(this Recipe rec, int stack = 1) where T : ModItem
        {
            if (HasCalamity() && rec is not null)
                return rec.AddIngredient(ItemType<T>(), stack);
            else
                return null;
        }
    }
}
