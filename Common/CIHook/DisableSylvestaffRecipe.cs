using CalamityInheritance.System.Configs;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.NPCs.DevourerofGods;
using CalamityMod.Tiles.Furniture.CraftingStations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CIHook
{
    public class DisableSylvestaffRecipe
    {
        public static void Load()
        {
            MethodInfo originalMethod = typeof(Sylvestaff).GetMethod(nameof(Sylvestaff.AddRecipes));
            MonoModHooks.Add(originalMethod, AddRecipes_Hook);
        }

        public static void AddRecipes_Hook(Sylvestaff self)
        {
            if (!CIServerConfig.Instance.FuckYouLGBT)
                return;

            self.CreateRecipe().
                AddIngredient(ItemID.RainbowRod).
                AddRecipeGroup("AnyGoldBar", 5).
                AddIngredient<Necroplasm>(10).
                AddIngredient<ShadowspecBar>(5).
                AddTile<DraedonsForge>().
                Register();
        }

    }
}
