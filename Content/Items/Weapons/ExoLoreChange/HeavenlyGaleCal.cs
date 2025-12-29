using System;
using System.Collections.Generic;
using System.Reflection;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Ranged;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class HeavenlyGaleCal : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemType<HeavenlyGale>();
        public override void ModifyTooltips(Item item, List<TooltipLine> o)
        {
            string t = Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo ? Language.GetTextValue($"{Generic.WeaponTextPath}Ranged.HeavenlyGaleChange") : null;
            if (t != null) o.Add(new TooltipLine(Mod, "Name", t));
        }
    }
    public class HeavenlyGaleCrystalArrow : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileType<ExoCrystalArrow>();
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            var usPlayer = Main.player[projectile.owner].CIMod();
            if ((usPlayer.LoreExo || usPlayer.PanelsLoreExo) && projectile.owner == Main.myPlayer)
            {
                // 挂载了hook来让原射弹发射时就是ai2=1f
                //天风的箭矢没有使用（暂时）ai2，因此这里会通过这个进行操作
                if (projectile.ai[2] == 1)
                {
                    int ex = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, projectile.velocity, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0], projectile.ai[1], 0f);
                    //原灾的重做其实已经足够优秀（而且耦合度太高了），这里给点额外更新了
                    Main.projectile[ex].extraUpdates += 1;
                }

            }
        }
    }
    public class HeavenlyGaleCrystalStrike : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileType<ExoLightningBolt>();
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            var usPlayer = Main.player[projectile.owner].CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo && projectile.owner == Main.myPlayer)
            {
                //5->10
                projectile.MaxUpdates = 10;
                //13->10
                projectile.localNPCHitCooldown = projectile.MaxUpdates * 10;
            }
        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            var usPlayer = Main.player[projectile.owner].CIMod();
            //因为总体闪电变多了因此这里射弹会降低20%伤害
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
                modifiers.SourceDamage *= 0.80f;
        }
    }
    public class HeavenlyGaleProjHook : GlobalItem
    {
        public static void Load(Mod mod)
        {
            MethodInfo originalMethod = typeof(HeavenlyGaleProj).GetMethod(nameof(HeavenlyGaleProj.SafeAI));
            MonoModHooks.Add(originalMethod, SafeAI_Hook);
        }

        public static void SafeAI_Hook(HeavenlyGaleProj self)
        {
            Vector2 armPosition = self.Owner.RotatedRelativePoint(self.Owner.MountedCenter, true);
            Vector2 tipPosition = armPosition + self.Projectile.velocity * self.Projectile.width * 0.45f;

            // Activate shot behavior if the owner stops channeling or otherwise cannot use the weapon.
            bool activatingShoot = self.ShootDelay <= 0 && Main.mouseLeft && !Main.mapFullscreen && !self.Owner.mouseInterface;
            if (Main.myPlayer == self.Projectile.owner && self.OwnerCanShoot && activatingShoot)
            {
                SoundEngine.PlaySound(HeavenlyGale.FireSound, self.Projectile.Center);
                self.ShootDelay = self.Owner.ActiveItem().useAnimation;
                self.Projectile.netUpdate = true;
            }

            // Update damage based on current ranged damage stat, since this projectile exists regardless of if it's being fired.
            self.Projectile.damage = self.Owner.ActiveItem() is null ? 0 : self.Owner.GetWeaponDamage(self.Owner.ActiveItem());

            self.UpdateProjectileHeldVariables(armPosition);
            self.ManipulatePlayerVariables();

            // Fire arrows.
            if (self.ShootDelay > 0f && LAPUtilities.FinalExtraUpdate(self.Projectile))
            {
                float shootCompletionRatio = 1f - self.ShootDelay / (self.Owner.ActiveItem().useAnimation - 1f);
                float bowAngularOffset = (float)Math.Sin(MathHelper.TwoPi * shootCompletionRatio) * 0.4f;
                float damageFactor = Utils.Remap(self.ChargeTimer, 0f, HeavenlyGale.MaxChargeTime, 1f, HeavenlyGale.MaxChargeDamageBoost);

                // Fire arrows.
                if (self.ShootDelay % HeavenlyGale.ArrowShootRate == 0)
                {
                    Vector2 arrowDirection = self.Projectile.velocity.RotatedBy(bowAngularOffset);

                    // Release a streak of energy.
                    Color energyBoltColor = CalamityUtils.MulticolorLerp(shootCompletionRatio, CalamityUtils.ExoPalette);
                    energyBoltColor = Color.Lerp(energyBoltColor, Color.White, 0.35f);
                    SquishyLightParticle exoEnergyBolt = new(tipPosition + arrowDirection * 16f, arrowDirection * 4.5f, 0.85f, energyBoltColor, 40, 1f, 5.4f, 4f, 0.08f);
                    GeneralParticleHandler.SpawnParticle(exoEnergyBolt);

                    // Update the tip position for one frame.
                    tipPosition = armPosition + arrowDirection * self.Projectile.width * 0.45f;

                    if (Main.myPlayer == self.Projectile.owner && self.Owner.HasAmmo(self.Owner.ActiveItem()))
                    {
                        Item heldItem = self.Owner.ActiveItem();
                        self.Owner.PickAmmo(heldItem, out int projectileType, out float shootSpeed, out int damage, out float knockback, out _);
                        damage = (int)(damage * damageFactor);
                        projectileType = ProjectileType<ExoCrystalArrow>();

                        bool createLightning = self.ChargeTimer / HeavenlyGale.MaxChargeTime >= HeavenlyGale.ChargeLightningCreationThreshold;
                        Vector2 arrowVelocity = arrowDirection * shootSpeed;
                        Projectile.NewProjectile(self.Projectile.GetSource_FromThis(), tipPosition, arrowVelocity, projectileType, damage, knockback, self.Projectile.owner, createLightning.ToInt(), 0f, 1);
                    }
                }

                self.ShootDelay--;
                if (self.ShootDelay <= 0f)
                    self.ChargeTimer = 0f;
            }

            // Create orange exo energy at the tip of the bow.
            Color energyColor = Color.Orange;
            Vector2 verticalOffset = Vector2.UnitY.RotatedBy(self.Projectile.rotation) * 8f;
            if (Math.Cos(self.Projectile.rotation) < 0f)
                verticalOffset *= -1f;

            if (Main.rand.NextBool(4))
            {
                SquishyLightParticle exoEnergy = new(tipPosition + verticalOffset, -Vector2.UnitY.RotatedByRandom(0.39f) * Main.rand.NextFloat(0.4f, 1.6f), 0.28f, energyColor, 25);
                GeneralParticleHandler.SpawnParticle(exoEnergy);
            }

            // Create light at the tip of the bow.
            DelegateMethods.v3_1 = energyColor.ToVector3();
            Utils.PlotTileLine(tipPosition - verticalOffset, tipPosition + verticalOffset, 10f, DelegateMethods.CastLightOpen);
            Lighting.AddLight(tipPosition, energyColor.ToVector3());

            // Create a puff of energy in a star shape and play a sound to indicate that the bow is at max charge.
            if (self.ShootDelay <= 0)
                self.ChargeTimer++;
            if (self.ChargeTimer == HeavenlyGale.MaxChargeTime)
            {
                SoundEngine.PlaySound(SoundID.Item158 with { Volume = 1.6f }, self.Projectile.Center);
                for (int i = 0; i < 75; i++)
                {
                    float offsetAngle = MathHelper.TwoPi * i / 75f;

                    // Parametric equations for an asteroid.
                    float unitOffsetX = (float)Math.Pow(Math.Cos(offsetAngle), 3D);
                    float unitOffsetY = (float)Math.Pow(Math.Sin(offsetAngle), 3D);

                    Vector2 puffDustVelocity = new Vector2(unitOffsetX, unitOffsetY) * 5f;
                    Dust magic = Dust.NewDustPerfect(tipPosition, 267, puffDustVelocity);
                    magic.scale = 1.8f;
                    magic.fadeIn = 0.5f;
                    magic.color = CalamityUtils.MulticolorLerp(i / 75f, CalamityUtils.ExoPalette);
                    magic.noGravity = true;
                }
                self.ChargeTimer++;
            }
        }

    }
}