using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        // 玩家击杀終灾的计数
        public int LegacyScal_PlayerKillCount = 0;
        // 玩家死于終灾的计数
        public int LegacyScal_PlayerDeathCount = 0;

        public override void SaveData(TagCompound tag)
        {
            // 終灾相关计数
            tag.Add("CILegacyScal_PlayerKillCount", LegacyScal_PlayerKillCount);
            tag.Add("CILegacyScal_PlayerDeathCount", LegacyScal_PlayerDeathCount);

            QolSaveData(tag);
        }
        public override void LoadData(TagCompound tag)
        {
            // 終灾相关计数
            LegacyScal_PlayerKillCount = tag.GetInt("CILegacyScal_PlayerKillCount");
            LegacyScal_PlayerDeathCount = tag.GetInt("CILegacyScal_PlayerDeathCount");

            QolLoadData(tag);
        }
    }
}
