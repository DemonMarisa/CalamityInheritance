using CalamityInheritance.Content.BaseClass.Buff;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Buff.Buffs.PotionBuff
{
    public class Invincible : CIBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.CI().InvincibleTimer = 2;
        }
    }
}
