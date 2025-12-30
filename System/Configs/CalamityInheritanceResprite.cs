using System.ComponentModel;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace CalamityInheritance.System.Configs
{
    [BackgroundColor(49, 32, 36, 216)]
    public class CIRespriteConfig : ModConfig
    {
        public static CIRespriteConfig Instance;
        public override void OnLoaded()
        {
            Instance = this;
        }
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message) => false;

        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool BloodOrange { get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool MiracleFruit { get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool Elderberry { get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool Dragonfruit { get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool CometShard { get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool EtherealCore { get; set; }
        [BackgroundColor(43, 56, 95, 182)]
        [DefaultValue(false)]
        [ReloadRequired]
        public bool PhantomHeart { get; set; }
    }
}