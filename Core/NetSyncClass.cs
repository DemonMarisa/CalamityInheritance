using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

//一个用于多人同步，写入数据包的句柄
//目前还没写完。
namespace CalamityInheritance.Core
{
    internal abstract class NetSyncHandler
    {
        internal byte HandlerType { get ; set;}
        public abstract void HandlerSocket (BinaryReader readSocket, int fromWho);
        //构造函数
        protected NetSyncHandler(byte handleType) => HandlerType = handleType;
        protected ModPacket GetPacket(byte socketType, int fromWho)
        {
            var packet = CalamityInheritance.Instance.GetPacket();
            packet.Write(HandlerType);
            packet.Write(socketType);
            if (Main.netMode == NetmodeID.Server)
                packet.Write((byte)fromWho);
            return packet;
        }
    } 
}