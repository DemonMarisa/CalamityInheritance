using CalamityInheritance.Common.CalamityModCross.RogueCheck;
using LAP.Core.NetCode;
using LAP.Core.NetCode.Content;
using LAP.Core.SystemsLoader;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.CINetCode
{
    public class ReadStealthProj : BaseLAPHandlePack
    {
        public override void Read(BinaryReader reader, int whoAmI)
        {
            // 从数据包中按写入顺序读取数据
            int index = reader.ReadInt32();
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket broadcastPacket = CalamityInheritance.Instance.GetPacket();
                broadcastPacket.Write(Type);
                broadcastPacket.Write(index);
                broadcastPacket.Send(-1, whoAmI);
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // 其它玩家收到包并添加后不需要再发送包了
                Projectile proj = Main.projectile[index];
                proj.SetStealthAttack();
            }
        }
        public static void SyncedStealth(Projectile proj)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            // 只在多人模式的客户端执行
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // 创建一个新的网络数据包
                ModPacket packet = LAP.LAP.Instance.GetPacket();
                // 写入一个自定义的消息类型，以便HandlePacket能识别
                packet.Write(LAPContent.PackHandleType<ReadCustomCD>());
                // 写入射弹索引
                packet.Write(proj.whoAmI);
                // 发送给服务器
                packet.Send();
            }
        }
    }
}
