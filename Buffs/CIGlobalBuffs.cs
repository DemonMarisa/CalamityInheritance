using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs
{
    public class CIGlobalBuffs: GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            //魔法球莫名其妙少了2伤害和2暴击，魔法药水莫名其妙少10伤害
            //灾厄你还是压缩大王啊
            if (type == BuffID.MagicPower)
                player.GetDamage<MagicDamageClass>() += 0.1f;

            else if (type == BuffID.Clairvoyance)
            {
                player.GetDamage<MagicDamageClass>() += 0.02f;
                player.GetCritChance<MagicDamageClass>() += 2;
            }

            //补回灾厄内部分死掉的挖矿速度提升
            //砍这些内容纯纯tm有病
            else if (type == BuffID.Mining) //挖矿药水 
                player.pickSpeed -= 0.10f;
            
            else if (type == BuffID.WellFed2) //中等饱食度
                player.pickSpeed -= 0.025f;

            else if (type == BuffID.WellFed3) //大饱食度
                player.pickSpeed -= 0.05f;
            else if (type == BuffID.SugarRush)
                player.pickSpeed -= 0.1f;
        }
    }
}
