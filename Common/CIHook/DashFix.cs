using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.EntitySources;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CIHook
{
    public class DashFix : ModSystem
    {
        public override void Load()
        {
            MethodInfo originalMethod = typeof(CalamityPlayer).GetMethod(nameof(CalamityPlayer.ModDashMovement));
            MonoModHooks.Add(originalMethod, ModDashMovement_Hook);
        }
        public static int DashCoolDown => LAP.LAP.Instance.InfernumMode == null ? 30 : 20;
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
        }
    }
}