using System;
using System.Threading;
using CalamityInheritance.Content.Items.Accessories;
using CalamityMod.Events;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Vanity;
using CalamityMod.Items.Fishing;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Materials;
using CalamityMod.Items.PermanentBoosters;
using CalamityMod.Items.Pets;
using CalamityMod.Items.Placeables.Furniture.DevPaintings;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Items.Weapons.Typeless;
using CalamityMod.NPCs.AcidRain;
using CalamityMod.NPCs.Astral;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.TownNPCs;
using CalamityMod.Tiles.Ores;
using CalamityMod.World;
using CalamityMod.World.Planets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs
{
    public partial class CalamityInheritanceGlobalNPC : GlobalNPC
    {

        #region Modify NPC Loot Main Hook
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            // Internal function to determine whether this NPC is the second Twin killed in a fight, regardless of which Twin it is.
            bool IsLastTwinStanding(DropAttemptInfo info)
            {
                NPC npc = info.npc;
                if (npc is null)
                    return false;

                if (npc.type == NPCID.Retinazer)
                    return !NPC.AnyNPCs(NPCID.Spazmatism);
                else if (npc.type == NPCID.Spazmatism)
                    return !NPC.AnyNPCs(NPCID.Retinazer);

                return false;
            }

            switch (npc.type)
            {
                #region Underground
                // AnomuraFungus
                // FungalCarapace @ 14.29% Normal, 25% Expert+
                case NPCID.AnomuraFungus:
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<FungalCarapace>(), 7, 4));
                break;
                #endregion
            }
        }
        #endregion
    }
}
