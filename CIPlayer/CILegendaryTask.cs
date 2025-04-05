using CalamityInheritance.Buffs.Legendary;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Projectiles.Melee;
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
using Terraria.ModLoader.IO;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer: ModPlayer
    {
        public void LegendarySaveData(ref TagCompound tag)
        {
            tag.Add("PBGTier1", PBGTier1);
            tag.Add("PBGTier2", PBGTier2);
            tag.Add("PBGTier3", PBGTier3);
            tag.Add("DukeTier1", DukeTier1);
            tag.Add("DukeTier2", DukeTier2);
            tag.Add("DukeTier3", DukeTier3);
            tag.Add("BetsyTier1", BetsyTier1);
            tag.Add("BetsyTier2", BetsyTier2);
            tag.Add("BetsyTier3", BetsyTier3);
            tag.Add("PlanteraTier1", PlanteraTier1);
            tag.Add("PlanteraTier2", PlanteraTier2);
            tag.Add("PlanteraTier3", PlanteraTier3);
            tag.Add("DestroyerTier1", DestroyerTier1);
            tag.Add("DestroyerTier2", DestroyerTier2);
            tag.Add("DestroyerTier3", DestroyerTier3);
            tag.Add("DefendTier1", DefendTier1);
            tag.Add("DefendTier2", DefendTier2);
            tag.Add("DefendTier3", DefendTier3);
        }
        public void LegendaryLoadData(ref TagCompound tag)
        {
            tag.TryGet("PBGTier1",        out PBGTier1);
            tag.TryGet("PBGTier2",        out PBGTier2);
            tag.TryGet("PBGTier3",        out PBGTier3);
            tag.TryGet("DukeTier1",       out DukeTier1);
            tag.TryGet("DukeTier2",       out DukeTier2);
            tag.TryGet("DukeTier3",       out DukeTier3);
            tag.TryGet("PlanteraTier1",   out PlanteraTier1);
            tag.TryGet("PlanteraTier2",   out PlanteraTier2);
            tag.TryGet("PlanteraTier3",   out PlanteraTier3);
            tag.TryGet("BetsyTier1",      out BetsyTier1);
            tag.TryGet("BetsyTier2",      out BetsyTier2);
            tag.TryGet("BetsyTier3",      out BetsyTier3);
            tag.TryGet("DestroyerTier1",  out DestroyerTier1);
            tag.TryGet("DestroyerTier2",  out DestroyerTier2);
            tag.TryGet("DestroyerTier3",  out DestroyerTier3);
            tag.TryGet("DefendTier1",     out DefendTier1);
            tag.TryGet("DefendTier2",     out DefendTier2);
            tag.TryGet("DefendTier3",     out DefendTier3);
        }
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

            //庇护之刃
            if (heldingItem.type == ModContent.ItemType<DefenseBlade>())
            {
                //T1
                if (DefendTier1)
                    DefenderBuff(target, hit, projectile);
            }

        }
        #region 传奇物品特殊效果(T3)
        private void DukeLegendaryBuff(NPC target, NPC.HitInfo hit)
        {
            //海爵剑T3：攻击综合增强属性()
            if (hit.Damage > 5)
                Player.AddBuff(ModContent.BuffType<DukeBuff>(), 30);
        }

        private void PBGLegendaryBuff(NPC target, NPC.HitInfo hit)
        {
            //T3孔雀翎: 攻击时将在接下来的15秒内提供buff，这个buff将会使你有1/10概率彻底无敌3秒
            if (target.life > 5 && hit.Damage > 5000 && GlobalLegendaryT3CD == 0)
            {
                Player.AddBuff(ModContent.BuffType<PBGBuff>(), 300);
                //给予45秒的CD
                GlobalLegendaryT3CD = 900;
            }
        }
        //庇护之刃T1：射弹击中敌人时，提升1%防御力，最高25%
        public void DefenderBuff(NPC target, NPC.HitInfo hit, Projectile projectile)
        {
            int proj = ModContent.ProjectileType<DefenseBeam>();
            if (target.life > 5 && projectile.type == proj || projectile.type == ModContent.ProjectileType<DefenseFlame>() || projectile.type == ModContent.ProjectileType<DefenseBlast>() && DefendTier1)
            {
                DefendTier1Timer = 180;
                DefenseBoost += 0.01f;
                if (DefenseBoost > 0.25f)
                    DefenseBoost = 0.25f;
                
            }
        }
        #endregion
        #region 传奇物品伤害任务
        //庇护之刃T2任务：手持庇护之刃承受超过2000点伤害
        //啊♂
        private void DefenseBladeTier2Task(Player.HurtInfo fk)
        {
            if (fk.Damage > 5)
                DefendTier2Pool += fk.Damage;
             
            if (DefendTier2Pool > 2000)
            {
                DefendTier2 = true;
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