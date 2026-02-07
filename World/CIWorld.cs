using CalamityInheritance.Content.Items.Placeables.Vanity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityInheritance.World
{
    public class CIWorld : ModSystem
    {
        public static bool malice = false; // 恶意
        public static bool defiled = false; // 神殇
        public static bool ironHeart = false; // 铁心
        public static bool armageddon = false; // 末日
        public override void SaveWorldData(TagCompound tag)
        {
            tag["MaliceMode2"] = malice;
            tag["DefiledMode2"] = defiled;
            tag["IronHeartMode2"] = ironHeart;
            tag["ArmageddonMode2"] = armageddon;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            malice = tag.GetBool("MaliceMode2");
            defiled = tag.GetBool("DefiledMode2");
            ironHeart = tag.GetBool("IronHeartMode2");
            armageddon = tag.GetBool("ArmageddonMode2");
        }
        #region 网络同步
        public override void NetSend(BinaryWriter writer)
        {
            BitsByte net1 = new BitsByte();
            net1[0] = malice;
            net1[1] = defiled;
            net1[2] = ironHeart;
            net1[3] = armageddon;
            net1[4] = false;
            net1[5] = false;
            net1[6] = false;
            net1[7] = false;
            writer.Write(net1);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte net1 = reader.ReadByte();
            malice = net1[0];
            defiled = net1[1];
            ironHeart = net1[2];
            armageddon = net1[3];
            _ = net1[4];
            _ = net1[5];
            _ = net1[6];
            _ = net1[7];
        }
        #endregion
        #region 开启与关闭
        public void UpdateMalice()
        {
            if (!malice)
            {
                if (!CalamityWorld.revenge)
                {
                    CalamityWorld.revenge = true;
                    CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.RevengeText", Color.Crimson);
                }
                if (!CalamityWorld.death)
                {
                    CalamityWorld.death = true;
                    CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.DeathText", Color.Crimson);
                }
                malice = true;
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.MaliceText", Color.LightGoldenrodYellow);
            }
            else
            {
                malice = false;
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.MaliceText2", Color.LightGoldenrodYellow);
            }

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData);
        }
        public void UpdateDefiled()
        {
            if (!defiled)
            {
                defiled = true;
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.DefiledText", Color.DarkSeaGreen);
            }
            else
            {
                defiled = false;
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.DefiledText2", Color.DarkSeaGreen);
            }

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData);
        }
        public void UpdateArmageddon()
        {
            if (!armageddon)
            {
                armageddon = true;
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.ArmageddonText", Color.Fuchsia);
            }
            else
            {
                armageddon = false;
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.ArmageddonText2", Color.Fuchsia);
            }

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData);
        }
        public void UpdateIronHeart()
        {
            if (!ironHeart)
            {
                ironHeart = true;
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.IronHeartText", Color.LightSkyBlue);
            }
            else
            {
                ironHeart = false;
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.IronHeartText2", Color.LightSkyBlue);
            }

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData);
        }
        #endregion
    }
}
