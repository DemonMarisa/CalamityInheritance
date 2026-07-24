using CalamityInheritance.Core.Path;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Weapons
{
    public abstract class CIMelee : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => $"{LocalizationPath.MeleeWeapon}";
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            player.BetterSwing();
        }
    }
}
