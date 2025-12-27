using CalamityInheritance.Content.Items.Ammo.FiniteUse;
using CalamityInheritance.Content.Items.Placeables.MusicBox;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.NPCs.TownNPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs
{
    public partial class CIGlobalNPC : GlobalNPC
    {
        #region Shop Stuff
        public override void ModifyShop(NPCShop shop)
        {
            int type = shop.NpcType;

            if (type == ModContent.NPCType<DILF>())
            {
                shop.AddWithCustomValue(ModContent.ItemType<ColdheartIcicle>(), Item.buyPrice(gold: 150));
            }
            if (type == ModContent.NPCType<Bandit>())
            {
                shop.ShopHelper<BouncingBetty>(Item.buyPrice(gold: 25), Condition.DownedMechBossAny);
                shop.ShopHelper<SylvanSlasher>(Item.buyPrice(platinum: 1), Condition.DownedMoonLord);
                shop.ShopHelper<LatcherMine>(Item.buyPrice(gold: 25), Condition.DownedMechBossAny);
            }
            if (type == NPCID.ArmsDealer)
            {
                shop.AddWithCustomValue(ModContent.ItemType<MagnumRounds>(), Item.buyPrice(gold: 10))
                .AddWithCustomValue(ModContent.ItemType<GrenadeRounds>(), Item.buyPrice(gold: 25))
                .AddWithCustomValue(ModContent.ItemType<ExplosiveShells>(), Item.buyPrice(gold: 50));
            }
        }
        #endregion
    }
}
