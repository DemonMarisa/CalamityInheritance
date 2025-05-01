using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.ItemDropRules;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.ID;
using CalamityInheritance.World;
using CalamityInheritance.Content.Items.Accessories.DashAccessories;
using CalamityMod.Items.TreasureBags;
using CalamityMod.Items.Materials;
using CalamityMod.World;
using CalamityInheritance.Content.Items.Placeables.Vanity;
using CalamityInheritance.Core;
using CalamityInheritance.NPCs;
using CalamityMod.Items.Placeables.Furniture.Trophies;

namespace CalamityInheritance.System.ModeChange.Armageddon
{
    public class ArmageddonDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            // 仅处理Boss
            if (!npc.boss)
                return;

            // 获取动态生成的掉落列表
            int bossDrops = Findbag(npc.type, npc);

            if(bossDrops != -1)
            {
                // 将基础规则包裹在双重条件下
                npcLoot.DefineConditionalDropSet(CIDropHelper.ArmageddonNoNor).Add(bossDrops);
                npcLoot.DefineConditionalDropSet(CIDropHelper.ArmageddonNoNor).Add(bossDrops);
                npcLoot.DefineConditionalDropSet(CIDropHelper.ArmageddonNoNor).Add(bossDrops);
                npcLoot.DefineConditionalDropSet(CIDropHelper.ArmageddonNoNor).Add(bossDrops);
                npcLoot.DefineConditionalDropSet(CIDropHelper.ArmageddonNoNor).Add(bossDrops);
            }
        }
        #region 找宝藏袋
        public static int Findbag(int type, NPC npc)
        {
            // 仅处理Boss类型
            if (!npc.boss)
                return -1;

            // 直接遍历掉落数据库
            foreach (var rule in Main.ItemDropsDB.GetRulesForNPCID(type, false))
            {
                // 提取规则中的掉落信息
                var dropInfos = new List<DropRateInfo>();
                rule.ReportDroprates(dropInfos, new DropRateInfoChainFeed(1f));

                // 寻找专家模式专属的宝藏袋
                foreach (var drop in dropInfos)
                {
                    Item item = ContentSamples.ItemsByType[drop.itemId];
                    if (item.expert)
                    {
                        return drop.itemId; // 返回第一个匹配的宝藏袋ID
                    }
                }
            }
            return -1;
        }
        #endregion
    }
}
