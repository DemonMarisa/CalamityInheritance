using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public override void SaveData(TagCompound tag)
        {
            tag.Add("CILegacyScal_PlayerKillCount", LegacyScal_PlayerKillCount);
            tag.Add("CILegacyScal_PlayerDeathCount", LegacyScal_PlayerDeathCount);
        }
        public override void LoadData(TagCompound tag)
        {
            LegacyScal_PlayerKillCount = tag.GetInt("CILegacyScal_PlayerKillCount");
            LegacyScal_PlayerDeathCount = tag.GetInt("CILegacyScal_PlayerDeathCount");
        }
    }
}
