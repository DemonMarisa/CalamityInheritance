using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Magic.Ray;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance
{
    public class CalamityInheritanceRecipes : ModSystem
    {
        // A place to store the recipe group so we can easily use it later
        public static RecipeGroup ElementalRayRecipeGroup;
        public static RecipeGroup PhantasmalFuryRecipeGroup;
        public static RecipeGroup HeliumFlashRecipeGroup;
        public static RecipeGroup WoodSwordRecipeGroup;
        public static RecipeGroup ExoTropyGroup;
        public override void Unload()
        {
            ElementalRayRecipeGroup = null;
            PhantasmalFuryRecipeGroup = null;
            HeliumFlashRecipeGroup = null;
            WoodSwordRecipeGroup = null;
            ExoTropyGroup = null;
        }
        public override void AddRecipeGroups()
        {
            // 创建并存储一个配方组
            // Language.GetTextValue("LegacyMisc.37") 是英文中的 "Any" 一词，并对应其他语言中的相应词汇
            ElementalRayRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<ElementalRay>())}",
                ModContent.ItemType<ElementalRay>(), ModContent.ItemType<ElementalRayold>());

            PhantasmalFuryRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<PhantasmalFury>())}",
                ModContent.ItemType<PhantasmalFury>(), ModContent.ItemType<PhantasmalFuryOld>());

            HeliumFlashRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<HeliumFlash>())}",
                ModContent.ItemType<HeliumFlash>(), ModContent.ItemType<HeliumFlashLegacy>());

            WoodSwordRecipeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.WoodenSword)}",
                                                   ItemID.WoodenSword,
                                                   ItemID.AshWoodSword,
                                                   ItemID.BorealWoodSword,
                                                   ItemID.EbonwoodSword,
                                                   ItemID.ShadewoodSword,
                                                   ItemID.PearlwoodSword,
                                                   ItemID.PalmWoodSword,
                                                   ModContent.ItemType<Basher>());

            ExoTropyGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<AresTrophy>())}",
                ModContent.ItemType<ThanatosTrophy>(), 
                ModContent.ItemType<ApolloTrophy>(),
                ModContent.ItemType<ArtemisTrophy>(),
                ModContent.ItemType<AresTrophy>());

            // 为了避免名称冲突，当模组物品是配方组的标志性或第一个物品时，命名配方组为：ModName:ItemName
            RecipeGroup.RegisterGroup("CalamityInheritance:AnyElementalRay", ElementalRayRecipeGroup);
            RecipeGroup.RegisterGroup("CalamityInheritance:AnyPhantasmalFury", PhantasmalFuryRecipeGroup);
            RecipeGroup.RegisterGroup("CalamityInheritance:AnyHeliumFlash", HeliumFlashRecipeGroup);
            RecipeGroup.RegisterGroup("CalamityInheritance:AnyWoodenSword", WoodSwordRecipeGroup);
            RecipeGroup.RegisterGroup("CalamityInheritance:AnyExoTropy", ExoTropyGroup);
        }
    }
}
