using CalamityInheritance.Core.GlobalInstance.Projectiles;
using LAP.Core.NetCode;
using LAP.Core.SystemsLoader;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.CINetCode
{
    public class ReadSendProjState : BaseLAPHandlePack
    {
        public override void Read(BinaryReader reader, int whoAmI)
        {
            int index = reader.ReadInt32();
            int type = reader.ReadInt32();
            bool tileCollide = reader.ReadBoolean();
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket broadcastPacket = LAP.LAP.Instance.GetPacket();
                broadcastPacket.Write(Type);
                broadcastPacket.Write(index);
                broadcastPacket.Write(type);
                broadcastPacket.Write(tileCollide);
                broadcastPacket.Send(-1, whoAmI);
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Projectile proj = Main.projectile[index];
                if (proj.type == type)
                {
                    proj.tileCollide = tileCollide;
                }
            }
        }
        public static void SyncedStealth(Projectile proj)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                return;
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = LAP.LAP.Instance.GetPacket();
                packet.Write(LAPContent.PackHandleType<ReadSendProjState>());
                packet.Write(proj.whoAmI);
                packet.Write(proj.type);
                packet.Write(proj.tileCollide);
                packet.Send();
            }
        }
    }
}
