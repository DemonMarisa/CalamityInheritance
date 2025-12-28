using CalamityInheritance.Buffs.StatDebuffs;
using CalamityInheritance.Content.Projectiles.Environment;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityInheritance.World;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Events;
using CalamityMod.Projectiles.Boss;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public bool MLG = false; // 恶魔纹章的刷怪增幅
        public void Defiled()
        {
            CIWorld modWorld = ModContent.GetInstance<CIWorld>();
            CalamityPlayer calPlayer = Player.Calamity();
            // 神殇
            if (modWorld.Defiled)
            {
                // 禁用翅膀
                Player.wingTime = 0;
                Player.wingTimeMax = 0;
                Player.rocketTime = 0;

                Player.CIMod().EmpressBooster = false;
                calPlayer.infiniteFlight = false;
            }
        }
        #region 恶意
        public void Malice()
        {
            CalamityPlayer calPlayer = Player.Calamity();
            CalamityInheritancePlayer cIPlayer = Player.CIMod();
            CIWorld modWorld = ModContent.GetInstance<CIWorld>();
            if (!modWorld.Malice && !CIServerConfig.Instance.WeatherChange)
                return;
            if (CalamityUtils.AnyBossNPCS())
                return;

            if (Player.whoAmI == Main.myPlayer)
            {
                #region 免疫判定
                bool immunityAll = cIPlayer.RoDPaladianShieldActive || cIPlayer.TheAbsorberOld || calPlayer.WearingPostMLSummonerSet;

                bool immunityToHotAndCold = Player.magmaStone || Player.frostArmor || calPlayer.fBarrier ||
                    calPlayer.frostFlare || calPlayer.rampartOfDeities || calPlayer.cryogenSoul || calPlayer.blazingCore ||
                    calPlayer.permafrostsConcoction || calPlayer.profanedCrystalBuffs || immunityAll;

                bool immunityToCold = Player.HasBuff(BuffID.Campfire) || Player.resistCold || calPlayer.eskimoSet ||
                    Player.buffImmune[BuffID.Frozen] || calPlayer.aAmpoule || Player.HasBuff(BuffID.Inferno) ||
                    immunityToHotAndCold || calPlayer.externalColdImmunity;

                bool immunityToHot = Player.lavaImmune || Player.lavaRose || Player.lavaMax > 0 ||
                    immunityToHotAndCold || calPlayer.externalHeatImmunity || Player.buffImmune[BuffID.OnFire] || Player.buffImmune[BuffID.OnFire3];
                #endregion
                #region 洞穴黑暗
                // 在System里面
                #endregion
                #region 星辉
                // Astral effects
                if (calPlayer.ZoneAstral && !calPlayer.gravityNormalizer)
                {
                    Player.gravity *= 0.75f;
                }
                #endregion
                #region 太空      
                // 太空
                if (inSpace)
                {
                    if (Main.dayTime)
                    {
                        if (!immunityToHot)
                            Player.AddBuff(BuffID.Burning, 2, false);
                    }
                    else
                    {
                        if (!immunityToCold)
                            Player.AddBuff(BuffID.Frostburn, 2, false);
                    }
                }
                #endregion
                #region 丛林
                // 丛林水中流血
                if (Player.ZoneJungle && Player.wet && !Player.lavaWet && !Player.honeyWet)
                {
                    if (Collision.DrownCollision(Player.position, Player.width, Player.height, Player.gravDir))
                        Player.AddBuff(BuffID.Bleeding, 300, false);
                }
                #endregion
                #region 下雨
                SPEnvironment(calPlayer);
                #endregion
                Blizzard(immunityToCold, cIPlayer);
                UnderworldHot(immunityToHot, cIPlayer);
            }
        }
        #region 环境变化
        public void SPEnvironment(CalamityPlayer calPlayer)
        {
            // Ice shards, lightning and sharknadoes
            bool nearPillar = Player.CIPillarZone();
            if (Player.ZoneOverworldHeight && !CalamityPlayer.areThereAnyDamnBosses && Main.invasionType == 0 && NPC.MoonLordCountdown == 0 && !inSpace && !DD2Event.Ongoing && !nearPillar)
            {
                // 倍率：越小越频繁
                float frequencyMult = 1f - Main.cloudAlpha; // 3 to 0.055
                if (Main.raining)
                {
                    // 下雪
                    if (Player.ZoneSnow)
                        SnowProj(frequencyMult);
                    // 闪电
                    LightingProj(frequencyMult);
                    // 沙龙卷
                    if (Player.ZoneBeach && !calPlayer.ZoneSulphur)
                        BeachPlus(frequencyMult);
                }
                else if (Player.ZoneBeach && !calPlayer.ZoneSulphur)
                    Beach(frequencyMult);
            }
        }
        #region 冰雹
        public void SnowProj(float frequencyMult)
        {
            Vector2 spawnPoint = new Vector2(Player.Center.X + Main.rand.Next(-1000, 1001), Player.Center.Y - Main.rand.Next(700, 801));
            Tile tileSafely = Framing.GetTileSafely((int)(spawnPoint.X / 16f), (int)(spawnPoint.Y / 16f));

            if (!tileSafely.HasTile)
            {
                // 频率
                int divisor = (int)((Main.hardMode ? 50f : 60f) * frequencyMult);
                // 计算风的向量
                float windVelocity = MathF.Sqrt(MathF.Abs(Main.windSpeedCurrent)) * MathF.Sign(Main.windSpeedCurrent)
                    * (Main.cloudAlpha + 0.5f) * 25f + Main.rand.NextFloat() * 0.2f - 0.1f;
                // 最终向量计算
                Vector2 velocity = new(windVelocity * 0.2f, 3f * Main.rand.NextFloat());
                // 发射弹幕
                if (Player.miscCounter % divisor == 0 && Main.rand.NextBool(3))
                    Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPoint, velocity, ModContent.ProjectileType<IceRain>(), 20, 0f, Player.whoAmI, 2f, 0f);
            }
        }
        #endregion
        #region 海滩
        public void Beach(float frequencyMult)
        {
            Vector2 sharknadoSpawnPoint = new Vector2(Player.Center.X - (float)Main.rand.Next(300, 701), Player.Center.Y - (float)Main.rand.Next(700, 801));

            if (Main.rand.NextBool(2)) // 改用更现代的随机判断
                sharknadoSpawnPoint.X = Player.Center.X + Main.rand.Next(300, 701);

            if (Player.miscCounter == 280 && Main.rand.NextBool(10) && Player.ownedProjectileCounts[ProjectileID.Sharknado] < 1)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath19, sharknadoSpawnPoint);
                // 坐标计算
                Point tileCoords = sharknadoSpawnPoint.ToTileCoordinates();
                tileCoords.X = Math.Clamp(tileCoords.X, 10, Main.maxTilesX - 10);
                tileCoords.Y = Math.Clamp(tileCoords.Y, 10, Main.maxTilesY - 110);

                for (int y = tileCoords.Y; y < Main.maxTilesY - 10; y++)
                {
                    Tile tile = Main.tile[tileCoords.X, y];
                    if ((tile.HasTile && Main.tileSolid[tile.TileType]) || tile.LiquidAmount >= 200)
                    {
                        tileCoords.Y = y;
                        break;
                    }
                }
                int proj = Projectile.NewProjectile(Player.GetSource_FromThis(), tileCoords.X * 16 + 8, tileCoords.Y * 16 - 24, 0f, 0f, ProjectileID.Sharknado, 50, 4f, Player.whoAmI, 16f, 24f);
                Main.projectile[proj].netUpdate = true;
            }
        }
        #endregion
        #region 海滩Plus
        public void BeachPlus(float frequencyMult)
        {
            Vector2 sharknadoSpawnPoint = new Vector2(Player.Center.X - (float)Main.rand.Next(300, 701), Player.Center.Y - (float)Main.rand.Next(700, 801));
            int randomFrequency = (int)(50f * frequencyMult);
            if (Main.rand.NextBool(2)) // 改用更现代的随机判断
                sharknadoSpawnPoint.X = Player.Center.X + Main.rand.Next(300, 701);

            if (Player.miscCounter == 280 && Main.rand.NextBool(randomFrequency + 1) && Player.ownedProjectileCounts[ProjectileID.Cthulunado] < 1)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath19, sharknadoSpawnPoint);
                // 坐标计算
                Point tileCoords = sharknadoSpawnPoint.ToTileCoordinates();
                tileCoords.X = Math.Clamp(tileCoords.X, 10, Main.maxTilesX - 10);
                tileCoords.Y = Math.Clamp(tileCoords.Y, 10, Main.maxTilesY - 110);

                for (int y = tileCoords.Y; y < Main.maxTilesY - 10; y++)
                {
                    Tile tile = Main.tile[tileCoords.X, y];
                    if ((tile.HasTile && Main.tileSolid[tile.TileType]) || tile.LiquidAmount >= 200)
                    {
                        tileCoords.Y = y;
                        break;
                    }
                }
                int proj = Projectile.NewProjectile(Player.GetSource_FromThis(), tileCoords.X * 16 + 8, tileCoords.Y * 16 - 24, 0f, 0f, ProjectileID.Cthulunado, 50, 4f, Player.whoAmI, 16f, 24f);
                Main.projectile[proj].netUpdate = true;
            }
        }
        #endregion
        #region 闪电
        public void LightingProj(float frequencyMult)
        {
            Vector2 spawnPoint = new Vector2(Player.Center.X + Main.rand.Next(-1000, 1001), Player.Center.Y - Main.rand.Next(700, 801));
            Tile tileSafely = Framing.GetTileSafely((int)(spawnPoint.X / 16f), (int)(spawnPoint.Y / 16f));

            int randomFrequency2 = (int)(20f * frequencyMult);
            if (AcidRainEvent.AcidRainEventIsOngoing && Player.Calamity().ZoneSulphur)
                randomFrequency2 = (int)(randomFrequency2 * 3.75);

            if (Player.miscCounter % (Main.hardMode ? 90 : 120) == 0 && Main.rand.NextBool(randomFrequency2 + 1))
            {
                if (!tileSafely.HasTile)
                {
                    float randomVelocity = Main.rand.NextFloat() - 0.5f;
                    Vector2 fireTo = new Vector2(spawnPoint.X + 100f * randomVelocity, spawnPoint.Y + 900f);
                    Vector2 ai0 = fireTo - spawnPoint;
                    Vector2 velocity = Vector2.Normalize(ai0) * 12f;
                    Projectile.NewProjectile(Player.GetSource_FromThis(), spawnPoint.X, spawnPoint.Y, 0f, velocity.Y, ModContent.ProjectileType<LightningMark>(), 0, 0f, Player.whoAmI, 0f, 0f);
                }
            }
        }
        #endregion
        #endregion
        #region 暴风雪
        public void Blizzard(bool immunityToCold, CalamityInheritancePlayer cIPlayer)
        {
            if (!Player.behindBackWall && Main.raining && Player.ZoneSnow && !immunityToCold && Player.ZoneOverworldHeight)
            {
                bool affectedByColdWater = Player.wet && !Player.lavaWet && !Player.honeyWet && !Player.arcticDivingGear;

                Player.AddBuff(ModContent.BuffType<MaliceModeCold>(), 2, false);
                // 增加计时器
                cIPlayer.maliceModeBlizzardTime++;
                if (affectedByColdWater)
                    cIPlayer.maliceModeBlizzardTime++;

            }
            else if (cIPlayer.maliceModeBlizzardTime > 0)
            {
                if (cIPlayer.maliceModeBlizzardTime > 0)
                {
                    // 减少
                    cIPlayer.maliceModeBlizzardTime--;
                    // 免疫冷冻时减的更多
                    if (immunityToCold)
                        cIPlayer.maliceModeBlizzardTime--;
                }
            }

            // Cold effects
            if (cIPlayer.maliceModeBlizzardTime > 1800)
                Player.AddBuff(BuffID.Frozen, 2, false);
            if (cIPlayer.maliceModeBlizzardTime > 1980)
                cIPlayer.KillPlayer();
        }
        #endregion
        #region 地狱
        public void UnderworldHot(bool immunityToHot, CalamityInheritancePlayer cIPlayer)
        {
            // Hot timer
            if (Player.ZoneUnderworldHeight && !immunityToHot)
            {
                bool affectedByHotLava = Player.lavaWet;
                Player.AddBuff(ModContent.BuffType<MaliceModeHot>(), 2, false);
                cIPlayer.maliceModeUnderworldTime++;
                if (affectedByHotLava)
                    cIPlayer.maliceModeUnderworldTime++;
            }
            else if (cIPlayer.maliceModeUnderworldTime > 0)
            {
                cIPlayer.maliceModeUnderworldTime--;

                if (immunityToHot)
                    cIPlayer.maliceModeUnderworldTime--;
            }
            // Hot effects
            if (cIPlayer.maliceModeUnderworldTime > 360)
                Player.AddBuff(BuffID.Weak, 2, false);
            if (cIPlayer.maliceModeUnderworldTime > 720)
                Player.AddBuff(BuffID.Slow, 2, false);
            if (cIPlayer.maliceModeUnderworldTime > 1080)
                Player.AddBuff(BuffID.OnFire, 2, false);
            if (cIPlayer.maliceModeUnderworldTime > 1440)
                Player.AddBuff(BuffID.Confused, 2, false);
            if (cIPlayer.maliceModeUnderworldTime > 1800)
                Player.AddBuff(BuffID.Burning, 2, false);
        }
        #endregion
        #endregion
        #region 铁心
        public void IronHeartChange()
        {
            CIWorld modWorld = ModContent.GetInstance<CIWorld>();
            CalamityPlayer calPlayer = Player.Calamity();
            if (modWorld.IronHeart)
            {
                if(Player.lifeRegen > 0)
                {
                    calPlayer.noLifeRegen = true;
                    Player.lifeRegen = 0;
                    Player.lifeSteal = 0;
                }
            }
        }
        #region Get Heal Life
        public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
            CIWorld modWorld = ModContent.GetInstance<CIWorld>();
            if (modWorld.IronHeart)
                healValue = 0;
        }
        #endregion
        public void ModeHit(ref Player.HurtModifiers modifiers)
        {
            int damageMin = 40 + (Player.statLifeMax2 / 10);
            if (modifiers.SourceDamage.Base < damageMin)
            {
                Player.endurance = 0f;
                modifiers.SourceDamage.Base = damageMin;
            }

            if (modifiers.SourceDamage.Base <= damageMin)
                SoundEngine.PlaySound(CISoundMenu.IronHeartHurt, Player.Center);
            else
                SoundEngine.PlaySound(CISoundMenu.IronHeartBigHurt, Player.Center);
        }
        #endregion
    }
}
