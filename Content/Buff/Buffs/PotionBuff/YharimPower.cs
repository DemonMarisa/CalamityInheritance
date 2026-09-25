using CalamityInheritance.Content.BaseClass.Buff;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Buff.Buffs.PotionBuff
{
    public class YharimPower : CIBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance += 0.04f;
            player.statDefense += 10;
            player.pickSpeed -= 0.1f;
            player.GetDamage<GenericDamageClass>() += 0.05f;
            player.GetCritChance<GenericDamageClass>() += 2;
            player.GetKnockback<SummonDamageClass>() += 1f;
            player.moveSpeed += 0.075f;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.075f;
        }
    }
}
