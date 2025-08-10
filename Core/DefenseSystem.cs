using Terraria.ModLoader;

namespace CalamityInheritance.Core
{
    public static class DefenseSystem
    {
        #region Item Name
        public static string NameArmorShell => "ArmoredShell";
        public static string NameRuinSoul => "RuinousSoul";
        public static Mod Calamity => CalamityInheritance.Calamity;
        #endregion
        #region GetItemName
        public static int TryGetItem(this string name)
        {
            if (Calamity.TryFind(name, out ModItem item))
                return item.Type;
            else return -1;
        }
        #endregion
    }
}