using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod;
using Terraria.GameContent.Bestiary;
using Terraria.Localization;
using Terraria.Utilities;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityInheritance.Content.Items.Potions;
using CalamityInheritance.Content.Items.Accessories;
using CalamityMod.Items.Ammo;
using CalamityInheritance.Content.Items.MiscItem;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityMod.Items.DraedonMisc;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Accessories.Ranged;
using CalamityInheritance.Content.Projectiles.NPCProj.Friendly;
using CalamityMod.Dusts;
using CalamityMod.NPCs.TownNPCs;
using CalamityMod.Items.SummonItems;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.CalClone;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer;
using CalamityMod.BiomeManagers;
using CalamityInheritance.Content.Items.Ammo.RangedAmmo;
using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityMod.Particles;
using CalamityMod.Events;
using CalamityMod.Items.Armor.Vanity;
using CalamityInheritance.Core;
using CalamityInheritance.Content.Items.Placeables.MusicBox;
using CalamityMod.Items.Placeables.Furniture;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Items.Weapons.Ranged;
using CalamityInheritance.System.DownedBoss;

namespace CalamityInheritance.NPCs.TownNPC
{
    [AutoloadHead]
    public class CalamitasNPCLegacy : ModNPC
    {
        private int Chat = 4;

        #region Prices
        public static int GeneralAccPrice => Item.buyPrice(0, 50, 0 , 0);
        public static int GeneralPostMLMatPrice => Item.buyPrice(0, 20, 0 , 0);
        public static string DialogueRoute => "Mods.CalamityInheritance.Dialogue";
        const short ChatOpt = 0;
        const short LoreShopOpt = 1;
        const short PotionShopOpt = 2;
        const short AmmoShopOpt = 3;
        const short MiscShopOpt = 4;
        const short ItemShopOpt = 5;
        const short MusicBoxShopOpt = 6;
        const short WineShopOpt = 7;
        #endregion
        public static int WhichButton;

        public static bool LoreShop;

        public static bool ItemShop;

        public static bool PotionShop;

        public static bool AmmoShop;

        public static bool MiscShop;

        public static bool MusicBoxShop;

        public static bool WineShop;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 23;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 5;
            NPCID.Sets.AttackFrameCount[NPC.type] = 2;
            NPCID.Sets.DangerDetectRange[NPC.type] = 3000;
            NPCID.Sets.AttackType[NPC.type] = 2;
            NPCID.Sets.AttackTime[NPC.type] = 10;
            NPCID.Sets.AttackAverageChance[NPC.type] = 1;
            NPCID.Sets.ShimmerTownTransform[Type] = false;
            NPC.Happiness
                .SetBiomeAffection<BrimstoneCragsBiome>(AffectionLevel.Like)
                .SetNPCAffection(ModContent.NPCType<SEAHOE>(), AffectionLevel.Like)
                .SetNPCAffection(NPCID.Wizard, AffectionLevel.Like);
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifiers);

            NPCID.Sets.MagicAuraColor[NPC.type] = new Color(240, 72, 89);
        }
        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.lavaImmune = true;
            NPC.width = 20;
            NPC.height = 46;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 862;
            NPC.defense = 120;
            NPC.lifeMax = 8800000;
            NPC.DR_NERD(0.75f);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 1f;
            AnimationType = NPCID.Guide;
        }

        //图鉴
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Mods.CalamityInheritance.Bestiary.ScalNPC")
            });
        }
        public override void AI()
        {
            Player player = Main.player[Main.myPlayer];
            CalamityInheritancePlayer CIPlayer = player.CIMod();
            int deaths = Main.player[Main.myPlayer].numberOfDeathsPVE;
            if (deaths < 1 && DownedBossSystem.downedCalamitas && CIPlayer.FreeEssence == 1)
                Chat = 3;
            else if (deaths < 1 && DownedBossSystem.downedCalamitas && CIPlayer.FreeEssence == 2)
                Chat = Main.rand.Next(5, 21);
            else
                Chat = Main.rand.Next(6, 21);
        }

        public override bool CanTownNPCSpawn(int numTownNPCs) => !NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>());
        public override void UpdateLifeRegen(ref int damage)
        {
            NPC.lifeRegen += 1000;
        }
        /*
        public override List<string> SetNPCNameList()
        {
            return new List<string>()
                {
                  Language.GetTextValue("Mods.CalamityInheritance.Name.ScalNPC")
            };
        }
        */
        public override bool PreAI()
        {
            // Disappear if the SCal boss is active. She's supposed to be the boss.
            // However, this doesn't happen in Boss Rush; the SCal there is a silent puppet created by Xeroc, not SCal herself.
            if (NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()) && !BossRushEvent.BossRushActive)
            {
                NPC.active = false;
                NPC.netUpdate = true;
                return false;
            }
            return true;
        }

        //对话
        public override string GetChat()
        {
            Player player = Main.player[Main.myPlayer];
            CalamityInheritancePlayer CIPlayer = player.CIMod();

            WeightedRandom<string> list = new WeightedRandom<string>();
            if(Main.rand.NextBool(100))
            {
                return Language.GetTextValue($"{DialogueRoute}.ScalFAPChat");
            }


            if (NPC.homeless)
            {
                if (Chat == 3 && CIPlayer.FreeEssence == 1)
                {
                    GiveReward(Main.LocalPlayer);
                    CIPlayer.FreeEssence = 2;
                }

                return Chat switch
                {
                    1 => Language.GetTextValue($"{DialogueRoute}.ScalHomelessChat1"),
                    2 => Language.GetTextValue($"{DialogueRoute}.ScalHomelessChat2"),
                    _ => Language.GetTextValue($"{DialogueRoute}.ScalHomelessChat4"),
                };
            }

            if (Chat == 3 && CIPlayer.FreeEssence == 1)
            {
                GiveReward(Main.LocalPlayer);
                CIPlayer.FreeEssence = 2;
            }
            if (Chat == 3)
                return Language.GetTextValue($"{DialogueRoute}.ScalHomelessChat3");
            if (Chat == 4)
            {
                return Language.GetTextValue($"{DialogueRoute}.ScalHomelessChat4");
            }
            if (Chat == 5)
                return Language.GetTextValue($"{DialogueRoute}.ScalHomelessChat5");
            if (Chat == 6)
                return Language.GetTextValue($"{DialogueRoute}.ScalHomelessChat6");
            if (Chat == 7)
                return Language.GetTextValue($"{DialogueRoute}.ScalHomelessChat7");

            list.Add(Language.GetTextValue($"{DialogueRoute}.ScalChatNor1"));
            list.Add(Language.GetTextValue($"{DialogueRoute}.ScalChatNor2"));
            list.Add(Language.GetTextValue($"{DialogueRoute}.ScalChatNor3"));
            list.Add(Language.GetTextValue($"{DialogueRoute}.ScalChatNor4"));
            list.Add(Language.GetTextValue($"{DialogueRoute}.ScalChatNor5"));
            list.Add(Language.GetTextValue($"{DialogueRoute}.ScalChatNor6"));

            if (Main.dayTime)
            {
                list.Add(Language.GetTextValue($"{DialogueRoute}.DayTime1"));
                list.Add(Language.GetTextValue($"{DialogueRoute}.DayTime2"));
                list.Add(Language.GetTextValue($"{DialogueRoute}.DayTime3"));
                list.Add(Language.GetTextValue($"{DialogueRoute}.DayTime4"));
                list.Add(Language.GetTextValue($"{DialogueRoute}.DayTime5"));
            }
            else
            {
                if (NPC.downedMoonlord)
                {
                    list.Add(Language.GetTextValue($"{DialogueRoute}.NightTime1"));
                }
                list.Add(Language.GetTextValue($"{DialogueRoute}.NightTime2"));
                list.Add(Language.GetTextValue($"{DialogueRoute}.NightTime3"));
                list.Add(Language.GetTextValue($"{DialogueRoute}.NightTime4"));
                list.Add(Language.GetTextValue($"{DialogueRoute}.NightTime5"));
                list.Add(Language.GetTextValue($"{DialogueRoute}.NightTime6"));
            }

            if (DownedBossSystem.downedDoG && Main.eclipse)
            {
                list.Add(Language.GetTextValue($"{DialogueRoute}.eclipse1"));
                list.Add(Language.GetTextValue($"{DialogueRoute}.eclipse1"));
            }
            if (Main.bloodMoon)
            {
                for (int i = 0; i < 3; i++)
                {
                    list.Add(Language.GetTextValue($"{DialogueRoute}.BloodMoon1"));
                }
                for (int j = 0; j < 3; j++)
                {
                    list.Add(Language.GetTextValue($"{DialogueRoute}.BloodMoon2"));
                }
            }
            int seahoe = NPC.FindFirstNPC(ModContent.NPCType<SEAHOE>());
            if (seahoe != -1)
            {
                list.Add(Language.GetTextValue($"{DialogueRoute}.ScalSeahoeChat"));
            }

            int armdealer = NPC.FindFirstNPC(NPCID.ArmsDealer);
            if (armdealer != -1)
            {
                list.Add(Language.GetTextValue($"{DialogueRoute}.armdealer1") + Main.npc[armdealer].GivenName + Language.GetTextValue($"{DialogueRoute}.armdealer2"));
            }
            if (!DownedBossSystem.downedCalamitas && Main.rand.NextBool(5))
            {
                list.Add(Language.GetTextValue($"{DialogueRoute}.PreDownScal"));
            }
            bool CalLore = player.InventoryHas(ModContent.ItemType<KnowledgeCalamitas>()) || player.PortableStorageHas(ModContent.ItemType<KnowledgeCalamitas>());
            bool CeremonialUrnItem = player.InventoryHas(ModContent.ItemType<CeremonialUrn>()) || player.PortableStorageHas(ModContent.ItemType<CeremonialUrn>());
            if (CalLore)
            {
                if (Main.rand.NextBool(10))
                {
                    return Language.GetTextValue($"{DialogueRoute}.ScalLoreChat");
                }
                list.Add(Language.GetTextValue($"{DialogueRoute}.ScalLoreChat"));
            }
            if (CeremonialUrnItem)
            {
                if (Main.rand.NextBool(10))
                {
                    return Language.GetTextValue($"{DialogueRoute}.ScalUrnChat");
                }
                list.Add(Language.GetTextValue($"{DialogueRoute}.ScalUrnChat"));
            }

            int ScalClone = NPC.FindFirstNPC(ModContent.NPCType<CalamitasClone>());
            int ScalBoss = NPC.FindFirstNPC(ModContent.NPCType<SupremeCalamitas>());
            if (ScalClone != -1)
            {
                return Language.GetTextValue($"{DialogueRoute}.ScalCalCloneChat");
            }
            if (ScalBoss != -1)
            {
                return Language.GetTextValue($"{DialogueRoute}.ScalSWChat");
            }
            return list;
        }

        public static void GiveReward(Player player)
        {
            player.QuickSpawnItem(player.GetSource_GiftOrReward(), ModContent.ItemType<CalamitousEssence>(), 1);
        }

        //商店与对话
        public override void SetChatButtons(ref string button, ref string button2)
        {
            switch (WhichButton)
            {
                case ChatOpt:
                    button = Language.GetTextValue($"{DialogueRoute}.ScalChatOption");
                    break;
                case LoreShopOpt:
                    button = Language.GetTextValue($"{DialogueRoute}.ScalloreShopOption");
                    break;
                case PotionShopOpt:
                    button = Language.GetTextValue($"{DialogueRoute}.ScalPotionShopOption");
                    break;
                case AmmoShopOpt:
                    button = Language.GetTextValue($"{DialogueRoute}.ScalAmmoShopOption");
                    break;
                case MiscShopOpt:
                    button = Language.GetTextValue($"{DialogueRoute}.ScalMiscShopOption");
                    break;
                case ItemShopOpt:
                    button = Language.GetTextValue($"{DialogueRoute}.ScalItemShopOption");
                    break;
                case MusicBoxShopOpt:
                    button = Language.GetTextValue($"{DialogueRoute}.ScalMusicBoxShopOpt");
                    break;
                case WineShopOpt:
                    button = Language.GetTextValue($"{DialogueRoute}.ScalWineShoppOpt");
                    break;
            }

            button2 = Language.GetTextValue($"{DialogueRoute}.Scalbutton2Option");

        }
        public override void AddShops()
        {
            LoreShop_List();
            PotionShop_List();
            AmmoShop_List();
            MiscShop_List();
            ItemShop_List();
            MusicBoxShop_List();
            WineShop_List();
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (!firstButton)
            {
                WhichButton++;
                if (WhichButton > WineShopOpt)
                {
                    WhichButton = ChatOpt;
                }
                return;
            }

            if (WhichButton == ChatOpt)
            {
                Main.npcChatText = LoreDialogue();
                return;
            }

            if (firstButton)
            {
                if (WhichButton == LoreShopOpt)
                {
                    shop = Language.GetTextValue($"{DialogueRoute}.ScalloreShopOption");
                }
                if (WhichButton == PotionShopOpt)
                {
                    shop = Language.GetTextValue($"{DialogueRoute}.ScalPotionShopOption");
                }
                if (WhichButton == AmmoShopOpt)
                {
                    shop = Language.GetTextValue($"{DialogueRoute}.ScalAmmoShopOption");
                }
                if (WhichButton == MiscShopOpt)
                {
                    shop = Language.GetTextValue($"{DialogueRoute}.ScalMiscShopOption");
                }
                if (WhichButton == ItemShopOpt)
                {
                    shop = Language.GetTextValue($"{DialogueRoute}.ScalItemShopOption");
                }
                if (WhichButton == MusicBoxShopOpt)
                {
                    shop = Language.GetTextValue($"{DialogueRoute}.ScalMusicBoxShopOpt");
                }
                if (WhichButton == WineShopOpt)
                {
                    shop = Language.GetTextValue($"{DialogueRoute}.ScalWineShoppOpt");
                }
            }
        }
        //lore对话
        public string LoreDialogue()
        {
            IList<string> list = new List<string>();
            list.Add(Language.GetTextValue($"{DialogueRoute}.ScalOtherLoreChat1BloodMoon"));
            list.Add(Language.GetTextValue($"{DialogueRoute}.ScalOtherLoreChat2"));
            list.Add(Language.GetTextValue($"{DialogueRoute}.ScalOtherLoreChat3"));
            list.Add(Language.GetTextValue($"{DialogueRoute}.ScalOtherLoreChat4"));
            list.Add(Language.GetTextValue($"{DialogueRoute}.ScalOtherLoreChat5"));
            list.Add(Language.GetTextValue($"{DialogueRoute}.ScalOtherLoreChat6"));
            list.Add(Language.GetTextValue($"{DialogueRoute}.ScalOtherLoreChat7"));
            list.Add(Language.GetTextValue($"{DialogueRoute}.ScalOtherLoreChat8"));
            list.Add(Language.GetTextValue($"{DialogueRoute}.ScalOtherLoreChat9"));
            return list[Main.rand.Next(list.Count)];
        }
        //lore商店
        public void LoreShop_List()
        {
            Condition tierThreeCondition = new($"{DialogueRoute}.DownedAnySentinels", () => DownedBossSystem.downedSignus || DownedBossSystem.downedStormWeaver || DownedBossSystem.downedCeaselessVoid);

            var dropsShop = new NPCShop(Type, Language.GetTextValue($"{DialogueRoute}.ScalloreShopOption"))
                .Add(new Item(ModContent.ItemType<KnowledgeKingSlime>()) { shopCustomPrice = Item.buyPrice(silver: 25) }, Condition.DownedKingSlime)

                .Add(new Item(ModContent.ItemType<KnowledgeDesertScourge>()) { shopCustomPrice = Item.buyPrice(silver: 50) }, CalamityConditions.DownedDesertScourge)

                .Add(new Item(ModContent.ItemType<KnowledgeEyeofCthulhu>()) { shopCustomPrice = Item.buyPrice(silver: 50) }, Condition.DownedEyeOfCthulhu)

                .Add(new Item(ModContent.ItemType<KnowledgeCrabulon>()) { shopCustomPrice = Item.buyPrice(gold: 1) }, CalamityConditions.DownedCrabulon)

                .Add(new Item(ModContent.ItemType<KnowledgeEaterofWorlds>()) { shopCustomPrice = Item.buyPrice(silver: 250) }, Condition.DownedEaterOfWorlds)
                //.Add(new Item(ModContent.ItemType<KnowledgeCorruption>()) { shopCustomPrice = Item.buyPrice(silver: 50) }, Condition.DownedEaterOfWorlds)

                .Add(new Item(ModContent.ItemType<KnowledgeBrainofCthulhu>()) { shopCustomPrice = Item.buyPrice(silver: 250) }, Condition.DownedBrainOfCthulhu)
                //.Add(new Item(ModContent.ItemType<KnowledgeCrimson>()) { shopCustomPrice = Item.buyPrice(silver: 50) }, Condition.DownedBrainOfCthulhu)

                .Add(new Item(ModContent.ItemType<KnowledgeHiveMind>()) { shopCustomPrice = Item.buyPrice(gold: 5) }, CalamityConditions.DownedHiveMind)

                .Add(new Item(ModContent.ItemType<KnowledgePerforators>()) { shopCustomPrice = Item.buyPrice(gold: 5) }, CalamityConditions.DownedPerforator)

                .Add(new Item(ModContent.ItemType<KnowledgeQueenBee>()) { shopCustomPrice = Item.buyPrice(gold: 5) }, Condition.DownedQueenBee)

                .Add(new Item(ModContent.ItemType<KnowledgeSkeletron>()) { shopCustomPrice = Item.buyPrice(gold: 10) }, Condition.DownedSkeletron)

                .Add(new Item(ModContent.ItemType<KnowledgeSlimeGod>()) { shopCustomPrice = Item.buyPrice(gold: 10) }, CalamityConditions.DownedSlimeGod)

                .Add(new Item(ModContent.ItemType<KnowledgeWallofFlesh>()) { shopCustomPrice = Item.buyPrice(gold: 15) }, Condition.Hardmode)

                .Add(new Item(ModContent.ItemType<KnowledgeDesertScourge>()) { shopCustomPrice = Item.buyPrice(gold: 15) }, Condition.DownedDestroyer)
                .Add(new Item(ModContent.ItemType<KnowledgeSkeletronPrime>()) { shopCustomPrice = Item.buyPrice(gold: 15) }, Condition.DownedSkeletronPrime)
                .Add(new Item(ModContent.ItemType<KnowledgeTwins>()) { shopCustomPrice = Item.buyPrice(gold: 15) }, Condition.DownedTwins)
                //.Add(new Item(ModContent.ItemType<KnowledgeMechs>()) { shopCustomPrice = Item.buyPrice(gold: 5) }, Condition.DownedMechBossAny)

                .Add(new Item(ModContent.ItemType<KnowledgeCryogen>()) { shopCustomPrice = Item.buyPrice(gold: 25) }, CalamityConditions.DownedCryogen)

                .Add(new Item(ModContent.ItemType<KnowledgeBrimstoneElemental>()) { shopCustomPrice = Item.buyPrice(gold: 25) }, CalamityConditions.DownedBrimstoneElemental)
                //.Add(new Item(ModContent.ItemType<KnowledgeBrimstoneCrag>()) { shopCustomPrice = Item.buyPrice(gold: 10) }, CalamityConditions.DownedBrimstoneElemental)

                .Add(new Item(ModContent.ItemType<KnowledgeAquaticScourge>()) { shopCustomPrice = Item.buyPrice(gold: 25) }, CalamityConditions.DownedAquaticScourge)
                //.Add(new Item(ModContent.ItemType<KnowledgeSulphurSea>()) { shopCustomPrice = Item.buyPrice(gold: 10) }, CalamityConditions.DownedAquaticScourge)

                .Add(new Item(ModContent.ItemType<KnowledgeCalamitasClone>()) { shopCustomPrice = Item.buyPrice(platinum: 1) }, CalamityConditions.DownedCalamitasClone)

                .Add(new Item(ModContent.ItemType<KnowledgePlantera>()) { shopCustomPrice = Item.buyPrice(gold: 50) }, Condition.DownedPlantera)

                .Add(new Item(ModContent.ItemType<KnowledgeAstrumAureus>()) { shopCustomPrice = Item.buyPrice(gold: 50) }, CalamityConditions.DownedPlaguebringer)

                .Add(new Item(ModContent.ItemType<KnowledgeLeviathanAnahita>()) { shopCustomPrice = Item.buyPrice(gold: 50) }, CalamityConditions.DownedLeviathan)
                //.Add(new Item(ModContent.ItemType<KnowledgeOcean>()) { shopCustomPrice = Item.buyPrice(gold: 15) }, CalamityConditions.DownedLeviathan)

                .Add(new Item(ModContent.ItemType<KnowledgeGolem>()) { shopCustomPrice = Item.buyPrice(gold: 50) }, Condition.DownedGolem)
                .Add(new Item(ModContent.ItemType<KnowledgeDukeFishron>()) { shopCustomPrice = Item.buyPrice(gold: 50) }, Condition.DownedDukeFishron)
                .Add(new Item(ModContent.ItemType<KnowledgePlaguebringerGoliath>()) { shopCustomPrice = Item.buyPrice(gold: 75) }, CalamityConditions.DownedPlaguebringer)
                .Add(new Item(ModContent.ItemType<KnowledgeRavager>()) { shopCustomPrice = Item.buyPrice(platinum: 1) }, CalamityConditions.DownedRavager)

                .Add(new Item(ModContent.ItemType<KnowledgeAstrumDeus>()) { shopCustomPrice = Item.buyPrice(gold: 50) }, CalamityConditions.DownedAstrumDeus)
                //.Add(new Item(ModContent.ItemType<KnowledgeAstralInfection>()) { shopCustomPrice = Item.buyPrice(gold: 15) }, CalamityConditions.DownedAstrumDeus)

                .Add(new Item(ModContent.ItemType<KnowledgeLunaticCultist>()) { shopCustomPrice = Item.buyPrice(gold: 75) }, Condition.DownedCultist)
                .Add(new Item(ModContent.ItemType<KnowledgeMoonLord>()) { shopCustomPrice = Item.buyPrice(gold: 75) }, Condition.DownedMoonLord)
                .Add(new Item(ModContent.ItemType<KnowledgeProfanedGuardians>()) { shopCustomPrice = Item.buyPrice(gold: 15) }, CalamityConditions.DownedGuardians)
                .Add(new Item(ModContent.ItemType<KnowledgeDragonfolly>()) { shopCustomPrice = Item.buyPrice(gold: 15) }, CalamityConditions.DownedBumblebird)
                .Add(new Item(ModContent.ItemType<KnowledgeProvidence>()) { shopCustomPrice = Item.buyPrice(platinum: 1) }, CalamityConditions.DownedProvidence)
                //.Add(new Item(ModContent.ItemType<KnowledgeSentinels>()) { shopCustomPrice = Item.buyPrice(gold: 25) }, tierThreeCondition)
                .Add(new Item(ModContent.ItemType<KnowledgePolterghast>()) { shopCustomPrice = Item.buyPrice(platinum: 1) }, CalamityConditions.DownedPolterghast)
                .Add(new Item(ModContent.ItemType<KnowledgeOldDuke>()) { shopCustomPrice = Item.buyPrice(gold: 150) }, CalamityConditions.DownedOldDuke)
                .Add(new Item(ModContent.ItemType<KnowledgeDevourerofGods>()) { shopCustomPrice = Item.buyPrice(gold: 150) }, CalamityConditions.DownedDevourerOfGods)
                .Add(new Item(ModContent.ItemType<KnowledgeYharon>()) { shopCustomPrice = Item.buyPrice(gold: 250) }, CalamityConditions.DownedYharon)
                .Add(new Item(ModContent.ItemType<KnowledgeExoMechs>()) { shopCustomPrice = Item.buyPrice(platinum: 5) }, CalamityConditions.DownedExoMechs)
                .Add(new Item(ModContent.ItemType<KnowledgeCalamitas>()) { shopCustomPrice = Item.buyPrice(platinum: 5) }, CalamityConditions.DownedSupremeCalamitas);
            dropsShop.Register();
        }
        //药水商店
        public void PotionShop_List()
        {
            var PotionShop = new NPCShop(Type, Language.GetTextValue($"{DialogueRoute}.ScalPotionShopOption"))
                .Add(new Item(ModContent.ItemType<CadancePotion>()) { shopCustomPrice = Item.buyPrice(silver: 50) }, Condition.Hardmode)
                .Add(new Item(ModContent.ItemType<RevivifyPotion>()) { shopCustomPrice = Item.buyPrice(silver: 50) }, Condition.Hardmode)
                .Add(new Item(ModContent.ItemType<TriumphPotion>()) { shopCustomPrice = Item.buyPrice(silver: 50) }, Condition.Hardmode)
                .Add(new Item(ModContent.ItemType<YharimsStimulants>()) { shopCustomPrice = Item.buyPrice(gold: 10) }, Condition.Hardmode)

                .Add(new Item(ModContent.ItemType<ShatteringPotion>()) { shopCustomPrice = Item.buyPrice(gold: 5) }, Condition.DownedGolem)
                .Add(new Item(ModContent.ItemType<TitanScalePotion>()) { shopCustomPrice = Item.buyPrice(gold: 5) }, Condition.DownedGolem)

                .Add(new Item(ModContent.ItemType<HolyWrathPotion>()) { shopCustomPrice = Item.buyPrice(gold: 20) }, Condition.DownedMoonLord)
                .Add(new Item(ModContent.ItemType<ProfanedRagePotion>()) { shopCustomPrice = Item.buyPrice(gold: 20) }, Condition.DownedMoonLord)
                //2铂金售价
                .Add(new Item(ModContent.ItemType<DraconicElixir>()) { shopCustomPrice = Item.buyPrice(gold: 50) }, CalamityConditions.DownedYharon);
            PotionShop.Register();
        }
        //弹药商店
        public void AmmoShop_List()
        {
            var AmmoShop = new NPCShop(Type, Language.GetTextValue($"{DialogueRoute}.ScalAmmoShopOption"))
                .Add(new Item(ModContent.ItemType<HyperiusBulletOld>()) { shopCustomPrice = Item.buyPrice(silver: 1) }, CalamityConditions.DownedAstrumDeus)

                .Add(new Item(ModContent.ItemType<ElysianArrowOld>()) { shopCustomPrice = Item.buyPrice(silver: 20) }, CalamityConditions.DownedProvidence)
                .Add(new Item(ModContent.ItemType<HolyFireBulletOld>()) { shopCustomPrice = Item.buyPrice(silver: 20) }, CalamityConditions.DownedProvidence)
                //神后的东西怎么能跟月前的东西一个售价呢
                .Add(new Item(ModContent.ItemType<GodSlayerSlug>()) { shopCustomPrice = Item.buyPrice(silver: 50) }, CalamityConditions.DownedDevourerOfGods)
                .Add(new Item(ModContent.ItemType<VanquisherArrowold>()) { shopCustomPrice = Item.buyPrice(silver: 50) }, CalamityConditions.DownedDevourerOfGods)
                .Add(new Item(ModContent.ItemType<VanquisherArrow>()) { shopCustomPrice = Item.buyPrice(silver: 50) }, CalamityConditions.DownedDevourerOfGods);
            AmmoShop.Register();
        }
        //杂项商店
        //为什么懒人需要卡黑墨水？
        //为什么有的稀有物品没有售卖？
        //为什么这么多东西没人去做个NPC之类的去卖？
        //为什么伽利略短剑要卡罗马短剑？
        //为什么蟹爪壳什么的是阴阳石合成物品，还没NPC去卖？
        //为什么懒人要加一个纸到合成表？
        //为什么其它售卖灾厄材料的mod，售卖的条件这么怪？
        //为什么嘉登召唤的解密这么难绷，要站桩这么久才能看几个字？
        //为什么我天顶世界三王全杀了也不卖神圣矿？
        //为什么为什么为什么为什么为什么为什么为什么为什么
        public void MiscShop_List()
        {
            var MiscShop = new NPCShop(Type, Language.GetTextValue($"{DialogueRoute}.ScalMiscShopOption"))
                .Add(new Item(ModContent.ItemType<ScalShopMessage>()) { shopCustomPrice = Item.buyPrice(platinum: 1145, gold: 14, silver:19, copper: 19) })
                .Add(new Item(ModContent.ItemType<WulfrumMetalScrap>()) { shopCustomPrice = Item.buyPrice(silver: 5) })
                .Add(new Item(ModContent.ItemType<EnergyCore>()) { shopCustomPrice = Item.buyPrice(gold: 1) })
                .Add(new Item(ModContent.ItemType<WulfrumBattery>()) { shopCustomPrice = Item.buyPrice(gold: 5) })
                .Add(new Item(ModContent.ItemType<CrawCarapace>()) { shopCustomPrice = Item.buyPrice(gold: 15) })
                .Add(new Item(ModContent.ItemType<GiantShell>()) { shopCustomPrice = Item.buyPrice(gold: 15) })
                .Add(new Item(ItemID.BlackInk) { shopCustomPrice = Item.buyPrice(gold: 1) })

                .Add(new Item(ModContent.ItemType<EssenceofEleum>()) { shopCustomPrice = Item.buyPrice(silver: 50) }, Condition.Hardmode)
                .Add(new Item(ModContent.ItemType<EssenceofSunlight>()) { shopCustomPrice = Item.buyPrice(silver: 50) }, Condition.Hardmode)
                .Add(new Item(ModContent.ItemType<EssenceofHavoc>()) { shopCustomPrice = Item.buyPrice(silver: 50) }, Condition.Hardmode)

                //瀚海狂鲨的鳞片出售就9银币，哥们你造15金币说是
                .Add(new Item(ModContent.ItemType<GrandScale>()) { shopCustomPrice = Item.buyPrice(gold: 10) }, Condition.DownedPlantera)
                //俺寻思这玩意蘑菇人卖都是50金币啊
                .Add(new Item(ItemID.Autohammer) { shopCustomPrice = Item.buyPrice(gold: 50) }, Condition.DownedPlantera)
                //bro正在试图用生命合金完成300%利润差价
                .Add(new Item(ModContent.ItemType<LifeAlloy>()) { shopCustomPrice = Item.buyPrice(gold: 5) }, CalamityConditions.DownedRavager)

                .Add(new Item(ItemID.Gladius) { shopCustomPrice = Item.buyPrice(gold: 5) }, CalamityConditions.DownedPolterghast)
                //下列材料的利润疑似高达200%
                .Add(new Item(ModContent.ItemType<UnholyEssence>()) { shopCustomPrice = Item.buyPrice(gold: 5) }, Condition.DownedMoonLord)
                .Add(new Item(ModContent.ItemType<Necroplasm>()) { shopCustomPrice = Item.buyPrice(gold: 5) }, Condition.DownedMoonLord)
                .Add(new Item(ModContent.ItemType<Bloodstone>()) { shopCustomPrice = Item.buyPrice(gold: 5) }, CalamityConditions.DownedProvidence)
                .Add(new Item(ModContent.ItemType<ReaperTooth>()) { shopCustomPrice = Item.buyPrice(gold: 5) }, CalamityConditions.DownedPolterghast)

                .Add(new Item(ModContent.ItemType<EndothermicEnergy>()) { shopCustomPrice = Item.buyPrice(gold: 5) }, CalamityConditions.DownedDevourerOfGods)
                .Add(new Item(ModContent.ItemType<NightmareFuel>()) { shopCustomPrice = Item.buyPrice(gold: 5) }, CalamityConditions.DownedDevourerOfGods)
                //5金币买入12金币卖出去，你小子 
                .Add(new Item(ModContent.ItemType<DarksunFragment>()) { shopCustomPrice = Item.buyPrice(gold: 15) }, CIConditions.DownedBuffedSolarEclipse)
                //化魂神晶利润率高达1000%
                .Add(new Item(ModContent.ItemType<AscendantSpiritEssence>()) { shopCustomPrice = Item.buyPrice(gold: 45) }, CalamityConditions.DownedDevourerOfGods)
                //你小子龙魂卖的比化魂神晶便宜
                .Add(new Item(ModContent.ItemType<YharonSoulFragment>()) { shopCustomPrice = Item.buyPrice(gold: 60) }, CalamityConditions.DownedYharon)

                .Add(new Item(ModContent.ItemType<CodebreakerBase>()) { shopCustomPrice = Item.buyPrice(gold: 5) })
                .Add(new Item(ModContent.ItemType<DecryptionComputer>()) { shopCustomPrice = Item.buyPrice(gold: 10) }, Condition.DownedSkeletron)
                .Add(new Item(ModContent.ItemType<LongRangedSensorArray>()) { shopCustomPrice = Item.buyPrice(gold: 15) }, Condition.DownedMechBossAny)
                .Add(new Item(ModContent.ItemType<AdvancedDisplay>()) { shopCustomPrice = Item.buyPrice(gold: 30) }, Condition.DownedGolem)
                .Add(new Item(ModContent.ItemType<VoltageRegulationSystem>()) { shopCustomPrice = Item.buyPrice(gold: 50) }, CalamityConditions.DownedProvidence)
                .Add(new Item(ModContent.ItemType<AuricQuantumCoolingCell>()) { shopCustomPrice = Item.buyPrice(platinum: 2) }, CalamityConditions.DownedYharon);
            MiscShop.Register();
        }
        //只卖你mod的稀有物品，其它mod等扔给杂项了
        //比如bro，魔君套需要五个神经元护符
        
        //统一抬了下价格，倒也不是因为说没钱花，而是感觉有些东西就卖这么少钱有点奇怪
        public void ItemShop_List()
        {
            var ItemShop = new NPCShop(Type, Language.GetTextValue($"{DialogueRoute}.ScalItemShopOption"))
                .Add(new Item(ModContent.ItemType<AmidiasSpark>()) { shopCustomPrice = Item.buyPrice(gold: 10) }, CalamityConditions.DownedDesertScourge)
                .Add(new Item(ItemID.FlyingCarpet) { shopCustomPrice = Item.buyPrice(gold: 10) })
                .Add(new Item(ModContent.ItemType<AncientShiv>()) { shopCustomPrice = Item.buyPrice(gold: 20) }, Condition.DownedSkeletron)
                .Add(new Item(ModContent.ItemType<PsychoticAmulet>()) { shopCustomPrice = Item.buyPrice(gold: 30) }, Condition.Hardmode)
                .Add(new Item(ModContent.ItemType<FrostBarrier>()) { shopCustomPrice = Item.buyPrice(gold: 30) }, Condition.Hardmode)
                .Add(new Item(ModContent.ItemType<LeadCore>()) { shopCustomPrice = Item.buyPrice(gold: 30) }, CalamityConditions.DownedAquaticScourge)
                .Add(new Item(ModContent.ItemType<MurasamaNeweffect>()) { shopCustomPrice = Item.buyPrice(platinum: 5) }, CalamityConditions.DownedYharon);
            ItemShop.Register();
        }

        public void MusicBoxShop_List()
        {
            var MusicShop = new NPCShop(Type, Language.GetTextValue($"{DialogueRoute}.ScalMusicBoxShopOpt"))
                .Add(new Item(ModContent.ItemType<CalamityTitleMusicBoxLegacy>()) { shopCustomPrice = Item.buyPrice(gold: 10) })
                .Add(new Item(ModContent.ItemType<Arcueid>()) { shopCustomPrice = Item.buyPrice(gold: 10) })
                .Add(new Item(ModContent.ItemType<BlessingOftheMoon>()) { shopCustomPrice = Item.buyPrice(gold: 10) })
                .Add(new Item(ModContent.ItemType<Kunoji>()) { shopCustomPrice = Item.buyPrice(gold: 10) })
                .Add(new Item(ModContent.ItemType<Kunoji>()) { shopCustomPrice = Item.buyPrice(gold: 10) })
                .Add(new Item(ModContent.ItemType<ProvidenceLegacy>()) { shopCustomPrice = Item.buyPrice(gold: 15) }, CalamityConditions.DownedProvidence)
                .Add(new Item(ModContent.ItemType<TyrantPart1>()) { shopCustomPrice = Item.buyPrice(gold: 20) }, CIConditions.DownedAnyYharon)
                .Add(new Item(ModContent.ItemType<RequiemsOfACruelWorld>()) { shopCustomPrice = Item.buyPrice(gold: 30) }, Condition.Hardmode)
                .Add(new Item(ModContent.ItemType<NowStopAskingWhere>()) { shopCustomPrice = Item.buyPrice(gold: 20) }, CIConditions.DownedAnyYharon);
            MusicShop.Register();
        }
        public void WineShop_List()
        {
            NPCShop shop = new(Type, Language.GetTextValue($"{DialogueRoute}.ScalWineShoppOpt"));
            shop.AddWithCustomValue(ItemID.LovePotion, Item.buyPrice(silver: 25), CalamityConditions.PotionSellingConfig, Condition.HappyEnough)
                .AddWithCustomValue(ModContent.ItemType<GrapeBeer>(), Item.buyPrice(silver: 30))
                .AddWithCustomValue(ModContent.ItemType<RedWine>(), Item.buyPrice(gold: 1))
                .AddWithCustomValue(ModContent.ItemType<Whiskey>(), Item.buyPrice(gold: 2))
                .AddWithCustomValue(ModContent.ItemType<Rum>(), Item.buyPrice(gold: 2))
                .AddWithCustomValue(ModContent.ItemType<Tequila>(), Item.buyPrice(gold: 2))
                .AddWithCustomValue(ModContent.ItemType<Fireball>(), Item.buyPrice(gold: 3))
                .AddWithCustomValue(ModContent.ItemType<PurpleHaze>(), Item.buyPrice(gold: 4))
                .AddWithCustomValue(ModContent.ItemType<Vodka>(), Item.buyPrice(gold: 2), Condition.DownedMechBossAll)
                .AddWithCustomValue(ModContent.ItemType<Screwdriver>(), Item.buyPrice(gold: 6), Condition.DownedMechBossAll)
                .AddWithCustomValue(ModContent.ItemType<WhiteWine>(), Item.buyPrice(gold: 6), Condition.DownedMechBossAll)
                .AddWithCustomValue(ModContent.ItemType<EvergreenGin>(), Item.buyPrice(gold: 8), Condition.DownedPlantera)
                .AddWithCustomValue(ModContent.ItemType<CaribbeanRum>(), Item.buyPrice(gold: 8), Condition.DownedPlantera)
                .AddWithCustomValue(ModContent.ItemType<Margarita>(), Item.buyPrice(gold: 8), Condition.DownedPlantera)
                .AddWithCustomValue(ModContent.ItemType<OldFashioned>(), Item.buyPrice(gold: 8), Condition.DownedPlantera)
                .AddWithCustomValue(ItemID.EmpressButterfly, Item.buyPrice(gold: 10), Condition.DownedPlantera)
                .AddWithCustomValue(ModContent.ItemType<Everclear>(), Item.buyPrice(gold: 3), CalamityConditions.DownedAstrumAureus)
                .AddWithCustomValue(ModContent.ItemType<BloodyMary>(), Item.buyPrice(gold: 4), CalamityConditions.DownedAstrumAureus, Condition.BloodMoon)
                .AddWithCustomValue(ModContent.ItemType<StarBeamRye>(), Item.buyPrice(gold: 6), CalamityConditions.DownedAstrumAureus, Condition.TimeNight)
                .AddWithCustomValue(ModContent.ItemType<Moonshine>(), Item.buyPrice(gold: 2), Condition.DownedGolem)
                .AddWithCustomValue(ModContent.ItemType<MoscowMule>(), Item.buyPrice(gold: 8), Condition.DownedGolem)
                .AddWithCustomValue(ModContent.ItemType<CinnamonRoll>(), Item.buyPrice(gold: 8), Condition.DownedGolem)
                .AddWithCustomValue(ModContent.ItemType<TequilaSunrise>(), Item.buyPrice(gold: 10), Condition.DownedGolem)
                .AddWithCustomValue(ItemID.BloodyMoscato, Item.buyPrice(gold: 1), Condition.DownedMoonLord, Condition.NpcIsPresent(NPCID.Stylist))
                .AddWithCustomValue(ItemID.BananaDaiquiri, Item.buyPrice(silver: 75), Condition.DownedMoonLord, Condition.NpcIsPresent(NPCID.Stylist))
                .AddWithCustomValue(ItemID.PeachSangria, Item.buyPrice(silver: 50), Condition.DownedMoonLord, Condition.NpcIsPresent(NPCID.Stylist))
                .AddWithCustomValue(ItemID.PinaColada, Item.buyPrice(gold: 1), Condition.DownedMoonLord, Condition.NpcIsPresent(NPCID.Stylist))
                .AddWithCustomValue(ModContent.ItemType<WeightlessCandle>(), Item.buyPrice(gold: 50))
                .AddWithCustomValue(ModContent.ItemType<VigorousCandle>(), Item.buyPrice(gold: 50))
                .AddWithCustomValue(ModContent.ItemType<ResilientCandle>(), Item.buyPrice(gold: 50))
                .AddWithCustomValue(ModContent.ItemType<SpitefulCandle>(), Item.buyPrice(gold: 50))
                .AddWithCustomValue(ModContent.ItemType<OddMushroom>(), Item.buyPrice(1))
                .AddWithCustomValue(ItemID.UnicornHorn, Item.buyPrice(0, 2, 50), Condition.HappyEnough, Condition.InHallow)
                .AddWithCustomValue(ItemID.Milkshake, Item.buyPrice(gold: 5), Condition.HappyEnough, Condition.InHallow, Condition.NpcIsPresent(NPCID.Stylist))
                .Register();
        }

        // Make this Town NPC teleport to the Queen statue when triggered.
        public override bool CanGoToStatue(bool toKingStatue) => !toKingStatue;

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            if (DownedBossSystem.downedCalamitas)
                damage = 15000;
            else
                damage = 50;
            knockback = 10f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 1;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<HellblastFriendly>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 1f;
        }
        // Explode into red dust on death.
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                DeathAshParticle.CreateAshesFromNPC(NPC);
                NPC.position = NPC.Center;
                NPC.width = NPC.height = 50;
                NPC.position.X -= NPC.width / 2;
                NPC.position.Y -= NPC.height / 2;
                for (int i = 0; i < 5; i++)
                {
                    int brimstone = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[brimstone].velocity *= 3f;
                    if (Main.rand.NextBool())
                    {
                        Main.dust[brimstone].scale = 0.5f;
                        Main.dust[brimstone].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }

                for (int i = 0; i < 10; i++)
                {
                    int fire = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 3f);
                    Main.dust[fire].noGravity = true;
                    Main.dust[fire].velocity *= 5f;

                    fire = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[fire].velocity *= 2f;
                }
            }
        }
    }
}
