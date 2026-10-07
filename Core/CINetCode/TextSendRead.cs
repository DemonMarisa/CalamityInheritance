using LAP.Core.NetCode;
using LAP.Core.SystemsLoader;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalamityInheritance.Core.CINetCode
{
    //public class TestPlayer : ModPlayer
    //{
    //    public bool Flag = false;
    //}

    //public class TestSystem : ModSystem
    //{
    //    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    //    {
    //        int mouseIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");
    //        if (mouseIndex != -1)
    //        {
    //            layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("CI Test UI", delegate ()
    //            {
    //                foreach (Player player in Main.player)
    //                {
    //                    if (player.active)
    //                    {
    //                        if (Main.mouseLeft && Main.mouseLeftRelease)
    //                        {
    //                            if (player.whoAmI == Main.myPlayer)
    //                            {
    //                                player.GetModPlayer<TestPlayer>().Flag = !player.GetModPlayer<TestPlayer>().Flag;
    //                                TextSendRead.SyncedStealth(player, player.GetModPlayer<TestPlayer>().Flag);
    //                            }
    //                        }
    //                        string text = "标记当前状态 ： " + player.GetModPlayer<TestPlayer>().Flag;
    //                        Terraria.Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value,
    //                            text,
    //                            player.Center.X - Main.screenPosition.X, player.Center.Y - Main.screenPosition.Y, Color.White, Color.Black, default);
    //                    }
    //                }
    //                return true;
    //            }, InterfaceScaleType.UI));
    //        }
    //    }
    //}

    //public class TextSendRead : BaseLAPHandlePack
    //{
    //    public override void Read(BinaryReader reader, int whoAmI)
    //    {
    //        // 从数据包中按写入顺序读取数据
    //        int player = reader.ReadInt32();
    //        bool flag = reader.ReadBoolean();
    //        if (Main.netMode == NetmodeID.Server)
    //        {
    //            ModPacket broadcastPacket = LAP.LAP.Instance.GetPacket();
    //            broadcastPacket.Write(Type);
    //            broadcastPacket.Write(player);
    //            broadcastPacket.Write(flag);
    //            broadcastPacket.Send(-1, whoAmI);
    //        }
    //        else if (Main.netMode == NetmodeID.MultiplayerClient)
    //        {
    //            Main.player[player].GetModPlayer<TestPlayer>().Flag = flag;
    //        }
    //    }
    //    public static void SyncedStealth(Player player, bool flag)
    //    {
    //        if (Main.netMode == NetmodeID.SinglePlayer)
    //            return;
    //        if (Main.netMode == NetmodeID.MultiplayerClient)
    //        {
    //            ModPacket packet = LAP.LAP.Instance.GetPacket();
    //            packet.Write(LAPContent.PackHandleType<TextSendRead>());
    //            packet.Write(player.whoAmI);
    //            packet.Write(flag);
    //            packet.Send();
    //        }
    //    }
    //}
}
