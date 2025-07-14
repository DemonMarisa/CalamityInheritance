using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles
{
    public class CalamityInheritanceGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public float MinionDamageValue = 1f;
        public float MinionProjDamageValue = 0f;

        public bool AMRextra = false;
        public bool ignoreDrAndDef = false;
        // 10个额外射弹AI
        internal const int MaxAIMode = 10;
        public float[] ProjNewAI = new float[MaxAIMode];

        public bool AMRextraTy = false;
        public bool ThrownMode = false;
        //标记这个射弹为魔法伤害, 目前用于归元漩涡(原灾)消失后生成的星流光束的斩切标记
        public bool PingAsMagic = false;
        //用于主射弹与附属射弹的单一敌怪单位的针对
        public int GlobalMainProjForceTarget = -1;
        //禁用超高频武器降低纳米火花生成数量的标记(主要是及高频，太卡了)
        public bool PingReducedNanoFlare = false;
        public int PingBeamMagic = -1;
        public bool PingAsSplit = false;
        public bool PingWhipStrike = false;
        public int StoreEU = -1;
        // 1帧影响
        public bool oneFrameEffect = false;
        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            CalamityInheritancePlayer usPlayer = player.CIMod();
            CalamityPlayer calPlayer = player.Calamity();
            if (!projectile.npcProj && !projectile.trap && projectile.friendly && projectile.damage > 0)
            {
                // 元素箭袋的额外AI, ban掉了打表的弹幕
                if (projectile.DamageType == DamageClass.Ranged
                    && usPlayer.ElemQuiver && CalamityInheritanceLists.rangedProjectileExceptionList.TrueForAll(x => projectile.type != x)
                    && Vector2.Distance(projectile.Center, Main.player[projectile.owner].Center) > 200f)// 他妈200像素，你泰能有200像素的手持弹幕？？？
                    ElemQuiver(projectile);
            
                if (projectile.CountsAsClass<RogueDamageClass>())
                {
                    if (usPlayer.ReaverRogueExProj)
                    {
                        if (Main.player[projectile.owner].miscCounter % 60 == 0 && projectile.FinalExtraUpdate() && projectile.owner == Main.myPlayer)
                        {
                            int damage = (int)player.GetTotalDamage<RogueDamageClass>().ApplyTo(60);
                            //这里被平方增长了
                            int newProjectileId = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<PhotosyntheticShard>(), damage, 0f, projectile.owner);
                            Main.projectile[newProjectileId].DamageType = DamageClass.Generic;
                        }
                    }
                    //给了孔雀翎一个特判
                    if (usPlayer.nanotechold)
                    {
                        int actualTimer = projectile.type == ModContent.ProjectileType<PBGLegendaryBeam>() ? 90 : 30;
                        Vector2 setVel = projectile.velocity.SafeNormalize(Vector2.Zero);
                        if (Main.player[projectile.owner].miscCounter % actualTimer == 0 && projectile.FinalExtraUpdate())
                        {
                            if (projectile.owner == Main.myPlayer)
                            {
                                int p = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<NanotechOld>(), (int)(projectile.damage * 0.05) , 0f, projectile.owner);
                                //确保这个东西指定为全局伤害
                                Main.projectile[p].DamageType = DamageClass.Generic;
                                Main.projectile[p].alpha = 255;
                            }
                        }
                    }
                }
            }

            if(!oneFrameEffect)
            {
                if (usPlayer.DeadshotBroochCI && projectile.CountsAsClass<RangedDamageClass>() && player.heldProj != projectile.whoAmI)
                {
                    if (CalamityInheritanceLists.ProjNoCIdeadshotBrooch.TrueForAll(x => projectile.type != x))
                        projectile.extraUpdates += 1;

                    if (projectile.type == ProjectileID.MechanicalPiranha)
                    {
                        projectile.localNPCHitCooldown *= 2;
                        projectile.timeLeft *= 2;
                    }
                }
                oneFrameEffect = true;
            }
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[projectile.owner];
            CalamityInheritancePlayer usPlayer = player.CIMod();

            if (projectile.CountsAsClass<RogueDamageClass>() && projectile.Calamity().stealthStrike)
            {
                int gloveArmorPenAmt = 20;
                if (usPlayer.nanotechold)
                    projectile.ArmorPenetration += gloveArmorPenAmt;
            }

            if (AMRextra == true)
            {
                IEntitySource source = projectile.GetSource_FromThis();
                int extraProjectileAmt = 4;
                if (projectile.owner == Main.myPlayer)
                    for (int x = 0; x < extraProjectileAmt; x++)
                    {
                        bool fromRight = x > 2;
                        Projectile proj = CalamityUtils.ProjectileBarrage(source, projectile.Center, projectile.Center, fromRight, 500f, 500f, 0f, 500f, 10f, projectile.type, (int)(projectile.damage * 0.3f), projectile.knockBack, projectile.owner, false, 5f);
                        CalamityUtils.Calamity(proj).pointBlankShotDuration = 0;
                    }

                AMRextra = false;
            }
            if (AMRextraTy == true)
            {
                IEntitySource source = projectile.GetSource_FromThis();
                int extraProjectileAmt = 8;
                for (int x = 0; x < extraProjectileAmt; x++)
                {
                    if (projectile.owner == Main.myPlayer)
                    {
                        bool fromRight = x > 3;
                        Projectile proj = CalamityUtils.ProjectileBarrage(source, projectile.Center, projectile.Center, fromRight, 500f, 500f, 0f, 500f, 10f, projectile.type, (int)(projectile.damage * 0.15f), projectile.knockBack, projectile.owner, false, 5f);
                        CalamityUtils.Calamity(proj).pointBlankShotDuration = 0;
                    }
                }
                AMRextraTy = false;
            }

            if(ignoreDrAndDef)
            {
                //Avoid touching things that you probably aren't meant to damage
                if (modifiers.SuperArmor || target.defense > 999 || target.Calamity().DR >= 0.95f || target.Calamity().unbreakableDR)
                    return;
                //Bypass defense
                modifiers.DefenseEffectiveness *= 0f;
                modifiers.FinalDamage *= 1f / (1f - target.Calamity().DR);
            }
            LevelBoost(projectile, usPlayer);
        }
        #region 元素箭袋
        //哈哈，已经变成史山了
        public static void ElemQuiver(Projectile projectile)
        {

            //特判: ban掉分裂的弹幕
            if (projectile.CalamityInheritance().PingAsSplit)
                return;

            #region 大量转字段
            //转具有实际意义的字段方便可读……应该。
            const short StyleSplitOnceShorterLifeTime   = 1;
            const short StyleSplitOnceSameLifeTime      = 2;
            const short StyleSplitRandomShorterLifeTime = 3;
            const short StyleSplitRandomSameLifeTime    = 4;
            int mode = CIConfig.Instance.ElementalQuiverSplitstyle; 

            //转bool字段，减少下方的代码冗余
            bool splitOnce    = mode == StyleSplitOnceSameLifeTime   || mode == StyleSplitOnceShorterLifeTime;
            bool splitRandome = mode == StyleSplitRandomSameLifeTime || mode == StyleSplitRandomShorterLifeTime;
            //查阅是否超出最大弹幕上限（数组上限）
            bool checkIfOutIndex = projectile.owner == Main.myPlayer && Main.player[projectile.owner].ownedProjectileCounts[projectile.type] < 200;
            #endregion
            //下列是射弹的基础属性
            double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - MathHelper.PiOver2;
            int pDamage = (int)(projectile.damage * 0.5f);
            int i = -1;

            //这个是只分裂一次
            if (splitOnce && Main.player[projectile.owner].miscCounter % 60 == 0 && projectile.FinalExtraUpdate() && checkIfOutIndex)
            {
                //-1 -> 1, 用于修改生成方向
                for (; i < 2; i += 2)
                {
                    Vector2 vel = new ((float)(Math.Sin(startAngle) * 8.0 * i), (float)(Math.Cos(startAngle) * 8.0 * i));
                    int p = Projectile.NewProjectile(Entity.GetSource_None(), projectile.Center, vel, projectile.type, pDamage, projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                    if (mode == StyleSplitOnceShorterLifeTime)
                        Main.projectile[p].timeLeft = 60;
                    Main.projectile[p].noDropItem = true;
                    Main.projectile[p].CalamityInheritance().PingAsSplit = true;
                    Main.projectile[p].netUpdate = true;
                }
            }
            //随机分裂
            if (splitRandome && checkIfOutIndex && Main.rand.Next(200) > 198)
            {
                //同上
                for (; i < 2; i += 2)
                {
                    Vector2 vel = new ((float)(Math.Sin(startAngle) * 8.0 * i), (float)(Math.Cos(startAngle) * 8.0 * i));
                    int p = Projectile.NewProjectile(Entity.GetSource_None(), projectile.Center, vel, projectile.type, pDamage, projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                    Main.projectile[p].DamageType = DamageClass.Default;
                    if (mode == StyleSplitRandomShorterLifeTime)
                        Main.projectile[p].timeLeft = 60;
                    Main.projectile[p].noDropItem = true;
                    Main.projectile[p].CalamityInheritance().PingAsSplit = true;
                    Main.projectile[p].netUpdate = true;
                }
            }
        }
        #endregion
        public void LevelBoost(Projectile projectile, CalamityInheritancePlayer cIPlayer)
        {
            projectile.ArmorPenetration += cIPlayer.rangeLevel * 2;
        }
    }
}