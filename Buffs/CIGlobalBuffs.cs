using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.Alcohol;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.CalPlayer;
using rail;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Buffs
{
    public class CIGlobalBuffs: GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            /*
            魔法球莫名其妙少了2伤害和2暴击，魔法药水莫名其妙少10伤害
            灾厄你还是压缩大王啊
            下面这个效果无论是否开启原版回调都是存在的，我就是看他不爽了
            */
            #region 法师buff回调
            if (type == BuffID.MagicPower)
                player.GetDamage<MagicDamageClass>() += 0.1f;
            else if (type == BuffID.Clairvoyance)
            {
                player.GetDamage<MagicDamageClass>() += 0.02f;
                player.GetCritChance<MagicDamageClass>() += 2;
            }
            #endregion
            #region 挖掘速度回调
            //补回灾厄内部分死掉的挖矿速度提升, 我也不知道灾厄砍这些是为什么
            else if (type == BuffID.Mining) //挖矿药水 
                player.pickSpeed -= 0.10f;
            else if (type == BuffID.WellFed2) //中等饱食度
                player.pickSpeed -= 0.025f;
            else if (type == BuffID.WellFed3) //大饱食度
                player.pickSpeed -= 0.05f;
            else if (type == BuffID.SugarRush) //糖果冲刺
                player.pickSpeed -= 0.1f;
            #endregion
            #region 魔君套的抵抗魔力病buff效果
            else if (type == BuffID.ManaSickness && player.CalamityInheritance().AncientAuricSet)
            {
                player.GetDamage<MagicDamageClass>() *= 1.5f;
            }
            #endregion
            //下面只有开启了原版数值回调才会启用
            if(CIServerConfig.Instance.VanillaUnnerf)
            {
                #region 日耀套回调, 但...
                if (type >= BuffID.SolarShield1 && type <= BuffID.SolarShield3)
                {
                    /*
                    *需注意的是：我并不知道原灾是如何实现对日耀套免伤从乘算修为加算的
                    *而且我也不想通过原灾的方式迂回回调
                    *因此，这里我用了自己的办法,
                    *在这里这个buff检测的唯一作用是ban掉原灾的加算免伤加成
                    *以及启用我自己的方法
                    */
                    player.endurance -= 0.25f;
                    player.CalamityInheritance().SolarShieldEndurence = true; //启用
                }
                #endregion
                #region 甲虫攻速回调
                if (type >= BuffID.BeetleMight1 && type <= BuffID.BeetleMight3 && player.beetleOffense)
                {
                    int getOrbs = player.beetleOrbs < 0 ? 0 : player.beetleOrbs;
                    if (getOrbs > 3) getOrbs = 3;
                    //每一级回调5%, 最后会被补正成(5%->10%, 10%->20%, 15%->30%)
                    player.GetAttackSpeed<MeleeDamageClass>() += 0.05f * getOrbs; 
                }
                #endregion
                #region 甲虫免伤回调
                else if (type >= BuffID.BeetleEndurance1 && type <= BuffID.BeetleEndurance3 && player.beetleDefense)
                {
                    int getOrbs = player.beetleOrbs < 0 ? 0 : player.beetleOrbs;
                    if (getOrbs > 3) getOrbs = 3;
                    //由于灾厄免伤曲线摆在那里，因此这里的回调可能到最后也不会怎么样, //但总之我也尽可能做了
                    //每一级回调15%, 最后会被补正成(10%->15%, 20%->30%, 30%->45%)
                    player.endurance += 0.05f * getOrbs;
                }
                #endregion
                #region 伤害星云回调
                else if(type == BuffID.NebulaUpDmg1) player.GetDamage<MagicDamageClass>() += 0.075f;//回调至15%
                else if(type == BuffID.NebulaUpDmg2) player.GetDamage<MagicDamageClass>() += 0.150f;//回调至30%
                else if(type == BuffID.NebulaUpDmg3) player.GetDamage<MagicDamageClass>() += 0.225f;//回调至45%%
                #endregion
                #region 生命星云回调
                //生命星云回血回调, 现在也能ban掉负数的生命恢复
                else if(type == BuffID.NebulaUpLife1)
                {
                    //这里牢灾对生命星云的ban 即使debuff下回血的方法做的非常奇怪，因此这里直接将玩家低于0回血的时候置成一个固定的+1HP/s
                    //哦对了喝酒给了特判
                    if(player.lifeRegen < 0 && player.HasBuff(ModContent.BuffType<AlcoholPoisoning>())) player.lifeRegen = 2; 
                    else player.lifeRegen += 2; //2HP/s -> 4HP/s
                } 
                else if(type == BuffID.NebulaUpLife2)
                {
                    if(player.lifeRegen < 0 && player.HasBuff(ModContent.BuffType<AlcoholPoisoning>())) player.lifeRegen = 2; 
                    else player.lifeRegen += 4; //4HP/s -> 6HP/s
                }
                else if(type == BuffID.NebulaUpLife3)
                {
                    if(player.lifeRegen < 0 && player.HasBuff(ModContent.BuffType<AlcoholPoisoning>())) player.lifeRegen = 2; 
                    else player.lifeRegen += 6; //5HP/s -> 8HP/s
                }
                #endregion
                #region 魔力星云回调
                #endregion
                #region 一些其他的冗余
                switch(type)
                {
                    case BuffID.WellFed://一级饱食
                        player.GetAttackSpeed<MeleeDamageClass>() += 0.05f;
                        player.moveSpeed += 0.1f; //回10%， 由于原灾对移速机制重做， 直接+20%可能会有一些问题
                        break;
                    case BuffID.WellFed2:
                        player.GetAttackSpeed<MeleeDamageClass>() += 0.075f;
                        player.moveSpeed += 0.125f; //回12.5%， 由于原灾对移速机制重做， 直接+30%可能会有一些问题
                        break;
                    case BuffID.WellFed3:
                        player.GetAttackSpeed<MeleeDamageClass>() += 0.10f;
                        player.moveSpeed += 0.15f; //回15%， 由于原灾对移速机制重做， 直接+40%可能会有一些问题
                        break;
                    case BuffID.Panic:
                        player.moveSpeed += 0.6f; //回调
                        break;
                    case BuffID.Sharpened:
                        player.GetArmorPenetration<MeleeDamageClass>() += 7; //回调
                        break;
                    case BuffID.Archery:
                        player.arrowDamage *= 1.05f; //回调至10%乘算
                        break;
                    case BuffID.Tipsy:
                        int gDefesne = player.GetCurrentDefense();
                        int bDefense = (int)(gDefesne * 0.05f);
                        player.statDefense += bDefense - 4; //将5%的防御抵消后再-4, 即置成原版的-4防御效果
                        player.GetAttackSpeed<MeleeDamageClass>() += 0.10f;
                        player.GetCritChance<MeleeDamageClass>() += 2;
                        break;
                }
                #endregion
            }
        }
    }
}
