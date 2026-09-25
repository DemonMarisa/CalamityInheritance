using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.DamageBuffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.LoreItems
{
    public class KnowledgeBrimstoneElemental : LoreItem
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
            player.CI().AddEffect("KnowledgeBrimstoneElemental", () =>
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    const int BaseDamage = 50;
                    int damage = player.CalcIntDamage<GenericDamageClass>(BaseDamage);
                    float range = 300f;
                    IEntitySource entitySource = player.GetSource_Accessory(player.HeldItem);
                    for (int i = 0; i < Main.maxNPCs; ++i)
                    {
                        NPC Npc = Main.npc[i];
                        if (!Npc.active || Npc.friendly || Npc.dontTakeDamage)
                            continue;

                        if (Vector2.Distance(player.Center, Npc.Center) <= range)
                        {
                            Npc.AddBuff(BuffType<CIBrimstoneFlames>(), 120);
                            if (player.miscCounter % 30 == 0)
                            {
                                int direct = player.Center.X > Npc.Center.X ? -1 : 1;
                                player.ApplyDamageToNPC(Npc, damage, 1f, direct);
                            }
                        }
                    }
                }
            });
        }
    }
}
