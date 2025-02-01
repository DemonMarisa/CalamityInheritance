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
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Armor.YharimAuric;

namespace CalamityInheritance.Content.Items
{
    public class CalamityInheritanceGlobalItemLoot : GlobalItem
    {
        public override bool InstancePerEntity => false;
        public override void ModifyItemLoot(Item item, ItemLoot itemloot)
        {
            if (item.type == ModContent.ItemType<DevourerofGodsBag>())
                itemloot.Add(ModContent.ItemType<Skullmasher>(), 10);

            if (item.type == ModContent.ItemType<OldDukeBag>())
                itemloot.Add(ModContent.ItemType<LeadCore>(), 1);

            if (item.type == ModContent.ItemType<AstrumDeusBag>())
            {
                itemloot.Add(ModContent.ItemType<Quasar>(), 10);
                itemloot.Add(ModContent.ItemType<AstralBulwark>(), 1);
            }
            if (item.type == ModContent.ItemType<YharonBag>())
            {
                itemloot.Add(ModContent.ItemType<DragonsBreathold>(), 5);
                itemloot.Add(ModContent.ItemType<VoidVortexLegacy>(), 10);
                if(CalamityInheritanceConfig.Instance.CustomShimmer == false) 
                {
                    itemloot.Add(ModContent.ItemType<YharimsGiftLegacy>(), 1);
                    itemloot.Add(ModContent.ItemType<DragonsBreathold>(),3);
                //关闭微光转化后，魔君礼物与旧龙息将正常掉落
                }
            }
            if (item.type == ModContent.ItemType<CeaselessVoidBag>())
                itemloot.Add(ModContent.ItemType<ArcanumoftheVoid>(),1);

            if (item.type == ModContent.ItemType<LeviathanBag>())
            {
                if(CalamityInheritanceConfig.Instance.CustomShimmer == false) //关闭微光转化后，利维坦龙涎香正常掉落
                {
                    itemloot.Add(ModContent.ItemType<LeviathanAmbergrisLegacy>(), 3);//利维坦龙涎香现在掉落概率为1/3
                }
            }
            if (item.type == ModContent.ItemType<CryogenBag>())
            {
                itemloot.Add(ModContent.ItemType<CryoBar>(), 3, 5, 15); //33%概率，数量5-15

                if(CalamityInheritanceConfig.Instance.CalBossesCanDropSoul == true)
                {
                    itemloot.Add(ItemID.SoulofMight, 1, 35, 45);
                }
            }
            if (item.type == ModContent.ItemType<BrimstoneWaifuBag>())
            {

                if(CalamityInheritanceConfig.Instance.CalBossesCanDropSoul == true)
                {
                    itemloot.Add(ItemID.SoulofFright, 1, 35, 45);
                }
            }

            if (item.type == ModContent.ItemType<AquaticScourgeBag>())
            {

                if(CalamityInheritanceConfig.Instance.CalBossesCanDropSoul == true)
                {
                    itemloot.Add(ItemID.SoulofSight, 1, 35, 45);
                }
            }
            //1.31 Scarlet:灾三王现在再次掉三王魂（可用config开关），掉魂的类型依据灾三王的boss主题色。掉落量为35-45随机

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
