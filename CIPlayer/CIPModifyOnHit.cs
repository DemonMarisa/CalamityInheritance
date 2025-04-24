using System;
using CalamityInheritance.Buffs.StatDebuffs;
using CalamityInheritance.Content.Projectiles.Magic;
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

            modifiers.ModifyHitInfo += (ref NPC.HitInfo hitInfo) =>
            {
                if (GodSlayerRangedSet && hitInfo.Crit && proj.DamageType == DamageClass.Ranged)
                {
                    int randomChance = (int)(Player.GetTotalCritChance(DamageClass.Ranged) - 100);
                    if (randomChance > 1)
                    {
                        if(Main.rand.Next(1,101) <= randomChance)
                        {
                            hitInfo.Damage *= 2;
                        }
                    }
                    else
                    {
                        if (Main.rand.NextBool(20))
                        {
                            hitInfo.Damage *= 4;
                        }
                    }
                }
                else if (proj.type == ModContent.ProjectileType<HeliumFlashBlastLegacy>() && hitInfo.Crit && proj.DamageType == DamageClass.Magic)
                {
                    int getOverCrtis = (int)(Player.GetTotalCritChance(DamageClass.Magic) - 100);
                    if(getOverCrtis > 1)
                    {
                        hitInfo.Damage *= Main.rand.Next(1,101) <= getOverCrtis? 2 : 1;
                    }
                }
                else if (PerunofYharimStats && hitInfo.Crit)
                {
                    int getOverCrtis = (int)(Player.GetTotalCritChance(DamageClass.Generic) - 100);
                    if(getOverCrtis > 1)
                    {
                        hitInfo.Damage *= Main.rand.Next(1,101) <= getOverCrtis? 2 : 1;
                    }
                }
            };
            if (SilvaMeleeSetLegacy)
            {
                if (Main.rand.NextBool(4) && (proj.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>() || proj.type == ModContent.ProjectileType<StepToolShadowChair>()))
                {
                    modifiers.ModifyHitInfo += (ref NPC.HitInfo hitInfo) =>
                    {
                        hitInfo.Damage *= 5;
                    };
                }
            }

            if (CIConfig.Instance.silvastun == true)
            {
                if (proj.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>() && SilvaStunDebuffCooldown <= 0 && SilvaMeleeSetLegacy && Main.rand.NextBool(4))
                {
                    //Main.NewText($"触发眩晕TMp", 255, 255, 255);
                    target.AddBuff(ModContent.BuffType<SilvaStun>(), 20);
                    SilvaStunDebuffCooldown = 1800;
                }
                if (proj.DamageType == DamageClass.Melee && SilvaStunDebuffCooldown <= 0 && SilvaMeleeSetLegacy && Main.rand.NextBool(4))
                {
                    //Main.NewText($"触发眩晕mp", 255, 255, 255);
                    target.AddBuff(ModContent.BuffType<SilvaStun>(), 20);
                    SilvaStunDebuffCooldown = 1800;
                }
            }
            ModifyCrtis(target, ref modifiers);
        }

        private void ModifyCrtis(NPC target, ref NPC.HitModifiers modifiers)
        {
            float totalCritsBuff = 0f;
            //远古鲨牙项链获得30%的暴击伤害加成。
            if (SpeedrunNecklace)
                totalCritsBuff += 0.3f;
            
            modifiers.CritDamage += totalCritsBuff;
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            CalamityPlayer calPlayer = Player.Calamity();
            CalamityInheritancePlayer usPlayer = Player.CIMod();
            ModifyCrtis(target, ref modifiers);

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
                    //Main.NewText($"造成伤害", 255, 255, 255);
                    modifiers.ModifyHitInfo += (ref NPC.HitInfo hitInfo) =>
                    {
                        hitInfo.Damage *= 5;
                    };
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