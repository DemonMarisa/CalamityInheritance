using CalamityInheritance.Core.Path;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Buff
{
    public abstract class CIDeBuff : ModBuff, ILocalizedModType
    {
        public new string LocalizationCategory => $"{LocalizationPath.CIDeBuff}";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            ExSSD();
        }
        public virtual void ExSSD()
        {

        }
    }
}
