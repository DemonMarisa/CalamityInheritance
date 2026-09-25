using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.Buffs;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeCrabulon : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Green;
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
            player.CI().AddEffect("KnowledgeCrabulon", () =>
            {
                if (player.ZoneGlowshroom || player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight)
                {
                    if (Main.myPlayer == player.whoAmI)
                        player.AddBuff(BuffType<Mushy>(), 2);
                    player.moveSpeed -= 0.1f;
                }
            });
        }
    }
}
