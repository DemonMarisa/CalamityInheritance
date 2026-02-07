using CalamityInheritance.Content.Items.Ammo.FiniteUse;
using CalamityInheritance.Content.Items.Placeables.MusicBox;
using CalamityInheritance.Content.Items.Weapons.Melee.Flails;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Content.Items.Weapons.Ranged.Cannos;
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
            if (type == NPCType<SeaKing>())
            {
                shop.Add(ItemType<UrchinFlail>());
                shop.Add(ItemType<CoralCannon>());
            }
            if (type == NPCType<Archmage>())
            {
                shop.AddWithCustomValue(ItemType<ColdheartIcicle>(), Item.buyPrice(gold: 150));
            }
            if (type == NPCType<Bandit>())
            {
                shop.ShopHelper<BouncingBetty>(Item.buyPrice(gold: 25), Condition.DownedMechBossAny);
                shop.ShopHelper<SylvanSlasher>(Item.buyPrice(platinum: 1), Condition.DownedMoonLord);
                shop.ShopHelper<LatcherMine>(Item.buyPrice(gold: 25), Condition.DownedMechBossAny);
            }
            if (type == NPCID.ArmsDealer)
            {
                shop.AddWithCustomValue(ItemType<MagnumRounds>(), Item.buyPrice(gold: 10))
                .AddWithCustomValue(ItemType<GrenadeRounds>(), Item.buyPrice(gold: 25))
                .AddWithCustomValue(ItemType<ExplosiveShells>(), Item.buyPrice(gold: 50));
            }
        }
        #endregion
    }
}
