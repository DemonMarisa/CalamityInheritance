using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Placeables.CraftingStations;
using CalamityInheritance.Content.NPCs.Boss.CalamitasClone;
using CalamityInheritance.Content.NPCs.Boss.Scal;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Tiles.CraftingStations;
using CalamityInheritance.Core.Path;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.SummonItems
{
    public class EyeofExtinction : ModItem, ILocalizedModType
    {
        public override string LocalizationCategory => LocalizationPath.SummonItem;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 42;
            Item.noMelee = true;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.maxStack = 1;
            Item.rare = RarityType<CatalystViolet>();
            Item.consumable = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossItem;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(NPCType<SupremeCalamitasLegacy>());
        }
        public override bool? UseItem(Player player)
        {
            SpawnBossUsingItem<SupremeCalamitasLegacy>(player, SoundID.Roar);
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AuricBarold>(10).
                AddTile<DraedonsForgeoldTile>().
                Register();
        }
    }
}
