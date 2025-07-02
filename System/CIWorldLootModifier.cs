using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.SummonItems;
using Terraria;
using Terraria.GameContent.Generation;
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
        }
        public static void AddHalibutCannonToAbyssChest(GenerationProgress progress, GameConfiguration config)
        {
            //只有且仅有一个箱子存在Terminus，因此，这里遍历存在Terminus的箱子
            if (!ModLoader.TryGetMod("CalamityMod", out Mod CalamityMod))
                return;
            ModItem termi = null;
            if (!CalamityMod?.TryFind("Terminus", out termi) ?? false)
                return;
            if (termi is null)
                return;

            int terminus = termi.Type;
            for (int i = 0; i < Main.maxChests; i++)
            {
                Chest getC = Main.chest[i];
                if (getC?.item.Any(s => s.stack >= 1 && s.type == terminus) ?? false)
                {
                    getC.AddItemToShop(new Item(ModContent.ItemType<HalibutCannonLegendary>()));
                }
            }
        }
    }
}