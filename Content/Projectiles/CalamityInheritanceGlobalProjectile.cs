using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Summon;
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

        private bool frameOneHacksExecuted = false;
        public float MinionDamageValue = 1f;
        public float MinionProjDamageValue = 0f;

        public bool AMRextra = false;

        public bool AMRextraTy = false;
        public bool ThrownMode = false;
        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            CalamityInheritancePlayer modPlayer = player.CIMod();
            CalamityPlayer modPlayer1 = player.Calamity();
            if (!projectile.npcProj && !projectile.trap && projectile.friendly && projectile.damage > 0)
            {
                // 元素箭袋的额外AI
                if(projectile.DamageType == DamageClass.Ranged)
                    ElemQuiver(projectile);
            }
            if (!projectile.npcProj && !projectile.trap && projectile.friendly && projectile.damage > 0)
            {
                //回调原版所有悠悠球的无敌帧
                //注意其他方面都不会回调，只回调了无敌帧，但也足够了
                //砍无敌帧太傻逼了，纯纯砍手感的
                switch(projectile.type)
                {
                    case ProjectileID.JungleYoyo:
                        projectile.localNPCHitCooldown = 10;
                        break;
                    case ProjectileID.Amarok:
                        projectile.localNPCHitCooldown = 10;
                        break;
                    case ProjectileID.CrimsonYoyo:
                        projectile.localNPCHitCooldown = 10;
                        break;
                    case ProjectileID.Chik:
                        projectile.localNPCHitCooldown = 10;
                        break;
                    case ProjectileID.Code1:
                        projectile.localNPCHitCooldown = 10;
                        break;
                    case ProjectileID.Code2:
                        projectile.localNPCHitCooldown = 10;
                        break;
                    case ProjectileID.FormatC:
                        projectile.localNPCHitCooldown = 10;
                        break;
                    case ProjectileID.Gradient:
                        projectile.localNPCHitCooldown = 10;
                        break;
                    case ProjectileID.HiveFive:
                        projectile.localNPCHitCooldown = 10;
                        break;
                    case ProjectileID.CorruptYoyo:
                        projectile.localNPCHitCooldown = 10;
                        break;
                    case ProjectileID.RedsYoyo:
                        projectile.localNPCHitCooldown = 10;
                        break;
                    case ProjectileID.ValkyrieYoyo:
                        projectile.localNPCHitCooldown = 10;
                        break;
                    case ProjectileID.Rally:
                        projectile.localNPCHitCooldown = 10;
                        break;
                    case ProjectileID.Valor:
                        projectile.localNPCHitCooldown = 10;
                        break;
                    case ProjectileID.Yelets:
                        projectile.localNPCHitCooldown = 10;
                        break;
                    case ProjectileID.WoodYoyo:
                        projectile.localNPCHitCooldown = 10;
                        break;
                }
            }
            if (!projectile.npcProj && !projectile.trap && projectile.friendly && projectile.damage > 0)
            {
                if (projectile.CountsAsClass<RogueDamageClass>())
                {
                    if (modPlayer.ReaverRogueExProj)
                    {
                        if (Main.player[projectile.owner].miscCounter % 60 == 0 && 
                            projectile.FinalExtraUpdate())
                        {
                            if (projectile.owner == Main.myPlayer)
                            {
                                int damage = (int)player.GetTotalDamage<RogueDamageClass>().ApplyTo(60);
                                //这里被平方增长了
                                //damage = player.ApplyArmorAccDamageBonusesTo(damage);
                                int newProjectileId = Projectile.NewProjectile(projectile.GetSource_FromThis(),
                                                                               projectile.Center, Vector2.Zero,
                                                                               ModContent.ProjectileType<PhotosyntheticShard>(),
                                                                               damage, 0f, projectile.owner);
                                Main.projectile[newProjectileId].DamageType = DamageClass.Generic;
                            }
                        }
                    }
                    //给了孔雀翎一个特判
                    if (modPlayer.nanotechold && projectile.type != ModContent.ProjectileType<PBGLegendaryBeam>())
                    {
                        if (Main.player[projectile.owner].miscCounter % 30 == 0 && projectile.FinalExtraUpdate())
                        {
                            if (projectile.owner == Main.myPlayer)
                            {
                                int p = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<NanotechOld>(), (int)(projectile.damage * 0.15) , 0f, projectile.owner);
                                //确保这个东西指定为全局伤害
                                Main.projectile[p].DamageType = DamageClass.Generic;
                            }
                        }
                    }
                }
            }
            if (!frameOneHacksExecuted)
            {
                if (modPlayer.DeadshotBroochCI && projectile.CountsAsClass<RangedDamageClass>() && player.heldProj != projectile.whoAmI)
                {
                    if (CalamityInheritanceLists.ProjNoCIdeadshotBrooch.TrueForAll(x => projectile.type != x))
                        projectile.extraUpdates += 1;

                    if (projectile.type == ProjectileID.MechanicalPiranha)
                    {
                        projectile.localNPCHitCooldown *= 2;
                        projectile.timeLeft *= 2;
                    }
                }

                if (projectile.CountsAsClass<RogueDamageClass>() && projectile.Calamity().stealthStrike)
                {
                    int gloveArmorPenAmt = 20;
                    if (modPlayer.nanotechold)
                        projectile.ArmorPenetration += gloveArmorPenAmt;
                }

                frameOneHacksExecuted = true;
            }
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[projectile.owner];
            CalamityInheritancePlayer modPlayer = player.CIMod();

            modifiers.ModifyHitInfo += (ref NPC.HitInfo hitInfo) =>
            {

                if (AMRextra == true && hitInfo.Crit)
                {
                    IEntitySource source = projectile.GetSource_FromThis();
                    int extraProjectileAmt = 4;
                    for (int x = 0; x < extraProjectileAmt; x++)
                    {
                        if (projectile.owner == Main.myPlayer)
                        {
                            bool fromRight = x > 2;
                            Projectile proj = CalamityUtils.ProjectileBarrage(source, projectile.Center, projectile.Center, fromRight, 500f, 500f, 0f, 500f, 10f, projectile.type, (int)(projectile.damage * 0.3f), projectile.knockBack, projectile.owner, false, 5f);
                            CalamityUtils.Calamity(proj).pointBlankShotDuration = 0;
                        }
                    }
                    AMRextra = false;
                }
                if (AMRextraTy == true && hitInfo.Crit)
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
            };
        }
        public void ElemQuiver(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            CalamityInheritancePlayer modPlayer = player.CIMod();
            CalamityPlayer modPlayer1 = player.Calamity();

            if (modPlayer.ElemQuiver
                && CalamityInheritanceLists.rangedProjectileExceptionList.TrueForAll(x => projectile.type != x)
                && Vector2.Distance(projectile.Center, Main.player[projectile.owner].Center) > 100f)
            {
                if (CIConfig.Instance.ElementalQuiverSplitstyle == 1)
                {
                    if (Main.player[projectile.owner].miscCounter % 60 == 0 && projectile.FinalExtraUpdate())
                    {
                        float spread = 180f * 0.0174f;
                        double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - (double)(spread / 2f);
                        if (projectile.owner == Main.myPlayer && Main.player[projectile.owner].ownedProjectileCounts[projectile.type] < 200)
                        {
                            int projectile2 = Projectile.NewProjectile(Entity.GetSource_None(), projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(startAngle) * 8.0), (float)(Math.Cos(startAngle) * 8.0), projectile.type, (int)(projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                            int projectile3 = Projectile.NewProjectile(Entity.GetSource_None(), projectile.Center.X, projectile.Center.Y, (float)((double)(0f - (float)Math.Sin(startAngle)) * 8.0), (float)((double)(0f - (float)Math.Cos(startAngle)) * 8.0), projectile.type, (int)(projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                            Main.projectile[projectile2].DamageType = DamageClass.Default;
                            Main.projectile[projectile3].DamageType = DamageClass.Default;
                            Main.projectile[projectile2].timeLeft = 60;
                            Main.projectile[projectile3].timeLeft = 60;
                            Main.projectile[projectile2].noDropItem = true;
                            Main.projectile[projectile3].noDropItem = true;
                        }
                    }
                }
                if (CIConfig.Instance.ElementalQuiverSplitstyle == 2)
                {
                    if (Main.player[projectile.owner].miscCounter % 60 == 0 && projectile.FinalExtraUpdate())
                    {
                        float spread = 180f * 0.0174f;
                        double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - (double)(spread / 2f);
                        if (projectile.owner == Main.myPlayer && Main.player[projectile.owner].ownedProjectileCounts[projectile.type] < 200)
                        {
                            int projectile2 = Projectile.NewProjectile(Entity.GetSource_None(), projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(startAngle) * 8.0), (float)(Math.Cos(startAngle) * 8.0), projectile.type, (int)(projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                            int projectile3 = Projectile.NewProjectile(Entity.GetSource_None(), projectile.Center.X, projectile.Center.Y, (float)((double)(0f - (float)Math.Sin(startAngle)) * 8.0), (float)((double)(0f - (float)Math.Cos(startAngle)) * 8.0), projectile.type, (int)(projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                            Main.projectile[projectile2].DamageType = DamageClass.Default;
                            Main.projectile[projectile3].DamageType = DamageClass.Default;
                            Main.projectile[projectile2].noDropItem = true;
                            Main.projectile[projectile3].noDropItem = true;
                        }
                    }
                }
                if (CIConfig.Instance.ElementalQuiverSplitstyle == 3)
                {
                    if (Main.rand.Next(200) > 198)
                    {
                        float spread = 180f * 0.0174f;
                        double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - (double)(spread / 2f);
                        if (projectile.owner == Main.myPlayer && Main.player[projectile.owner].ownedProjectileCounts[projectile.type] < 200)
                        {
                            int projectile2 = Projectile.NewProjectile(Entity.GetSource_None(), projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(startAngle) * 8.0), (float)(Math.Cos(startAngle) * 8.0), projectile.type, (int)(projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                            int projectile3 = Projectile.NewProjectile(Entity.GetSource_None(), projectile.Center.X, projectile.Center.Y, (float)((double)(0f - (float)Math.Sin(startAngle)) * 8.0), (float)((double)(0f - (float)Math.Cos(startAngle)) * 8.0), projectile.type, (int)(projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                            Main.projectile[projectile2].DamageType = DamageClass.Default;
                            Main.projectile[projectile3].DamageType = DamageClass.Default;
                            Main.projectile[projectile2].timeLeft = 60;
                            Main.projectile[projectile3].timeLeft = 60;
                            Main.projectile[projectile2].noDropItem = true;
                            Main.projectile[projectile3].noDropItem = true;
                        }
                    }
                }
                if (CIConfig.Instance.ElementalQuiverSplitstyle == 4)
                {
                    if (Main.rand.Next(200) > 198)
                    {
                        float spread = 180f * 0.0174f;
                        double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - (double)(spread / 2f);
                        if (projectile.owner == Main.myPlayer && Main.player[projectile.owner].ownedProjectileCounts[projectile.type] < 200)
                        {
                            int projectile2 = Projectile.NewProjectile(Entity.GetSource_None(), projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(startAngle) * 8.0), (float)(Math.Cos(startAngle) * 8.0), projectile.type, (int)(projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                            int projectile3 = Projectile.NewProjectile(Entity.GetSource_None(), projectile.Center.X, projectile.Center.Y, (float)((double)(0f - (float)Math.Sin(startAngle)) * 8.0), (float)((double)(0f - (float)Math.Cos(startAngle)) * 8.0), projectile.type, (int)(projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                            Main.projectile[projectile2].DamageType = DamageClass.Default;
                            Main.projectile[projectile3].DamageType = DamageClass.Default;
                            Main.projectile[projectile2].noDropItem = true;
                            Main.projectile[projectile3].noDropItem = true;
                        }
                    }
                }
            }
        }
    }
}