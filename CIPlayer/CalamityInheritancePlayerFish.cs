using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Accessories;
using Microsoft.Xna.Framework;
using CalamityMod.CalPlayer;
using CalamityMod;
using CalamityMod.Items.Fishing.AstralCatches;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityInheritance.Content.Items.Weapons.Ranged;

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
                                rareItemList.Add(ModContent.ItemType<PolarisParrotfishLegacy>());
                            }
                            if (rareItemList.Any())
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
