using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Accessories.Wings;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Magic.Ray;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance
{
    public class CIRecipeGroup : ModSystem
    {
        public static string PreFix = "CalamityInheritance:";
        public static RecipeGroup WoodSwordRecipeGroup;
        // 任何邪恶瓶
        public static RecipeGroup AnyEvilFlask;
        public static RecipeGroup AnyDeificAmulet;
        public static RecipeGroup AnyTracersElysian;
        public static RecipeGroup AnyTerratomere;
        public static RecipeGroup AnyElementalRay;
        public static RecipeGroup AnyPhantasmalFury; 
        public static RecipeGroup LoreSentinal;
        public static RecipeGroup AnyEradicator;
        public static RecipeGroup AnyRottenMatter;
        public override void AddRecipeGroups()
        {
            int[] woodSword = [ItemID.WoodenSword, ItemID.WoodenSword, ItemID.AshWoodSword, ItemID.BorealWoodSword, ItemID.EbonwoodSword, ItemID.ShadewoodSword, ItemID.PearlwoodSword, ItemID.PalmWoodSword, ModContent.ItemType<Basher>()];
            LAPUtilities.CreatRecipeGroup(ref WoodSwordRecipeGroup, PreFix + WoodSwordRecipeGroup, woodSword);

            int[] EvilFlask = [ModContent.ItemType<CorruptFlask>(), ModContent.ItemType<CrimsonFlask>()];
            LAPUtilities.CreatRecipeGroup(ref AnyEvilFlask, PreFix + AnyEvilFlask, EvilFlask);

            int[] DeificAmulet = [ModContent.ItemType<DeificAmulet>(), ModContent.ItemType<DeificAmuletLegacy>()];
            LAPUtilities.CreatRecipeGroup(ref AnyDeificAmulet, PreFix + AnyDeificAmulet, DeificAmulet);

            int[] TracersElysian = [ModContent.ItemType<TracersElysian>(), ModContent.ItemType<FasterGodSlayerTracers>()];
            LAPUtilities.CreatRecipeGroup(ref AnyTracersElysian, PreFix + AnyTracersElysian, TracersElysian);

            int[] Terratomere = [ModContent.ItemType<TerratomereOld>(), ModContent.ItemType<Terratomere>()];
            LAPUtilities.CreatRecipeGroup(ref AnyTerratomere, PreFix + AnyTerratomere, Terratomere);

            int[] ElementalRay = [ModContent.ItemType<ElementalRay>(), ModContent.ItemType<ElementalRayold>()];
            LAPUtilities.CreatRecipeGroup(ref AnyElementalRay, PreFix + AnyElementalRay, ElementalRay);

            int[] PhantasmalFury = [ModContent.ItemType<PhantasmalFury>(), ModContent.ItemType<PhantasmalFuryOld>()];
            LAPUtilities.CreatRecipeGroup(ref AnyPhantasmalFury, PreFix + AnyPhantasmalFury, PhantasmalFury);

            int[] loreSentinal = [ModContent.ItemType<LoreStormWeaver>(), ModContent.ItemType<LoreCeaselessVoid>(), ModContent.ItemType<LoreSignus>()];
            LAPUtilities.CreatRecipeGroup(ref LoreSentinal, PreFix + LoreSentinal, loreSentinal);

            int[] Eradicator = [ModContent.ItemType<Eradicator>(), ModContent.ItemType<LoreCeaselessVoid>(), ModContent.ItemType<MeleeTypeEradicator>()];
            LAPUtilities.CreatRecipeGroup(ref AnyEradicator, PreFix + AnyEradicator, Eradicator);

            int[] RottenMatter = [ModContent.ItemType<RottenMatter>(), ModContent.ItemType<BloodSample>()];
            LAPUtilities.CreatRecipeGroup(ref AnyRottenMatter, PreFix + AnyRottenMatter, RottenMatter);
        }
        public override void Unload()
        {
            WoodSwordRecipeGroup = null;
            AnyEvilFlask = null;
            AnyDeificAmulet = null;
            AnyTracersElysian = null;
            AnyTerratomere = null;
            AnyElementalRay = null;
            AnyPhantasmalFury = null;
            LoreSentinal = null;
            AnyEradicator = null;
            AnyRottenMatter = null;
        }
    }
}
