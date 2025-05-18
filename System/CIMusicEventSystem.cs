using CalamityMod.NPCs;
using CalamityMod.Systems;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using CalamityInheritance.System.Configs;
using CalamityInheritance.System.DownedBoss;

namespace CalamityInheritance.System
{

    public record class MusicEventEntry(string Id, int Song, TimeSpan Length, TimeSpan IntroSilence, TimeSpan OutroSilence, Func<bool> ShouldPlay, Func<bool> Enabled);

    public class CIMusicEventSystem : ModSystem
    {
        #region Statics

        public static MusicEventEntry CurrentEvent { get; set; } = null;

        public static DateTime? TrackStart { get; set; } = null;

        // 淡出
        public static DateTime? TrackEnd { get; set; } = null;

        public static int LastPlayedEvent { get; set; } = -1;

        public static TimeSpan? OutroSilence { get; set; } = null;

        // 淡入
        public static bool NoFade { get; set; } = false;

        public static Thread EventTrackerThread { get; set; } = null;

        public static List<string> PlayedEvents { get; set; } = [];

        public static List<MusicEventEntry> EventCollection { get; set; } = [];

        // 确保玩家不会进入世界后继续播放一大堆曲子
        private static bool oldWorld { get; set; } = true;

        #endregion

        #region Events List
        public override void OnModLoad()
        {
            //223.5d为莉莉音乐包的开局音乐加残酷世界之传说的时长，进入世界后的音乐是优先播放莉莉的
            double CalamityTitleTime = CalamityInheritance.Instance.liliesmusicMod == null ? 175.5d : 233.5d;

            static void AddEntry(string eventId, string songName, TimeSpan length, Func<bool> shouldPlay, Func<bool> enabled, TimeSpan? introSilence = null, TimeSpan? outroSilence = null)
            {
                string musicPath = "CalamityInheritance/Music/" + songName;

                MusicEventEntry entry = new(eventId, MusicLoader.GetMusicSlot(musicPath), length, introSilence ?? TimeSpan.Zero, outroSilence ?? TimeSpan.Zero, shouldPlay, enabled);
                EventCollection.Add(entry);
            }

            static void CalAddEntry(string eventId, string songName, TimeSpan length, Func<bool> shouldPlay, Func<bool> enabled, TimeSpan? introSilence = null, TimeSpan? outroSilence = null)
            {
                MusicEventEntry entry = new(eventId, CalamityInheritance.Instance.GetMusicFromMusicMod(songName).Value, length, introSilence ?? TimeSpan.Zero, outroSilence ?? TimeSpan.Zero, shouldPlay, enabled);
                EventCollection.Add(entry);
            }
            //进入世界播放残酷世界之传说
            CalAddEntry("FirstEnterWorld", "CalamityTitle", TimeSpan.FromSeconds(CalamityTitleTime),
                () => true, () => CIConfig.Instance.TaleOfACruelWorld);

            CalAddEntry("CIClonoeDefeated", "Interlude1", TimeSpan.FromSeconds(214.577d),
                () => CIDownedBossSystem.DownedCalClone, () => CalamityConfig.Instance.Interlude1,
                outroSilence: TimeSpan.FromSeconds(7.5f));

            AddEntry("YharonDefeated", "Tyrant", TimeSpan.FromSeconds(110.5d),
                () => DownedBossSystem.downedYharon, () => CIConfig.Instance.Tyrant1,
                outroSilence: TimeSpan.FromSeconds(7.5f));

            AddEntry("LegacyYharonDefeated", "Tyrant", TimeSpan.FromSeconds(110.5d),
                () => CIDownedBossSystem.DownedLegacyYharonP2, () => CIConfig.Instance.Tyrant1,
                outroSilence: TimeSpan.FromSeconds(7.5f));

            AddEntry("ExoMechsDefeated", "RequiemsOfACruelWorld", TimeSpan.FromSeconds(364.032d),
                () => DownedBossSystem.downedExoMechs, () => CIConfig.Instance.Exomechs,
                outroSilence: TimeSpan.FromSeconds(7.5f));

            AddEntry(null, "CatastrophesbeforeCalamity", TimeSpan.FromSeconds(364.1d),
                () => CalamityGlobalNPC.SCalAcceptance != -1, () => CIConfig.Instance.Scal,
                outroSilence: TimeSpan.FromSeconds(7.5f));

            CalAddEntry("LegacyScalDefeated", "Interlude3", TimeSpan.FromSeconds(295.932d),
                () => CIDownedBossSystem.DownedLegacyScal, () => CIConfig.Instance.Scal,
                outroSilence: TimeSpan.FromSeconds(7.5f));

        }

        public override void Unload() => EventCollection.Clear();

        #endregion

        #region Event Handling

        public override void PostUpdateTime()
        {
            // If the player has already completed conditions to trigger certain music events, we don't
            // want to queue a bunch of tracks to play as soon as they enter the world, so instead just mark them as played
            if (oldWorld)
            {
                foreach (MusicEventEntry entry in EventCollection)
                {
                    if (entry.ShouldPlay())
                        PlayedEvents.Add(entry.Id);
                }

                oldWorld = false;
            }

            //PlayedEvents.Remove("YharonDefeated");

            // If the event has just finished, we want a little silence before fading back to normal
            if (TrackEnd is not null)
            {
                // `silence` is the time after a track ends before music goes back to normal
                TimeSpan silence = OutroSilence.Value;
                TimeSpan postTrack = DateTime.Now - TrackEnd.Value;

                // Play silence for the time specified
                if (postTrack < silence)
                {
                    int silenceSlot = MusicLoader.GetMusicSlot(Mod, "Music/Silence");
                    Main.musicBox2 = silenceSlot;
                }

                else
                {
                    LastPlayedEvent = -1;
                    TrackEnd = null;
                    OutroSilence = null;
                }

                return;
            }

            // Only check for new events to play if none is currently playing
            // This makes sure events always finish before a new one starts
            if (CurrentEvent is null)
            {
                foreach (MusicEventEntry musicEvent in EventCollection)
                {
                    // Make sure the event hasn't already played and SHOULD play
                    if (!PlayedEvents.Contains(musicEvent.Id) && musicEvent.ShouldPlay())
                    {
                        // Even if an event isn't marked as enabled, it should be counted
                        // as "played" so it isn't played when the player doesn't expect it
                        PlayedEvents.Add(musicEvent.Id);

                        // Events are always enabled on the server
                        if (Main.dedServ || musicEvent.Enabled())
                        {
                            // Assign the current event and start time
                            CurrentEvent = musicEvent;
                            TrackStart = DateTime.Now + musicEvent.IntroSilence;

                            // On clients, use a background thread to make sure the track always plays for exactly
                            // the specified length, regardless of if the game gets minimized, lags, or time becomes
                            // detangled from a consistent 60fps in any other way
                            if (!Main.dedServ)
                            {
                                EventTrackerThread = new(WatchMusicEvent);
                                EventTrackerThread.Start();
                            }

                            break;
                        }
                    }
                }
            }

            if (TrackStart is not null)
            {
                if (TrackStart > DateTime.Now)
                {
                    int silenceSlot = MusicLoader.GetMusicSlot(Mod, "Music/Silence");
                    Main.musicBox2 = silenceSlot;
                    NoFade = true;
                }

                else
                {
                    Main.musicBox2 = CurrentEvent.Song;

                    if (NoFade)
                    {
                        //Main.musicFade[CurrentEvent.Song] = 1f;
                        NoFade = true;
                    }

                    // If the event has finished playing, mark the end as now and clear the current event
                    if (DateTime.Now - TrackStart >= CurrentEvent.Length)
                    {
                        int silenceSlot = MusicLoader.GetMusicSlot(Mod, "Music/Silence");
                        Main.musicBox2 = silenceSlot;
                        Main.musicFade[CurrentEvent.Song] = 0f;

                        TrackEnd = DateTime.Now;
                        LastPlayedEvent = CurrentEvent.Song;
                        OutroSilence = CurrentEvent.OutroSilence;

                        TrackStart = null;
                        CurrentEvent = null;
                    }
                }
            }
        }

        /// <summary>
        /// Watches for the game minimizing at any point, and adjusts the amount of time to play the song for accordingly
        /// </summary>
        public static void WatchMusicEvent()
        {
            DateTime? minimized = null;

            while (CurrentEvent is not null)
            {
                bool musicPaused = !Main.instance.IsActive;

                if (musicPaused && !minimized.HasValue)
                    minimized = DateTime.Now;

                else if (!musicPaused && minimized.HasValue)
                {
                    TrackStart += DateTime.Now - minimized.Value;
                    minimized = null;
                }
            }

            EventTrackerThread = null;
        }

        #endregion

        #region Event Saving

        public override void SaveWorldData(TagCompound tag)
        {
            tag["CIcalamityPlayedMusicEventCount"] = PlayedEvents.Count;
            for (int i = 0; i < PlayedEvents.Count; i++)
                tag[$"CIcalamityPlayedMusicEvent{i}"] = PlayedEvents[i];
        }

        public override void LoadWorldData(TagCompound tag)
        {
            PlayedEvents.Clear();
            
            if (tag.TryGet("CIcalamityPlayedMusicEventCount", out int playedMusicEventCount))
            {
                for (int i = 0; i < playedMusicEventCount; i++)
                {
                    if (tag.TryGet($"CIcalamityPlayedMusicEvent{i}", out string playedEvent))
                        PlayedEvents.Add(playedEvent);
                }
            }
            
            oldWorld = false;
        }

        public override void OnWorldUnload()
        {
            oldWorld = true;
            TrackStart = null;
            TrackEnd = null;
            CurrentEvent = null;
            PlayedEvents.Clear();
            NoFade = false;
            LastPlayedEvent = -1;
        }

        #endregion

        #region Event Syncing

        public static void SendSyncRequest()
        {
            ModPacket packet = CalamityInheritance.Instance.GetPacket();
            packet.Write((byte)CalamityModMessageType.MusicEventSyncRequest);
            packet.Send();
        }

        public static void FulfillSyncRequest(int requester)
        {
            // Only fulfill requests as the server host
            if (!Main.dedServ)
                return;

            ModPacket packet = CalamityInheritance.Instance.GetPacket();
            packet.Write((byte)CalamityModMessageType.MusicEventSyncResponse);

            int trackCount = PlayedEvents.Count;
            packet.Write(trackCount);

            for (int i = 0; i < trackCount; i++)
                packet.Write(PlayedEvents[i]);

            packet.Send(toClient: requester);
        }

        public static void ReceiveSyncResponse(BinaryReader reader)
        {
            // Only receive info on clients
            if (Main.dedServ)
                return;

            PlayedEvents.Clear();
            int trackCount = reader.ReadInt32();

            for (int i = 0; i < trackCount; i++)
                PlayedEvents.Add(reader.ReadString());
        }

        #endregion
    }

    public class CalamityModMusicEventPlayer : ModPlayer
    {
        public override void OnEnterWorld()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient && Player.whoAmI != Main.myPlayer)
                MusicEventSystem.SendSyncRequest();
        }
    }
}
