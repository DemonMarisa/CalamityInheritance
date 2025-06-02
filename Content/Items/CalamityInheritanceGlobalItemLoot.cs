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
using Terraria.DataStructures;
using CalamityInheritance.Buffs.Summon;
using CalamityInheritance.Content.Items.Weapons.Summon;
using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using Terraria.ModLoader.Default;
using CalamityInheritance.Content.Items.Placeables.Vanity;
using CalamityInheritance.Content.Items.Qol;
using CalamityMod.Items.TreasureBags.MiscGrabBags;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items
{
    public class LoreLootRemove : GlobalItem
    {
        public override void OnSpawn(Item item, IEntitySource source)
        {
            //不准浪费时间，滚回去
            if (!CIServerConfig.Instance.LoreDrop)
                return;
            DoDropLegacy(ref item);
        }
        public override bool OnPickup(Item item, Player player)
        {
            //basically这个是通过生成时直接删除lore来实现。
            int getType = item.type;
            //不准浪费时间，滚回去
            if (!CIServerConfig.Instance.LoreDrop)
                return base.OnPickup(item, player);

            if (CalamityInheritanceLists.LoreCal.Contains(getType))
            {
                item.active = false;
                return false;
            }
            return base.OnPickup(item, player);
        }
        
        public static void DoDropLegacy(ref Item item)
        {
            int getType = item.type;
            if (CalamityInheritanceLists.LoreCal.Contains(getType))
                item.active = false;
        }
    }
    public class CalamityInheritanceGlobalItemLoot : GlobalItem
    {
        public override bool InstancePerEntity => false;
        public override void ModifyItemLoot(Item item, ItemLoot itemloot)
        {
            if (item.type == ModContent.ItemType<StarterBag>())
            {
                itemloot.Add(ModContent.ItemType<Death>());
                itemloot.Add(ModContent.ItemType<Armageddon>());
                itemloot.Add(ModContent.ItemType<DefiledRune>());
                itemloot.Add(ModContent.ItemType<IronHeart>());
                itemloot.Add(ModContent.ItemType<Malice>());
                itemloot.Add(ModContent.ItemType<Revenge>());
                itemloot.Add(ModContent.ItemType<DraedonsPanel>());
            }

            if (item.type == ModContent.ItemType<DevourerofGodsBag>())
                itemloot.Add(ModContent.ItemType<Skullmasher>(), 10);

            if (item.type == ModContent.ItemType<OldDukeBag>())
            {
                itemloot.Add(ModContent.ItemType<LeadCore>(), 1);
                itemloot.Add(ModContent.ItemType<InsidiousImpalerLegacy>(), 3);
            }

            if (item.type == ModContent.ItemType<AstrumDeusBag>())
            {
                itemloot.Add(ModContent.ItemType<Quasar>(), 10);
                itemloot.Add(ModContent.ItemType<AstralBulwark>(), 1);
            }
            if (item.type == ModContent.ItemType<YharonBag>())
            {
                itemloot.Add(ModContent.ItemType<DragonsBreathold>(), 5);
                itemloot.Add(ModContent.ItemType<VoidVortexLegacy>(), 10);
                itemloot.Add(ModContent.ItemType<YharimsGiftLegacy>(), 1);
            }
            if (item.type == ModContent.ItemType<CeaselessVoidBag>())
                itemloot.Add(ModContent.ItemType<ArcanumoftheVoid>(),1);

            if (item.type == ModContent.ItemType<RavagerBag>())
                itemloot.Add(ModContent.ItemType<BloodPactLegacy>(), 10);

            if (item.type == ModContent.ItemType<LeviathanBag>())
            {
                itemloot.Add(ModContent.ItemType<LeviathanAmbergrisLegacy>(), 3);//利维坦龙涎香现在掉落概率为1/3
            }
            if (item.type == ModContent.ItemType<CryogenBag>())
            {
                itemloot.Add(ModContent.ItemType<CryoBar>(), 3, 10, 20); //33%概率，数量10-20
                itemloot.Add(ModContent.ItemType<GlacialCrusher>(), 3, 1 ,1);
                itemloot.Add(ModContent.ItemType<BittercoldStaff>(), 3, 1 ,1);

                if(CIServerConfig.Instance.CalBossesCanDropSoul == true)
                {
                    itemloot.Add(ItemID.SoulofMight, 1, 35, 45);
                }
            }
            if (item.type == ModContent.ItemType<PerforatorBag>())
            {
                itemloot.Add(ModContent.ItemType<BloodClotStaff>(), 3);
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
                itemloot.Add(ModContent.ItemType<PristineFuryLegacy>(), 4);
                itemloot.Add(ModContent.ItemType<SamuraiBadge>(), 10);
            }
            if (item.type == ModContent.ItemType<DevourerofGodsBag>())
                itemloot.Add(ModContent.ItemType<MeleeTypeEradicator>(), 3);

            if (item.type == ModContent.ItemType<RavagerBag>())
            {
                itemloot.Add(ModContent.ItemType<MeleeTypeCorpusAvertor>(), 3);
            }
            if (item.type == ModContent.ItemType<PlaguebringerGoliathBag>())
            {
                itemloot.Add(ModContent.ItemType<BlightSpewerLegacy>(), 4);
            }
            if (item.type == ModContent.ItemType<HiveMindBag>())
            {
                itemloot.Add(ModContent.ItemType<ShadethrowerLegacy>(), 4);
                //暗影之雨
                itemloot.Add(ModContent.ItemType<ShadowdropStaff>(), 5);
            }
                
            if (item.type == ModContent.ItemType<SlimeGodBag>())
                itemloot.Add(ModContent.ItemType<OverloadedBlasterLegacy>(), 4);

            if (item.type == ModContent.ItemType<AstrumAureusBag>())
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
