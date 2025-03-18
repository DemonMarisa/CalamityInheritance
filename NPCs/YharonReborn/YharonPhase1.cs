// using CalamityInheritance.Content.Items;
// using Terraria;
// using Terraria.ID;
// using Terraria.ModLoader;

// namespace CalamityInheritance.NPCs.YharonReborn
// {
//     [AutoloadBossHead]
//     public class YharonPhase1 : ModNPC
//     {
//         public override void SetStaticDefaults()
//         {
//             Main.npcFrameCount[NPC.type] = 4;
//             NPCID.Sets.TrailingMode[NPC.type] = 1;
//         }
//         public override void SetDefaults()
//         {
//             //直接写死，我们不采用任何原灾的差分
//             NPC.damage = 400;
//             NPC.npcSlots = 16f;
//             NPC.width = 120;
//             NPC.height = 120;
//             NPC.value  = CIShopValue.RarityPriceDeepBlue;
//             NPC.defense = 60;
//         }
//     }
// }