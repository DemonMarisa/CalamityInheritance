using System;
using CalamityInheritance.Buffs.Legendary;
using CalamityInheritance.Buffs.StatDebuffs;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Content.Projectiles.Summon;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.NPCs.Abyss;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer: ModPlayer
    {
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            CalamityPlayer calPlayer = Player.Calamity();
            CalamityInheritancePlayer usPlayer = Player.CIMod();

            if (Player.name == "TrueScarlet" || Player.name == "FakeAqua")
            {
                if ((usPlayer.SCalLore || usPlayer.PanelsSCalLore )&& target.type == ModContent.NPCType<ReaperShark>())
                {
                    modifiers.SetInstantKill();
                }
            }

            //T3效果：寒冰神性最后结算时总会附加造成射弹初始伤害的1/8，这是一个防后效果
            //如果敌怪附加低温虹吸，则将伤害提高为完整的射弹初始伤害1/4
            if (IsColdDivityActiving && ColdDivityTier3 && proj.type == ModContent.ProjectileType<CryogenPtr>())
            {
                int dmg = proj.damage / 9;
                if (target.HasBuff(ModContent.BuffType<CryoDrain>()))
                    dmg += proj.damage / 9;
                if (proj.CalamityInheritance().PingWhipStrike)
                    dmg /= 2;
                modifiers.FinalDamage += dmg;
            }

            if (SilvaMeleeSetLegacy)
            {
                if (Main.rand.NextBool(4) && proj.TrueMeleeClass())
                {
                    modifiers.FinalDamage *= 5;
                }
            }

            ModifyCrtis(proj ,target, ref modifiers);

            if (GodSlayerRangedSet && proj.DamageType.CountsAsClass<RangedDamageClass>())
            {
                int randomChance = (int)(Player.GetTotalCritChance(DamageClass.Ranged) - 100);

                if(randomChance > 0)
                {
                    if (Main.rand.Next(1, 101) <= randomChance)
                        modifiers.FinalDamage *= 2;
                }
                else if (Main.rand.NextBool(20))
                    modifiers.FinalDamage *= 4;
            }
        }
        public float GetWantedCrits<Type>() where Type: DamageClass
        {
            return (Player.GetTotalCritChance<Type>() + 4f - 100f) / 100f;
        }
        public void ModifyCrtis(object anyDamageSrc, NPC target, ref NPC.HitModifiers modifiers)
        {
            //职业伤害类型的判定。
            bool isRogue = anyDamageSrc.WantedDamageClass<RogueDamageClass>();
            bool isMagic = anyDamageSrc.WantedDamageClass<MagicDamageClass>();
            //日食魔镜强制暴击。
            //这个请先于之前所有计算执行，不然他吃不完所有的爆伤加成
            if (EMirror && isRogue && Player.CheckStealth())
                modifiers.SetCrit();

            #region 暴伤乘区
            float totalCritsBuff = 0f;
            //氦闪爆伤加成
            if (Player.ActiveItem().type == ModContent.ItemType<HeliumFlashLegacy>())
            {
                int chanceToSupreCrit = (int)Player.GetTotalCritChance<MagicDamageClass>() - 100 + 4;
                if (Main.rand.Next(1, 101) <= chanceToSupreCrit && chanceToSupreCrit > 1)
                    totalCritsBuff += 1f;
            }
            //魔君之怒，这个是全局加成。
            if (PerunofYharimStats)
            {
                float giveBuff = GetWantedCrits<GenericDamageClass>();
                totalCritsBuff += giveBuff;
            }
            //远古鲨牙项链获得30%的暴击伤害加成。
            if (SpeedrunNecklace)
                totalCritsBuff += 0.3f;
            //除非特殊，不然不要尝试在基于暴击概率上给爆伤的计算里面试图不取溢出暴击概率计算
            //但凡多10%爆伤加成都是翻倍的输出
            if (OverloadManaPower && Player.statMana > Player.statManaMax2 / 2 && isMagic)
            {
                //这个totalCrtis是不会算初始的4%暴击的，这里补上
                float giveBuff = Player.GetTotalCritChance<MagicDamageClass>() + 4f - 100f;
                if (giveBuff > 0f)
                {
                    //转化为1f
                    giveBuff /= 100f; 
                    //最后除以10. 取1/10
                    giveBuff /= 8f;
                    //200%暴击概率 -> 12.5%爆伤加成
                    totalCritsBuff += giveBuff;
                }

            }
            if (EMirror && isRogue)
            {
                float giveBuff = Player.GetTotalCritChance<RogueDamageClass>() + 4f - 100f;
                if (giveBuff > 0f)
                {
                    giveBuff /= 100f; 
                    //最后除以7. 取1/7
                    giveBuff /= 7f;
                    //200%暴击概率 -> 15%
                    totalCritsBuff = giveBuff;
                    //日食魔镜会一定程度上贯穿敌怪防御
                    modifiers.DefenseEffectiveness *= 0.90f;
                }
            }

            #endregion
            //给予爆伤加成            
            modifiers.CritDamage += totalCritsBuff;
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            CalamityPlayer calPlayer = Player.Calamity();
            CalamityInheritancePlayer usPlayer = Player.CIMod();
            ModifyCrtis(item, target, ref modifiers);

            if (Player.name == "TrueScarlet" || Player.name == "FakeAqua")
            {
                if ((usPlayer.SCalLore || usPlayer.PanelsSCalLore) && target.type == ModContent.NPCType<ReaperShark>())
                {
                    modifiers.SetInstantKill();
                }
            }

            if (SilvaMeleeSetLegacy)
            {
                //Main.NewText($"触发判定", 255, 255, 255);
                if (Main.rand.NextBool(4) && item.DamageType == DamageClass.Melee || item.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>())
                {
                    modifiers.FinalDamage *= 5;
                }
            }

            if (CIConfig.Instance.silvastun == true)
            {
                if (item.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>() && SilvaStunDebuffCooldown <= 0 && SilvaMeleeSetLegacy && Main.rand.NextBool(4))
                {
                    //Main.NewText($"触发眩晕im", 255, 255, 255);
                    target.AddBuff(ModContent.BuffType<SilvaStun>(), 20);
                    SilvaStunDebuffCooldown = 1800;
                }
            }

            var source = Player.GetSource_ItemUse(item);

            if (item.DamageType == DamageClass.Melee)
            {
                BuffStatsTitanScaleTrueMelee = 600;
            }
        }
        public void ModifyHitNPCBoth(Projectile proj, NPC target, ref NPC.HitModifiers modifiers, DamageClass damageClass)
        {
            CalamityInheritancePlayer modPlayer = Player.CIMod();
            if (Player.name == "TrueScarlet" || Player.name == "FakeAqua")
            {
                if ((modPlayer.SCalLore || modPlayer.PanelsSCalLore) && target.type == ModContent.NPCType<ReaperShark>())
                {
                    modifiers.SetInstantKill();
                }
            }
        }
    }
}