using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.Bumblebirb;
using CalamityMod.NPCs.Polterghast;
using CalamityMod.NPCs.Providence;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer: ModPlayer
    {
        public void LegendaryDamageTask(Projectile projectile, NPC target, NPC.HitInfo hit)
        {
            Player player = Main.player[projectile.owner];
            var heldingItem = player.ActiveItem();
            //孔雀翎(T2)
            if (heldingItem.type == ModContent.ItemType<PBGLegendary>())
            {
                PBGLegendaryDamageTask(target, hit);
                if (PBGTier3)
                    PBGLegendaryBuff(target, hit);
            }
            //海爵剑(T2)
            if (heldingItem.type == ModContent.ItemType<DukeLegendary>())
            {
                DukeLegendaryDamageTask(target, hit);
                if (DukeTier3)
                    DukeLegendaryBuff(target, hit);
            }
            //维苏威阿斯(T2)
            if (heldingItem.type == ModContent.ItemType<RavagerLegendary>())
                RavagerLegendaryDamageTask(target, hit);

            //叶流(T2)
            if (heldingItem.type == ModContent.ItemType<PlanteraLegendary>())
                PlanteraLegendaryDamageTask(target, hit);

            //SHPC(T2)
            if (heldingItem.type == ModContent.ItemType<DestroyerLegendary>())
                DestroyerLegendaryDamageTask(target, hit, projectile);


        }
        #region 传奇物品特殊效果(T3)
        private void DukeLegendaryBuff(NPC target, NPC.HitInfo hit)
        {
            //海爵剑T3：持续攻击增强防御属性，最高增强50点防御力与40%伤害减免
            if (hit.Damage > 5 && DukeDefenseCounter >= 0)
            {
                //最大50层
                if (DukeDefenseCounter < 51)
                    DukeDefenseCounter++;
                //五秒
                DukeDefenseTimer = 300;
                //玩家的防御力视击中的次数提升
                Player.statDefense += DukeDefenseCounter;
                //每次攻击增加0.5%免伤，50次攻击后为25%
                Player.endurance += 0.005f * DukeDefenseCounter;
            }
        }

        private void PBGLegendaryBuff(NPC target, NPC.HitInfo hit)
        {
            //T3孔雀翎特殊效果：一次击中敌人超过5000伤害时，自身生命值被置零以下时，以一定概率，将自己的生命值强行置0以停止掉血, 这一效果仅短暂持续1秒
            if (target.life > 5 && hit.Damage > 5000 && GlobalLegendaryT3CD == 0 && !AncientSilvaSet)
            {
                //孔雀翎攻速极快，因此1/75概率才是最合适的
                if (Player.lifeRegen < 0 && Main.rand.NextBool(75))
                {
                    Player.lifeRegen = 0;
                    GlobalLegendaryT3CD = 60;
                }
            }
        }
        #endregion
        #region 传奇物品伤害任务
        private void DestroyerLegendaryDamageTask(NPC target, NPC.HitInfo hit, Projectile projectile)
        {
            //T2:在四柱期间内，对任意四根天界柱造成合计一根天界柱的最大血量的250%伤害
            NPC towerMark = Main.npc[NPCID.LunarTowerSolar];
            if ((target.type == NPCID.LunarTowerStardust || target.type == NPCID.LunarTowerSolar || target.type == NPCID.LunarTowerNebula || target.type == NPCID.LunarTowerVortex) &&
                 projectile.DamageType == DamageClass.Magic && !DestroyerTier2)
            {
                DamagePool += hit.Damage;
                if (DamagePool > towerMark.lifeMax * 2.5f)
                {
                    DestroyerTier2 = true;
                    //这里应该需要一个诺法雷的充能音效
                }
            }
        }
        private void PlanteraLegendaryDamageTask(NPC target, NPC.HitInfo hit)
        {
            var usPlayer = Player.CIMod();
            //T2: 金龙30%伤害
            if (target.type == ModContent.NPCType<Bumblefuck>() && !PlanteraTier2 && Main.LocalPlayer.ZoneJungle)
            {
                DamagePool += hit.Damage;
                if (hit.Damage > target.life * 0.3f)
                {
                    CIFunction.DustCircle(Player.Center, 32f, 1.8f, DustID.DryadsWard, true, 10f);
                    SoundEngine.PlaySound(CISoundID.SoundFallenStar with { Volume = .5f }, Player.Center);
                    usPlayer.PlanteraTier2 = true;
                    DamagePool = 0;
                }
            }
        }

        private void RavagerLegendaryDamageTask(NPC target, NPC.HitInfo hit)
        {
            var usPlayer = Player.CIMod();
            //T2: 在地狱对亵渎天神造成50%伤害
            if (target.type == ModContent.NPCType<Providence>() && !BetsyTier2 && Main.LocalPlayer.ZoneUnderworldHeight)
            {
                DamagePool += hit.Damage;
                if (usPlayer.DamagePool >= target.lifeMax * 0.5f)
                {
                    CIFunction.DustCircle(Player.Center, 32f, 1.8f, DustID.Meteorite, true, 10f);
                    SoundEngine.PlaySound(CISoundID.SoundBomb with { Volume = .5f }, Player.Center);
                    BetsyTier2 = true;
                    DamagePool = 0;
                }
            }
        }

        private void DukeLegendaryDamageTask(NPC target, NPC.HitInfo hit)
        {
            var usPlayer = Player.CIMod();
            //T2: 海爵剑杀死一只猎魂鲨
            if (target.type == ModContent.NPCType<ReaperShark>() && !DukeTier2)
            {
                usPlayer.DamagePool += hit.Damage;
                if (usPlayer.DamagePool > target.lifeMax * 0.8f)
                {
                    CIFunction.DustCircle(Player.Center, 32f, 1.8f, DustID.Water, true, 10f);
                    SoundEngine.PlaySound(SoundID.NPCDeath19 with { Volume = .5f }, Player.Center);
                    //记得清空伤害池子，因为这个是共用的
                    usPlayer.DamagePool = 0;
                    usPlayer.DukeTier2 = true;
                }
            }
        }

        private void PBGLegendaryDamageTask(NPC target, NPC.HitInfo hit)
        {
            var usPlayer = Player.CIMod();
            //T2: 使用孔雀翎对噬魂幽花造成最后一击
            if (target.type == ModContent.NPCType<Polterghast>() && hit.Damage > target.life && PBGTier2)
            {
                CIFunction.DustCircle(Player.Center, 32f, 1.8f, DustID.TerraBlade, true, 10f);
                SoundEngine.PlaySound(CISoundID.SoundFallenStar with { Volume = .5f }, Player.Center);
                PBGTier2 = true;
            }
        }
        #endregion
    }
}