using CalamityInheritance.Core.Path;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Buff
{
    public abstract class CIBuff : ModBuff, ILocalizedModType
    {
        public new string LocalizationCategory => $"{LocalizationPath.CIDamageBuff}";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            ExSSD();
        }
        public virtual void ExSSD()
        {

        }
    }
}
