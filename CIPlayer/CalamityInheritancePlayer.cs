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
using CalamityMod.Projectiles.Summon;


namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public static readonly SoundStyle AbsorberHit = new("CalamityMod/Sounds/Custom/AbilitySounds/SilvaActivation") { Volume = 0.7f };
        #region CIRogue
        public float ForceStealthConsumption = 1f; //玩家潜伏消耗乘算
        public float ForceStealthDamageMultiple = 1f;  //强制玩家进入潜伏值乘算
        #endregion
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
        public bool SolarShieldEndurence = false; //日耀盾免伤计算
        #endregion
        #region 武器效果
        public float AnimusDamage = 1f;
        public bool BuffPolarisBoost = false;
        
        public bool photovisceratorCrystal = false;
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
        public int CITotalEnergyShielding => CISpongeShieldDurability;
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
        #endregion
        public bool YharonFlightBooster = false;
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
            ForceHammerStealth = false;
            CIDashID = string.Empty;
            ElysianAegis = false;
            #region Summon
            MagicHatOld = false;
            MidnnightSunBuff = false;
            ReaverSummonerOrb = false;
            cosmicEnergy = false;
            #endregion
            
        }
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
            AnimusDamage = 1f;
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
        }
        #endregion
        #region Post Hurt
        public override void PostHurt(Player.HurtInfo hurtInfo)
        {
            Player.Calamity().GemTechState.PlayerOnHitEffects(hurtInfo.Damage);

            bool hardMode = Main.hardMode;
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
            if (LoreDestroyer)
            {
                Player.runAcceleration *= 0.95f;
            }
            if (LoreTwins)
            {
                if (Player.statLife < (int)(Player.statLifeMax2 * 0.5))
                    Player.runAcceleration *= 0.95f;
            }
            if (LorePrime)
            {
                Player.runAcceleration *= 0.95f;
            }

            if (!Player.mount.Active)
            {
                _ = 1f +
                    (AuricSilvaSet ? 0.05f : 0f);
                _ = 1f +
                    (AuricSilvaSet ? 0.05f : 0f);
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (nanotechold && RaiderStacks < 150)
            {
                RaiderStacks++;
            }

            if (LoreProvidence)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 420, false);
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Player.dead)
                return;

            CalamityPlayer modPlayer = Player.Calamity();
            if (CalamityInheritanceKeybinds.BoCLoreTeleportation.JustPressed && BoCLoreTeleportation == true && Main.myPlayer == Player.whoAmI)
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
        public void ForceVariousEffects()
        {
            if (auricsilvaCountdown > 0 && AuricGetSilvaEffect && AuricSilvaSet && Player.dashDelay < 0 || CIDashDelay < 0)
            {
                if (Player.lifeRegen < 0)
                    Player.lifeRegen = 0;
            }
        }
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
        }

        #endregion

        #region Frame Effects
        public override void FrameEffects()
        {
            if (Player.body == EquipLoader.GetEquipSlot(Mod, "AncientAuricTeslaBodyArmor", EquipType.Body))
            {
                Player.back = (sbyte)EquipLoader.GetEquipSlot(Mod, "AncientAuricTeslaBodyArmor", EquipType.Back);
            }
            if (Player.body == EquipLoader.GetEquipSlot(Mod, "AuricTeslaBodyArmorold", EquipType.Body))
            {
                Player.back = (sbyte)EquipLoader.GetEquipSlot(Mod, "AuricTeslaBodyArmorold", EquipType.Back);
            }
            if (Player.body == EquipLoader.GetEquipSlot(Mod, "YharimAuricTeslaBodyArmor", EquipType.Body))
            {
                Player.back = (sbyte)EquipLoader.GetEquipSlot(Mod, "YharimAuricTeslaBodyArmor", EquipType.Back);
            }
        }
        #endregion

        #region 盗贼潜伏
        /// <summary>
        /// 强制玩家使用本模组的潜伏判定进行潜伏攻击,
        //  这一操作应当与原灾的SteathStrikeAvalible进行或操作
        /// </summary>
        /// <returns></returns>
        public bool ForceStealthStrike()
        {
            CalamityPlayer calPlayer = Player.Calamity();
            if(calPlayer.rogueStealthMax <= 0f && !calPlayer.wearingRogueArmor) //玩家连潜伏值, 以及连盗贼盔甲都没穿, 直接取否返回
                return false;
            float stealthConsumeMultipler = 0.25f;
            #region 将原灾的降潜伏饰品单独加入进来计算
            if(calPlayer.stealthStrike85Cost)   stealthConsumeMultipler *= 0.85f;
            if(calPlayer.stealthStrike75Cost)   stealthConsumeMultipler *= 0.75f;
            if(calPlayer.stealthStrikeHalfCost) stealthConsumeMultipler *= 0.50f;
            //附:假定玩家同时佩戴了三个不同的潜伏饰品, 那么这一乘算系数将会变成0.15f
            //即不经过任何处理的前提下, 只需要大于15%最大潜伏值即可触发本模组的潜伏攻击
            //让原灾物品触发潜伏我并不知道有什么办法, 只能先这样待定
            #endregion
            stealthConsumeMultipler *= ForceStealthConsumption; //与本模组的潜伏消耗量再乘算
            if(stealthConsumeMultipler < 0.1f) //触发潜伏攻击的最低条件是潜伏值的10%
                stealthConsumeMultipler = 0.1f;
            return calPlayer.rogueStealth >= calPlayer.rogueStealthMax * stealthConsumeMultipler; 
        }
        #endregion

        public void ForceStealthOnUseConsume()
        {
            CalamityPlayer calPlayer = Player.Calamity();
            calPlayer.stealthStrikeThisFrame = true;
            calPlayer.stealthAcceleration = 1f;
            /*
            *他这个是先把潜伏*100f(相当于转成一个int整数后再取倒数来获得潜伏的消耗比例) -> (1)
            *而后, 把你潜伏最大值乘以这个倒数. (??????) -> (2)
            *然后你当前最大的潜伏值再减去损失的潜伏值 -> (3)
            *按理来说, 以潜伏值1.20f为例, 他会先进行第一步变成1/120, 然后再用1.20f去乘以这个1/120得到实际消耗值后, 再用最大的潜伏值-实际消耗值便能损失的潜伏值
            *WHAT???
            *附:翻译成自然语言的话:
            *(1)1.20 * 100 = 120后取倒数变成1/120
            *(2)120/100 * 1/120 得到 1/100(也就是1.20f * 1/120 = 1/100) 
            *(3)最大潜伏值减去这个值, 按理来说这样计算之后应该是1.20f - 0.01f = 1.19 便是减少潜伏值的数量
            *WHAT?
            *Comment: (1)和(2)完全没必要, 你不想让他低于1潜伏值你直接tm的取消耗的潜伏值=最大潜伏值-0.01f不就完事了,  为什么还要tm取倒数又大搞计算?
            */
            // float getReduceRatio = calPlayer.flatStealthLossReduction / (calPlayer.rogueStealthMax * 100f); //(1)
            // float leftStealth = calPlayer.rogueStealthMax * getReduceRatio; //(2）

            float minStealthPoint = 0.01f;
            float lostStealthAlt = calPlayer.rogueStealthMax - minStealthPoint;//(3)

            if(ForceStealthConsumption < 1f)
            calPlayer.rogueStealth -= ForceStealthConsumption * lostStealthAlt;

            else calPlayer.rogueStealth -= lostStealthAlt;
        }
    }
}