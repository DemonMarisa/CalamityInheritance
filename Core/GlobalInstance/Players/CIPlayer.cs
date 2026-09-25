using CalamityInheritance.Common.CalamityModCross;
using LAP.Core.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public int CurAOTCCharge;
        public bool CanUseOldLordDash;
        public bool BlockDefenseDamage;
        public float ContactDamageReduction;
        public float HurtHeal;
        public float FinalDefenseMult = 1f;
        public bool CanHaveDaawnlightLegacy;
        public float Stealth;
        public float CritDamageAdd;
        public bool gravityNormalizer;
        public bool ImmediatelyDeath;
        public float EnemySpawnRateMult;
        public float EnemyMaxSpawnMult;
        public int ExGrabRange;
        public float ExShootSpeed;
        public void MainResetEffects()
        {
            if (BlockDefenseDamage)
                Player.SetImmunityDefenseDamage();
            BlockDefenseDamage = false;
            ContactDamageReduction = 1f;
            HurtHeal = 0f;
            FinalDefenseMult = 1f;
            CanHaveDaawnlightLegacy = false;
            if (Player.velocity.Length() < 0.1f && Stealth <= 1f)
                Stealth += 0.05f;
            else if (Player.velocity.Length() > 0.1f && Stealth > 0)
                Stealth -= 0.05f;
            CritDamageAdd = 0;
            gravityNormalizer = false;
            ImmediatelyDeath = false;
            EnemySpawnRateMult = 1f;
            EnemyMaxSpawnMult = 1f;
            ExGrabRange = 0;
            ExShootSpeed = 0;
        }
        public void MainOnHurt(Player.HurtInfo info)
        {
            if (HurtHeal != 0)
                Player.NCHeal((int)(info.Damage * HurtHeal));
        }
        public void MainPostUpdateMisc()
        {
            if (gravityNormalizer)
            {
                Player.buffImmune[BuffID.VortexDebuff] = true;
                if (Player.ReducedSpaceGravity())
                {
                    Player.gravity = Terraria.Player.defaultGravity;
                    if (Player.wet)
                    {
                        if (Player.honeyWet) Player.gravity = 0.1f;
                        else if (Player.merman) Player.gravity = 0.3f;
                        else if (Player.trident && !Player.lavaWet)  Player.gravity = Player.controlUp ? 0.1f : 0.25f;
                        else Player.gravity = 0.2f;
                    }
                }
            }
        }
        public void MultDefense_PostUpdate()
        {
            if (FinalDefenseMult != 1f)
                Player.statDefense *= FinalDefenseMult;
        }
        public override void PostUpdate()
        {
            MultDefense_PostUpdate();
            UpdateShield_PostUpdate();
        }
    }
}
