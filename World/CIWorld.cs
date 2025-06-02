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
        // Modes
        public bool Malice = false; // 恶意
        public bool Defiled = false; // 神殇
        public bool IronHeart = false; // 铁心
        public bool Armageddon = false; // 末日

        public static bool malice = false; // 恶意
        public static bool defiled = false; // 神殇
        public static bool ironHeart = false; // 铁心
        public static bool armageddon = false; // 末日
        public override void PostUpdateWorld()
        {
            malice = Malice;
            defiled = Defiled;
            ironHeart = IronHeart;
            armageddon = Armageddon;
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag["MaliceMode"] = Malice;
            tag["DefiledMode"] = Defiled;
            tag["IronHeartMode"] = IronHeart;
            tag["ArmageddonMode"] = Armageddon;

            tag["MaliceMode2"] = malice;
            tag["DefiledMode2"] = defiled;
            tag["IronHeartMode2"] = ironHeart;
            tag["ArmageddonMode2"] = armageddon;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            Malice = tag.GetBool("MaliceMode");
            Defiled = tag.GetBool("DefiledMode");
            IronHeart = tag.GetBool("IronHeartMode");
            Armageddon = tag.GetBool("ArmageddonMode");

            malice = tag.GetBool("MaliceMode2");
            defiled = tag.GetBool("DefiledMode2");
            ironHeart = tag.GetBool("IronHeartMode2");
            armageddon = tag.GetBool("ArmageddonMode2");
        }
        #region 网络同步
        public override void NetSend(BinaryWriter writer)
        {
            BitsByte net1 = new BitsByte();
            net1[0] = Malice;
            net1[1] = Defiled;
            net1[2] = IronHeart;
            net1[3] = Armageddon;
            net1[4] = malice;
            net1[5] = defiled;
            net1[6] = ironHeart;
            net1[7] = armageddon;
            writer.Write(net1);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte net1 = reader.ReadByte();
            Malice = net1[0];
            Defiled = net1[1];
            IronHeart = net1[2];
            Armageddon = net1[3];
            malice = net1[4];
            defiled = net1[5];
            ironHeart = net1[6];
            armageddon = net1[7];
        }
        #endregion
        #region 开启与关闭
        public void UpdateMalice()
        {
            if (!Malice)
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
                Malice = true;
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.MaliceText", Color.LightGoldenrodYellow);
            }
            else
            {
                Malice = false;
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.MaliceText2", Color.LightGoldenrodYellow);
            }

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData);
        }
        public void UpdateDefiled()
        {
            if (!Defiled)
            {
                Defiled = true;
                defiled = true;
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.DefiledText", Color.DarkSeaGreen);
            }
            else
            {
                Defiled = false;
                defiled = false;
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.DefiledText2", Color.DarkSeaGreen);
            }

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData);
        }
        public void UpdateArmageddon()
        {
            if (!Armageddon)
            {
                Armageddon = true;
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.ArmageddonText", Color.Fuchsia);
            }
            else
            {
                Armageddon = false;
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.ArmageddonText2", Color.Fuchsia);
            }

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData);
        }
        public void UpdateIronHeart()
        {
            if (!IronHeart)
            {
                IronHeart = true;
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.IronHeartText", Color.LightSkyBlue);
            }
            else
            {
                IronHeart = false;
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.IronHeartText2", Color.LightSkyBlue);
            }

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData);
        }
        #endregion
    }
}
