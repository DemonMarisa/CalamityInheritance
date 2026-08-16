using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.Blance;
using CalamityInheritance.Content.CDs;
using CalamityInheritance.Content.Items.Accessories.Defense;
using LAP.Core.SystemsLoader;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public bool AnyShield;
        public int stateShield;
        public int stateShieldMax;
        public float ShieldRegenPool;
        public void ResetShield()
        {
            stateShieldMax = 0;
            if (!AnyShield)
            {
                ShieldRegenPool = 0;
                stateShield = 0;
            }
            AnyShield = false;
        }
        public void UpdateShield_PostUpdateMisc()
        {
            if (!AnyShield)
                return;
            if (CIsponge)
            {
                stateShieldMax = Player.statLifeMax2;
                if (stateShieldMax < 800)
                    stateShieldMax = 800;
            }
            if (!Player.HasCD<EnergyShieldDisplay>())
                Player.AddCD(LAPContent.CDType<EnergyShieldDisplay>(), stateShieldMax);
        }
        public void UpdateShield_PostUpdate()
        {
            if (AnyShield)
            {
                if (!Player.HasCD<EnergyShieldCD>())
                {
                    ShieldRegenPool += stateShieldMax / (float)CIAccessoriesBlance.CITotalShieldRechargeTime;
                    if (ShieldRegenPool > 1)
                    {
                        stateShield += (int)ShieldRegenPool;
                        ShieldRegenPool -= (int)ShieldRegenPool;
                    }
                    if (stateShield > stateShieldMax)
                        stateShield = stateShieldMax;
                }
            }
        }
        public void ModifyHurtInfo_Shield(ref Player.HurtInfo info)
        {
            if (!AnyShield)
                return;
            int totalDamageBlocked;
            if (stateShield != 0)
            {
                totalDamageBlocked = Math.Min(stateShield, info.Damage);
                stateShield -= totalDamageBlocked;
                if (stateShield <= 0)
                {
                    Player.AddCD(LAPContent.CDType<EnergyShieldCD>(), CIAccessoriesBlance.CIShieldRechargeDelay);
                    SoundEngine.PlaySound(CISounds.RoverDriveBreak, Player.Center);
                    stateShield = 0;
                    ShieldRegenPool = 0;
                    if (CIsponge)
                        TheSpongeLegacy.TheSponge_Hurt(Player, true, info);
                }
                else
                {
                    Player.AddCD(LAPContent.CDType<EnergyShieldCD>(), CIAccessoriesBlance.CIShieldRechargeRelay);
                    SoundEngine.PlaySound(CISounds.RoverDriveHit, Player.Center);
                    if (CIsponge)
                        TheSpongeLegacy.TheSponge_Hurt(Player, false, info);
                }
                FreeDodgeThisDamage = true;
                // 如果任何护盾受到了伤害，则显示文本以指示护盾受到了伤害
                string shieldDamageText = (-totalDamageBlocked).ToString();
                Rectangle location = new Rectangle((int)Player.position.X, (int)Player.position.Y - 16, Player.width, Player.height);
                CombatText.NewText(location, Color.LightBlue, Language.GetTextValue(shieldDamageText));
                // 无敌帧
                int shieldHitIFrames = 60;
                Player.GiveIFrames(info.CooldownCounter, shieldHitIFrames, true);
            }
        }
    }
}
