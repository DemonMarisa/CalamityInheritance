using CalamityInheritance.Content.Items.Ammo.FiniteUse;
using CalamityInheritance.Content.Items.Placeables.MusicBox;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityMod;
using CalamityMod.NPCs.TownNPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs
{
    public partial class CalamityInheritanceGlobalNPC : GlobalNPC
    {
        #region Shop Stuff
        public override void ModifyShop(NPCShop shop)
        {
            int type = shop.NpcType;

            if (type == ModContent.NPCType<DILF>())
            {
                shop.AddWithCustomValue(ModContent.ItemType<ColdheartIcicle>(), Item.buyPrice(gold: 150));
            }
            if (type == ModContent.NPCType<THIEF>())
            {
                shop.AddWithCustomValue(ModContent.ItemType<SylvanSlasher>(), Item.buyPrice(gold: 100), Condition.DownedMoonLord);
            }
            if (type == ModContent.NPCType<FAP>())
            {
                shop.AddWithCustomValue(ModContent.ItemType<ProvidenceLegacy>(), Item.buyPrice(gold: 10), CalamityConditions.DownedProvidence);
                shop.AddWithCustomValue(ModContent.ItemType<DoGNonStop>(), Item.buyPrice(gold: 10), CalamityConditions.DownedDevourerOfGods);
                shop.AddWithCustomValue(ModContent.ItemType<DoGLegacy>(), Item.buyPrice(gold: 10), CalamityConditions.DownedDevourerOfGods);
                shop.AddWithCustomValue(ModContent.ItemType<TyrantPart1>(), Item.buyPrice(gold: 10), CalamityConditions.DownedYharon);
                shop.AddWithCustomValue(ModContent.ItemType<RequiemsOfACruelWorld>(), Item.buyPrice(gold: 10), CalamityConditions.DownedExoMechs);
                shop.AddWithCustomValue(ModContent.ItemType<NowStopAskingWhere>(), Item.buyPrice(gold: 10), CalamityConditions.DownedSupremeCalamitas);
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
