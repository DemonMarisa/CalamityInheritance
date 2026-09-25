using CalamityInheritance.Common.Blance;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Core.Path;
using LAP.Core.BaseClass;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Weapons
{
    public abstract class CIRogue : BaseSkillWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => LocalizationPath.RogueWeapon;
        public override void SetDefaults()
        {
            Item.width = Item.height = 32;
            Item.DamageType = RogueDamage.Instance;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.LAP().UseWeaponSkill = true;
            Item.LAP().UseCustomWeaponSkill = true;
            Item.LAP().WeaponSkillFocusCost = CIWeaponsBlance.StealthAttackFocusCost;
            Item.LAP().WeaponSkillRealFocusCost = CIWeaponsBlance.StealthAttackFocusCost;
            ExSD();
            Item.LAP().SkillShootSpeed = Item.shootSpeed;
            Item.LAP().WeaponSkillTime = Item.useTime;
            PostSD();
        }
        public virtual void ExSD()
        {
        }
        public virtual void PostSD()
        {

        }
        public override bool CanUseWeaponSkill(Player player)
        {
            return player.CheckFocus(Item.LAP().WeaponSkillRealFocusCost, false);
        }
        public override void UpdateHoldItem(Player player)
        {
            UpdateHoldRogue(player);
        }
        public virtual void UpdateHoldRogue(Player player)
        {

        }
    }
}
