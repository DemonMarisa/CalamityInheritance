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
using CalamityInheritance.Content.Items.Placeables.MusicBox;
using CalamityMod.NPCs.DesertScourge;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;

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
        public static bool CheckBag<T>(int type) where T : ModItem => type == ModContent.ItemType<T>();
        public override void ModifyItemLoot(Item item, ItemLoot loot)
        {
            if (item.type == ModContent.ItemType<StarterBag>())
            {
                loot.LootAdd<Death>();
                loot.LootAdd<Armageddon>();
                loot.LootAdd<DefiledRune>();
                loot.LootAdd<IronHeart>();
                loot.LootAdd<Malice>();
                loot.LootAdd<Revenge>();
                loot.LootAdd<DraedonsPanel>();
                loot.LootAdd<CalamityTitleMusicBoxLegacy>();
            }
            if (item.type == ModContent.ItemType<DesertScourgeBag>())
            {
                loot.LootAdd<AquaticDischarge>(4);
            }
            if (item.type == ModContent.ItemType<DevourerofGodsBag>())
                loot.LootAdd<Skullmasher>(10);

            if (item.type == ModContent.ItemType<OldDukeBag>())
            {
                loot.LootAdd<LeadCore>(1);
                loot.LootAdd<InsidiousImpalerLegacy>(3);
            }

            if (item.type == ModContent.ItemType<AstrumDeusBag>())
            {
                loot.LootAdd<Quasar>(10);
                loot.LootAdd<AstralBulwark>(1);
            }
            if (item.type == ModContent.ItemType<YharonBag>())
            {
                loot.LootAdd<DragonsBreathold>(5);
                loot.LootAdd<VoidVortexLegacy>(10);
                loot.LootAdd<YharimsGiftLegacy>(1);
            }

            if (item.type == ModContent.ItemType<CeaselessVoidBag>())
                loot.LootAdd<ArcanumoftheVoid>(1);

            if (item.type == ModContent.ItemType<RavagerBag>())
                loot.LootAdd<BloodPactLegacy>(10);

            if (item.type == ModContent.ItemType<LeviathanBag>())
            {
                loot.LootAdd<LeviathanAmbergrisLegacy>(3);//利维坦龙涎香现在掉落概率为1/3
            }
            if (item.type == ModContent.ItemType<CryogenBag>())
            {
                loot.LootAdd<CryoBar>(3, 10, 20); //33%概率，数量10-20
                loot.LootAdd<GlacialCrusher>(3, 1 ,1);
                loot.LootAdd<BittercoldStaff>(3, 1 ,1);
            }
            if (item.type == ModContent.ItemType<PerforatorBag>())
            {
                loot.LootAdd<BloodClotStaff>(3);
            }
            //重写了掉魂的方法
            #region DropSoul
            bool dropSoul = CIServerConfig.Instance.CalBossesCanDropSoul;
            if (dropSoul)
            {
                if (CheckBag<AquaticScourgeBag>(item.type))
                    GiveSoul(ItemID.SoulofSight);
                if (CheckBag<BrimstoneWaifuBag>(item.type))
                    GiveSoul(ItemID.SoulofFright);
                if (CheckBag<CryogenBag>(item.type))
                    GiveSoul(ItemID.SoulofMight);
                void GiveSoul(int soulID)
                {
                    loot.LootAdd(soulID, 1, 35, 45);
                }
            }
            #endregion
            if (item.type == ModContent.ItemType<ProvidenceBag>())
            {
                loot.LootAdd<PristineFuryLegacy>(4);
                loot.LootAdd<SamuraiBadge>(10);
            }
            if (item.type == ModContent.ItemType<DevourerofGodsBag>())
                loot.LootAdd<MeleeTypeEradicator>(3);

            if (item.type == ModContent.ItemType<RavagerBag>())
            {
                loot.LootAdd<MeleeTypeCorpusAvertor>(3);
            }
            if (item.type == ModContent.ItemType<PlaguebringerGoliathBag>())
            {
                loot.LootAdd<BlightSpewerLegacy>(4);
            }
            if (item.type == ModContent.ItemType<HiveMindBag>())
            {
                loot.LootAdd<ShadethrowerLegacy>(4);
                //暗影之雨
                loot.LootAdd<ShadowdropStaff>(5);
            }
                
            if (item.type == ModContent.ItemType<SlimeGodBag>())
                loot.LootAdd<OverloadedBlasterLegacy>(4);

            if (item.type == ModContent.ItemType<AstrumAureusBag>())
                loot.LootAdd<AuroraBlazerLegacy>(4);
            switch (item.type)
            {
                #region Boss Treasure Bags
                case ItemID.MoonLordBossBag:
                    loot.LootAdd<GrandDad>(10);
                    break;
                case ItemID.GolemBossBag:
                    loot.LootAdd<LeadWizard>(10);
                    break;
                #endregion
            }
        }
    }
}
