using CalamityInheritance.Core.Path;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Items
{
    public abstract class CIAccessories : ModItem, ILocalizedModType
    {
        public const int Combat = 0;
        public const int Movement = 1;
        public const int Restorative = 2;
        public const int Defense = 3;
        public const int Wings = 4;
        public const int Misc = 5;
        public virtual int AccessoriesStyle => 0;
        public override string LocalizationCategory => GetLocalization(AccessoriesStyle);
        internal static string GetLocalization(int style)
        {
            return style switch
            {
                Combat => LocalizationPath.CombatAccessories,
                Movement => LocalizationPath.MovementAccessories,
                Restorative => LocalizationPath.RestorativeAccessories,
                Defense => LocalizationPath.DefenseAccessories,
                Wings => LocalizationPath.Wings,
                Misc => LocalizationPath.MiscAccessories,
                _ => LocalizationPath.MiscAccessories
            };
        }
    }
}