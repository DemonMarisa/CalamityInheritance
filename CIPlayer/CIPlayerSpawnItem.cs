using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.System.DownedBoss;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public bool currentBloodMoon = false;
        public void CISpawnItem()
        {
            if(!CIDownedBossSystem.DownedBloodMoon)
            {
                if (CIDownedBossSystem.DownedBloodMoon)
                    return;

                if (Main.bloodMoon)
                    currentBloodMoon = true;

                if (!Main.bloodMoon && currentBloodMoon)
                {
                    Player.QuickSpawnItem(Player.GetSource_GiftOrReward(), ModContent.ItemType<KnowledgeBloodMoon>(), 1);
                    CIDownedBossSystem.DownedBloodMoon = true;
                    currentBloodMoon = false;
                }
            }
        }

    }
}
