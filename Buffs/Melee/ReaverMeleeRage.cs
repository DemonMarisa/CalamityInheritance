using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Buffs.Melee
{
    public class ReaverMeleeRage : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //速览: 永恒套的怒气buff现在触发不再有任何条件，但提供10点防御力与10%近战攻速与伤害，不提供暴击概率
            player.GetDamage<MeleeDamageClass>() += 0.10f;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.10f;
            player.statDefense += 10;
        }
    }
}
