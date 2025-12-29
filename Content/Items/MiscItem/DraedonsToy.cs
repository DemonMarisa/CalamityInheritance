using CalamityInheritance.Rarity.Special;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.MiscItem
{
    public class DraedonsToy: CIMisc, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.MiscItem";
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
            //todo:boss的数值似乎有点不对
            if (player.whoAmI == Main.myPlayer)
            {
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                int worm = NPCID.TheDestroyer;
                int skull = NPCID.SkeletronPrime;
                int redEye = NPCID.Retinazer;
                int greeEye = NPCID.Spazmatism;
                Main.npc[worm].lifeMax *= 2;
                Main.npc[worm].defDamage *= (int)1.5;
                Main.npc[worm].velocity*= (int)1.5;
                Main.npc[worm].scale*= (int)1.5;

                Main.npc[skull].lifeMax *= 2;
                Main.npc[skull].defDamage *= (int)1.5;
                Main.npc[skull].velocity*= (int)1.5;
                Main.npc[skull].scale*= (int)1.5;

                Main.npc[redEye].lifeMax *= 2;
                Main.npc[redEye].defDamage *= (int)1.5;
                Main.npc[redEye].velocity*= (int)1.5;
                Main.npc[redEye].scale*= (int)1.5;

                Main.npc[greeEye].lifeMax *= 2;
                Main.npc[greeEye].defDamage *= (int)1.5;
                Main.npc[greeEye].velocity*= (int)1.5;
                Main.npc[greeEye].scale*= (int)1.5;
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
        //还没做完，暂时注释掉合成代码让他不可合成
        // public override void AddRecipes()
        // {
        //     CreateRecipe().
        //         AddIngredient(ItemID.MechanicalEye, 1).
        //         AddIngredient(ItemID.MechanicalSkull, 1).
        //         AddIngredient(ItemID.MechanicalWorm, 1).
        //         AddIngredient(ItemID.SoulofSight, 10).
        //         AddIngredient(ItemID.SoulofFright, 10).
        //         AddIngredient(ItemID.SoulofMight, 10).
        //         DisableDecraft().
        //         AddTile(TileID.LunarCraftingStation).
        //         Register();
                
        // }
    }
}
