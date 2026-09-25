using CalamityInheritance.Content.BaseClass.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeAquaticScourge : LoreItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 26;
            Item.rare = ItemRarityID.Pink;
            Item.consumable = false;
        }
        public override void UpdateInventory(Player player)
        {
            if (Item.favorited)
                AddEffect(player);
        }
        public static void AddEffect(Player player)
        {
            player.CI().AddEffect("KnowledgeAquaticScourgeEffect", () =>
            {
                if (player.wellFed)
                {
                    player.statDefense += 1;
                    player.GetDamage(DamageClass.Generic) += 0.025f;
                    player.GetCritChance<GenericDamageClass>() += 1;
                    player.GetKnockback(DamageClass.Summon).Base += 0.25f;
                    player.moveSpeed += 0.1f;
                }
                else
                {
                    player.statDefense -= 1;
                    player.GetDamage(DamageClass.Generic) -= 0.025f;
                    player.GetCritChance<GenericDamageClass>() -= 1;
                    player.GetKnockback(DamageClass.Summon).Base -= 0.25f;
                    player.moveSpeed -= 0.1f;
                }
            });
        }
    }
}
