using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using System;

namespace CalamityInheritance.Buffs.Potions
{
    public class YharimPower : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //击退
            player.CIMod().BuffStatsYharimsStin = true;
            if (!Main.zenithWorld)
            {
                player.endurance += 0.04f;
                player.statDefense += 10;
                player.pickSpeed -= 0.1f;
                player.GetDamage<GenericDamageClass>() += 0.05f;
                player.GetCritChance<GenericDamageClass>() += 2;
                player.GetKnockback<SummonDamageClass>() += 1f;
                player.moveSpeed += 0.075f;
                player.GetAttackSpeed<MeleeDamageClass>() += 0.075f;
                return;
            }
            // BYD谁让你写不在天顶也十倍速度的
            if (Main.zenithWorld)
            {
                player.moveSpeed += 10;
                player.wingTime += 3.0f;
                if ((double)Math.Abs(player.velocity.X) > 1.05 || (double)Math.Abs(player.velocity.Y) > 1.05)
                    player.GetAttackSpeed<GenericDamageClass>() += 2.0f;
            }
        }
    }
}
