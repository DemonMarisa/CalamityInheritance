using CalamityInheritance.Content.BaseClass.Buff;
using CalamityInheritance.Content.Buff.Debuffs;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Buff.Buffs.PotionBuff
{
    public class ArmorShattering : CIBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //给碎甲debuff
            player.CI().AddHitAddBuff(BuffType<CIArmorCrunch>(), 180);
            player.GetDamage<ThrowingDamageClass>() += 0.08f;
            player.GetDamage<MeleeDamageClass>() += 0.08f;
            player.GetCritChance<ThrowingDamageClass>() += 8;
            player.GetCritChance<MeleeDamageClass>() += 8;
        }
    }
}
