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
using CalamityInheritance.System.Configs;
using CalamityMod.Projectiles.Summon;

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
                if(CIServerConfig.Instance.CustomShimmer == false) 
                {
                    itemloot.Add(ModContent.ItemType<YharimsGiftLegacy>(), 1);
                    itemloot.Add(ModContent.ItemType<DragonsBreathold>(),3);
                //关闭微光转化后，魔君礼物与旧龙息将正常掉落
                }
            }
            if (item.type == ModContent.ItemType<CeaselessVoidBag>())
                itemloot.Add(ModContent.ItemType<ArcanumoftheVoid>(),1);

            if (item.type == ModContent.ItemType<RavagerBag>())
                itemloot.Add(ModContent.ItemType<BloodPactLegacy>(), 10);

            if (item.type == ModContent.ItemType<LeviathanBag>())
            {
                if(CIServerConfig.Instance.CustomShimmer == false) //关闭微光转化后，利维坦龙涎香正常掉落
                {
                    itemloot.Add(ModContent.ItemType<LeviathanAmbergrisLegacy>(), 3);//利维坦龙涎香现在掉落概率为1/3
                }
            }
            if (item.type == ModContent.ItemType<CryogenBag>())
            {
                itemloot.Add(ModContent.ItemType<CryoBar>(), 3, 10, 20); //33%概率，数量10-20
                itemloot.Add(ModContent.ItemType<GlacialCrusher>(), 3, 1 ,1);
                itemloot.Add(ModContent.ItemType<BittercoldStaff>(), 3, 1 ,1);
                //冰灵宝藏袋临时添加寒霜法杖，不然某些人做冰灵旋刃得坐大的，过会回归了那个被移除的法杖这个就会换掉

                if(CIServerConfig.Instance.CalBossesCanDropSoul == true)
                {
                    itemloot.Add(ItemID.SoulofMight, 1, 35, 45);
                }
            }
            if (item.type == ModContent.ItemType<BrimstoneWaifuBag>())
            {

                if(CIServerConfig.Instance.CalBossesCanDropSoul == true)
                {
                    itemloot.Add(ItemID.SoulofFright, 1, 35, 45);
                }
            }

            if (item.type == ModContent.ItemType<AquaticScourgeBag>())
            {

                if(CIServerConfig.Instance.CalBossesCanDropSoul == true)
                {
                    itemloot.Add(ItemID.SoulofSight, 1, 35, 45);
                }
            }
            //1.31 Scarlet:灾三王现在再次掉三王魂（可用config开关），掉魂的类型依据灾三王的boss主题色。掉落量为35-45随机

            if (item.type == ModContent.ItemType<ProvidenceBag>())
            {
                if(!CIServerConfig.Instance.CustomShimmer)
                itemloot.Add(ModContent.ItemType<PristineFuryLegacy>(), 4);

                itemloot.Add(ModContent.ItemType<SamuraiBadge>(), 10);
            }
            if (item.type == ModContent.ItemType<DevourerofGodsBag>())
            {
                //Scarlet:只有在微光关闭的时候这玩意才会正常掉落
                if(CIServerConfig.Instance.CustomShimmer == false)
                {
                    itemloot.Add(ModContent.ItemType<MeleeTypeEradicator>(), 3);
                }
            }

            if (item.type == ModContent.ItemType<RavagerBag>())
            {

                if(CIServerConfig.Instance.CustomShimmer == false)
                {
                    itemloot.Add(ModContent.ItemType<MeleeTypeCorpusAvertor>(), 3);
                }

            }
            if (item.type == ModContent.ItemType<PlaguebringerGoliathBag>())
            {
                if (!CIServerConfig.Instance.CustomShimmer)
                itemloot.Add(ModContent.ItemType<BlightSpewerLegacy>(), 4);
            }
            if (item.type == ModContent.ItemType<HiveMindBag>() && !CIServerConfig.Instance.CustomShimmer)
                itemloot.Add(ModContent.ItemType<ShadethrowerLegacy>(), 4);
                
            if (item.type == ModContent.ItemType<SlimeGodBag>() && !CIServerConfig.Instance.CustomShimmer)
                itemloot.Add(ModContent.ItemType<OverloadedBlasterLegacy>(), 4);

            if (item.type == ModContent.ItemType<AstrumAureusBag>() && !CIServerConfig.Instance.CustomShimmer)
                itemloot.Add(ModContent.ItemType<AuroraBlazerLegacy>(), 4);
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
