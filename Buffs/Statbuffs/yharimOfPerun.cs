using CalamityInheritance.Utilities;
using CalamityMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Statbuffs
{
    public class yharimOfPerun : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            int dashCounter = player.CIMod().AncientAuricDashCounter;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.40f * dashCounter;
            player.GetAttackSpeed<RangedDamageClass>() += 0.30f * dashCounter; 
            player.GetAttackSpeed<MagicDamageClass>() += 0.35f * dashCounter;
            player.GetCritChance<GenericDamageClass>() += 100 * dashCounter; //所有职业获得100暴击概率
            player.manaCost *= 0.20f;
            player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += 2f * dashCounter;
            player.GetAttackSpeed<RogueDamageClass>() += 0.30f * dashCounter;
        }
    }
}