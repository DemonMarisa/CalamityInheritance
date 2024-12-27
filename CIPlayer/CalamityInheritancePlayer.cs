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
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Buffs.StatDebuffs;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {

        public bool ElementalQuiver;
        public bool CoreOfTheBloodGod;
        public bool fleshTotemold;
        public double contactDamageReduction = 0D;
        public bool FungalCarapace = false;
        public bool ODsulphurskin = false;
        public int ProjectilHitCounter;
        #region dash
        public int dashTimeMod;
        public bool HasReducedDashFirstFrame = false;
        public bool HasIncreasedDashFirstFrame = false;
        public int CIDashDelay;
        #endregion
        #region Lore
        public bool kingSlimeLore = false;//
        public bool desertScourgeLore = false;//
        public bool crabulonLore = false;//
        public bool eaterOfWorldsLore = false;//
        public bool BoCLoreTeleportation = false;//
        public bool hiveMindLore = false;//
        public bool perforatorLore = false;//
        public bool queenBeeLore = false;//
        public bool skeletronLore = false;//
        public bool wallOfFleshLore = false;//
        public bool twinsLore = false;//
        public bool destroyerLore = false;//
        public bool aquaticScourgeLore = false;//
        public bool skeletronPrimeLore = false;//
        public bool brimstoneElementalLore = false;//
        public bool calamitasCloneLore = false;//
        public bool planteraLore = false;//
        public bool leviathanAndSirenLore = false;//
        public bool astrumAureusLore = false;//
        public bool astrumDeusLore = false;//
        public bool golemLore = false;//
        public bool plaguebringerGoliathLore = false;//
        public bool dukeFishronLore = false;//
        public bool boomerDukeLore = false;//
        public bool ravagerLore = false;//
        public bool lunaticCultistLore = false;//
        public bool moonLordLore = false;//
        public bool providenceLore = false;//
        public bool polterghastLore = false;//
        public bool DoGLore = false;//
        public bool yharonLore = false;//
        public bool SCalLore = false;//
        public bool oceanLore = false;
        public bool corruptionLore = false;//
        public bool crimsonLore = false;//
        public bool underworldLore = false;
        #endregion
        #region Buffs
        public bool armorShattering = false;
        public bool Revivify = false;
        public bool cadence = false;
        public bool draconicSurge = false;
        public bool penumbra = false;
        public bool profanedRage = false;
        public bool holyWrath = false;
        public bool tScale = false;
        public int titanBoost = 0;
        public bool triumph = false;
        public bool yPower = false;
        public bool invincible = false;
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

        #endregion
        #region Set Bonuses
        public bool GodSlayerReborn = false;
        public bool GodSlayerDMGprotect = false;
        public bool godSlayerReflect = false;
        public bool godSlayerMagic = false;
        public bool hasFiredThisFrame = false;
        public bool godSlayerRangedold = false;
        public bool godSlayerSummonold = false;
        #endregion
        #region ResetEffects
        public override void ResetEffects()
        {
            int percentMaxLifeIncrease = 0;

            ElementalQuiver = false;
            fleshTotemold = false;
            CoreOfTheBloodGod = false;

            if (CoreOfTheBloodGod)
                percentMaxLifeIncrease += 25;

            contactDamageReduction = 0D;

            if (!CIsponge)
                CISpongeShieldDurability = 0;

            CIsponge = false;
            CIspongeShieldVisible = false;
            FungalCarapace = false;
            ODsulphurskin = false;
            #region Lore
            kingSlimeLore = false;
            desertScourgeLore = false;
            crabulonLore = false;
            BoCLoreTeleportation = false;
            eaterOfWorldsLore = false;
            hiveMindLore = false;
            perforatorLore = false;
            queenBeeLore = false;
            skeletronLore = false;
            wallOfFleshLore = false;
            twinsLore = false;
            destroyerLore = false;
            aquaticScourgeLore = false;
            skeletronPrimeLore = false;
            brimstoneElementalLore = false;
            calamitasCloneLore = false;
            planteraLore = false;
            leviathanAndSirenLore = false;
            astrumAureusLore = false;
            astrumDeusLore = false;
            golemLore = false;
            plaguebringerGoliathLore = false;
            dukeFishronLore = false;
            boomerDukeLore = false;
            ravagerLore = false;
            lunaticCultistLore = false;
            moonLordLore = false;
            providenceLore = false;
            polterghastLore = false;
            DoGLore = false;
            yharonLore = false;
            SCalLore = false;
            oceanLore = false;
            corruptionLore = false;
            crimsonLore = false;
            underworldLore = false;
            #endregion
            #region Buffs
            armorShattering = false;
            Revivify = false;
            cadence = false;
            draconicSurge = false;
            penumbra = false;
            profanedRage = false;
            holyWrath = false;
            tScale = false;
            triumph = false;
            yPower = false;
            invincible = false;
            #endregion
            #region Set Bonuses
            GodSlayerReborn = false;
            GodSlayerDMGprotect = false;
            godSlayerReflect = false;
            godSlayerMagic = false;
            hasFiredThisFrame = false;
            godSlayerRangedold = false;
            godSlayerSummonold = false;
            #endregion
            CIDashID = string.Empty;
        }
        #endregion

        #region UpdateDead
        public override void UpdateDead()
        {
            armorShattering = false;
            Revivify = false;
            cadence = false;
            draconicSurge = false;
            penumbra = false;
            profanedRage = false;
            holyWrath = false;
            tScale = false;
            titanBoost = 0;
            triumph = false;
            yPower = false;
            invincible = false;

            #region Set Bonuses
            GodSlayerDMGprotect = false;
            godSlayerReflect = false;
            godSlayerMagic = false;
            hasFiredThisFrame = false;
            godSlayerRangedold = false;
            godSlayerSummonold = false;
            #endregion
        }
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
            Player.Calamity().GemTechState.PlayerOnHitEffects((int)hurtInfo.Damage);
            bool hardMode = Main.hardMode;
            if (Player.whoAmI == Main.myPlayer)
            {
                if (FungalCarapace)
                {
                    CalamityPlayer modPlayer = Player.Calamity();
                    var source = Player.GetSource_Accessory(modPlayer.FindAccessory(ModContent.ItemType<FungalCarapace>()));
                    if (hurtInfo.Damage > 0)
                    {
                        // 播放声音
                        SoundEngine.PlaySound(SoundID.NPCHit45, Player.position);

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
            if (destroyerLore)
            {
                Player.runAcceleration *= 0.95f;
            }
            if (twinsLore)
            {
                if (Player.statLife < (int)(Player.statLifeMax2 * 0.5))
                    Player.runAcceleration *= 0.95f;
            }
            if (skeletronPrimeLore)
            {
                Player.runAcceleration *= 0.95f;
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

            if (providenceLore)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 420, false);
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (CalamityInheritanceKeybinds.BoCLoreTeleportation.JustPressed && BoCLoreTeleportation == true && Main.myPlayer == Player.whoAmI)
            {
                if (!Player.chaosState)
                {
                    Vector2 vector31;
                    vector31.X = (float)Main.mouseX + Main.screenPosition.X;
                    if (Player.gravDir == 1f)
                    {
                        vector31.Y = (float)Main.mouseY + Main.screenPosition.Y - (float)Player.height;
                    }
                    else
                    {
                        vector31.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
                    }
                    vector31.X -= (float)(Player.width / 2);
                    if (vector31.X > 50f && vector31.X < (float)(Main.maxTilesX * 16 - 50) && vector31.Y > 50f && vector31.Y < (float)(Main.maxTilesY * 16 - 50))
                    {
                        int num275 = (int)(vector31.X / 16f);
                        int num276 = (int)(vector31.Y / 16f);
                        if ((Main.tile[num275, num276].WallType != 87 || (double)num276 <= Main.worldSurface || NPC.downedPlantBoss) && !Collision.SolidCollision(vector31, Player.width, Player.height))
                        {
                            Player.Teleport(vector31, 1, 0);
                            NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, (float)Player.whoAmI, vector31.X, vector31.Y, 1, 0, 0);
                            Player.AddBuff(BuffID.ChaosState, 480, true);
                            Player.AddBuff(BuffID.Confused, 150, true);
                        }
                    }
                }
            }
        }
        public override void Initialize()
        {
            ProjectilHitCounter = 0;
        }
    }
}
