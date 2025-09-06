using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
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

        public bool PingWhipStrike = false;
        public int StoreEU = -1;
        public bool PingRampartFallenStar = false;
        // 1帧影响
        public bool oneFrameEffect = false;
        public bool IfR99 = false;
        public int R99TargetIndex = -1;
        public int CurR99Chance = 0;
        public bool MouseRight = false;
        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            CalamityInheritancePlayer usPlayer = player.CIMod();
            CalamityPlayer calPlayer = player.Calamity();
            if (!projectile.npcProj && !projectile.trap && projectile.friendly && projectile.damage > 0)
            {

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
                                int p = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<NanotechOldProj>(), (int)(projectile.damage * 0.05), 0f, projectile.owner);
                                //确保这个东西指定为全局伤害
                                Main.projectile[p].DamageType = DamageClass.Generic;
                                Main.projectile[p].alpha = 255;
                            }
                        }
                    }
                }
            }

            if (!oneFrameEffect)
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
            if (IfR99)
            {
                //R99无视防御与dr
                modifiers.DefenseEffectiveness *= 0f;
                modifiers.FinalDamage *= 1f / (1f - target.Calamity().DR);
            }
            LevelBoost(projectile, usPlayer);
        }

        public static void LevelBoost(Projectile projectile, CalamityInheritancePlayer cIPlayer)
        {
            projectile.ArmorPenetration += cIPlayer.rangeLevel * 2;
        }
    }
}