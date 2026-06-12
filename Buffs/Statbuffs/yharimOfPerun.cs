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
            player.GetDamage<GenericDamageClass>() += 0.4f;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.80f;
            player.GetAttackSpeed<RangedDamageClass>() += 0.6f;
            player.GetAttackSpeed<MagicDamageClass>() += 0.7f;
            player.GetCritChance<GenericDamageClass>() += 100; //所有职业获得100暴击概率
            player.manaCost *= 0.20f;
            player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += 2f;
            player.GetAttackSpeed<RogueDamageClass>() += 0.60f;
        }
    }
}