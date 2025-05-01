using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityInheritance.World
{
    public class CIWorld : ModSystem
    {
        // Modes
        public static bool Malice = false; // 恶意
        public static bool Defiled = false; // 神殇
        public static bool IronHeart = false; // 铁心
        public static bool Armageddon = false; // 末日
        
        public override void SaveWorldData(TagCompound tag)
        {
            tag["MaliceMode"] = Malice;
            tag["DefiledMode"] = Defiled;
            tag["IronHeartMode"] = IronHeart;
            tag["ArmageddonMode"] = Armageddon;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            Malice = tag.GetBool("MaliceMode");
            Defiled = tag.GetBool("DefiledMode");
            IronHeart = tag.GetBool("IronHeartMode");
            Armageddon = tag.GetBool("ArmageddonMode");
        }
        
    }
}
