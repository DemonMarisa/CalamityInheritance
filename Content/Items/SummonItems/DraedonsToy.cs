using CalamityInheritance.Content.Rarity.Special;
using CalamityInheritance.Core.Path;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.SummonItems
{
    public class DraedonsToy : ModItem, ILocalizedModType
    {
        public override string LocalizationCategory => LocalizationPath.SummonItem;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[NPCID.SkeletronPrime] = true;
            NPCID.Sets.MPAllowedEnemies[NPCID.Retinazer] = true;
            NPCID.Sets.MPAllowedEnemies[NPCID.Spazmatism] = true;
            NPCID.Sets.MPAllowedEnemies[NPCID.TheDestroyer] = true;
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 42;
            Item.maxStack = 1;
            Item.consumable = false;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.rare = RarityType<MurasamRed>();
        }
        public override bool CanUseItem(Player player)
        {
            bool canSummon = NPC.AnyNPCs(NPCID.TheDestroyer) && NPC.AnyNPCs(NPCID.SkeletronPrime) && NPC.AnyNPCs(NPCID.Retinazer) && NPC.AnyNPCs(NPCID.Spazmatism);
            return !canSummon;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                int worm = NPCID.TheDestroyer;
                int skull = NPCID.SkeletronPrime;
                int redEye = NPCID.Retinazer;
                int greeEye = NPCID.Spazmatism;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, worm);
                    NPC.SpawnOnPlayer(player.whoAmI, skull);
                    NPC.SpawnOnPlayer(player.whoAmI, redEye);
                    NPC.SpawnOnPlayer(player.whoAmI, greeEye);
                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: worm);
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: skull);
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: redEye);
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: greeEye);
                }
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MechanicalEye, 1).
                AddIngredient(ItemID.MechanicalSkull, 1).
                AddIngredient(ItemID.MechanicalWorm, 1).
                AddIngredient(ItemID.SoulofSight, 10).
                AddIngredient(ItemID.SoulofFright, 10).
                AddIngredient(ItemID.SoulofMight, 10).
                DisableDecraft().
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
