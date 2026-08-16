using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Core.Utils;
using CalamityMod;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

#pragma warning disable RS0030

namespace CalamityInheritance.Common.CalamityModCross
{
    public static class CalCrossUtils
    {
        [JITWhenModsEnabled("CalamityMod")]
        public static void ChargeCalamityItem(Player player)
        {
            SoundEngine.PlaySound(CISounds.AuricQuantumCoolingCellInstallNew, Main.player[Main.myPlayer].Center);
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i].type > ItemID.Count && player.inventory[i].Calamity().UsesCharge)
                    player.inventory[i].Calamity().Charge = player.inventory[i].Calamity().MaxCharge;
            }
        }
        public static void SetRogueArmor(this Player player, float stealthMax, bool halfCost = false)
        {
            if (CIUtils.HasCalamity())
                player.RogueArmor_Jit(stealthMax, halfCost);
        }

        [JITWhenModsEnabled("CalamityMod")]
        internal static void RogueArmor_Jit(this Player player, float stealthMax, bool halfCost = false)
        {
            player.Calamity().wearingRogueArmor = true;
            player.Calamity().rogueStealthMax += stealthMax;
            if (halfCost)
                player.Calamity().stealthStrikeHalfCost = true;
        }
        public static void SetInfiniteFlight(this Player player)
        {
            if (CIUtils.HasCalamity())
                SetInfiniteFlight_Jit(player);
        }
        [JITWhenModsEnabled("CalamityMod")]
        internal static void SetInfiniteFlight_Jit(Player player)
        {
            player.Calamity().infiniteFlight = true;
        }
        public static void SetDefenseDamageRatio(this Player player, float value)
        {
            if (CIUtils.HasCalamity())
                SetDefenseDamageRatio_Jit(player, value);
        }
        [JITWhenModsEnabled("CalamityMod")]
        internal static void SetDefenseDamageRatio_Jit(Player player, float value)
        {
            player.Calamity().defenseDamageRatio = value;
        }
        public static void SetImmunityDefenseDamage(this Player player)
        {
            if (CIUtils.HasCalamity())
                SetImmunityDefenseDamage_Jit(player);
        }
        [JITWhenModsEnabled("CalamityMod")]
        internal static void SetImmunityDefenseDamage_Jit(Player player)
        {
            player.Calamity().externalDefenseDamageImmunity = true;
        }
        public static void CostRogueStealth(this Player player, float value)
        {
            if (CIUtils.HasCalamity())
                CostRogueStealth_Jit(player, value);
        }
        [JITWhenModsEnabled("CalamityMod")]
        internal static void CostRogueStealth_Jit(Player player, float value)
        {
            player.Calamity().rogueStealth = player.Calamity().rogueStealthMax * value;
        }
        public static bool IsTrueMelee(this DamageClass damageClass)
        {
            if (CIUtils.HasCalamity())
            {
                return damageClass.CountsAsClass<TrueMelee>() || IsTrueMelee_Jit(damageClass);
            }
            else
            {
                return damageClass.CountsAsClass<TrueMelee>();
            }
        }
        [JITWhenModsEnabled("CalamityMod")]
        internal static bool IsTrueMelee_Jit(DamageClass damageClass)
        {
            return damageClass.CountsAsClass<TrueMeleeDamageClass>();
        }
    }
}
