using CalamityInheritance.CIPlayer;
using CalamityInheritance.NPCs.Boss.CalamitasClone;
using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityInheritance.NPCs.Boss.Yharon;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Events;
using CalamityMod.Items.Materials;
using CalamityMod.NPCs.Yharon;
using CalamityMod.Rarities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

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
            Item.rare = RarityType<DeepBlue>();
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossItem;
        }

        public override bool CanUseItem(Player player)
        {
            return !BossRushEvent.BossRushActive && !NPC.AnyNPCs(NPCType<YharonLegacy>()) && player.ZoneJungle;
        }

        public override bool? UseItem(Player player)
        {
            int npcType = NPCType<YharonLegacy>();

            switch (Main.netMode)
            {
                // SP: Spawn Boss Immediately
                case NetmodeID.SinglePlayer:
                    NPC.SpawnOnPlayer(player.whoAmI, npcType);
                    break;

                // MP: Ask server to spawn one
                case NetmodeID.MultiplayerClient:
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, npcType);
                    break;
            }
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
