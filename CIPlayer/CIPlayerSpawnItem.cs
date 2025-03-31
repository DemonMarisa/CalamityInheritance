using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
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
                    CIDownedBossSystem.DownedBloodMoon = true;
                }
            }
        }
    }
}
