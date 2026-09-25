using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Core.GlobalInstance.Players.Dash;
using LAP.Core.SystemsLoader;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeCryogen : LoreItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Pink;
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
            player.CI().AddEffect("KnowledgeCryogen", () =>
            {
                player.SetLAPDash(LAPContent.DashType<OrnateShieldDash>());
                player.dashType = DashID.None;
                player.statDefense -= 10;
            });
        }
    }
}
