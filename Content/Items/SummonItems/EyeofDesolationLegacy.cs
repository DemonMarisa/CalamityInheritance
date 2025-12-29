using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.MiscItem;
using CalamityInheritance.NPCs.Boss.CalamitasClone;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items.Materials;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.Yharon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.SummonItems
{
    public class EyeofDesolationLegacy : CIMisc, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.MiscItem";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 11; // Pirate Map (1 above Mechanical Skull)
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.rare = ItemRarityID.LightPurple;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossItem;
        }

        public override bool CanUseItem(Player player)
        {
            return !Main.IsItDay() && !NPC.AnyNPCs(NPCType<CalamitasCloneLegacy>()) && !BossRushEvent.BossRushActive;
        }
        public override bool? UseItem(Player player)
        {
            CIFunction.SpawnBossUsingItem<CalamitasCloneLegacy>(player, SoundID.Roar);
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.HellstoneBar, 10).
                AddIngredient<EssenceofHavoc>(7).
                AddIngredient<BlightedLens>(3).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}