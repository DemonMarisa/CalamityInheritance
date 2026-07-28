using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Magic.MagicGun.Misc;
using CalamityInheritance.Content.Items.Weapons.Melee.Axes;
using CalamityInheritance.Content.Items.Weapons.Melee.CurvedSword;
using CalamityInheritance.Content.Items.Weapons.Melee.Fists;
using CalamityInheritance.Content.Items.Weapons.Melee.GreatSwords;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using CalamityInheritance.Content.Items.Weapons.Melee.Swords;
using CalamityInheritance.Content.Items.Weapons.Ranged.Bow;
using CalamityInheritance.Content.Items.Weapons.Ranged.HandGun;
using CalamityInheritance.Content.Items.Weapons.Ranged.ShotGun;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.NPCs.CILoots
{
    public class CINPCLoots : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot Loot)
        {
            // 火星人掉落空
            if (npc.type == NPCID.MartianSaucer)
                Loot.Add(ItemType<NullificationPistolLegacy>(), 5);
            // 红恶魔
            if (npc.type == NPCID.RedDevil)
                Loot.Add(ItemType<DemonicBoneAsh>(), 3);
            // 带向导人偶的恶魔
            if (npc.type == NPCID.VoodooDemon)
            {
                Loot.Add(ItemType<BladecrestOathswordLegacy>(), 3);
                Loot.Add(ItemType<DemonicBoneAsh>(), 2);
            }
            // 恶魔
            if (npc.type == NPCID.Demon)
            {
                Loot.Add(ItemType<BladecrestOathswordLegacy>(), 3);
                Loot.Add(ItemType<DemonicBoneAsh>(), 5);
            }
            // 古蛇
            if (npc.type == NPCID.BoneSerpentHead)
            {
                Loot.Add(ItemType<DemonicBoneAsh>(), 3);
                Loot.Add(ItemType<OldLordOathswordLegacy>(), 5);
            }
            switch (npc.type)
            {
                case NPCID.VortexRifleman:
                    // 交叉集火
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemType<ConclaveCrossfire>(), 100, 50));
                    break;
                case NPCID.DarkCaster:
                    // 远古短剑
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemType<AncientShiv>(), 25, 15));
                    break;
                    // 节日矛
                case NPCID.PresentMimic:
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemType<HolidayHalberd>(), 7, 5));
                    break;
                case NPCID.MartianWalker:
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemType<WingmanLegacy>(), 7, 5));
                    break;
                //蚁狮掉弓和爪子
                case NPCID.Antlion:
                case NPCID.FlyingAntlion:
                case NPCID.WalkingAntlion:
                case NPCID.GiantWalkingAntlion:
                case NPCID.GiantFlyingAntlion:
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemType<AntlionBow>(), 50, 33));
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemType<MandibleClaws>(), 50, 33));
                    break;
                case NPCID.GoblinWarrior:
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemType<Warblade>(), 25, 20));
                    break;
                //骷髅掉战斧
                case NPCID.Skeleton:
                case NPCID.ArmoredSkeleton:
                    Loot.Add(ItemDropRule.NormalvsExpert(ItemType<Waraxe>(), 20, 15));
                    break;
            }

        }
    }
    public static class CIDropHelper
    {
        public static IItemDropRule Add(this ILoot loot, int itemID, int dropRateInt = 1, int minQuantity = 1, int maxQuantity = 1)
        {
            return loot.Add(ItemDropRule.Common(itemID, dropRateInt, minQuantity, maxQuantity));
        }
    }
}
