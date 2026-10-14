using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.HitEffect;
using CalamityInheritance.Core.Keys;
using CalamityInheritance.Core.Path;
using LAP.Core.BaseClass;
using LAP.Core.StateMachine.SynedHitEffect;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityInheritance.Content.BaseClass.Weapons
{
    public abstract class CIExoWeapon : BaseSkillWeapon, ILocalizedModType
    {
        public override string LocalizationCategory => LocalizationPath.ExoWeapon;
        public bool UseAlt;
        public override void SaveData(TagCompound tag)
        {
            tag.Add("CalamityInheritance:UseAlt", UseAlt);
        }
        public override void LoadData(TagCompound tag)
        {
            UseAlt = tag.GetBool("CalamityInheritance:UseAlt");
        }
        public override void UpdateHoldItem(Player player)
        {
            UpdateHold(player);
            WeaponTransformation(player);
        }
        public virtual void UpdateHold(Player player)
        {

        }
        public override bool CanUseWeaponSkill(Player player)
        {
            return false;
        }
        public virtual void WeaponTransformation(Player player)
        {
            if (Main.myPlayer != player.whoAmI)
                return;
            if (CIKeybinds.WeaponTransformation.JustPressed)
            {
                UseAlt = !UseAlt;
                HitEffectManager.SpawnHitEffect(HitEffectManager.HEType<MiscTransEffect>(), player.whoAmI, player.GetSource_FromThis(), player.Center, Vector2.Zero);
            }
        }
    }
}
