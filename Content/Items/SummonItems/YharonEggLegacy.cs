using CalamityMod.Events;
using CalamityMod.Items.Materials;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Rarities;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.NPCs.Boss.Calamitas;
using Terraria.Audio;
using CalamityInheritance.NPCs.Boss.Yharon;
using CalamityInheritance.Utilities;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.CIPlayer;
using CalamityMod.CalPlayer;

namespace CalamityInheritance.Content.Items.SummonItems
{
    public class YharonEggLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.MiscItem";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 19; // Celestial Sigil
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossItem;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<Yharon>()) && !BossRushEvent.BossRushActive && !NPC.AnyNPCs(ModContent.NPCType<YharonLegacy>());
        }

        public override bool? UseItem(Player player)
        {
            CalamityPlayer calPlayer = player.Calamity();
            CalamityInheritancePlayer modPlayer = player.CIMod();
            if (!CIDownedBossSystem.DownedBuffedSolarEclipse && (modPlayer.DarkSunRings == false || calPlayer.darkSunRing == false))
                CIFunction.SpawnBossUsingItem(player,ModContent.NPCType<YharonLegacy>(), Yharon.FireSound);
            else
                CIFunction.SpawnBossUsingItem(player, ModContent.NPCType<Yharon>(), Yharon.FireSound);

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<EffulgentFeather>(15).
                AddIngredient<LifeAlloy>(10).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
