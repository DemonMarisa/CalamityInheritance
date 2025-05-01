using CalamityMod.CalPlayer.Dashes;
using CalamityMod.CalPlayer;
using CalamityMod.EntitySources;
using CalamityMod.Enums;
using System;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace CalamityInheritance.Common.CIHook
{
    public static class CalamityInheritanceDashHook
    {
        public static void Load(Mod mod)
        {
            MethodInfo originalMethod = typeof(CalamityPlayer).GetMethod(nameof(CalamityPlayer.ModDashMovement));
            MonoModHooks.Add(originalMethod, ModDashMovement_Hook);
        }

        public static int DashCoolDown = CalamityInheritance.Instance.infernumMode == null ? 30 : 20;
        //用于修改原灾冲刺的hook
        public static void ModDashMovement_Hook(CalamityPlayer self)
        {
            if (self.Player.whoAmI != Main.myPlayer)
                return;

            var source = new ProjectileSource_PlayerDashHit(self.Player);

            // Handle collision slam-through effects.
            if (self.HasCustomDash && self.Player.dashDelay < 0)
            {
                Rectangle hitArea = new Rectangle((int)(self.Player.position.X + self.Player.velocity.X * 0.5 - 4f), (int)(self.Player.position.Y + self.Player.velocity.Y * 0.5 - 4), self.Player.width + 8, self.Player.height + 8);
                foreach (NPC n in Main.ActiveNPCs)
                {
                    // Ignore critters with the Guide to Critter Companionship
                    if (self.Player.dontHurtCritters && NPCID.Sets.CountsAsCritter[n.type])
                        continue;

                    if (!n.dontTakeDamage && !n.friendly && n.Calamity().dashImmunityTime[self.Player.whoAmI] <= 0)
                    {
                        if (hitArea.Intersects(n.getRect()) && (n.noTileCollide || self.Player.CanHit(n)))
                        {
                            DashHitContext hitContext = default;
                            self.UsedDash.OnHitEffects(self.Player, n, source, ref hitContext);

                            // Don't bother doing anything if no damage is done.
                            if (hitContext.damageClass is null || hitContext.BaseDamage <= 0)
                                continue;

                            // Duplicated from the way TML edits vanilla ram dash damage (and Shield of Cthulhu)
                            int dashDamage = (int)self.Player.GetTotalDamage(hitContext.damageClass).ApplyTo(hitContext.BaseDamage);
                            float dashKB = self.Player.GetTotalKnockback(hitContext.damageClass).ApplyTo(hitContext.BaseKnockback);
                            bool rollCrit = Main.rand.Next(100) < self.Player.GetTotalCritChance(hitContext.damageClass);

                            self.Player.ApplyDamageToNPC(n, dashDamage, dashKB, hitContext.HitDirection, rollCrit, hitContext.damageClass, true);
                            if (n.Calamity().dashImmunityTime[self.Player.whoAmI] < 12)
                                n.Calamity().dashImmunityTime[self.Player.whoAmI] = 12;

                            self.Player.GiveImmuneTimeForCollisionAttack(hitContext.PlayerImmunityFrames);
                        }
                    }
                }
            }

            if (self.Player.dashDelay > 0) //Speed Blaster
            {
                self.VerticalSpeedBlasterDashTimer = 0;
                self.LastUsedDashID = string.Empty;
                return;
            }

            if (self.Player.dashDelay > 0)
            {
                self.VerticalGodslayerDashTimer = 0;
                self.LastUsedDashID = string.Empty;
                return;
            }

            if (self.Player.dashDelay < 0)
            {
                int dashDelayToApply = DashCoolDown;
                if (self.UsedDash.CollisionType == DashCollisionType.ShieldSlam)
                    dashDelayToApply = DashCoolDown;
                else if (self.UsedDash.CollisionType == DashCollisionType.ShieldBonk)
                    dashDelayToApply = DashCoolDown;
                if (self.DashID == "Deep Diver")
                    dashDelayToApply = 23;

                float dashSpeed = 12f;
                float dashSpeedDecelerationFactor = 0.985f;
                float runSpeed = Math.Max(self.Player.accRunSpeed, self.Player.maxRunSpeed);
                float runSpeedDecelerationFactor = 0.94f;

                self.LastUsedDashID = self.DashID;

                // Handle mid-dash effects.
                self.UsedDash.MidDashEffects(self.Player, ref dashSpeed, ref dashSpeedDecelerationFactor, ref runSpeedDecelerationFactor);
                if (self.UsedDash.IsOmnidirectional && self.VerticalGodslayerDashTimer < 25)
                {
                    self.VerticalGodslayerDashTimer++;
                    if (self.VerticalGodslayerDashTimer >= 25)
                    {
                        self.Player.dashDelay = dashDelayToApply;
                        // Stop the player from going flying
                        self.Player.velocity *= 0.2f;
                    }
                }

                if (self.UsedDash.IsOmnidirectional && self.VerticalSpeedBlasterDashTimer < 25)
                {
                    self.VerticalSpeedBlasterDashTimer++;
                    if (self.VerticalSpeedBlasterDashTimer >= 25)
                    {
                        self.Player.dashDelay = dashDelayToApply;
                        // Stop the player from going flying
                        self.Player.velocity *= 0.2f;
                    }
                }

                if (self.HasCustomDash)
                {
                    self.Player.vortexStealthActive = false;
                    /*
                    // Decide the player's facing direction.
                    if (self.Player.velocity.X != 0f)
                        self.Player.ChangeDir(Math.Sign(self.Player.velocity.X));
                    */
                    // Handle mid-dash movement.
                    if (self.UsedDash.IsOmnidirectional)
                    {
                        if (self.Player.velocity.Length() > dashSpeed)
                        {
                            self.Player.velocity *= dashSpeedDecelerationFactor;
                            return;
                        }
                        if (self.Player.velocity.Length() > runSpeed)
                        {
                            self.Player.velocity *= runSpeedDecelerationFactor;
                            return;
                        }
                    }
                    else
                    {
                        if (self.Player.velocity.X > dashSpeed || self.Player.velocity.X < -dashSpeed)
                        {
                            self.Player.velocity.X *= dashSpeedDecelerationFactor;
                            return;
                        }
                        if (self.Player.velocity.X > runSpeed || self.Player.velocity.X < -runSpeed)
                        {
                            self.Player.velocity.X *= runSpeedDecelerationFactor;
                            return;
                        }
                    }

                    // Dash delay depends on the type of dash used.
                    self.Player.dashDelay = dashDelayToApply;

                    if (self.UsedDash.IsOmnidirectional)
                    {
                        if (self.Player.velocity.Length() < 0f)
                        {
                            self.Player.velocity.Normalize();
                            self.Player.velocity *= -runSpeed;
                            return;
                        }
                        if (self.Player.velocity.Length() > 0f)
                        {
                            self.Player.velocity.Normalize();
                            self.Player.velocity *= runSpeed;
                            return;
                        }
                    }
                    else
                    {
                        if (self.Player.velocity.X < 0f)
                        {
                            self.Player.velocity.X = -runSpeed;
                            return;
                        }
                        if (self.Player.velocity.X > 0f)
                        {
                            self.Player.velocity.X = runSpeed;
                            return;
                        }
                    }
                }
            }

            // Handle first-frame effects.
            else if (self.HasCustomDash && !self.Player.mount.Active)
            {
                if (self.DoADash(self.UsedDash.CalculateDashSpeed(self.Player)))
                    self.UsedDash.OnDashEffects(self.Player);
            }
        }
    }
}
