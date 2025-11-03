using CalamityInheritance.System.Configs;
using CalamityMod;
using LAP.Core.LAPConditions;
using LAP.Core.MusicEvent;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityInheritance.System.MusicEventHandle
{
    public class MusicEvent_AlwaysUpdate : ModSystem
    {
        public bool HasAddExoTheme = false;
        public bool HasPlayTitle = false;
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("CIHasPlayTitle", HasPlayTitle);
            tag.Add("CIHasAddExoTheme", HasAddExoTheme);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            HasPlayTitle = tag.GetBool("CIHasPlayTitle");
            HasAddExoTheme = tag.GetBool("CIHasAddExoTheme");
        }
        public override void PostUpdateTime()
        {
            if (DownedBossSystem.downedExoMechs && !HasAddExoTheme)
            {
                MusicEventManger.AddMusicEventEntry("CalamityInheritance/Music/RequiemsOfACruelWorld", TimeSpan.FromSeconds(295.532d), () => CIConfig.Instance.Exomechs, TimeSpan.FromSeconds(5d));
                HasAddExoTheme = true;
            }
            if (!HasPlayTitle)
            {
                MusicEventManger.AddMusicEventEntry("CalamityModMusic/Sounds/Music/CalamityTitle", TimeSpan.FromSeconds(175.5d), () => CIConfig.Instance.TaleOfACruelWorld, TimeSpan.FromSeconds(5d));
                HasPlayTitle = true;
            }
        }
    }
}
