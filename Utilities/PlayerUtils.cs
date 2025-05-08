using CalamityMod.CalPlayer;
using Terraria;
using static Terraria.Player;
using CalamityMod;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Projectiles;
using System.Reflection;
using System;
using Terraria.ModLoader;
using CalamityMod.Balancing;
using Microsoft.Xna.Framework.Graphics;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.World;
using Terraria.Audio;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Utilities
{
    public static partial class CIFunction
    {
        #region Cooldowns
        // 移除冷却
        public static void RemoveCooldown(this Player player, string id)
        {
            CalamityPlayer calamityPlayer = player.Calamity();
            CalamityInheritancePlayer inheritancePlayer = player.CIMod();

            RemoveCooldownFromModPlayer(calamityPlayer, id);
            RemoveCooldownFromModPlayer(inheritancePlayer, id);
        }
        private static void RemoveCooldownFromModPlayer(dynamic player, string id)
        {
            player.cooldowns.Remove(id);
        }
        #endregion
        public static CalamityInheritanceGlobalProjectile CalamityInheritance(this Projectile proj)
        {
            return proj.GetGlobalProjectile<CalamityInheritanceGlobalProjectile>();
        }

        /// <summary>
        /// Gets the total amount of extra immunity frames from a hit granted by various Calamity effects.
        /// </summary>
        /// <param name="player">The player whose extra immunity frames are being computed.</param>
        /// <returns>The amount of extra immunity frames to grant.</returns>
        public static int GetExtraHitIFrames(this Player player, HurtInfo hurtInfo)
        {
            CalamityInheritancePlayer modPlayer = player.CIMod();
            int extraIFrames = 0;
            // Ozzatron 20FEB2024: Moved extra iframes from Seraph Tracers to Rampart of Deities to counteract its loss of Charm of Myths
            // This stacks with the above Deific Amulet effect
            if (modPlayer.AuricTracersFrames && hurtInfo.Damage > 200)
                extraIFrames += 30;
            if (modPlayer.RoDPaladianShieldActive)
                extraIFrames += 30;
            if (modPlayer.YharimsInsignia)
                extraIFrames += 40;
            return extraIFrames;
        }
        /// <summary>
        /// 不是，哥们，player.calamity().stealthstrikeavailble()真的很多字
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool CheckStealth(this Player p) => p.Calamity().StealthStrikeAvailable();
        /// <summary>
        /// 不是哥们，这个也很多字，真的
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool CheckExoLore(this Player p) => p.CIMod().LoreExo || p.CIMod().PanelsLoreExo;

        /// <summary>
        /// 杀了玩家
        /// </summary>
        public static void KillPlayer(Player Player)
        {
            var source = Player.GetSource_Death();
            Player.lastDeathPostion = Player.Center;
            Player.lastDeathTime = DateTime.Now;
            Player.showLastDeath = true;
            int coinsOwned = (int)Utils.CoinsCount(out bool flag, Player.inventory, new int[0]);
            if (Main.myPlayer == Player.whoAmI)
            {
                Player.lostCoins = coinsOwned;
                Player.lostCoinString = Main.ValueToCoins(Player.lostCoins);
            }
            if (Main.myPlayer == Player.whoAmI)
            {
                Main.mapFullscreen = false;
            }
            if (Main.myPlayer == Player.whoAmI)
            {
                Player.trashItem.SetDefaults(0, false);
                if (Player.difficulty == PlayerDifficultyID.SoftCore || Player.difficulty == PlayerDifficultyID.Creative)
                {
                    for (int i = 0; i < 59; i++)
                    {
                        if (Player.inventory[i].stack > 0 && ((Player.inventory[i].type >= ItemID.LargeAmethyst && Player.inventory[i].type <= ItemID.LargeDiamond) || Player.inventory[i].type == ItemID.LargeAmber))
                        {
                            int droppedLargeGem = Item.NewItem(source, (int)Player.position.X, (int)Player.position.Y, Player.width, Player.height, Player.inventory[i].type, 1, false, 0, false, false);
                            Main.item[droppedLargeGem].netDefaults(Player.inventory[i].netID);
                            Main.item[droppedLargeGem].Prefix(Player.inventory[i].prefix);
                            Main.item[droppedLargeGem].stack = Player.inventory[i].stack;
                            Main.item[droppedLargeGem].velocity.Y = Main.rand.Next(-20, 1) * 0.2f;
                            Main.item[droppedLargeGem].velocity.X = Main.rand.Next(-20, 21) * 0.2f;
                            Main.item[droppedLargeGem].noGrabDelay = 100;
                            Main.item[droppedLargeGem].favorited = false;
                            Main.item[droppedLargeGem].newAndShiny = false;
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                            {
                                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, droppedLargeGem, 0f, 0f, 0f, 0, 0, 0);
                            }
                            Player.inventory[i].SetDefaults(0, false);
                        }
                    }
                }
                else if (Player.difficulty == PlayerDifficultyID.MediumCore)
                {
                    Player.DropItems();
                }
                else if (Player.difficulty == PlayerDifficultyID.Hardcore)
                {
                    Player.DropItems();
                    Player.KillMeForGood();
                }
            }

            if (CIWorld.IronHeart)
                SoundEngine.PlaySound(CISoundMenu.IronHeartDeath, Player.Center);
            else
                SoundEngine.PlaySound(SoundID.PlayerKilled, Player.Center);
            Player.headVelocity.Y = Main.rand.Next(-40, -10) * 0.1f;
            Player.bodyVelocity.Y = Main.rand.Next(-40, -10) * 0.1f;
            Player.legVelocity.Y = Main.rand.Next(-40, -10) * 0.1f;
            Player.headVelocity.X = Main.rand.Next(-20, 21) * 0.1f + 2 * 0;
            Player.bodyVelocity.X = Main.rand.Next(-20, 21) * 0.1f + 2 * 0;
            Player.legVelocity.X = Main.rand.Next(-20, 21) * 0.1f + 2 * 0;
            if (Player.stoned)
            {
                Player.headPosition = Vector2.Zero;
                Player.bodyPosition = Vector2.Zero;
                Player.legPosition = Vector2.Zero;
            }
            for (int j = 0; j < 100; j++)
            {
                Dust.NewDust(Player.position, Player.width, Player.height, DustID.LifeDrain, 2 * 0, -2f, 0, default, 1f);
            }
            Player.mount.Dismount(Player);
            Player.dead = true;
            Player.respawnTimer = 600;
            if (Main.expertMode)
            {
                Player.respawnTimer = (int)(Player.respawnTimer * 1.5);
            }
            Player.immuneAlpha = 0;
            Player.palladiumRegen = false;
            Player.iceBarrier = false;
            Player.crystalLeaf = false;

            if (Player.whoAmI == Main.myPlayer)
            {
                try
                {
                    WorldGen.saveToonWhilePlaying();
                }
                catch
                {
                }
            }
        }
    }
}
