
using CalamityInheritance.Rarity;
using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Items.MiscItem;
using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Tiles.Furniture.CraftingStations;

namespace CalamityInheritance.Content.Items.SummonItems
{
    public class EyeofExtinction : CIMisc, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.MiscItem";
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
        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(NPCType<SupremeCalamitasLegacy>());
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                int npcType = NPCType<SupremeCalamitasLegacy>();

                switch (Main.netMode)
                {
                    // SP: Spawn Boss Immediately
                    case NetmodeID.SinglePlayer:
                        NPC.SpawnBoss((int)player.Center.X, (int)(player.Center.Y - 400), npcType, player.whoAmI);
                        break;

                    // MP: Ask server to spawn one
                    case NetmodeID.MultiplayerClient:
                        NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, npcType);
                        break;
                }
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AuricBarold>(10).
                AddIngredient<AshesofCalamity>(25).
                AddTile<DraedonsForgeold>().
                Register();
        }
    }
}
