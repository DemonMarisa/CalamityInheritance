using CalamityInheritance.Common.Blance;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Core.Keys;
using CalamityInheritance.Core.Path;
using LAP.Core.BaseClass;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Weapons
{
    public abstract class CIMeleeRogue : BaseSkillWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => LocalizationPath.RogueMeleeWeapon;
        public bool IsMelee = false;
        public Color EffectColor;
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
            if (IsMelee)
                return false;
            return player.CheckFocus(Item.LAP().WeaponSkillRealFocusCost, false);
        }
        public override void UpdateHoldItem(Player player)
        {
            UpdateHold(player);
            WeaponTransformation();
        }
        public virtual void UpdateHold(Player player)
        {

        }
        public virtual void WeaponTransformation()
        {
            if (CIKeybinds.WeaponTransformation.JustPressed)
            {
                if (IsMelee)
                {
                    IsMelee = !IsMelee;
                    OnTranseToRogue();
                    Item.DamageType = RogueDamage.Instance;
                }
                else
                {
                    IsMelee = !IsMelee;
                    OnTranseToMelee();
                    Item.DamageType = DamageClass.Melee;
                }
            }
        }
        public virtual void OnTranseToRogue()
        {

        }
        public virtual void OnTranseToMelee()
        {
        }
    }
}
