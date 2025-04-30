
using CalamityInheritance.Rarity;
using CalamityMod.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
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
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.consumable = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
        public override bool CanUseItem(Player player)
        {
            return (!NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()));
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                int getBoss = ModContent.NPCType<SupremeCalamitasLegacy>();

                if (Main.netMode != NetmodeID.MultiplayerClient)
                    NPC.SpawnBoss((int)player.Center.X, (int)(player.Center.Y - 400), getBoss, player.whoAmI);
                else
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: getBoss);
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
