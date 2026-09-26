using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.Blance;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.HitEffect;
using CalamityInheritance.Content.Particles;
using CalamityInheritance.Core.Keys;
using CalamityInheritance.Core.Path;
using LAP.Content.Particles;
using LAP.Core.BaseClass;
using LAP.Core.Presets.Content;
using LAP.Core.StateMachine.SynedHitEffect;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass.Weapons
{
    public abstract class CIMeleeRogue : BaseSkillWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => LocalizationPath.RogueMeleeWeapon;
        public bool IsMelee = false;
        public Color EffectColor = Color.White;
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
            WeaponTransformation(player);
        }
        public virtual void UpdateHold(Player player)
        {

        }
        public virtual void WeaponTransformation(Player player)
        {
            if (Main.myPlayer != player.whoAmI)
                return;
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
                HitEffectManager.SpawnHitEffect(HitEffectManager.HEType<WeaponTransEffect>(), player.whoAmI, player.GetSource_FromThis(), player.Center, Vector2.Zero);
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
