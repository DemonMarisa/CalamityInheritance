using CalamityInheritance.Content.Items.Accessories;
using Terraria.ModLoader;
using CalamityMod;
using System.Collections.Generic;
using Terraria;
using System;
using Terraria.Audio;
using Terraria.ID;
using Microsoft.Xna.Framework;
using CalamityMod.CalPlayer;
using CalamityMod.Buffs.DamageOverTime;
using Terraria.GameInput;
using CalamityMod.Cooldowns;
using CalamityMod.Dusts;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.World;
using CalamityInheritance.UI;
using CalamityInheritance.Content.Projectiles.ArmorProj;
using CalamityInheritance.Core;
using CalamityInheritance.System.Configs;
using Hjson;
using CalamityInheritance.Sounds.Custom;


namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public static readonly SoundStyle AbsorberHit = new("CalamityMod/Sounds/Custom/AbilitySounds/SilvaActivation") { Volume = 0.7f };

        #region Timer and Counter
        public int modStealth = 1000;
        public int summonProjCooldown = 0;
        public int ProjectilHitCounter;
        public int ProjectilHitCounter2;
        public bool PopTextFlight = false;
        //是否给过灾厄精华
        //1就是没给
        //2就是给了
        public int FreeEssence = 1;
        //这个用于传奇物品的总伤害计数
        public int DamagePool = 0;
        public bool SolarShieldEndurence = false; //日耀盾免伤计算
        #endregion
        #region 武器效果
        public float AnimusDamage = 1f;
        public int DNAImmnue = 0;
        public int DNAImmnueActive = 0;
        public bool BuffPolarisBoost = false;
        public bool photovisceratorCrystal = false;
        //我不太确认bool数组new的时候是否会自动为false，所以这样写了
        //孔雀翎升级存储
        public bool PBGTier1 = false;
        public bool PBGTier2 = false;
        public bool PBGTier3 = false;
        //海爵剑升级存储
        public bool DukeTier1 = false;
        public bool DukeTier2 = false;
        public bool DukeTier3 = false;
        //叶流升级存储
        public bool PlanteraTier1 = false;
        public bool PlanteraTier2 = false;
        public bool PlanteraTier3 = false;
        //维苏威阿斯升级存储
        public bool BetsyTier1 = false;
        public bool BetsyTier2 = false;
        public bool BetsyTier3 = false;
        //SHPC升级存储
        public bool DestroyerTier1 = false;
        public bool DestroyerTier2 = false;
        public bool DestroyerTier3 = false;
        //庇护之刃升级存储
        public bool DefendTier1 = false;
        public bool DefendTier2 = false;
        public bool DefendTier3 = false;
        public int DefendTier1Timer = 0;
        public float DefenseBoost = 0f;
        public int DefendTier2Pool = 0;
        //寒冰神性升级存储
        public bool ColdDivityTier1 = false;
        public bool ColdDivityTier2 = false;
        public bool ColdDivityTier3 = false;
        public bool IsColdDivityActiving = false;
        //特殊：暴君水晶升级存储，但是...
        public bool YharimsKilledExo = false;
        public bool YharimsKilledScal = false;
        public bool YharimsFuckDragon = false;
        public bool PBGLegendaryDyeable = false;
        public Color PBGBeamColor;
        #endregion
        #region dash
        public int dashTimeMod;
        public bool HasReducedDashFirstFrame = false;
        public bool HasIncreasedDashFirstFrame = false;
        //这是一个会自动归零的数值，速度为1帧1点，也许可以用于除冲刺外的计时
        public int CIDashDelay;
        public bool ElysianAegis = false;
        public bool ElysianGuard = false;
        public float shieldInvinc = 5f;
        #endregion
        #region Energy Shields
        public Dictionary<string, DateTime> cooldowns = new Dictionary<string, DateTime>();//没有任何用处，仅用来防止报错，至少目前是
        public bool CIHasAnyEnergyShield => CIsponge;
        public bool freeDodgeFromShieldAbsorption = false;
        public bool CIdrawnAnyShieldThisFrame = false;
        // TODO -- Some way to show the player their total shield points.
        public int CITotalMaxShieldDurability => CIsponge ? TheSpongetest.CIShieldDurabilityMax : 0;

        public int CISpongeShieldDurability = 0;

        public bool CIsponge = false;
        public bool CIspongeShieldVisible = false;
        internal float CIspongeShieldPartialRechargeProgress = 0f;
        internal bool CIplayedSpongeShieldSound = false;

        public int ShieldDurabilityMax = 0;

        #endregion
        #region Summon
        public bool MagicHatOld = false;
        public bool MidnnightSunBuff = false;
        public bool cosmicEnergy = false;
        public bool IsAncientClasper = false;
        public bool bloodClot = false;
        #endregion
        #region 禁止生成物品
        public bool cIdisableVoodooSpawns = false;
        public bool cIdisablePerfCystSpawns = false;
        public bool cIdisableHiveCystSpawns = false;
        public bool cIdisableNaturalScourgeSpawns = false;
        public bool cIdisableAnahitaSpawns = false;
        #endregion
        #region ResetEffects
        public override void ResetEffects()
        {
            //生命上限（们）
            ResetLifeMax();
            //贴图切换现已全部包装成函数，并单独分出来在PlayerResprite.cs内
            RespriteOptions(); 
            //传颂全部包装
            ResetLore(); 
            //套装奖励全部封装
            ResetArmorSet();
            //饰品全部封装
            ResetAccessories();
            //buff全部封装
            ResetBuff();
            //熟练度处理
            GiveBoost();
            IsColdDivityActiving = false;
            PBGLegendaryDyeable = false;
            PBGBeamColor = default;
            ForceHammerStealth = false;
            CIDashID = string.Empty;
            ElysianAegis = false;
            #region Summon
            MagicHatOld = false;
            MidnnightSunBuff = false;
            ReaverSummonerOrb = false;
            cosmicEnergy = false;
            IsAncientClasper = false;
            bloodClot = false;
            #endregion
            #region 禁止生成物品
            cIdisableVoodooSpawns = false;
            cIdisablePerfCystSpawns = false;
            cIdisableHiveCystSpawns = false;
            cIdisableNaturalScourgeSpawns = false;
            cIdisableAnahitaSpawns = false;
            #endregion
        }

        #endregion
        #region 旧位置保存
        public readonly Queue<Vector2> oldPositions = new Queue<Vector2>();
        public int MaxoldPositions = 6; // 最多保存多少个
        #endregion
        #region UpdateDead
        public override void UpdateDead()
        {
            //套装奖励全部封装
            UpdateDeadArmorSet();
            //饰品效果全部封装
            UpdateDeadAccessories();
            //buff全部封装
            UpdateDeadBuff();
            ForceHammerStealth = false;
            BuffExoApolste = false;
            IfCloneHtting = false; //克隆大锤子是否在攻击
            DraedonsHeartLegacyStats = false;
            DNAImmnue = 0;
            AnimusDamage = 1f;
            //这个用于传奇物品的总伤害计数
            DamagePool = 0;
            DefendTier1Timer = 0;
            DefenseBoost = 0f;
            DNAImmnueActive = 0;
        }
        public override void PostUpdate()
        {
            
        }
        #region TeleportMethods
        public static Vector2? GetJunglePosition(Player player)
        {
            bool canSpawn = false;
            int teleportStartX = Abyss.AtLeftSideOfWorld ? (int)(Main.maxTilesX * 0.65) : (int)(Main.maxTilesX * 0.2);
            int teleportRangeX = (int)(Main.maxTilesX * 0.15);
            int teleportStartY = (int)Main.worldSurface - 75;
            int teleportRangeY = 50;

            Player.RandomTeleportationAttemptSettings settings = new Player.RandomTeleportationAttemptSettings
            {
                mostlySolidFloor = true,
                avoidAnyLiquid = true,
                avoidLava = true,
                avoidHurtTiles = true,
                avoidWalls = true,
                attemptsBeforeGivingUp = 1000,
                maximumFallDistanceFromOrignalPoint = 30
            };

            Vector2 vector = player.CheckForGoodTeleportationSpot(ref canSpawn, teleportStartX, teleportRangeX, teleportStartY, teleportRangeY, settings);

            if (canSpawn)
            {
                return (Vector2?)vector;
            }
            return null;
        }
        #endregion

        public override void PreUpdate()
        {
            #region 更新旧位置保存
            oldPositions.Enqueue(Player.position);
            while (oldPositions.Count > MaxoldPositions)
                oldPositions.Dequeue();
            #endregion

            if (HasCustomDash && UsedDash.IsOmnidirectional)
                Player.maxFallSpeed = 50f;

            if(HasCustomDash)
            {
                if (CIDashDelay > 0)
                {
                    CIDashDelay--;
                }
                else if (CIDashDelay < 0)
                {
                    CIDashDelay++;
                }
            }
            // 条件的更新
            PreUp();
        }
        #endregion
        #region Post Hurt
        public override void PostHurt(Player.HurtInfo hurtInfo)
        {
            Player.Calamity().GemTechState.PlayerOnHitEffects(hurtInfo.Damage);

            bool hardMode = Main.hardMode;
            
            if (GodSlayerMelee && hurtInfo.Damage > 80)
            {
                var source = Player.GetSource_Misc("24");
                SoundEngine.PlaySound(SoundID.Item73, Player.Center);
                float spread = 45f * 0.0174f;
                double startAngle = Math.Atan2(Player.velocity.X, Player.velocity.Y) - spread / 2;
                double deltaAngle = spread / 8f;
                double offsetAngle;
                int shrapnelDamage = Player.ApplyArmorAccDamageBonusesTo(Player.CalcIntDamage<MeleeDamageClass>(1500));
                if (Player.whoAmI == Main.myPlayer)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                        Projectile.NewProjectile(source, Player.Center.X, Player.Center.Y, (float)(Math.Sin(offsetAngle) * 10f), (float)(Math.Cos(offsetAngle) * 10f), ModContent.ProjectileType<GodSlayerDart>(), shrapnelDamage, 5f, Player.whoAmI, 1f, 0f);
                        Projectile.NewProjectile(source, Player.Center.X, Player.Center.Y, (float)(-Math.Sin(offsetAngle) * 10f), (float)(-Math.Cos(offsetAngle) * 10f), ModContent.ProjectileType<GodSlayerDart>(), shrapnelDamage, 5f, Player.whoAmI, 1f, 0f);
                    }
                }
            }
            if (Player.whoAmI == Main.myPlayer)
            {
                if (AstralBulwark)
                {
                    var source = Player.GetSource_Accessory(FindAccessory(ModContent.ItemType<AstralBulwark>()));
                    int astralStarDamage = (int)Player.GetBestClassDamage().ApplyTo(320);
                    astralStarDamage = Player.ApplyArmorAccDamageBonusesTo(astralStarDamage);
                    int starAmt = 5;
                    for (int n = 0; n < starAmt; n++)
                    {
                        CalamityUtils.ProjectileRain(source , Player.Center, 400f, 100f, 500f, 800f, 29f, ModContent.ProjectileType<AstralStar>(), astralStarDamage , 5f, Player.whoAmI);
                    }
                }

                if (FungalCarapace)
                {
                    CalamityPlayer modPlayer = Player.Calamity();
                    var source = Player.GetSource_Accessory(modPlayer.FindAccessory(ModContent.ItemType<FungalCarapace>()));
                    if (hurtInfo.Damage > 0)
                    {
                        // 播放声音
                        SoundEngine.PlaySound(SoundID.NPCHit45, Player.position);
                        Player.moveSpeed += 0.2f;

                        // 弹幕生成参数
                        float spread = 45f * 0.0174f;
                        double startAngle = Math.Atan2(Player.velocity.X, Player.velocity.Y) - spread / 2;
                        double deltaAngle = spread / 8f;
                        double offsetAngle;
                        int fDamage = (int)Player.GetBestClassDamage().ApplyTo(70);

                        if (Player.whoAmI == Main.myPlayer)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                // 确定生成位置
                                float xPos = Main.rand.NextBool() ? Player.Center.X + 100 : Player.Center.X - 100;
                                Vector2 spawnPos = new Vector2(xPos, Player.Center.Y + Main.rand.Next(-100, 101));

                                // 计算角度
                                offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;

                                // 创建弹幕
                                var spore1 = Projectile.NewProjectileDirect(
                                    source,
                                    spawnPos,
                                    new Vector2((float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f)),
                                    ProjectileID.TruffleSpore,
                                    fDamage,
                                    1.25f,
                                    Player.whoAmI
                                );

                                var spore2 = Projectile.NewProjectileDirect(
                                    source,
                                    spawnPos,
                                    new Vector2((float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f)),
                                    ProjectileID.TruffleSpore,
                                    fDamage,
                                    1.25f,
                                    Player.whoAmI
                                );

                                // 设置时间
                                spore1.timeLeft = 300;
                                spore2.timeLeft = 300;
                            }
                        }
                    }
                }
            }
            
        }
        #endregion
        #region PreUpdate

        #endregion
        public Item FindAccessory(int itemID)
        {
            for (int i = 0; i < 10; i++)
            {
                if (Player.armor[i].type == itemID)
                    return Player.armor[i];
            }
            return new Item();
        }
        public override void PostUpdateRunSpeeds()
        {
            CalamityPlayer modPlayer = Player.Calamity();
            if (LoreDestroyer || PanelsLoreDestroyer)
            {
                Player.runAcceleration *= 0.95f;
            }
            if (LoreTwins || PanelsLoreTwins)
            {
                if (Player.statLife < (int)(Player.statLifeMax2 * 0.5))
                    Player.runAcceleration *= 0.95f;
            }
            if (LorePrime || PanelsLoreTwins)
            {
                Player.runAcceleration *= 0.95f;
            }

            if (!Player.mount.Active)
            {
                _ = 1f +
                    (AuricSilvaFakeDeath ? 0.05f : 0f);
                _ = 1f +
                    (AuricSilvaFakeDeath ? 0.05f : 0f);
            }

            #region DashEffects

            if (!string.IsNullOrEmpty(DeferredDashID))
            {
                CIDashID = DeferredDashID;
                DeferredDashID = string.Empty;
            }

            if (Player.pulley && HasCustomDash)
            {
                ModDashMovement();
            }

            else if (Player.grappling[0] == -1 && !Player.tongued)
            {
                ModHorizontalMovement();

                //下面这两行代码会导致弑神冲刺出错，弑神冲刺上下冲刺时无法正常进入cd，也没有特效，但是移除了会导致无法冲刺
                //加入ID判定后不出错了，为什么dom要这么写ID
                if (HasCustomDash && modPlayer.DashID != "Godslayer Armor")
                {
                    ModDashMovement();
                }
            }
            #endregion
        }
        public override void PostUpdateEquips()
        {
            if (AncientAeroSet)
            {
                //获取翅膀
                int wSlot = EquipLoader.GetEquipSlot(Mod, "AncientAeroArmor", EquipType.Wings);

                Player.noFallDmg = true;
                if (Player.equippedWings == null)
                {
                    Player.wingsLogic = wSlot;
                    Player.wingTime = 1;
                    Player.wingTimeMax = 1;
                    Player.equippedWings = Player.armor[1];
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (nanotechold && RaiderStacks < 150 && hit.DamageType == ModContent.GetInstance<RogueDamageClass>())
            {
                RaiderStacks++;
                if (InitNanotechSound <= 0)
                {
                    SoundEngine.PlaySound(CISoundMenu.Slasher, Player.Center);
                    InitNanotechSound = 1;
                }
            }

            if (LoreProvidence || PanelsLoreProvidence)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 420, false);
            }
            if (LoreJungleDragon)
            {
                target.AddBuff(ModContent.BuffType<Dragonfire>(), 300, false);
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Player.dead)
                return;

            CalamityPlayer modPlayer = Player.Calamity();
            if (CalamityInheritanceKeybinds.BoCLoreTeleportation.JustPressed && (BoCLoreTeleportation || PanelsBoCLoreTeleportation) && Main.myPlayer == Player.whoAmI)
            {
                if (!Player.chaosState)
                {
                    Vector2 vector31;
                    vector31.X = Main.mouseX + Main.screenPosition.X;
                    if (Player.gravDir == 1f)
                    {
                        vector31.Y = Main.mouseY + Main.screenPosition.Y - Player.height;
                    }
                    else
                    {
                        vector31.Y = Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
                    }
                    vector31.X -= Player.width / 2;
                    if (vector31.X > 50f && vector31.X < Main.maxTilesX * 16 - 50 && vector31.Y > 50f && vector31.Y < Main.maxTilesY * 16 - 50)
                    {
                        int tileX = (int)(vector31.X / 16f);
                        int tileY = (int)(vector31.Y / 16f);
                        if ((Main.tile[tileX, tileY].WallType != 87 || tileY <= Main.worldSurface || NPC.downedPlantBoss) && !Collision.SolidCollision(vector31, Player.width, Player.height))
                        {
                            Player.Teleport(vector31, 1, 0);
                            NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, Player.whoAmI, vector31.X, vector31.Y, 1, 0, 0);
                            Player.AddBuff(BuffID.ChaosState, 480, true);
                            Player.AddBuff(BuffID.Confused, 150, true);
                        }
                    }
                }
            }
            if (CalamityKeybinds.ArmorSetBonusHotKey.JustPressed)
            {
                if (AuricbloodflareRangedSoul && !Player.HasCooldown(BloodflareRangedSet.ID))
                {
                    if (Player.whoAmI == Main.myPlayer)
                        Player.AddCooldown(BloodflareRangedSet.ID, 1800);

                    SoundEngine.PlaySound(BloodflareHeadRanged.ActivationSound, Player.Center);
                    for (int d = 0; d < 64; d++)
                    {
                        int dust = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 16f), Player.width, Player.height - 16, (int)CalamityDusts.Necroplasm, 0f, 0f, 0, default, 1f);
                        Main.dust[dust].velocity *= 3f;
                        Main.dust[dust].scale *= 1.15f;
                    }
                    int dustAmt = 36;
                    for (int d = 0; d < dustAmt; d++)
                    {
                        Vector2 source = Vector2.Normalize(Player.velocity) * new Vector2(Player.width / 2f, Player.height) * 0.75f;
                        source = source.RotatedBy((double)((d - (dustAmt / 2 - 1)) * MathHelper.TwoPi / dustAmt), default) + Player.Center;
                        Vector2 dustVel = source - Player.Center;
                        int phanto = Dust.NewDust(source + dustVel, 0, 0, (int)CalamityDusts.Necroplasm, dustVel.X * 1.5f, dustVel.Y * 1.5f, 100, default, 1.4f);
                        Main.dust[phanto].noGravity = true;
                        Main.dust[phanto].noLight = true;
                        Main.dust[phanto].velocity = dustVel;
                    }
                    float spread = 45f * 0.0174f;
                    double startAngle = Math.Atan2(Player.velocity.X, Player.velocity.Y) - spread / 2;
                    double deltaAngle = spread / 8f;
                    double offsetAngle;

                    int damage = (int)Player.GetTotalDamage<RangedDamageClass>().ApplyTo(300f);
                    damage = Player.ApplyArmorAccDamageBonusesTo(damage);

                    if (Player.whoAmI == Main.myPlayer)
                    {
                        var source = Player.GetSource_Misc("1");
                        for (int i = 0; i < 8; i++)
                        {
                            float ai1 = Main.rand.NextFloat() + 0.5f;
                            float randomSpeed = Main.rand.Next(1, 7);
                            float randomSpeed2 = Main.rand.Next(1, 7);
                            offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                            int soul = Projectile.NewProjectile(source, Player.Center.X, Player.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f) + randomSpeed, ModContent.ProjectileType<BloodflareSoulold>(), damage, 0f, Player.whoAmI, 0f, ai1);
                            if (soul.WithinBounds(Main.maxProjectiles))
                                Main.projectile[soul].DamageType = DamageClass.Generic;
                            int soul2 = Projectile.NewProjectile(source, Player.Center.X, Player.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f) + randomSpeed2, ModContent.ProjectileType<BloodflareSoulold>(), damage, 0f, Player.whoAmI, 0f, ai1);
                            if (soul2.WithinBounds(Main.maxProjectiles))
                                Main.projectile[soul2].DamageType = DamageClass.Generic;
                        }
                    }
                }
            }
            if (CalamityInheritanceKeybinds.AegisHotKey.JustPressed)
            {
                if (ElysianAegis)
                {
                    ElysianGuard = !ElysianGuard;
                }
            }
            if (CalamityInheritanceKeybinds.AstralArcanumUIHotkey.JustPressed && AstralArcanumEffect && !CalamityPlayer.areThereAnyDamnBosses)
            {
                AstralArcanumUI.Toggle();
            }
        }
        public override void Initialize()
        {
            ProjectilHitCounter = 0;
            ProjectilHitCounter2 = 0;
        }
        #region Limitations
        //这个已经在misceeffcet变为常驻效果
        // public void ForceVariousEffects()
        // {
        //     if (DoAuricSilvaCountdown > 0 && AuricSilvaFakeDeath && Player.dashDelay < 0 || CIDashDelay < 0)
        //     {
        //         if (Player.lifeRegen < 0)
        //             Player.lifeRegen = 0;
        //     }
        // }
        #endregion

        #region MeleeEffects
        public override void MeleeEffects(Item item, Rectangle hitbox)
        {
            if (ReaverMeleeBlast) //战士永恒套的近战粒子效果,注意这一效果将同样应用在召唤套装上
            {
                if (Main.rand.NextBool(3))
                {
                    Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.GreenFairy, Player.velocity.X * 0.2f + Player.direction * 3, Player.velocity.Y * 0.2f, 100, default, 0.75f);
                }
            }
            if (item.CountsAsClass<TrueMeleeDamageClass>() && SMushroom && Player.whoAmI == Main.myPlayer)
            {
                if (CalamityLists.MushroomWeaponIDs.Contains(item.type) && Player.whoAmI == Main.myPlayer)
                {
                    if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.1) ||
                        Player.itemAnimation == (int)(Player.itemAnimationMax * 0.3) ||
                        Player.itemAnimation == (int)(Player.itemAnimationMax * 0.5) ||
                        Player.itemAnimation == (int)(Player.itemAnimationMax * 0.7) ||
                        Player.itemAnimation == (int)(Player.itemAnimationMax * 0.9))
                    {
                        float yVel = 0f;
                        float xVel = 0f;
                        float yOffset = 0f;
                        float xOffset = 0f;
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.9))
                        {
                            yVel = -7f;
                        }
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.7))
                        {
                            yVel = -6f;
                            xVel = 2f;
                        }
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.5))
                        {
                            yVel = -4f;
                            xVel = 4f;
                        }
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.3))
                        {
                            yVel = -2f;
                            xVel = 6f;
                        }
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.1))
                        {
                            xVel = 7f;
                        }
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.7))
                        {
                            xOffset = 26f;
                        }
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.3))
                        {
                            xOffset -= 4f;
                            yOffset -= 20f;
                        }
                        if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.1))
                        {
                            yOffset += 6f;
                        }
                        if (Player.direction == -1)
                        {
                            if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.9))
                            {
                                xOffset -= 8f;
                            }
                            if (Player.itemAnimation == (int)(Player.itemAnimationMax * 0.7))
                            {
                                xOffset -= 6f;
                            }
                        }
                        yVel *= 1.5f;
                        xVel *= 1.5f;
                        xOffset *= Player.direction;
                        yOffset *= Player.gravDir;
                        Projectile.NewProjectile(Player.GetSource_FromThis(), hitbox.X + hitbox.Width / 2 + xOffset, hitbox.Y + hitbox.Height / 2 + yOffset, Player.direction * xVel, yVel * Player.gravDir, ProjectileID.Mushroom, 0, 0f, Player.whoAmI);
                    }
                }
            }

        }

        #endregion

        #region Frame Effects
        public override void FrameEffects()
        {
            if (Player.body == EquipLoader.GetEquipSlot(Mod, "AuricTeslaBodyArmorold", EquipType.Body))
                Player.back = (sbyte)EquipLoader.GetEquipSlot(Mod, "AuricTeslaBodyArmorold", EquipType.Back);
            if (Player.body == EquipLoader.GetEquipSlot(Mod, "YharimAuricTeslaBodyArmor", EquipType.Body))
                Player.back = (sbyte)EquipLoader.GetEquipSlot(Mod, "YharimAuricTeslaBodyArmor", EquipType.Back);
        }
        #endregion

        #region 盗贼潜伏
        #endregion

    }
}