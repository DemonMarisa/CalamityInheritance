using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.World;

namespace CalamityInheritance.System.ModeChange.Defiled
{
    public class DefiledChange : GlobalNPC
    {
        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
        {
            if (CIWorld.Defiled)
            {
                npc.value *= 2;
            }
        }

        public override void OnKill(NPC npc)
        {
            if (CIWorld.Defiled)
            {
                int baseGold = (int)npc.value;
                int extraGold = baseGold * 2;
                Item.NewItem(npc.GetSource_Loot(), npc.position, ItemID.GoldCoin, extraGold);
            }
        }
    }
}
