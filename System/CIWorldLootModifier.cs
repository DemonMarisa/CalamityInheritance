using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using CalamityInheritance.Content.Items.Accessories.Combats;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.SummonItems;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace CalamityInheritance.System
{
    public class CIWorldChestLootModify : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            tasks.Add(new PassLegacy("Added Halibut Cannon", AddHalibutCannonToAbyssChest));
            tasks.Add(new PassLegacy("Added Luxors Gift Legacy", AddLuxorsGiftLegacy));
        }
        public static void AddHalibutCannonToAbyssChest(GenerationProgress progress, GameConfiguration config)
        {
            int terminus = ItemType<Terminus>();
            for (int i = 0; i < Main.maxChests; i++)
            {
                Chest getC = Main.chest[i];
                if (getC?.item.Any(s => s.stack >= 1 && s.type == terminus) ?? false)
                {
                    getC.AddItemToShop(new Item(ItemType<HalibutCannonLegendary>()));
                }
            }
        }
        public static void AddLuxorsGiftLegacy(GenerationProgress progress, GameConfiguration config)
        {
            int Luxors = ItemType<LuxorsGift>();
            for (int i = 0; i < Main.maxChests; i++)
            {
                Chest getC = Main.chest[i];
                if (getC?.item.Any(s => s.stack >= 1 && s.type == Luxors) ?? false)
                {
                    getC.AddItemToShop(new Item(ItemType<LuxorsGiftLegacy>()));
                }
            }
        }
    }
}