using CalamityInheritance.Content.BaseClass.Buff;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Buff.Buffs
{
    public class KamiBuff : CIBuff
    {
        public const float RunSpeedBoost = 0.15f;
        public const float RunAccelerationBoost = 0.15f;
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 1.15f;
            player.maxRunSpeed *= 1.15f;
            player.GetDamage<GenericDamageClass>() += 0.15f;
        }
    }
}
