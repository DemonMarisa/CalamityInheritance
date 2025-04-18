﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.CustomRecipes;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityInheritance.System.DownedBoss
{
    // RE我草你妈，为什么不给EOW和BOC写单独的DownedBoss
    public class CIDownedBossSystem : ModSystem
    {
        public static bool _downedEOW = false;
        public static bool _downedBOC = false;
        public static bool _downedBloodMoon = false;
        public static bool _downedLegacySCal = false;

        public static bool DownedEOW
        {
            get => _downedEOW;
            set
            {
                if (!value)
                    _downedEOW = false;
                else
                    NPC.SetEventFlagCleared(ref _downedEOW, -1);
            }
        }
        public static bool DownedBOC
        {
            get => _downedBOC;
            set
            {
                if (!value)
                    _downedBOC = false;
                else
                    NPC.SetEventFlagCleared(ref _downedBOC, -1);
            }
        }
        public static bool DownedBloodMoon
        {
            get => _downedBloodMoon;
            set
            {
                if (!value)
                    _downedBloodMoon = false;
                else
                    NPC.SetEventFlagCleared(ref _downedBloodMoon, -1);
            }
        }
        public static bool DownedLegacyScal
        {
            get => _downedLegacySCal;
            set
            {
                if (!value)
                    _downedLegacySCal = false;
                else
                    NPC.SetEventFlagCleared(ref _downedLegacySCal, -1);
            }
        }
        public static void ResetAllFlags()
        {
            DownedEOW = false;
            DownedBOC = false;
            DownedBloodMoon = false;
            DownedLegacyScal = false;
        }
        public override void OnWorldLoad() => ResetAllFlags();

        public override void OnWorldUnload() => ResetAllFlags();

        public override void SaveWorldData(TagCompound tag)
        {
            List<string> downed = new List<string>();

            // 肉前击败的boss
            if (DownedEOW)
                downed.Add("CIEOW");
            if (DownedBOC)
                downed.Add("CIBOC");
            if (DownedBloodMoon)
                downed.Add("CIBloodMoon");
            if (DownedLegacyScal)
                downed.Add("CILegacyScal");

            tag["CIdownedFlags"] = downed;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            IList<string> downed = tag.GetList<string>("CIdownedFlags");

            DownedEOW = downed.Contains("CIEOW");
            DownedBOC = downed.Contains("CIBOC");
            DownedBloodMoon = downed.Contains("CIBloodMoon");
            DownedLegacyScal = downed.Contains("CILegacyScal");
        }
        //发送数据包
        #region 网络同步
        public override void NetSend(BinaryWriter writer)
        {
            BitsByte net1 = new BitsByte();
            //一个比特=8个字节，如果有部分字节暂时用不上，这些字节是一定得用各种方法占用掉让其形成一个完整的比特的
            //不然发送的时候会有点问题
            net1[0] = DownedEOW;
            net1[1] = DownedBOC;
            net1[2] = DownedBloodMoon;
            net1[3] = DownedLegacyScal;
            net1[4] = false;
            net1[5] = false;
            net1[6] = false;
            net1[7] = false;
            writer.Write(net1);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte net1 = reader.ReadByte();
            DownedEOW = net1[0];
            DownedBOC = net1[1];
            DownedBloodMoon = net1[2];
            DownedLegacyScal = net1[3];
            //空字节接收的时候就丢弃
            _ = net1[4];
            _ = net1[5];
            _ = net1[6];
            _ = net1[7];
        }
        #endregion
    }
}
