using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Healing;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Typeless;
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
        // Force Class Types
        public bool forceMelee = false;
        public bool forceRanged = false;
        public bool forceMagic = false;
        public bool forceRogue = false;
        public bool forceMinion = false;
        public bool forceHostile = false;
        public bool rogue = false;
        public bool forceTypeless = false;

        public override bool InstancePerEntity => true;

        private bool frameOneHacksExecuted = false;
        public override void AI(Projectile projectile)
        {
            if (rogue)
            {
                projectile.DamageType = (DamageClass)(object)ModContent.GetInstance<RogueDamageClass>();
            }
            if (forceMelee)
            {
                projectile.hostile = false;
                projectile.friendly = true;
                projectile.DamageType = DamageClass.Melee;
            }
            else if (forceRanged)
            {
                projectile.hostile = false;
                projectile.friendly = true;
                projectile.DamageType = DamageClass.Ranged;
            }
            else if (forceMagic)
            {
                projectile.hostile = false;
                projectile.friendly = true;
                projectile.DamageType = DamageClass.Magic;
                rogue = false;
            }
            else if (forceMinion)
            {
                projectile.hostile = false;
                projectile.friendly = true;
                projectile.DamageType = DamageClass.Summon;
                rogue = false;
            }
            else if (forceRogue)
            {
                projectile.hostile = false;
                projectile.friendly = true;
                projectile.DamageType = DamageClass.Throwing;
            }
            else if (forceTypeless)
            {
                projectile.hostile = false;
                projectile.friendly = true;
            }
            else if (forceHostile)
            {
                projectile.hostile = true;
                projectile.friendly = false;
            }

            Player player = Main.player[projectile.owner];
            CalamityInheritancePlayer modPlayer = player.CalamityInheritance();
            CalamityPlayer modPlayer1 = player.Calamity();
            if (!projectile.npcProj && !projectile.trap && projectile.friendly && projectile.damage > 0)
            {
                if (modPlayer.ElementalQuiver && projectile.DamageType == DamageClass.Ranged && CalamityInheritanceLists.rangedProjectileExceptionList.TrueForAll(x => projectile.type != x))
                {
                    if (CalamityInheritanceConfig.Instance.ElementalQuiverSplitstyle == 1)
                    {
                        if (Main.player[projectile.owner].miscCounter % 60 == 0 && projectile.FinalExtraUpdate())
                        {
                            float spread = 180f * 0.0174f;
                            double startAngle = Math.Atan2(((Entity)projectile).velocity.X, ((Entity)projectile).velocity.Y) - (double)(spread / 2f);
                            if (projectile.owner == Main.myPlayer && Main.player[projectile.owner].ownedProjectileCounts[projectile.type] < 200)
                            {
                                int projectile2 = Projectile.NewProjectile(Entity.GetSource_None(), ((Entity)projectile).Center.X, ((Entity)projectile).Center.Y, (float)(Math.Sin(startAngle) * 8.0), (float)(Math.Cos(startAngle) * 8.0), projectile.type, (int)((double)projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                                int projectile3 = Projectile.NewProjectile(Entity.GetSource_None(), ((Entity)projectile).Center.X, ((Entity)projectile).Center.Y, (float)((double)(0f - (float)Math.Sin(startAngle)) * 8.0), (float)((double)(0f - (float)Math.Cos(startAngle)) * 8.0), projectile.type, (int)((double)projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                                Main.projectile[projectile2].DamageType = DamageClass.Default;
                                Main.projectile[projectile3].DamageType = DamageClass.Default;
                                Main.projectile[projectile2].timeLeft = 60;
                                Main.projectile[projectile3].timeLeft = 60;
                                Main.projectile[projectile2].noDropItem = true;
                                Main.projectile[projectile3].noDropItem = true;
                            }
                        }
                    }
                    if (CalamityInheritanceConfig.Instance.ElementalQuiverSplitstyle == 2)
                    {
                        if (Main.player[projectile.owner].miscCounter % 60 == 0 && projectile.FinalExtraUpdate())
                        {
                            float spread = 180f * 0.0174f;
                            double startAngle = Math.Atan2(((Entity)projectile).velocity.X, ((Entity)projectile).velocity.Y) - (double)(spread / 2f);
                            if (projectile.owner == Main.myPlayer && Main.player[projectile.owner].ownedProjectileCounts[projectile.type] < 200)
                            {
                                int projectile2 = Projectile.NewProjectile(Entity.GetSource_None(), ((Entity)projectile).Center.X, ((Entity)projectile).Center.Y, (float)(Math.Sin(startAngle) * 8.0), (float)(Math.Cos(startAngle) * 8.0), projectile.type, (int)((double)projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                                int projectile3 = Projectile.NewProjectile(Entity.GetSource_None(), ((Entity)projectile).Center.X, ((Entity)projectile).Center.Y, (float)((double)(0f - (float)Math.Sin(startAngle)) * 8.0), (float)((double)(0f - (float)Math.Cos(startAngle)) * 8.0), projectile.type, (int)((double)projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                                Main.projectile[projectile2].DamageType = DamageClass.Default;
                                Main.projectile[projectile3].DamageType = DamageClass.Default;
                                Main.projectile[projectile2].noDropItem = true;
                                Main.projectile[projectile3].noDropItem = true;
                            }
                        }
                    }
                    if (CalamityInheritanceConfig.Instance.ElementalQuiverSplitstyle == 3)
                    {
                        if (Main.rand.Next(200) > 198)
                        {
                            float spread = 180f * 0.0174f;
                            double startAngle = Math.Atan2(((Entity)projectile).velocity.X, ((Entity)projectile).velocity.Y) - (double)(spread / 2f);
                            if (projectile.owner == Main.myPlayer && Main.player[projectile.owner].ownedProjectileCounts[projectile.type] < 200)
                            {
                                int projectile2 = Projectile.NewProjectile(Entity.GetSource_None(), ((Entity)projectile).Center.X, ((Entity)projectile).Center.Y, (float)(Math.Sin(startAngle) * 8.0), (float)(Math.Cos(startAngle) * 8.0), projectile.type, (int)((double)projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                                int projectile3 = Projectile.NewProjectile(Entity.GetSource_None(), ((Entity)projectile).Center.X, ((Entity)projectile).Center.Y, (float)((double)(0f - (float)Math.Sin(startAngle)) * 8.0), (float)((double)(0f - (float)Math.Cos(startAngle)) * 8.0), projectile.type, (int)((double)projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                                Main.projectile[projectile2].DamageType = DamageClass.Default;
                                Main.projectile[projectile3].DamageType = DamageClass.Default;
                                Main.projectile[projectile2].timeLeft = 60;
                                Main.projectile[projectile3].timeLeft = 60;
                                Main.projectile[projectile2].noDropItem = true;
                                Main.projectile[projectile3].noDropItem = true;
                            }
                        }
                    }
                    if (CalamityInheritanceConfig.Instance.ElementalQuiverSplitstyle == 4)
                    {
                        if (Main.rand.Next(200) > 198)
                        {
                            float spread = 180f * 0.0174f;
                            double startAngle = Math.Atan2(((Entity)projectile).velocity.X, ((Entity)projectile).velocity.Y) - (double)(spread / 2f);
                            if (projectile.owner == Main.myPlayer && Main.player[projectile.owner].ownedProjectileCounts[projectile.type] < 200)
                            {
                                int projectile2 = Projectile.NewProjectile(Entity.GetSource_None(), ((Entity)projectile).Center.X, ((Entity)projectile).Center.Y, (float)(Math.Sin(startAngle) * 8.0), (float)(Math.Cos(startAngle) * 8.0), projectile.type, (int)((double)projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                                int projectile3 = Projectile.NewProjectile(Entity.GetSource_None(), ((Entity)projectile).Center.X, ((Entity)projectile).Center.Y, (float)((double)(0f - (float)Math.Sin(startAngle)) * 8.0), (float)((double)(0f - (float)Math.Cos(startAngle)) * 8.0), projectile.type, (int)((double)projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                                Main.projectile[projectile2].DamageType = DamageClass.Default;
                                Main.projectile[projectile3].DamageType = DamageClass.Default;
                                Main.projectile[projectile2].noDropItem = true;
                                Main.projectile[projectile3].noDropItem = true;
                            }
                        }
                    }
                }
            }

            if (!projectile.npcProj && !projectile.trap && projectile.friendly && projectile.damage > 0)
            {
                if (projectile.CountsAsClass<RogueDamageClass>())
                {
                    if (modPlayer.reaverRogueExProj)
                    {
                        if (Main.player[projectile.owner].miscCounter % 60 == 0 && projectile.FinalExtraUpdate())
                        {
                            if (projectile.owner == Main.myPlayer)
                            {
                                int damage = (int)player.GetTotalDamage<RogueDamageClass>().ApplyTo(60);
                                damage = player.ApplyArmorAccDamageBonusesTo(damage);
                                int newProjectileId = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<PhotosyntheticShard>(), damage, 0f, projectile.owner);
                                if (newProjectileId != Main.maxProjectiles)
                                {
                                    Main.projectile[newProjectileId].CalamityInheritance().forceRogue = true;
                                }
                            }
                        }
                    }
                    if (modPlayer.nanotechold)
                    {
                        if (Main.player[projectile.owner].miscCounter % 30 == 0 && projectile.FinalExtraUpdate())
                        {
                            if (projectile.owner == Main.myPlayer)
                            {
                                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<NanotechOld>(), (int)(projectile.damage * 0.15) , 0f, projectile.owner);
                            }
                        }
                    }
                }
            }
            if (!frameOneHacksExecuted)
            {
                if (modPlayer.CIdeadshotBrooch && projectile.CountsAsClass<RangedDamageClass>() && player.heldProj != projectile.whoAmI)
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
            CalamityInheritancePlayer modPlayer = player.CalamityInheritance();

            modifiers.ModifyHitInfo += (ref NPC.HitInfo hitInfo) =>
            {

                if (modPlayer.AMRextra == true && hitInfo.Crit && CalamityInheritanceLists.AMRextraProjList.TrueForAll(x => projectile.type != x))
                {
                    IEntitySource source = projectile.GetSource_FromThis();
                    int extraProjectileAmt = 12;
                    for (int x = 0; x < extraProjectileAmt; x++)
                    {
                        if (projectile.owner == Main.myPlayer)
                        {
                            bool fromRight = x > 5;
                            Projectile proj = CalamityUtils.ProjectileBarrage(source, projectile.Center, projectile.Center, fromRight, 500f, 500f, 0f, 500f, 10f, projectile.type, (int)((float)projectile.damage * 0.3f), projectile.knockBack, projectile.owner, false, 5f);
                            CalamityUtils.Calamity(proj).pointBlankShotDuration = 0;
                        }
                    }
                    modPlayer.AMRextra = false;
                }
                if (modPlayer.AMRextraTy == true && hitInfo.Crit && CalamityInheritanceLists.AMRextraProjList.TrueForAll(x => projectile.type != x))
                {
                    IEntitySource source = projectile.GetSource_FromThis();
                    int extraProjectileAmt = 8;
                    for (int x = 0; x < extraProjectileAmt; x++)
                    {
                        if (projectile.owner == Main.myPlayer)
                        {
                            bool fromRight = x > 3;
                            Projectile proj = CalamityUtils.ProjectileBarrage(source, projectile.Center, projectile.Center, fromRight, 500f, 500f, 0f, 500f, 10f, projectile.type, (int)((float)projectile.damage * 0.15f), projectile.knockBack, projectile.owner, false, 5f);
                            CalamityUtils.Calamity(proj).pointBlankShotDuration = 0;
                        }
                    }
                    modPlayer.AMRextraTy = false;
                }
            };
        }
    }
}