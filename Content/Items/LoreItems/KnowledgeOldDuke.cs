using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Rarity;
using Terraria;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeOldDuke : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = RarityType<AbsoluteGreen>();
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
            {
                AddEffect(player);
            }
        }
        public static void AddEffect(Player player)
        {
            player.CI().AddEffect("KnowledgeOldDuke", () =>
            {
                if (player.CalPlayerInfo().ZoneAbyss || player.CalPlayerInfo().ZoneSulphur)
                {
                    player.breath = player.breathMax + 91;
                    player.AddDR(0.2f);
                    player.statDefense += 30;
                    player.buffImmune[BuffType<CISulphuricPoisoning>()] = true;
                    player.buffImmune[BuffType<CICrushDepth>()] = true;
                    player.lifeRegen += 3;
                }
                if (!player.CalPlayerInfo().ZoneAbyss || !player.CalPlayerInfo().ZoneSulphur)
                {
                    player.endurance -= 0.1f;
                    player.statDefense -= 15;
                    player.accRunSpeed -= 0.5f;
                    player.lifeRegen -= 3;
                }
            });
        }
    }
}
