using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Armor.GodSlayerOld;
using CalamityInheritance.Content.Items.Armor.Silva;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Magic.Ray;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Armor.Silva;
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
        public static RecipeGroup CosmicShivGroup; 
        public static RecipeGroup DeificAmuletGroup;

        public static RecipeGroup GodSlayerBodyGroup;
        public static RecipeGroup GodSlayerLegGroup;
        public static RecipeGroup GodSlayerHeadMeleeGroup;
        public static RecipeGroup GodSlayerHeadRangedGroup;
        public static RecipeGroup GodSlayerHeadRogueGroup;
        public static RecipeGroup SilvaBodyGroup;
        public static RecipeGroup SilvaLegGroup;
        public static RecipeGroup SilvaHeadMagicGroup;
        public static RecipeGroup SilvaHeadSummonGroup;

        public override void Unload()
        {
            ElementalRayRecipeGroup = null;
            PhantasmalFuryRecipeGroup = null;
            HeliumFlashRecipeGroup = null;
            WoodSwordRecipeGroup = null;
            ExoTropyGroup = null;
            CosmicShivGroup = null;
            DeificAmuletGroup = null;

            GodSlayerBodyGroup = null;
            GodSlayerLegGroup = null;
            GodSlayerHeadMeleeGroup = null;
            GodSlayerHeadRangedGroup = null;
            GodSlayerHeadRogueGroup = null;

            SilvaBodyGroup = null;
            SilvaLegGroup = null;
            SilvaHeadMagicGroup = null;
            SilvaHeadSummonGroup = null;
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

            CosmicShivGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<CosmicShiv>())}",
                ModContent.ItemType<CosmicShiv>(), ModContent.ItemType<CosmicShivold>());

            DeificAmuletGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<DeificAmulet>())}",
                ModContent.ItemType<DeificAmulet>(), ModContent.ItemType<DeificAmuletLegacy>());

            #region 新旧弑神
            GodSlayerBodyGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<GodSlayerChestplate>())}",
                ModContent.ItemType<GodSlayerChestplateold>(), ModContent.ItemType<GodSlayerChestplate>());

            GodSlayerLegGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<GodSlayerLeggings>())}",
                ModContent.ItemType<GodSlayerLeggingsold>(), ModContent.ItemType<GodSlayerLeggings>());

            GodSlayerHeadMeleeGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<GodSlayerHeadMelee>())}",
                ModContent.ItemType<GodSlayerHeadMeleeold>(), ModContent.ItemType<GodSlayerHeadMelee>());

            GodSlayerHeadRangedGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<GodSlayerHeadRanged>())}",
                ModContent.ItemType<GodSlayerHeadRangedold>(), ModContent.ItemType<GodSlayerHeadRanged>());

            GodSlayerHeadRogueGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<GodSlayerHeadRogue>())}",
                ModContent.ItemType<GodSlayerHeadRogueold>(), ModContent.ItemType<GodSlayerHeadRogue>());
            #endregion
            #region 新旧林海
            SilvaBodyGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<SilvaArmor>())}",
                ModContent.ItemType<SilvaArmorold>(), ModContent.ItemType<SilvaArmor>());

            SilvaLegGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<SilvaLeggings>())}",
                ModContent.ItemType<SilvaLeggingsold>(), ModContent.ItemType<SilvaLeggings>());

            SilvaHeadMagicGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<SilvaHeadMagic>())}",
                ModContent.ItemType<SilvaHeadMagicold>(), ModContent.ItemType<SilvaHeadMagic>());

            SilvaHeadSummonGroup = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ModContent.ItemType<SilvaHeadSummon>())}",
                ModContent.ItemType<SilvaHeadSummonold>(), ModContent.ItemType<SilvaHeadSummon>());

            #endregion
            // 为了避免名称冲突，当模组物品是配方组的标志性或第一个物品时，命名配方组为：ModName:ItemName
            RecipeGroup.RegisterGroup("CalamityInheritance:AnyElementalRay", ElementalRayRecipeGroup);
            RecipeGroup.RegisterGroup("CalamityInheritance:AnyPhantasmalFury", PhantasmalFuryRecipeGroup);
            RecipeGroup.RegisterGroup("CalamityInheritance:AnyHeliumFlash", HeliumFlashRecipeGroup);
            RecipeGroup.RegisterGroup("CalamityInheritance:AnyWoodenSword", WoodSwordRecipeGroup);
            RecipeGroup.RegisterGroup("CalamityInheritance:AnyExoTropy", ExoTropyGroup);
            RecipeGroup.RegisterGroup("CalamityInheritance:AnyCosmicShiv", CosmicShivGroup);
            RecipeGroup.RegisterGroup("CalamityInheritance:AnyDeificAmulet", DeificAmuletGroup);
            #region 新旧弑神
            RecipeGroup.RegisterGroup("CalamityInheritance:AnyGodSlayerBody", GodSlayerBodyGroup);
            RecipeGroup.RegisterGroup("CalamityInheritance:AnyGodSlayerLeg", GodSlayerLegGroup);
            RecipeGroup.RegisterGroup("CalamityInheritance:AnyGodSlayerHeadMelee", GodSlayerHeadMeleeGroup);
            RecipeGroup.RegisterGroup("CalamityInheritance:AnyGodSlayerHeadRanged", GodSlayerHeadRangedGroup);
            RecipeGroup.RegisterGroup("CalamityInheritance:AnyGodSlayerHeadRogue", GodSlayerHeadRogueGroup);
            #endregion
            #region 新旧林海
            RecipeGroup.RegisterGroup("CalamityInheritance:AnySilvaBody", SilvaBodyGroup);
            RecipeGroup.RegisterGroup("CalamityInheritance:AnySilvaLeg", SilvaLegGroup);
            RecipeGroup.RegisterGroup("CalamityInheritance:AnySilvaHeadMagic", SilvaHeadMagicGroup);
            RecipeGroup.RegisterGroup("CalamityInheritance:AnySilvaHeadSummon", SilvaHeadSummonGroup);
            #endregion
        }

        public void ItemTrain()
        {
            
        }
    }
}
