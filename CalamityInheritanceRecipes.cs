using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Melee.Boomerang;
using CalamityInheritance.Content.Items.Weapons.Rogue.Boomerang;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
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
        public static RecipeGroup AnyTerratomere;
        public static RecipeGroup AnyPhantasmalFury; 
        public static RecipeGroup LoreSentinal;
        public static RecipeGroup AnyRottenMatter;
        public static RecipeGroup AnyEradicator;
        public override void AddRecipeGroups()
        {
            int[] woodSword = [ItemID.WoodenSword, ItemID.WoodenSword, ItemID.AshWoodSword, ItemID.BorealWoodSword, ItemID.EbonwoodSword, ItemID.ShadewoodSword, ItemID.PearlwoodSword, ItemID.PalmWoodSword, ModContent.ItemType<Basher>()];
            LAPUtilities.CreatRecipeGroup(ref WoodSwordRecipeGroup, PreFix + WoodSwordRecipeGroup, woodSword);

            int[] EvilFlask = [ModContent.ItemType<CorruptFlask>(), ModContent.ItemType<CrimsonFlask>()];
            LAPUtilities.CreatRecipeGroup(ref AnyEvilFlask, PreFix + AnyEvilFlask, EvilFlask);

            int[] DeificAmulet = [ModContent.ItemType<DeificAmulet>(), ModContent.ItemType<DeificAmuletLegacy>()];
            LAPUtilities.CreatRecipeGroup(ref AnyDeificAmulet, PreFix + AnyDeificAmulet, DeificAmulet);

            int[] Terratomere = [ModContent.ItemType<TerratomereOld>(), ModContent.ItemType<Terratomere>()];
            LAPUtilities.CreatRecipeGroup(ref AnyTerratomere, PreFix + AnyTerratomere, Terratomere);

            int[] PhantasmalFury = [ModContent.ItemType<PhantasmalFury>(), ModContent.ItemType<PhantasmalFuryOld>()];
            LAPUtilities.CreatRecipeGroup(ref AnyPhantasmalFury, PreFix + AnyPhantasmalFury, PhantasmalFury);

            int[] loreSentinal = [ModContent.ItemType<LoreStormWeaver>(), ModContent.ItemType<LoreCeaselessVoid>(), ModContent.ItemType<LoreSignus>()];
            LAPUtilities.CreatRecipeGroup(ref LoreSentinal, PreFix + LoreSentinal, loreSentinal);

            int[] RottenMatter2 = [ModContent.ItemType<RottenMatter>(), ModContent.ItemType<BloodSample>()];
            LAPUtilities.CreatRecipeGroup(ref AnyRottenMatter, PreFix + AnyRottenMatter, RottenMatter2);

            int[] AnyEradicator2 = [ModContent.ItemType<Eradicator_Rogue>(), ModContent.ItemType<Eradicator_Melee>()];
            LAPUtilities.CreatRecipeGroup(ref AnyEradicator, PreFix + AnyEradicator, AnyEradicator2);
        }
        public override void Unload()
        {
            WoodSwordRecipeGroup = null;
            AnyEvilFlask = null;
            AnyDeificAmulet = null;
            AnyTerratomere = null;
            AnyPhantasmalFury = null;
            LoreSentinal = null;
            AnyRottenMatter = null;
            AnyEradicator = null;
        }
    }
}
