using CalamityInheritance.Content.Items.Accessories.Combats;
using CalamityInheritance.Content.Items.Accessories.Defense;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityMod;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public class CalamityInheritancePlayerFish : ModPlayer
    {
        #region Catch Fish
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            Player player = Main.player[Main.myPlayer];
            CalamityPlayer calPlayer = player.Calamity();

            int bait = attempt.playerFishingConditions.BaitItemType;
            int power = attempt.playerFishingConditions.BaitPower + attempt.playerFishingConditions.PolePower;
            int questFish = attempt.questFish;
            int poolSize = attempt.waterTilesCount;
            bool water = !attempt.inHoney && !attempt.inLava;

            // Handle the increased chance of crates from Enchanted Pearl and the Supreme Fishing Station
            if (calPlayer.enchantedPearl || calPlayer.fishingStation)
            {
                int chanceForCrates = (calPlayer.enchantedPearl ? 10 : 0) +
                    (calPlayer.fishingStation ? 10 : 0);

                int poolSizeAmt = poolSize / 10;
                if (poolSizeAmt > 100)
                    poolSizeAmt = 100;

                int fishingPowerDivisor = power + poolSizeAmt;

                int chanceForIronCrate = 1000 / fishingPowerDivisor;
                int chanceForBiomeCrate = 2000 / fishingPowerDivisor;
                int chanceForGoldCrate = 3000 / fishingPowerDivisor;
                int chanceForRareItems = 4000 / fishingPowerDivisor;

                if (water)
                {
                    if (Main.rand.Next(100) < chanceForCrates)
                    {
                        if (Main.rand.NextBool(chanceForRareItems) && calPlayer.enchantedPearl && calPlayer.fishingStation && Player.cratePotion)
                        {
                            List<int> rareItemList = new List<int>();

                            if (calPlayer.ZoneAstral)
                            {
                                rareItemList.Add(ItemType<PolarisParrotfishLegacy>());
                                rareItemList.Add(ItemType<UrsaSergeantLegacy> ());
                            }
                            if (calPlayer.ZoneSunkenSea)
                            {
                                rareItemList.Add(ItemType<AbyssalAmulet>());
                            }
                            if (rareItemList.Count != 0)
                            {
                                int rareItemAmt = rareItemList.Count;
                                int caughtRareItem = rareItemList[Main.rand.Next(rareItemAmt)];
                                itemDrop = caughtRareItem;
                            }
                        }
                    }
                }
            }
        }
            #endregion
    }
}
