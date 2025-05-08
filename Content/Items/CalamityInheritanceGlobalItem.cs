using System;
using System.Runtime.Serialization.Formatters;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Content.Projectiles.ArmorProj;
using CalamityInheritance.Content.Projectiles.Summon;
using CalamityInheritance.System.Configs;
using CalamityInheritance.UI;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Reaver;
using CalamityMod.Items.SummonItems;
using CalamityMod.NPCs.OldDuke;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items
{
    public partial class CalamityInheritanceGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void UpdateInventory(Item item, Player player)
        {
            var mplr = player.CIMod();
            //微光湖附近, 全传奇武器
            mplr.DukeTier1          = SetShimmeUpgrade(mplr.DukeTier1,          ModContent.ItemType<DukeLegendary>(),       ref player, DustID.Water);
            mplr.BetsyTier1         = SetShimmeUpgrade(mplr.BetsyTier1,         ModContent.ItemType<RavagerLegendary>(),    ref player, DustID.Meteorite);
            mplr.PBGTier1           = SetShimmeUpgrade(mplr.PBGTier1,           ModContent.ItemType<PBGLegendary>(),        ref player, DustID.TerraBlade);
            mplr.PlanteraTier1      = SetShimmeUpgrade(mplr.PlanteraTier1,      ModContent.ItemType<PlanteraLegendary>(),   ref player, DustID.DryadsWard);
            mplr.ColdDivityTier1    = SetShimmeUpgrade(mplr.ColdDivityTier1,    ModContent.ItemType<CyrogenLegendary>(),    ref player, DustID.Ice);
            mplr.DestroyerTier1     = SetShimmeUpgrade(mplr.DestroyerTier1,     ModContent.ItemType<DestroyerLegendary>(),  ref player, DustID.Silver);
            mplr.DefendTier1        = SetShimmeUpgrade(mplr.DefendTier1,        ModContent.ItemType<DefenseBlade>(),        ref player, DustID.GoldCoin);

            //海爵剑T3: 佩戴蠕虫围巾召唤老猪
            if (mplr.IsWearingBloodyScarf && CIFunction.IsThereNpcNearby(ModContent.NPCType<OldDuke>(), player, 3200f) && !mplr.DukeTier3)
            {
                if (CIFunction.FindInventoryItem(ref player, ModContent.ItemType<DukeLegendary>(), 1))
                {
                    LegendaryUpgradeTint(DustID.Water, player);
                    mplr.DukeTier3 = true;
                }
            }
            if (!mplr.YharimsKilledExo && DownedBossSystem.downedExoMechs)
            {
                if (CIFunction.FindInventoryItem(ref player, ModContent.ItemType<YharimsCrystalLegendary>(), 1))
                {
                    mplr.YharimsKilledExo = true;
                    LegendaryUpgradeTint(DustID.GoldCoin, player);
                }
            }
            if (!mplr.YharimsKilledScal && DownedBossSystem.downedCalamitas)
            {
                if (CIFunction.FindInventoryItem(ref player, ModContent.ItemType<YharimsCrystalLegendary>(), 1))
                {
                    mplr.YharimsKilledScal = true;
                    LegendaryUpgradeTint(DustID.GemRuby, player);
                }
            }
            base.UpdateInventory(item, player);

        }

        internal static bool SetShimmeUpgrade(bool legendT1, int itemID, ref Player player, short dustID)
        {
            if (legendT1)
                return false;
            if (player.ZoneShimmer && CIFunction.FindInventoryItem(ref player, itemID, 1))
            {
                LegendaryUpgradeTint(dustID, player);
                return true;
            }
            return false;
        }

        public override bool? UseItem(Item item, Player player)
        {
            int SHPC = ModContent.ItemType<DestroyerLegendary>();
            var cplr = player.Calamity();
            var mplr = player.CIMod();
            //SHPCT2: 月总在场时饮用葡萄汁
            if (item.type == ItemID.GrapeJuice && CIFunction.IsThereNpcNearby(NPCID.MoonLordHead, player, 3200f) && !player.CIMod().DestroyerTier2)
            {
                if (CIFunction.FindInventoryItem(ref player, SHPC, 1))
                {
                    player.CIMod().DestroyerTier2 = true;
                    LegendaryUpgradeTint(DustID.Silver, player);
                }
            }
            //叶柳T2：持有弹药回收buff时于丛林处食用黄金菜肴
            if (item.type == ItemID.GoldenDelight && Main.LocalPlayer.ZoneJungle && !player.CIMod().PlanteraTier2)
            {
                if (CIFunction.FindInventoryItem(ref player, ModContent.ItemType<PlanteraLegendary>(), 1))
                {
                    player.CIMod().PlanteraTier2 = true;
                    LegendaryUpgradeTint(DustID.DryadsWard, player);
                }
            }
            //寒冰神性T2: 雪原饮用温暖药水
            if (Main.LocalPlayer.ZoneSnow && !player.CIMod().ColdDivityTier2 && item.type == ItemID.WarmthPotion)
            {
                if (CIFunction.FindInventoryItem(ref player, ModContent.ItemType<CyrogenLegendary>(), 1))
                {
                    player.CIMod().ColdDivityTier2 = true;
                    LegendaryUpgradeTint(DustID.Ice, player);
                }
            }

            //SHPCT3: 召唤噬魂花……在吃下两个魔力上限物品后
            if (item.type == ModContent.ItemType<NecroplasmicBeacon>() && cplr.cShard && cplr.eCore && !mplr.DestroyerTier3)
            {
                if (CIFunction.FindInventoryItem(ref player, SHPC, 1))
                {
                    mplr.DestroyerTier3 = true;
                    LegendaryUpgradeTint(DustID.Silver, player);
                }
            }
            //叶流T3: 携带元素箭袋在丛林召唤一只丛林龙
            if (item.type == ModContent.ItemType<YharonEgg>() && (mplr.ElemQuiver|| mplr.IsWearingElemQuiverCal) && !mplr.PlanteraTier3)
            {
                if (CIFunction.FindInventoryItem(ref player, ModContent.ItemType<PlanteraLegendary>(), 1))
                {
                    mplr.PlanteraTier3 = true;
                    LegendaryUpgradeTint(DustID.DryadsWard, player);
                }
            }
            //庇护之刃T3: 防御力大于320点时召唤神明吞噬者
            if (item.type == ModContent.ItemType<CosmicWorm>() && !mplr.DefendTier3 && player.statDefense >= 320)
            {
                if (CIFunction.FindInventoryItem(ref player, ModContent.ItemType<DefenseBlade>(), 1))
                {
                    mplr.DefendTier3 = true;
                    LegendaryUpgradeTint(DustID.Gold, player);
                }
            }
            //孔雀翎T3：佩戴神圣护符召唤一次神吞
            if (item.type == ModContent.ItemType<CosmicWorm>() && !mplr.PBGTier3 && (mplr.deificAmuletEffect || player.Calamity().dAmulet))
            {
                if (CIFunction.FindInventoryItem(ref player, ModContent.ItemType<PBGLegendary>(), 1))
                {
                    mplr.PBGTier3 = true;
                    LegendaryUpgradeTint(DustID.TerraBlade, player);
                }
            }
            //寒冰神性T3: 在雪原地表召唤亵渎天神
            if (item.type == ModContent.ItemType<ProfanedCore>() && !mplr.ColdDivityTier3 && Main.LocalPlayer.ZoneSnow)
            {
                if (CIFunction.FindInventoryItem(ref player, ModContent.ItemType<CyrogenLegendary>(), 1))
                {
                    mplr.ColdDivityTier3 = true;
                    LegendaryUpgradeTint(DustID.Ice, player);
                }
            }
            
            return base.UseItem(item, player);
        }
        public static void LegendaryUpgradeTint(int dType, Player plr)
        {
            CIFunction.DustCircle(plr.Center, 32f, 1.8f, dType, true, 10f);
            SoundEngine.PlaySound(CISoundID.SoundFallenStar with { Volume = .5f }, plr.Center);
        }
        public override bool AltFunctionUse(Item item, Player player)
        {
            //冰灵传奇物品的一些我自己都看不懂的东西, 我从灾厄那复制的
            if (player.ActiveItem().type == ModContent.ItemType<CyrogenLegendary>())
            {
                bool canContinue = true;
                int count = 0;
                foreach (Projectile p in Main.ActiveProjectiles)
                {
                    if (p.type == ModContent.ProjectileType<CryogenPtr>() && p.owner == player.whoAmI)
                    {
                        if (p.ai[1] > 1f)
                        {
                            canContinue = false;
                            break;
                        }
                        else if (p.ai[1] == 0f)
                        {
                            if (((CryogenPtr)p.ModProjectile).Idle)
                                count++;
                        }
                    }
                }
                if (canContinue && count > 0)
                {
                    NPC tar = CalamityUtils.MinionHoming(Main.MouseWorld, 1000f, player);
                    if (tar != null)
                    {
                        int pAmt = count;
                        float angleVariance = MathHelper.TwoPi / pAmt;
                        float angle = 0f;

                        var source = player.GetSource_ItemUse(player.ActiveItem());
                        for (int i = 0; i < pAmt; i++)
                        {
                            if (Main.projectile.Length == Main.maxProjectiles)
                                break;
                            int pDmg = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(80);
                            int projj = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<CryogenPtr>(), pDmg, 1f, player.whoAmI, angle, 2f);
                            Main.projectile[projj].originalDamage = item.damage;

                            angle += angleVariance;
                            for (int j = 0; j < 22; j++)
                            {
                                Dust dust = Dust.NewDustDirect(Main.projectile[projj].position, Main.projectile[projj].width, Main.projectile[projj].height, DustID.Ice);
                                dust.velocity = Vector2.UnitY * Main.rand.NextFloat(3f, 5.5f) * Main.rand.NextBool().ToDirectionInt();
                                dust.noGravity = true;
                            }
                        }
                    }
                }
                return false;
            }
            return base.AltFunctionUse(item, player);
        }
        public int timesUsed = 0;
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            var usPlayer = player.CIMod(); 
            if (item.type == ItemID.AncientChisel)
                player.pickSpeed -= 0.15f; //回调饰品的挖掘速度

            if (item.type == ItemID.HandOfCreation)
                usPlayer.IfGodHand = true;

            if (item.type == ModContent.ItemType<SigilofCalamitas>())
                usPlayer.IfCalamitasSigile = true;
            
            if (item.type == ModContent.ItemType<BloodyWormScarf>())
                usPlayer.IsWearingBloodyScarf = true;

            if (item.type == ModContent.ItemType<ElementalQuiver>())
                usPlayer.IsWearingElemQuiverCal = true;
                
                
            if(CIServerConfig.Instance.VanillaUnnerf) //下面都是开启返厂原版数值之后的回调
            {
                VanillaAccesoriesUnnerf(item, player);  //饰品
                CalamityAccesoriesUnerf(item, player);  //灾厄相关的饰品
            }
            
        }
       
        public static void CalamityAccesoriesUnerf(Item item, Player player)
        {
            #region 补正饰品挖矿速度
            if (item.type == ModContent.ItemType<AncientFossil>())
            {
                player.pickSpeed -= 0.25f; //补正
            }
            if (item.type == ModContent.ItemType<SpelunkersAmulet>())
            {
                player.pickSpeed -= 0.05f;
            }
            if (item.type == ModContent.ItemType<ArchaicPowder>())
            {
                player.pickSpeed -= 0.05f;
            }
            #endregion
            #region 用于召唤位的叠加
            //是否佩戴了斯塔提斯诅咒?
            if (item.type == ModContent.ItemType<StatisCurse>())
            {
                //是否佩戴了原灾的核子之源？如果是，那就补正斯塔提斯诅咒的栏位    
                player.maxMinions += player.Calamity().nucleogenesis ? 3 : 0;
                //用于其他的堆叠操作，因为原灾没有对这个饰品的判定
                player.CIMod().WearingStatisCurse = true;
            }
            //是否佩戴占星?
            if (item.type == ModContent.ItemType<StarTaintedGenerator>())
            {
                //如果佩戴了核子之源？补正两个栏位
                player.maxMinions += player.Calamity().nucleogenesis ? 2 : 0;
            }
            //是否佩戴斯塔提斯祝福?
            if (item.type == ModContent.ItemType<StatisBlessing>())
            {
                //如果佩戴了斯塔提斯诅咒, 或者核子之源？补正两个栏位
                player.maxMinions += (player.CIMod().WearingStatisCurse || player.Calamity().nucleogenesis)? 2 : 0;
            }
            #endregion
        }
        #region GrabChanges
        public override void GrabRange(Item item, Player player, ref int grabRange)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            int itemGrabRangeBoost = 0 +
                (usPlayer.LoreWallofFlesh || usPlayer.PanelsLoreWallofFlesh ? 100 : 0) +
                (usPlayer.LorePlantera || usPlayer.PanelsLorePlantera ? 150 : 0) +
                (usPlayer.LorePolter || usPlayer.PanelsLorePolter ? 300 : 0);

            grabRange += itemGrabRangeBoost;
        }
        #endregion
        #region Shoot
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockBack)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();

            if (player.ownedProjectileCounts[ModContent.ProjectileType<UniverseSplitterField>()] > 0)

            if (usPlayer.LoreWallofFlesh || usPlayer.PanelsLoreWallofFlesh)
                velocity *= 1.10f;
            if (usPlayer.LorePlantera || usPlayer.PanelsLorePlantera)
                velocity *= 1.15f;
            if (usPlayer.LorePolter || usPlayer.PanelsLorePolter)
                velocity *= 1.20f;
        }
        #endregion

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            CalamityPlayer calPlayer = player.Calamity();
            if (usPlayer.GodSlayerRangedSet && calPlayer.canFireGodSlayerRangedProjectile)
            {
                if (item.CountsAsClass<RangedDamageClass>() && !item.channel)
                {
                    calPlayer.canFireGodSlayerRangedProjectile = false;
                    if (player.whoAmI == Main.myPlayer)
                    {
                        // God Slayer Ranged Shrapnel: 100%, soft cap starts at 800 base damage
                        int shrapnelRoundDamage = CalamityUtils.DamageSoftCap(damage * 2, 1500);
                        shrapnelRoundDamage = player.ApplyArmorAccDamageBonusesTo(shrapnelRoundDamage);

                        Projectile.NewProjectile(source, position, velocity * 1.25f, ModContent.ProjectileType<GodSlayerShrapnelRound>(), shrapnelRoundDamage, 2f, player.whoAmI);
                    }
                }
            }

            if (usPlayer.AuricbloodflareRangedSoul && calPlayer.canFireBloodflareRangedProjectile)
            {
                if (item.CountsAsClass<RangedDamageClass>() && !item.channel)
                {
                    calPlayer.canFireBloodflareRangedProjectile = false;
                    if (player.whoAmI == Main.myPlayer)
                    {
                        // Bloodflare Ranged Bloodsplosion: 80%, soft cap starts at 150 base damage
                        // This is intentionally extremely low because this effect can be grossly overpowered with sniper rifles and the like.
                        int bloodsplosionDamage = CalamityUtils.DamageSoftCap(damage * 0.8, 1200);
                        bloodsplosionDamage = player.ApplyArmorAccDamageBonusesTo(bloodsplosionDamage);

                        Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<BloodBomb>(), bloodsplosionDamage, 2f, player.whoAmI);
                    }
                }
            }

            if (usPlayer.ReaverRangedRocket && usPlayer.ReaverRocketFires)
            {
                if (item.CountsAsClass<RangedDamageClass>() && !item.channel)
                {
                    usPlayer.ReaverRocketFires = false;
                    if (player.whoAmI == Main.myPlayer)
                    {
                        Projectile.NewProjectile(source, position, velocity * 0.001f, ModContent.ProjectileType<ReaverRangedRocketMark>(), damage, 2f, player.whoAmI, 0f, 0f);
                    }
                }
            }
            return true;
        }

        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (CIConfig.Instance.turnoffCorner)
            {
                if (item.ModItem != null && item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>())
                {
                    Texture2D iconTexture = ModContent.Request<Texture2D>("CalamityInheritance/ExtraTextures/Mark").Value;
                    Vector2 iconPosition = position + new Vector2(4f, 4f);
                    float iconScale = 0.45f;

                    spriteBatch.Draw(iconTexture, iconPosition, null, Color.White, 0f, Vector2.Zero, iconScale, SpriteEffects.None, 0f);
                }
            }
        }
        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ItemID.WizardHat &&
               (body.type == ItemID.AmethystRobe ||
                body.type == ItemID.TopazRobe ||
                body.type == ItemID.SapphireRobe ||
                body.type == ItemID.EmeraldRobe ||
                body.type == ItemID.RubyRobe ||
                body.type == ItemID.DiamondRobe ||
                body.type == ItemID.AmberRobe))
                return "WizardHatCI";

            if (head.type == ItemID.MagicHat &&
               (body.type == ItemID.AmethystRobe ||
                body.type == ItemID.TopazRobe ||
                body.type == ItemID.SapphireRobe ||
                body.type == ItemID.EmeraldRobe ||
                body.type == ItemID.RubyRobe ||
                body.type == ItemID.DiamondRobe ||
                body.type == ItemID.AmberRobe))
                return "MagicHatCI";

            if (head.type == ItemID.SolarFlareHelmet &&
                body.type == ItemID.SolarFlareBreastplate &&
                legs.type == ItemID.SolarFlareLeggings)
                return "SolarSetCI";

            #region 塔防套件
            //和尚
            if (head.type == ItemID.MonkBrows &&
                body.type == ItemID.MonkShirt &&
                legs.type == ItemID.MonkPants)
                return "MonkSetCI";
            //渗透忍者
            if (head.type == ItemID.MonkAltHead &&
                body.type == ItemID.MonkAltShirt &&
                legs.type == ItemID.MonkAltPants)
                return "ShinobiNinjaSetCI";
            //学徒
            if (head.type == ItemID.ApprenticeHat &&
                body.type == ItemID.ApprenticeRobe &&
                legs.type == ItemID.ApprenticeTrousers)
                return "AppernticeSetCI";
            //暗黑艺术家
            if (head.type == ItemID.ApprenticeAltHead &&
                body.type == ItemID.ApprenticeAltShirt &&
                legs.type == ItemID.ApprenticeAltPants)
                return "DarkArtistSetCI";
            //女猎手
            if (head.type == ItemID.HuntressWig &&
                body.type == ItemID.HuntressJerkin &&
                legs.type == ItemID.HuntressPants)
                return "HuntressSetCI";
            //小红帽
            if (head.type == ItemID.HuntressAltHead &&
                body.type == ItemID.HuntressAltShirt &&
                legs.type == ItemID.HuntressAltPants)
                return "RedRidingSetCI";
            //侍卫
            if (head.type == ItemID.SquireGreatHelm &&
                body.type == ItemID.SquirePlating &&
                legs.type == ItemID.SquireGreaves)
                return "SquireSetCI";
            //圣骑士
            if (head.type == ItemID.SquireAltHead &&
                body.type == ItemID.SquireAltShirt &&
                legs.type == ItemID.SquireAltPants)
                return "VKnight";
            #endregion

            //寒霜套
            if (head.type == ItemID.FrostHelmet &&
                body.type == ItemID.FrostBreastplate &&
                legs.type == ItemID.FrostLeggings)
                return "FrostArmorSetCI";
            //丛林套
            if (head.type == ItemID.JungleHat &&
                body.type == ItemID.JungleShirt &&
                legs.type == ItemID.JunglePants)
                return "JungleSetCI";
            //死灵套
            if ((head.type == ItemID.NecroHelmet || head.type == ItemID.AncientNecroHelmet) &&
                body.type == ItemID.NecroBreastplate &&
                legs.type == ItemID.NecroGreaves)
                return "NecroSetCI";
            //熔岩套
            if (head.type == ItemID.MoltenHelmet &&
                body.type == ItemID.MoltenBreastplate &&
                legs.type == ItemID.MoltenGreaves)
                return "MoltensetCI";
            return "";
        }
        public override void UpdateArmorSet(Player player, string set)
        {
            if (CIServerConfig.Instance.VanillaUnnerf) //下面都是开启返厂原版数值之后的回调
                VanillarArmorSetUnnerf(player, set);
        }
        public override void UpdateEquip(Item item, Player player)
        {
            #region 挖矿速度补正(原灾)
            if (item.type == ModContent.ItemType<ReaverHeadExplore>())
            {
                player.pickSpeed -= 0.2f; //补正成40%
            }
            #endregion

            if (CIServerConfig.Instance.VanillaUnnerf) //下面都是开启返厂原版数值之后的回调
            {
                VanillaArmorUnnerf(item, player); //盔甲
            }
        }
        public static void VanillarArmorSetUnnerf(Player player, string set)
        {
            //回调盔甲奖励的数值, 这里需要一个ban掉原灾tooltip或者说修改的方法
            if (set == "WizardHatCI")
            {
                player.GetCritChance<MagicDamageClass>() += 6; //回调到10%
            }
            else if (set == "MagicHatCI")
            {
                player.statManaMax2 += 20; //回调
            }
            else if (set == "SolarSetCI")
            {
                player.endurance += 0.12f;//回调12%的常驻免伤
            }
            #region 塔防套件回调散件高数值
            //圣骑士
            else if (set == "VKnight")
            {
                player.lifeRegen -= 6;
                player.GetDamage<SummonDamageClass>() -= 0.1f;
                player.GetCritChance<MeleeDamageClass>() -= 10;
            }
            //和尚
            else if (set == "MonkSetCI")
            {
                player.GetDamage<SummonDamageClass>() -= 0.15f;
                player.GetDamage<MeleeDamageClass>() -= 0.1f;
                player.GetAttackSpeed<MeleeDamageClass>() -= 0.1f;
                player.GetCritChance<MeleeDamageClass>() -= 10;
            }
            //忍者
            else if (set == "ShinobiNinjaSetCI")
            {
                player.GetDamage<SummonDamageClass>() -= 0.3f;
                player.GetDamage<MeleeDamageClass>() -= 0.1f;
                player.GetAttackSpeed<MeleeDamageClass>() -= 0.1f;
                player.GetCritChance<MeleeDamageClass>() -= 10;
            }
            //侍卫
            else if (set == "SquireSetCI")
            {
                player.lifeRegen -= 3;
                player.GetDamage<SummonDamageClass>() -= 0.15f;
                player.GetCritChance<MeleeDamageClass>() -= 10;
            }
            //小红帽
            else if (set == "RedRidingSetCI")
            {
                player.GetDamage<SummonDamageClass>() -= 0.1f;
                player.GetDamage<RangedDamageClass>() -= 0.1f;
            }
            //女猎手
            else if (set == "HuntressSetCI")
            {
                player.GetDamage<SummonDamageClass>() -= 0.1f;
                player.GetDamage<RangedDamageClass>() -= 0.1f;
            }
            //暗黑艺术家
            else if (set == "DarkArtistSetCI")
            {
                player.GetDamage<SummonDamageClass>() -= 0.1f;
                player.GetCritChance<MagicDamageClass>() -= 15;
            }
            //学徒
            else if (set == "AppernticeSetCI")
            {
                player.GetDamage<SummonDamageClass>() -= 0.05f;
                player.GetCritChance<MagicDamageClass>() -= 15;
            }
            #endregion
            #region 寒霜套回调
            else if (set == "FrostArmorSetCI")
            {
                //ban掉原灾的效果
                player.Calamity().frostSet = false;
                //复原原本的效果
                player.GetDamage<MeleeDamageClass>() += 0.1f;
                player.GetDamage<RangedDamageClass>() += 0.1f;
            }
            #endregion
            #region 丛林套回调
            else if (set == "JungleSetCI")
            {
                //回调魔力消耗
                player.manaCost -= 0.06f;
            }
            #endregion
            #region 死灵套回调
            else if (set == "NecroSetCI")
            {
                //ban掉原灾的效果 
                player.Calamity().necroSet = false;
            }
            #endregion
            #region 熔岩套回调
            else if (set == "MoltensetCI")
            {
                //注：灾厄的熔岩套把10%近战伤害转成了10%真近战伤害
                //取消真近战伤害加成
                player.GetDamage<TrueMeleeDamageClass>() -= 0.10f;
                //恢复为普通近战伤害加成
                player.GetDamage<MeleeDamageClass>() += 0.10f;
            }
            #endregion
        }
        public static void VanillaAccesoriesUnnerf(Item item, Player player)
        {
            //饰品回调
            var calPlayer = player.Calamity();
            #region 手套
            switch (item.type)
            {
                /*
                *附：灾厄实现手套不可堆叠的逻辑是采用一个手套等级
                *他们先通过舍弃了所有手套的原有攻速之后，再赋予一个手套等级，然后在player类里面进行操作
                *如绿手套是1级，火手套是4级，那4级大于1级就不能堆叠(通过 Level > 4 ? 0.14f : ...)这种表达式方法递归实现
                *这里的处理是，查看玩家手套等级的大小，然后补正低级手套原本能提供的攻速
                */
                case ItemID.FeralClaws:
                    //绿手套本身就少了2%
                    player.GetAttackSpeed<MeleeDamageClass>() += 0.02f;
                    //存在二级手套极以上，补正
                    if (calPlayer.gloveLevel > 1) player.GetAttackSpeed<MeleeDamageClass>() += 0.10f;
                    break;
                case ItemID.PowerGlove:
                    //存在三级手套极以上，补正
                    if (calPlayer.gloveLevel > 2) player.GetAttackSpeed<MeleeDamageClass>() += 0.12f;
                    break;
                case ItemID.BerserkerGlove:
                    //狂战士手套本身没有手套等级, 因此直接回调
                    player.GetAttackSpeed<MeleeDamageClass>() += 0.12f;
                    break;
                case ItemID.MechanicalGlove:
                    //存在四级手套极以上，补正
                    if (calPlayer.gloveLevel > 3) player.GetAttackSpeed<MeleeDamageClass>() += 0.12f;
                    break;
                case ItemID.FireGauntlet:
                    //存在元素手套，补正
                    if (calPlayer.gloveLevel > 4) player.GetAttackSpeed<MeleeDamageClass>() += 0.14f;
                    break;
                default:
                    break;
            }
            #endregion
            #region 其余攻速削弱
            switch (item.type)
            {
                case ItemID.SunStone:
                    if (Main.dayTime) player.GetAttackSpeed<MeleeDamageClass>() += 0.1f; //回调10%
                    break;

                case ItemID.MoonStone:
                    if (!Main.dayTime) player.GetAttackSpeed<MeleeDamageClass>() += 0.1f; //回调10%
                    break;

                case ItemID.MoonShell:
                    if (!Main.dayTime) player.GetAttackSpeed<MeleeDamageClass>() += 0.051f; //回调10%
                    break;

                case ItemID.MoonCharm:
                    if (!Main.dayTime) player.GetAttackSpeed<MeleeDamageClass>() += 0.051f; //回调10%
                    break;

                case ItemID.CelestialStone:
                    player.GetAttackSpeed<MeleeDamageClass>() += 0.1f; //回调10%
                    break;

                case ItemID.CelestialShell:
                    player.GetAttackSpeed<MeleeDamageClass>() += 0.1f; //回调10%
                    if (!Main.dayTime)
                        player.GetAttackSpeed<MeleeDamageClass>() += 0.051f; //他真的狠精准，我哭死
                    break;
                default:
                    break;
            }
            #endregion
            #region 冗余削弱
            if (item.type == ItemID.SniperScope)
            {
                player.GetDamage<RangedDamageClass>() += 0.03f;
                player.GetCritChance<RangedDamageClass>() += 0.03f; //牢灾这0.03f的暴击概率削弱还没改呢
            }
            if (item.type == ItemID.MagicQuiver)
            {
                player.arrowDamage += 0.05f;
            }
            if (item.type == ItemID.MoltenQuiver)
            {
                player.arrowDamage += 0.03f;
            }
            if (item.type == ItemID.WormScarf)
            {
                player.endurance += 0.03f; //回调17%
            }
            if (item.type == ItemID.EmpressFlightBooster)
            {
                player.CIMod().EmpressBooster = true;
                
            }
            #endregion
        }
        public static void VanillaArmorUnnerf(Item item, Player player)
        {
            //备注：这些东西都要修改tooltip，或者说直接禁用原灾的tooltip修改也可以
            switch (item.type)
            {
                case ItemID.VortexHelmet: //回调星璇数值
                    player.GetDamage<RangedDamageClass>() += 0.06f;
                    player.GetCritChance<RangedDamageClass>() += 2;
                    break;
                //流星套
                case ItemID.MeteorHelmet:
                case ItemID.MeteorLeggings:
                case ItemID.MeteorSuit:
                    player.GetDamage<MagicDamageClass>() += 0.01f;//我也不知道灾厄削弱这1%干嘛
                    break;
                //丛林帽
                case ItemID.JungleHat:
                    player.statManaMax2 += 20;
                    player.GetCritChance<MagicDamageClass>() += 3;
                    break;
                //丛林护腿
                case ItemID.JunglePants:
                    player.GetCritChance<MagicDamageClass>() += 3;
                    break;
                //日耀头盔
                case ItemID.SolarFlareHelmet:
                    player.GetCritChance<MeleeDamageClass>() += 6;  //回调日耀套:20->26
                    break;
                //精金近战
                case ItemID.AdamantiteHelmet:
                    player.GetAttackSpeed<MeleeDamageClass>() += 0.05f; //回调5%
                    break;
                //魔法帽
                case ItemID.MagicHat:
                    player.GetDamage<MagicDamageClass>() += 0.06f;//回调
                    break;
                //稽古
                case ItemID.Gi:
                    player.jumpSpeedBoost -= 0.5f;  //取消跳跃速度 (是的，原灾在这写了个50%提升)
                    player.GetAttackSpeed<MeleeDamageClass>() += 0.1f;//回调攻速
                    break;
                #region 法袍们
                case ItemID.AmethystRobe:
                    player.manaCost -= 0.01f; //4% -> 5%
                    break;
                case ItemID.TopazRobe:
                    player.statManaMax2 += 20;
                    player.manaCost -= 0.02f; //5% -> 7%
                    break;
                case ItemID.SapphireRobe:
                    player.manaCost -= 0.03f; //6% -> 9%
                    break;
                case ItemID.EmeraldRobe:
                    player.statManaMax2 += 20;
                    player.manaCost -= 0.04f; //7% -> 11%
                    break;
                case ItemID.RubyRobe:
                case ItemID.AmberRobe:
                    player.manaCost -= 0.05f; //8% -> 13%
                    break;
                case ItemID.DiamondRobe:
                    player.statManaMax2 += 20;
                    player.manaCost -= 0.06f; //9% -> 15%
                    break;

                #endregion 
                #region 塔防散件的回调
                #region 和尚 
                case ItemID.MonkBrows:
                    player.GetAttackSpeed<MeleeDamageClass>() += 0.1f;
                    break;
                case ItemID.MonkPants:
                    player.GetDamage<SummonDamageClass>() += 0.05f;
                    player.GetCritChance<MeleeDamageClass>() += 10;
                    break;
                case ItemID.MonkShirt:
                    player.GetDamage<SummonDamageClass>() += 0.1f;
                    player.GetDamage<MeleeDamageClass>() += 0.1f;
                    break;
                #endregion
                #region 女猎手
                case ItemID.HuntressJerkin:
                    player.GetDamage<RangedDamageClass>() += 0.1f;
                    player.GetDamage<SummonDamageClass>() += 0.1f;
                    break;
                #endregion
                #region 学徒
                case ItemID.ApprenticeTrousers:
                    player.GetDamage<SummonDamageClass>() += 0.05f;
                    player.GetCritChance<MagicDamageClass>() += 15;
                    break;
                #endregion
                #region 侍卫
                case ItemID.SquireGreatHelm:
                    player.lifeRegen += 6;
                    break;
                case ItemID.SquirePlating:
                    player.GetDamage<MeleeDamageClass>() += 0.05f;
                    player.GetDamage<SummonDamageClass>() += 0.05f;
                    break;
                case ItemID.SquireGreaves:
                    player.GetDamage<SummonDamageClass>() += 0.10f;
                    player.GetCritChance<MeleeDamageClass>() += 10;
                    break;
                #endregion
                #region 圣骑士
                case ItemID.SquireAltShirt:
                    player.lifeRegen += 6;
                    break;
                case ItemID.SquireAltPants:
                    player.GetDamage<SummonDamageClass>() += 0.1f;
                    player.GetCritChance<MeleeDamageClass>() += 10;
                    break;
                #endregion
                #region 小红帽
                case ItemID.HuntressAltShirt:
                    player.GetDamage<RangedDamageClass>() += 0.1f;
                    player.GetDamage<SummonDamageClass>() += 0.1f;
                    break;
                #endregion
                #region 黑暗艺术家 
                case ItemID.ApprenticeAltPants:
                    player.GetDamage<SummonDamageClass>() += 0.1f;
                    player.GetCritChance<MagicDamageClass>() += 15;
                    break;
                #endregion
                #region 渗透忍者 
                case ItemID.MonkAltHead:
                    player.GetDamage<MeleeDamageClass>() += 0.1f;
                    player.GetDamage<SummonDamageClass>() += 0.1f;
                    break;
                case ItemID.MonkAltShirt:
                    player.GetDamage<SummonDamageClass>() += 0.1f;
                    player.GetAttackSpeed<MeleeDamageClass>() += 0.1f;
                    break;
                case ItemID.MonkAltPants:
                    player.GetDamage<SummonDamageClass>() += 0.1f;
                    player.GetCritChance<MeleeDamageClass>() += 10;
                    break;
                #endregion
                #endregion
                default:
                    break;
            }
        }
        public override bool CanUseItem(Item item, Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            CalamityGlobalItem modItem = item.Calamity();

            // Restrict behavior when reading Dreadon's Log.
            if (CalPopupGUIManager.AnyGUIsActive)
                return false;

            return true;
        }
    }
}