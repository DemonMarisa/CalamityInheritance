using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityMod.Items.TreasureBags;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using Terraria.ID;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Accessories.Melee;

namespace CalamityInheritance.Content.Items
{
    public class CalamityInheritanceGlobalItemLoot : GlobalItem
    {
        public override bool InstancePerEntity => false;
        public override void ModifyItemLoot(Item item, ItemLoot itemloot)
        {
            if (item.type == ModContent.ItemType<DevourerofGodsBag>())
                itemloot.Add(ModContent.ItemType<Skullmasher>(), 10);
            if (item.type == ModContent.ItemType<AstrumDeusBag>())
            {
                itemloot.Add(ModContent.ItemType<Quasar>(), 10);
                itemloot.Add(ModContent.ItemType<AstralBulwark>(), 1);
            }
            if (item.type == ModContent.ItemType<YharonBag>())
            {
                itemloot.Add(ModContent.ItemType<VoidVortexLegacy>(), 10);
                itemloot.Add(ModContent.ItemType<YharimsGiftLegacy>(), 1);
            }
            if (item.type == ModContent.ItemType<CeaselessVoidBag>())
                itemloot.Add(ModContent.ItemType<ArcanumoftheVoid>(),1);

            if (item.type == ModContent.ItemType<LeviathanBag>())
                itemloot.Add(ModContent.ItemType<LeviathanAmbergrisLegacy>(), 1);

            if (item.type == ModContent.ItemType<ProvidenceBag>())
                itemloot.Add(ModContent.ItemType<SamuraiBadge>(), 10);
            switch (item.type)
            {
                #region Boss Treasure Bags
                case ItemID.MoonLordBossBag:
                    itemloot.Add(ModContent.ItemType<GrandDad>(), 10);
                    break;
                case ItemID.GolemBossBag:
                    itemloot.Add(ModContent.ItemType<LeadWizard>(), 10);
                    break;
                    #endregion
            }
        }
    }
}
