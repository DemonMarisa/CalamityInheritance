using System;
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
        internal static bool _downedBloodMoon = false;
        internal static bool _downedLegacySCal = false;
        internal static bool _downedLegacyYharonP1 = false;
        internal static bool _downedLegacyYharonP2 = false;
        internal static bool _downedBuffedSolarEclipse = false;
        internal static bool _downedCalClone = false;
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
        public static bool DownedLegacyYharonP1
        {
            get => _downedLegacyYharonP1;
            set
            {
                if (!value)
                    _downedLegacyYharonP1 = false;
                else
                    NPC.SetEventFlagCleared(ref _downedLegacyYharonP1, -1);
            }
        }
        public static bool DownedLegacyYharonP2
        {
            get => _downedLegacyYharonP2;
            set
            {
                if (!value)
                    _downedLegacyYharonP2 = false;
                else
                    NPC.SetEventFlagCleared(ref _downedLegacyYharonP2, -1);
            }
        }
        public static bool DownedBuffedSolarEclipse
        {
            get => _downedBuffedSolarEclipse;
            set
            {
                if (!value)
                    _downedBuffedSolarEclipse = false;
                else
                    NPC.SetEventFlagCleared(ref _downedBuffedSolarEclipse, -1);
            }
        }
        public static bool DownedCalClone
        {
            get => _downedCalClone;
            set
            {
                if (!value)
                    _downedCalClone = false;
                else
                    NPC.SetEventFlagCleared(ref _downedCalClone, -1);
            }
        }
        public static void ResetAllFlags()
        {
            DownedBloodMoon = false;
            DownedLegacyScal = false;
            DownedLegacyYharonP1 = false;
            DownedLegacyYharonP2 = false;
            DownedBuffedSolarEclipse = false;
            DownedCalClone = false;
        }
        public override void OnWorldLoad() => ResetAllFlags();

        public override void OnWorldUnload() => ResetAllFlags();

        public override void SaveWorldData(TagCompound tag)
        {
            List<string> downed = new List<string>();

            if (DownedBloodMoon)
                downed.Add("CIBloodMoon");
            if (DownedLegacyScal)
                downed.Add("CILegacyScal");
            if (DownedLegacyYharonP1)
                downed.Add("CILegacyYharonP1");
            if (DownedLegacyYharonP2)
                downed.Add("CILegacyYharonP2");
            if (DownedBuffedSolarEclipse)
                downed.Add("CIDownedBuffedSolarEclipse");
            if (DownedCalClone)
                downed.Add("CIDownedCalClone");

            tag["CIdownedFlags"] = downed;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            IList<string> downed = tag.GetList<string>("CIdownedFlags");

            DownedBloodMoon = downed.Contains("CIBloodMoon");
            DownedLegacyScal = downed.Contains("CILegacyScal");
            DownedLegacyYharonP1 = downed.Contains("CILegacyYharonP1");
            DownedLegacyYharonP2 = downed.Contains("CILegacyYharonP2");
            DownedBuffedSolarEclipse = downed.Contains("CIDownedBuffedSolarEclipse");
            DownedCalClone = downed.Contains("CIDownedCalClone");
        }
        //发送数据包
        #region 网络同步
        public override void NetSend(BinaryWriter writer)
        {
            BitsByte net1 = new BitsByte();
            //一个比特=8个字节，如果有部分字节暂时用不上，这些字节是一定得用各种方法占用掉让其形成一个完整的比特的
            //不然发送的时候会有点问题
            net1[0] = false;
            net1[1] = false;
            net1[2] = DownedBloodMoon;
            net1[3] = DownedLegacyScal;
            net1[4] = DownedLegacyYharonP1;
            net1[5] = DownedBuffedSolarEclipse;
            net1[6] = DownedCalClone;
            net1[7] = DownedLegacyYharonP2;
            writer.Write(net1);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte net1 = reader.ReadByte();
            _ = net1[0];
            _ = net1[1];
            DownedBloodMoon = net1[2];
            DownedLegacyScal = net1[3];
            //空字节接收的时候就丢弃
            DownedLegacyYharonP1 = net1[4];
            DownedBuffedSolarEclipse = net1[5];
            DownedCalClone = net1[6];
            DownedLegacyYharonP2 = net1[7];
        }
        #endregion
    }
}
