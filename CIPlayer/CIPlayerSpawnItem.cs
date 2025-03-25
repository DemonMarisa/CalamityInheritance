using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public bool bloodMoonLoreSpawn = true;
        public bool currentBloodMoon = false;
        public void CISpawnItem()
        {
            CalamityPlayer calPlayer = Player.Calamity();
            CalamityInheritancePlayer clPlayer = Player.CIMod();
            if(bloodMoonLoreSpawn)
            {
                if (Main.bloodMoon)
                {
                    currentBloodMoon = true;
                }
                if(!Main.bloodMoon && currentBloodMoon && bloodMoonLoreSpawn)
                {
                    Player.QuickSpawnItem(Player.GetSource_GiftOrReward(), ModContent.ItemType<KnowledgeBloodMoon>(), 1);
                    bloodMoonLoreSpawn = false;
                }
            }
        }
    }
}
