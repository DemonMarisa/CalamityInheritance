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

        //wtf这个是能被复重写的？
        public override void SaveData(TagCompound tag)
        {
            // 終灾相关计数
            tag.Add("CILegacyScal_PlayerKillCount", LegacyScal_PlayerKillCount);
            tag.Add("CILegacyScal_PlayerDeathCount", LegacyScal_PlayerDeathCount);
            // 恶魔纹章
            tag.Add("CIMLG", MLG);
            QolSaveData(tag);
            //传奇物品样式保存
            LegendarySaveData(tag);
            // 熟练度储存
            LevelSaveData(tag);
        }
        public override void LoadData(TagCompound tag)
        {
            // 終灾相关计数
            LegacyScal_PlayerKillCount = tag.GetInt("CILegacyScal_PlayerKillCount");
            LegacyScal_PlayerDeathCount = tag.GetInt("CILegacyScal_PlayerDeathCount");
            // 恶魔纹章
            MLG = tag.GetBool("CIMLG");
            QolLoadData(tag);
            LegendaryLoadData(tag);
            LevelLoadData(tag);
        }
    }
}
