using CalamityInheritance.Utilities;
using CalamityInheritance.World;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
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
            CalamityPlayer calPlayer = Player.Calamity();
            // 神殇
            if (CIWorld.Defiled)
            {
                // 禁用翅膀
                Player.wingTime = 0;
                Player.wingTimeMax = 0;
                Player.CIMod().EmpressBooster = false;
                calPlayer.infiniteFlight = false;
            }
        }
        public void Malice()
        {
            CalamityPlayer calPlayer = Player.Calamity();
            CalamityInheritancePlayer cIPlayer = Player.CIMod();
            if (CIWorld.Malice)
            {
                if (Player.whoAmI == Main.myPlayer)
                {
                    #region 免疫判定
                    bool immunityAll = cIPlayer.RoDPaladianShieldActive || cIPlayer.TheAbsorberOld || calPlayer.WearingPostMLSummonerSet;

                    bool immunityToHotAndCold = Player.magmaStone || Player.frostArmor || calPlayer.fBarrier ||
                        calPlayer.frostFlare || calPlayer.rampartOfDeities || calPlayer.cryogenSoul || calPlayer.snowman || calPlayer.blazingCore ||
                        calPlayer.permafrostsConcoction || calPlayer.profanedCrystalBuffs;

                    bool immunityToCold = Player.HasBuff(BuffID.Campfire) || Player.resistCold || calPlayer.eskimoSet ||
                        Player.buffImmune[BuffID.Frozen] || calPlayer.aAmpoule || Player.HasBuff(BuffID.Inferno) ||
                        immunityToHotAndCold || calPlayer.externalColdImmunity;

                    bool immunityToHot = Player.lavaImmune || Player.lavaRose || Player.lavaMax > 0 || 
                        immunityToHotAndCold || calPlayer.externalHeatImmunity || Player.buffImmune[BuffID.OnFire] || Player.buffImmune[BuffID.OnFire3];
                    #endregion
                    #region 洞穴黑暗
                    if (Player.ZoneRockLayerHeight && !calPlayer.ZoneAbyss && !Player.ZoneUnderworldHeight)
                    {
                        float WorldRotio = (Player.Center.Y - 200) / Main.bottomWorld;
                        if (WorldRotio > 0.15f)
                            WorldRotio -= 0.15f;
                        calPlayer.caveDarkness = WorldRotio;
                    }
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
                    if (Player.InSpace())
                    {
                        if (Main.dayTime)
                        {
                            if (!immunityToHot && !immunityAll)
                                Player.AddBuff(BuffID.Burning, 2, false);
                        }
                        else
                        {
                            if (!immunityToCold && !immunityAll)
                                Player.AddBuff(BuffID.Frostburn, 2, false);
                        }
                    }
                    #endregion
                    #region 丛林
                    // 丛林水中流血
                    if (Player.ZoneJungle && Player.wet && !Player.lavaWet && !Player.honeyWet)
                    {
                        if (Player.IsUnderwater())
                            Player.AddBuff(BuffID.Bleeding, 300, false);
                    }
                    #endregion
                }
            }
        }
        public void SPEnvironment()
        {
            // Ice shards, lightning and sharknadoes
            bool nearPillar = Player.PillarZone();
            if (Player.ZoneOverworldHeight && !CalamityPlayer.areThereAnyDamnBosses && Main.invasionType == 0 &&
                NPC.MoonLordCountdown == 0 && !Player.InSpace() && !DD2Event.Ongoing && !nearPillar)
            {
                Vector2 sharknadoSpawnPoint = new Vector2(Player.Center.X - (float)Main.rand.Next(300, 701), Player.Center.Y - (float)Main.rand.Next(700, 801));
                if (point.X > Main.maxTilesX / 2)
                    sharknadoSpawnPoint.X = Player.Center.X + (float)Main.rand.Next(300, 701);

                if (Main.raining)
                {
                    float frequencyMult = (1f - Main.cloudAlpha) * CalamityConfig.Instance.DeathWeatherMultiplier; // 3 to 0.055

                    Vector2 spawnPoint = new Vector2(Player.Center.X + (float)Main.rand.Next(-1000, 1001), Player.Center.Y - (float)Main.rand.Next(700, 801));
                    Tile tileSafely = Framing.GetTileSafely((int)(spawnPoint.X / 16f), (int)(spawnPoint.Y / 16f));

                    if (Player.ZoneSnow)
                    {
                        if (!tileSafely.active())
                        {
                            int divisor = (int)((Main.hardMode ? 50f : 60f) * frequencyMult);
                            float windVelocity = (float)Math.Sqrt((double)Math.Abs(Main.windSpeedCurrent)) * (float)Math.Sign(Main.windSpeedCurrent) * (Main.cloudAlpha + 0.5f) * 25f + Main.rand.NextFloat() * 0.2f - 0.1f;
                            Vector2 velocity = new Vector2(windVelocity * 0.2f, 3f * Main.rand.NextFloat());

                            if (Player.miscCounter % divisor == 0 && Main.rand.NextBool(3))
                                Projectile.NewProjectile(spawnPoint.X, spawnPoint.Y, velocity.X, velocity.Y, ModContent.ProjectileType<IceRain>(), 20, 0f, Player.whoAmI, 2f, 0f);
                        }
                    }
                    else
                    {
                        if (player.ZoneBeach && !modPlayer.ZoneSulphur)
                        {
                            int randomFrequency = (int)(50f * frequencyMult);
                            if (player.miscCounter == 280 && Main.rand.NextBool(randomFrequency) && player.ownedProjectileCounts[ProjectileID.Cthulunado] < 1)
                            {
                                Main.PlaySound(SoundID.NPCDeath19, (int)sharknadoSpawnPoint.X, (int)sharknadoSpawnPoint.Y);
                                int num331 = (int)(sharknadoSpawnPoint.Y / 16f);
                                int num332 = (int)(sharknadoSpawnPoint.X / 16f);
                                int num333 = 100;
                                if (num332 < 10)
                                    num332 = 10;
                                if (num332 > Main.maxTilesX - 10)
                                    num332 = Main.maxTilesX - 10;
                                if (num331 < 10)
                                    num331 = 10;
                                if (num331 > Main.maxTilesY - num333 - 10)
                                    num331 = Main.maxTilesY - num333 - 10;

                                int spawnAreaY = Main.maxTilesY - num331;
                                for (int num334 = num331; num334 < num331 + spawnAreaY; num334++)
                                {
                                    Tile tile = Main.tile[num332, num334];
                                    if ((tile.active() && Main.tileSolid[(int)tile.type]) || tile.liquid >= 200)
                                    {
                                        num331 = num334;
                                        break;
                                    }
                                }

                                int num336 = Projectile.NewProjectile((float)(num332 * 16 + 8), (float)(num331 * 16 - 24), 0f, 0f, ProjectileID.Cthulunado, 50, 4f, player.whoAmI, 16f, 24f);
                                Main.projectile[num336].netUpdate = true;
                            }
                        }

                    }
                    int randomFrequency2 = (int)(20f * frequencyMult);
                    if (CalamityWorld.rainingAcid && player.Calamity().ZoneSulphur)
                        randomFrequency2 = (int)(randomFrequency2 * 3.75);
                    if (player.miscCounter % (Main.hardMode ? 90 : 120) == 0 && Main.rand.NextBool(randomFrequency2))
                    {
                        if (!tileSafely.active())
                        {
                            float randomVelocity = Main.rand.NextFloat() - 0.5f;
                            Vector2 fireTo = new Vector2(spawnPoint.X + 100f * randomVelocity, spawnPoint.Y + 900f);
                            Vector2 ai0 = fireTo - spawnPoint;
                            Vector2 velocity = Vector2.Normalize(ai0) * 12f;
                            Projectile.NewProjectile(spawnPoint.X, spawnPoint.Y, 0f, velocity.Y, ModContent.ProjectileType<LightningMark>(), 0, 0f, player.whoAmI, 0f, 0f);
                        }
                    }
                }
            }
            else
            {
                if (player.ZoneBeach && !modPlayer.ZoneSulphur)
                {
                    if (player.miscCounter == 280 && Main.rand.NextBool(10) && player.ownedProjectileCounts[ProjectileID.Sharknado] < 1)
                    {
                        Main.PlaySound(SoundID.NPCDeath19, (int)sharknadoSpawnPoint.X, (int)sharknadoSpawnPoint.Y);
                        int num331 = (int)(sharknadoSpawnPoint.Y / 16f);
                        int num332 = (int)(sharknadoSpawnPoint.X / 16f);
                        int num333 = 100;
                        if (num332 < 10)
                            num332 = 10;
                        if (num332 > Main.maxTilesX - 10)
                            num332 = Main.maxTilesX - 10;
                        if (num331 < 10)
                            num331 = 10;
                        if (num331 > Main.maxTilesY - num333 - 10)
                            num331 = Main.maxTilesY - num333 - 10;

                        int spawnAreaY = Main.maxTilesY - num331;
                        for (int num334 = num331; num334 < num331 + spawnAreaY; num334++)
                        {
                            Tile tile = Main.tile[num332, num334];
                            if ((tile.active() && Main.tileSolid[(int)tile.type]) || tile.liquid >= 200)
                            {
                                num331 = num334;
                                break;
                            }
                        }

                        int num336 = Projectile.NewProjectile((float)(num332 * 16 + 8), (float)(num331 * 16 - 24), 0.01f, 0f, ProjectileID.Sharknado, 25, 4f, player.whoAmI, 16f, 15f);
                        Main.projectile[num336].netUpdate = true;
                    }
                }
            }
        }
    }
}
