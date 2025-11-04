using System;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Sounds.Custom;
using CalamityMod;
using CalamityMod.Particles;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Ranged
{
    //我现在知道algt说的我的代码能力比他强是什么意思了。
    public class ACTKarasawaHoldout : ModProjectile, ILocalizedModType
    {
        public ThanatosSmokeParticleSet SmokeDrawer = new ThanatosSmokeParticleSet(-1, 3, 0f, 16f, 0.5f);
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => $"{Generic.WeaponPath}/Ranged/ACTKarasawa";
        #region 别名
        public ref float AttackCharge => ref Projectile.ai[0];
        public ref float AttackRecoilTimer => ref Projectile.ai[1];
        public ref float AttackTimer => ref Projectile.ai[2];
        public Player Plr => Main.player[Projectile.owner];
        //ifHouldout?
        public bool IsHolding => Plr.channel && !Plr.noItems && !Plr.CCed;
        public SlotId KarasawaChargeSlot;
        public SlotId KarasawaFullChargeSlot;
        #endregion
        #region 攻击枚举
        #endregion
        #region 射弹属性
        const short FailCD = 240;
        #endregion
        public override void SetDefaults()
        {
            Projectile.width = 94;
            Projectile.height = 44;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 36000;
            Projectile.tileCollide = false;
        }
        //bro，他是个手持射弹。
        public override bool? CanDamage() => false;
        public override void AI()
        {
            //真有点holyShit了，ACT他们这里一个注释都没写😅 什么外表的光鲜之下是腐烂的内在
            //绝大部分代码我都是看不懂的，但我也尽可能封装+打注释让他看着整洁了
            Projectile.MaxUpdates = 1;
            Vector2 rrp = Plr.RotatedRelativePoint(Plr.MountedCenter, true);
            //what?
            Vector2 tip = rrp + Projectile.velocity * 94f;
            Vector2 spherePos = tip + Projectile.velocity * Math.Clamp((AttackCharge - 90f) / 36f, 0f, 10f);
            Color mainColor = Color.Lerp(Color.DodgerBlue, Color.Red, Math.Clamp((AttackCharge - 300f) / 150f, 0f, 1f));
            DoAttack(tip, mainColor);
            //用于检测是否应该干掉射弹
            KillHoldout();
            ActiveSound chargeSound;
            ActiveSound pulseSound;
            // ???
            if (SoundEngine.TryGetActiveSound(KarasawaChargeSlot, out chargeSound) && chargeSound.IsPlaying)
                chargeSound.Position = Projectile.Center;
            if (SoundEngine.TryGetActiveSound(KarasawaFullChargeSlot, out pulseSound) && pulseSound.IsPlaying)
                pulseSound.Position = Projectile.Center;
            #region 充能阶段的提示粒子
            switch (AttackCharge)
            {
                case 0f:
                    DoBeginCharge(rrp);
                    break;
                case 36f:
                    if (!Main.dedServ)
                        DoChargeTint(tip, 1);
                    break;
                case 56f:
                    if (!Main.dedServ)
                        DoChargeTint(tip, 2);
                    break;
                case 76f:
                    if (!Main.dedServ)
                        DoChargeTint(tip, 3);
                    break;
            }
            #endregion
            #region 充能过程的提示粒子
            if (AttackCharge > 90f)
                DoChargeAnimatedTint(tip, spherePos, mainColor);
            if (AttackCharge > 310f && AttackTimer % 150 == 10f)
                DoChargeAnimatedTint2();
            if (AttackCharge > 300f || AttackCharge == -1f)
                DoChargeAnimatedTintNo(spherePos, mainColor);
            #endregion
            #region 其他处理
            if (AttackTimer <= 0)
                AttackCharge++;
            else AttackCharge--;
            UpdateHeld(rrp);
            ManualDrawPlayer();
            #endregion

        }
        public override bool PreDraw(ref Color lightColor)
        {
            SmokeDrawer.DrawSet(Plr.RotatedRelativePoint(Plr.MountedCenter - Vector2.UnitY * 4f, true, true) + Projectile.velocity * Projectile.width * 0.8f);

            if (Projectile.ai[0] == -1f)
            {
                return base.PreDraw(ref lightColor);
            }
            float ChargePercent = Math.Clamp(Projectile.ai[0] / 450f, 0f, 1f);
            float sightsSize = 33f;
            float halfAngle = (1f - ChargePercent * 0.965f) * 2.0943952f / 2f;
            Texture2D texture = ModContent.Request<Texture2D>(Texture, (ReLogic.Content.AssetRequestMode)2).Value;
            Color sightsColor = Color.Lerp(Color.DodgerBlue, Color.Red, Math.Clamp((Projectile.ai[0] - 300f) / 150f, 0f, 1f)) * 0.8f;
            Effect spreadVFX = Filters.Scene["CalamityMod:SpreadTelegraph"].GetShader().Shader;
            spreadVFX.Parameters["centerOpacity"].SetValue(0.45f);
            spreadVFX.Parameters["mainOpacity"].SetValue(ChargePercent);
            spreadVFX.Parameters["halfSpreadAngle"].SetValue(halfAngle);
            spreadVFX.Parameters["edgeColor"].SetValue(sightsColor.ToVector3());
            spreadVFX.Parameters["centerColor"].SetValue(sightsColor.ToVector3());
            spreadVFX.Parameters["edgeBlendLength"].SetValue(0.07f);
            spreadVFX.Parameters["edgeBlendStrength"].SetValue(8f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, spreadVFX, Main.GameViewMatrix.TransformationMatrix);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + Projectile.velocity * 33f, null, Color.White, Projectile.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), sightsSize, (SpriteEffects)(Plr.direction == -1 ? Math.PI : 0), 0f); //预瞄线光照
            Main.spriteBatch.End();
            Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            return base.PreDraw(ref lightColor);
        }
        public override Color? GetAlpha(Color lightColor) => lightColor;
        #region 方法列表
        private void ManualDrawPlayer()
        {
            Plr.heldProj = Projectile.whoAmI;
            Plr.itemTime = 2;
            Plr.itemAnimation = 2;
            Plr.itemRotation = ((Plr.Calamity().mouseWorld - Plr.MountedCenter) * Plr.direction).ToRotation();
            Plr.ChangeDir((Plr.Calamity().mouseWorld - Plr.MountedCenter).X >= 0f ? 1 : -1);
            Projectile.rotation = (Plr.Calamity().mouseWorld - Plr.MountedCenter).ToRotation() + (Plr.direction == -1 ? (float)Math.PI : 0);
            Projectile.spriteDirection = Projectile.direction;

            float speedLimit = (Projectile.ai[0] >= 300f ? 3f : 4f) * Plr.moveSpeed;
            if (Projectile.ai[0] >= 90f && Plr.velocity.Length() > speedLimit && !Plr.pulley)
            {
                Plr.velocity.X = Math.Min(Math.Abs(Plr.velocity.X), speedLimit) * (Plr.velocity.X > 0).ToDirectionInt();
                Plr.velocity.Y = Math.Min(Math.Abs(Plr.velocity.Y), speedLimit) * (Plr.velocity.Y > 0).ToDirectionInt();
            }

            if (Projectile.ai[0] != -1f) Plr.Calamity().GeneralScreenShakePower = Math.Max(Math.Clamp((Projectile.ai[0] - 90f) * 0.01f, 0f, 3.6f), Plr.Calamity().GeneralScreenShakePower);
        }

        private void UpdateHeld(Vector2 rrp)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                Player player = Main.player[Projectile.owner];
                float interpolant = Utils.GetLerpValue(5f, 25f, Projectile.Distance(player.LocalMouseWorld()), true);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Plr.SafeDirectionTo(player.LocalMouseWorld(), null), interpolant);
                Projectile.netUpdate = true;
            }
            Projectile.Center = rrp + Projectile.velocity * MathHelper.Clamp(30f - Projectile.ai[2], 0f, 30f) + new Vector2(0f, -1.5f);

            //震动
            float rumble = MathHelper.Clamp(AttackCharge, 0f, 450f);
            if (IsHolding)
            {
                Projectile.position += Main.rand.NextVector2Circular(rumble / 100f, rumble / 100f);
            }
        }

        private void DoChargeAnimatedTintNo(Vector2 spherePos, Color mainColor)
        {
            if (Main.netMode != NetmodeID.Server && AttackCharge != -1f)
            {
                float distance = Math.Clamp(AttackCharge / 100f, 3f, 4.5f);
                Vector2 pos = spherePos + Main.rand.NextVector2CircularEdge(64f, 64f);
                Particle par = new SquishyLightParticle(pos, pos.DirectionTo(spherePos) * Main.rand.NextFloat(2f, 4f), Main.rand.NextFloat(0.1f, 0.15f) * distance, mainColor, Main.rand.Next(12, 17), 0.2f, 4f);
                GeneralParticleHandler.SpawnParticle(par);

            }
            SmokeDrawer.ParticleSpawnRate = AttackCharge == -1f ? 9999999 : 3;
            SmokeDrawer.BaseMoveRotation = MathHelper.Pi / 2 + Projectile.spriteDirection * (Projectile.position.X - Projectile.oldPosition.X) * 0.04f;
            SmokeDrawer.Update();
        }

        private void DoChargeAnimatedTint2()
        {
            SoundStyle pulse = CISoundMenu.KarasawaEnergyPulse;
            pulse.IsLooped = false;
            KarasawaFullChargeSlot = SoundEngine.PlaySound(pulse, Projectile.Center);
        }

        private void DoChargeAnimatedTint(Vector2 tip, Vector2 spherePos, Color mainColor)
        {
            Plr.mount.Dismount(Plr);
            if (!Main.dedServ)
            {
                int dCounts = (int)Math.Round((double)MathHelper.SmoothStep(1f, 5f, Math.Clamp(AttackCharge / 450f, 0f, 1f)));
                float outWard = MathHelper.SmoothStep(30f, 90f, Math.Clamp(AttackCharge / 450f, 0f, 1f));
                float dScale = MathHelper.Lerp(0.3f, 1.5f, Math.Clamp(AttackCharge / 450f, 0f, 1f));
                for (int i = 0; i < dCounts; i++)
                {
                    Vector2 dPos = tip + Main.rand.NextVector2Unit(0f, MathHelper.TwoPi) * outWard * Main.rand.NextFloat(0.75f, 1.1f);
                    Vector2 dVel = (tip - dPos) * 0.085f + Plr.velocity * 0.5f;
                    Dust d = Dust.NewDustPerfect(dPos, 264, null, 0, default, 1f);
                    d.velocity = dVel;
                    d.scale = dScale * Main.rand.NextFloat(0.75f, 1.15f);
                    d.color = mainColor * Main.rand.NextFloat(0.65f, 1f);
                    d.noGravity = true;
                }
                Particle par = new SquishyLightParticle(spherePos, Main.rand.NextVector2Circular(1f, 1f), Main.rand.NextFloat(0.12f, 0.16f) * Math.Clamp((AttackCharge - 64f) / 26f, 1f, 15f), mainColor, Main.rand.Next(12, 17), 0.2f, 0.5f);
                GeneralParticleHandler.SpawnParticle(par);

                float strength = Math.Clamp((AttackCharge - 90) / 180f, 0f, 2f);
                Lighting.AddLight(spherePos, mainColor.R / 255f * strength, mainColor.G / 255f * strength, mainColor.B / 255f * strength);
                Lighting.AddLight(tip - Projectile.velocity * 50f, Math.Clamp(AttackCharge / 1000, 0.09f, 0.45f), 0f, 0f);
            }
        }

        private void DoChargeTint(Vector2 tip, int tintTime)
        {
            //act这里赋值了同一个东西4次，诗人我吃。
            //这里按照同一个意思的前提下改写成了公式。
            //1f, 1.5f, 2f
            float dScaleMul = 0.5f + 0.5f * tintTime;
            //66 44 22
            int dCoutnsMul = 88 - 22 * tintTime;
            for (int i = 0; i < dCoutnsMul; i++)
            {
                Vector2 dPos = tip + Main.rand.NextVector2Unit(0f, MathHelper.TwoPi) * 90f * Main.rand.NextFloat(0.75f, 1.1f) * Projectile.velocity * 14f;
                Vector2 dVel = (tip - Projectile.velocity * 14f - dPos) * 0.085f + Plr.velocity * 0.5f;
                Dust d = Dust.NewDustPerfect(dPos, 264, null, 0, default, 1f);
                d.velocity = dVel;
                d.scale = dScaleMul * Main.rand.NextFloat(0.75f, 1.15f);
                d.color = Color.Lerp(Color.DodgerBlue, Color.HotPink, Math.Clamp(AttackCharge / 450f, 0f, 1f) * Main.rand.NextFloat(0.65f, 1f));
                d.noGravity = true;
                d.noLight = true;
            }
        }

        private void DoBeginCharge(Vector2 rrp)
        {
            //武器没充能，就充能
            KarasawaChargeSlot = SoundEngine.PlaySound(CISoundMenu.KarasawaCharge, Projectile.Center);
            for (int i = 0; i < 3; i++)
                GeneralParticleHandler.SpawnParticle(new SmallSmokeParticle(rrp + Projectile.velocity * 15f, Projectile.velocity.RotatedByRandom(0.8f).RotatedBy(-0.3f) * Main.rand.NextFloat(0.8f, 1.2f), Color.White, Color.DimGray, 1f, 100f));
        }

        private void KillHoldout()
        {
            if (Plr.dead || !IsHolding && AttackCharge != -1f || AttackTimer <= 0 && AttackCharge == -1f)
            {
                Projectile.Kill();
                return;
            }
        }

        private void DoAttack(Vector2 tip, Color mainColor)
        {
            //原代码中我不清楚为什么要检测玩家没手持任意射弹的时候才执行这些
            if (!IsHolding && AttackCharge >= 300f && AttackTimer <= 0)
            {
                float multiplier = (float)Math.Pow(Math.Clamp(AttackCharge - 200f, 100f, 250f) * 0.01f, 2);

                if (multiplier >= 6.25f) multiplier = 8.125f;
                //音效控制, 虽然怎么写是你们的自由，但是WTF?
                SoundStyle style = CISoundMenu.KarasawaLaunch;
                style.Volume = 1f;
                if (AttackCharge < 450f)
                {
                    style.Volume = 0.9f;
                    style.Pitch = 0.1f;
                }
                SoundEngine.PlaySound(style, Projectile.Center);
                //发射射弹
                if (Main.myPlayer == Plr.whoAmI)
                {
                    Vector2 vel = Projectile.velocity.SafeNormalize(Vector2.UnitY) * 5f;
                    //byd还有高手啊？
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), tip - vel * 20f, vel, ModContent.ProjectileType<ACTKarasawaBoom>(), (int)(Projectile.damage / 6.25f * multiplier), Projectile.knockBack, Plr.whoAmI, Math.Clamp((AttackCharge - 300f) / 150f, 0f, 1f), AttackCharge >= 450f ? 0f : -1f);
                }
                //光效
                if (!Main.dedServ)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        GeneralParticleHandler.SpawnParticle(new SquishyLightParticle(tip, Projectile.velocity.RotatedByRandom(0.8f) * Main.rand.NextFloat(6f, 8f), Math.Clamp(AttackCharge / 300f, 1f, 1.5f), mainColor, Main.rand.Next(48, 57), 1, 1.65f));
                    }
                }
                Vector2 pushback = Projectile.velocity.SafeNormalize(Vector2.UnitX) * Math.Clamp(-AttackCharge / 75f, -6f, -4f);
                Vector2 newPlayerVelocity = Plr.velocity + pushback;
                Plr.velocity = newPlayerVelocity;
                //震屏
                Plr.Calamity().GeneralScreenShakePower = Math.Max(Math.Clamp(AttackCharge * 0.1f, 30f, 45f), Plr.Calamity().GeneralScreenShakePower);
                CancelChargeSounds();
                AttackCharge = -1f;
                AttackTimer = 45f;
            }
        }
        //充能被干掉的音效提示
        private void CancelChargeSounds()
        {
            ActiveSound chargeSound;
            ActiveSound pulseSound;
            if (SoundEngine.TryGetActiveSound(KarasawaChargeSlot, out chargeSound) && chargeSound != null)
            {
                chargeSound.Stop();
            }
            if (SoundEngine.TryGetActiveSound(KarasawaFullChargeSlot, out pulseSound) && pulseSound != null)
            {
                pulseSound.Stop();
            }
        }
        #endregion
    }
}