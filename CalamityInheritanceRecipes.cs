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
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance
{
    public class CIRecipeGroup : ModSystem
    {
        public static string PreFix = "CalamityInheritance:";
        public static string WoodSwordRecipeGroup;
        // public static string AnyEvilFlask;
        public static string AnyTerratomere;
        public static string AnyPhantasmalFury; 
        public static string LoreSentinal;
        public static string AnyRottenMatter;
        public static string AnyEradicator;
        public static string AnyIchorOrCursedFlame;
        public override void AddRecipeGroups()
        {
            int[] woodSword = [ItemID.WoodenSword, ItemID.WoodenSword, ItemID.AshWoodSword, ItemID.BorealWoodSword, ItemID.EbonwoodSword, ItemID.ShadewoodSword, ItemID.PearlwoodSword, ItemID.PalmWoodSword, ItemType<Basher>()];
            WoodSwordRecipeGroup = LAPUtilities.CreatRecipeGroup(PreFix + "WoodSwordRecipeGroup", woodSword);

            //int[] EvilFlask = [ItemType<CorruptFlask>(), ItemType<CrimsonFlask>()];
            //AnyEvilFlask = LAPUtilities.CreatRecipeGroup(PreFix + "AnyEvilFlask", EvilFlask);

            int[] Terratomere = [ItemType<TerratomereOld>(), ItemType<Terratomere>()];
            AnyTerratomere = LAPUtilities.CreatRecipeGroup(PreFix + "AnyTerratomere", Terratomere);

            int[] PhantasmalFury = [ItemType<PhantasmalFury>(), ItemType<PhantasmalFuryOld>()];
            AnyPhantasmalFury = LAPUtilities.CreatRecipeGroup(PreFix + "AnyPhantasmalFury", PhantasmalFury);

            int[] loreSentinal = [ItemType<LoreStormWeaver>(), ItemType<LoreCeaselessVoid>(), ItemType<LoreSignus>()];
            LoreSentinal = LAPUtilities.CreatRecipeGroup(PreFix + "LoreSentinal", loreSentinal);

            int[] RottenMatter2 = [ItemType<RottenMatter>(), ItemType<BloodSample>()];
            AnyRottenMatter = LAPUtilities.CreatRecipeGroup(PreFix + "AnyRottenMatter", RottenMatter2);

            int[] AnyEradicator2 = [ItemType<Eradicator_Rogue>(), ItemType<Eradicator_Melee>()];
            AnyEradicator = LAPUtilities.CreatRecipeGroup(PreFix + "AnyEradicator", AnyEradicator2);

            int[] cursedFlameAndIchor = [ItemID.Ichor, ItemID.CursedFlame];
            AnyIchorOrCursedFlame = LAPUtilities.CreatRecipeGroup(PreFix + "AnyIchorOrCursedFlame", cursedFlameAndIchor);
        }
        public override void Unload()
        {
            WoodSwordRecipeGroup = null;
            // AnyEvilFlask = null;
            AnyTerratomere = null;
            AnyPhantasmalFury = null;
            LoreSentinal = null;
            AnyRottenMatter = null;
            AnyEradicator = null;
            AnyIchorOrCursedFlame = null;
        }
    }
}
