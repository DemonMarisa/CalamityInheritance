using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs.Legendary
{
    public class DukeBuff: ModBuff    
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
            //10%伤害，10%攻速，5%暴击，1击退，1HP，20防御，10免伤，10移速
            player.GetDamage<MeleeDamageClass>() += 0.10f;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.10f;
            player.GetKnockback<MeleeDamageClass>() += 1;
            player.GetCritChance<MeleeDamageClass>() += 5;
            player.statDefense += 20;
            player.lifeRegen += 2;
            player.endurance += 0.1f;
            player.moveSpeed += 0.1f;
        }
    }
}