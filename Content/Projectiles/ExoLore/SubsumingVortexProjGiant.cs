using System;
using System.Collections.Generic;
using System.IO;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Magic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoLore
{
    public class SubsumingVortexProjGiant: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public Player Owner => Main.player[Projectile.owner];

        public float Time
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public bool HasBeenReleased
        {
            get => Projectile.ai[1] == 1f;
            set => Projectile.ai[1] = value.ToInt();
        }
        public ref float AttackTiemr => ref Projectile.ai[2]; 
        public const int ExplodeTime = 45;

        public const float StartingScale = 0.0004f;

        public const float IdealScale = 2.7f;

        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";
        public bool InitSound = false;

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.scale = StartingScale;
            Projectile.timeLeft = 90000;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 7;
            Projectile.hide = true;
        }

        // Vanilla Terraria does not sync projectile scale by default.
        public override void SendExtraAI(BinaryWriter writer) => writer.Write(Projectile.scale);

        public override void ReceiveExtraAI(BinaryReader reader) => Projectile.scale = reader.ReadSingle();
        //Do not do anything.
        public override bool? CanDamage() => !HasBeenReleased || (HasBeenReleased && AttackTiemr > 120f);

        public override void AI()
        {
            //We just play once.
            if (Time < 1)
            {
                SoundEngine.PlaySound(CISoundMenu.VortexStart, Projectile.Center);
                InitSound = false;
            }
            //Init one time if just fully charged
            if (Time >= SubsumingVortex.LargeVortexChargeupTime && !InitSound)
            {
                SoundEngine.PlaySound(CISoundMenu.VortexDone, Projectile.Center);    
                InitSound = true;
            }
            // If the player has channeled the vortex for long enough and it hasn't been released yet, release it.
            if ((!Owner.Calamity().mouseRight || Owner.noItems || Owner.CCed) && !HasBeenReleased)
            {
                //If charging time is at maxninum, Set damage at max.
                if (Time >= SubsumingVortex.LargeVortexChargeupTime)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Projectile.velocity = Projectile.SafeDirectionTo(Main.MouseWorld) * SubsumingVortex.ReleaseSpeed;
                        Projectile.damage = (int)(Projectile.damage * SubsumingVortex.ReleaseDamageFactor);
                        HasBeenReleased = true;
                        Projectile.netUpdate = true;
                    }
                }
                //If not, scaling the damage.
                else if (Time >= SubsumingVortex.VortexShootDelay)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Projectile.velocity = Projectile.SafeDirectionTo(Main.MouseWorld) * SubsumingVortex.ReleaseSpeed;
                        Projectile.damage = (int)(Projectile.damage * (1f + Time * 0.0152f));
                        HasBeenReleased = true;
                        Projectile.netUpdate = true;
                    }
                }
                //Or just kill the projecilte.
                else
                    Projectile.Kill();
                return;
            }
            //Disappear if released, too far from the target, and hasn't exploded yet.
            if (HasBeenReleased && Projectile.timeLeft > ExplodeTime && !Projectile.WithinRange(Owner.Center, 2000f))
            {
                Projectile.Kill();
                return;
            }
            //Search the target.
            NPC potentialTarget = Projectile.Center.ClosestNPCAt(SubsumingVortex.SmallVortexTargetRange - 100f);
            //Release energy from the book.
            if (Time >= SubsumingVortex.VortexShootDelay)
            {
                Vector2 bookPosition = Owner.Center + Vector2.UnitX * Owner.direction * 22f;
                if (Main.rand.NextBool())
                {
                    Vector2 energyVelocity = Main.rand.NextVector2Circular(3f, 3f);
                    Color energyColor = CalamityUtils.MulticolorLerp(Main.rand.NextFloat(), CalamityUtils.ExoPalette);
                    SquishyLightParticle exoEnergy = new(bookPosition, energyVelocity, 0.55f, energyColor, 40, 1f, 1.5f);
                    GeneralParticleHandler.SpawnParticle(exoEnergy);
                }

                //Fire vortices at nearby target if not fully charged and has not been released yet.
                if (potentialTarget != null && Time % SubsumingVortex.VortexReleaseRate == SubsumingVortex.VortexReleaseRate - 1 && Time < SubsumingVortex.LargeVortexChargeupTime && !HasBeenReleased)
                {
                    //CheckMana returns true if the mana cost can be paid..
                    bool allowContinuedUse = Owner.CheckMana(Owner.ActiveItem(), -1, true, false);
                    bool vortexStillInUse = Owner.Calamity().mouseRight && allowContinuedUse && !Owner.noItems && !Owner.CCed;
                    if (vortexStillInUse)
                    {
                        SoundEngine.PlaySound(Utils.SelectRandom(Main.rand, SubsumingVortexold.TossSound), Projectile.Center);
                        if (Main.myPlayer == Projectile.owner)
                        {
                            float hue = (Time - SubsumingVortex.VortexShootDelay) / 125f;
                            Vector2 vortexVelocity = Projectile.SafeDirectionTo(potentialTarget.Center) * 8f;
                            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vortexVelocity, ModContent.ProjectileType<SubsumingVortexProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, hue);
                        }
                        Projectile.netUpdate = true;
                    }
                }
            }
            DoDrawDust();
            DoVisual();
            if (HasBeenReleased)
                DoHoming(potentialTarget);
            AdjustPlayerValues();
            Time++;
            // Slow down.
            if (Projectile.timeLeft < ExplodeTime)
                Projectile.velocity *= 0.8f;
        }

        private void DoHoming(NPC tar)
        {
            if (tar is null)
                return;
            AttackTiemr++;
            //Start charging enemy, stop the player following. 
            if (AttackTiemr > 240f)
                Projectile.HomingNPCBetter(tar, 1800f, 16f, 20f, 0, 16f, null, true);
            else
                Owner.HomeInPlayer(Projectile, 20f, 16f, 0.5f, true, 150f);

            //Keep Spawning it.
            if (Time % SubsumingVortex.VortexReleaseRate == SubsumingVortex.VortexReleaseRate - 1)
            {
                SoundEngine.PlaySound(Utils.SelectRandom(Main.rand, SubsumingVortexold.TossSound), Projectile.Center);
                if (Main.myPlayer == Projectile.owner)
                {
                    float hue = (Time - SubsumingVortex.VortexShootDelay) / 125f;
                    Vector2 vortexVelocity = Projectile.SafeDirectionTo(tar.Center) * 8f;
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vortexVelocity, ModContent.ProjectileType<SubsumingVortexProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, hue);
                }
                Projectile.netUpdate = true;
            }
            
        }

        private void DoDrawDust()
        {
            if (Main.rand.NextBool() && Projectile.Opacity <= 0.6f)
                return;

            //Create swirling exo energy.
            float dustSpawnChance = Utils.Remap(Time, 12f, SubsumingVortex.VortexShootDelay + 8f, 0.25f, 0.7f);
            for (int i = 0; i < 3; i++)
            {
                //Don't do anythign if it should not  spawn dust.
                if (Main.rand.NextFloat() > dustSpawnChance)
                    continue;

                float spawnOffsetAngle = Main.rand.NextFloat(MathHelper.TwoPi);
                float hue = (float)Math.Sin(spawnOffsetAngle + Time / 26f) * 0.5f + 0.5f;
                float spawnOffsetFactor = Main.rand.NextFloat(0.3f, 0.95f);
                float energyScale = Projectile.scale * Main.rand.NextFloat(0.18f, 0.3f);
                if (energyScale > 1f)
                    energyScale = 1f;

                Vector2 energySpawnPosition = Projectile.Center + spawnOffsetAngle.ToRotationVector2() * Projectile.Size * spawnOffsetFactor;
                Vector2 energyVelocity = (spawnOffsetAngle - MathHelper.PiOver2).ToRotationVector2() * (Main.rand.NextFloat(5f, 10f) * spawnOffsetFactor) * dustSpawnChance;
                Color energyColor = CalamityUtils.MulticolorLerp(hue, CalamityUtils.ExoPalette);
                SquishyLightParticle exoEnergy = new(energySpawnPosition, energyVelocity, energyScale, energyColor, 32, 1f, 1.5f);
                GeneralParticleHandler.SpawnParticle(exoEnergy);
            }
        }

        private void DoVisual()
        {
            //Resize.
            Projectile.Opacity = Utils.GetLerpValue(0f, 20f, Time, true) * Utils.GetLerpValue(0f, ExplodeTime, Projectile.timeLeft, true);
            Projectile.scale = Utils.Remap(Time, 0f, SubsumingVortex.LargeVortexChargeupTime, StartingScale, IdealScale);
            Projectile.scale *= Utils.Remap(Projectile.timeLeft, ExplodeTime, 1f, 1f, 5.4f);
            Projectile.ExpandHitboxBy((int)(Projectile.scale * 62f));

            //Emit light.
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 1.3f);
            //Hover in front of the owner.
            if (Main.myPlayer == Projectile.owner && !HasBeenReleased)
            {
                // Smoothly approach a sinusoidal offset as time goes on.
                float verticalOffset = Utils.Remap(Time, 0f, 90f, -30f, (float)Math.Cos(Projectile.timeLeft / 32f) * 30f);
                Vector2 hoverDestination = Owner.Top + new Vector2(Owner.direction * Projectile.scale * 30f, verticalOffset);
                hoverDestination += (Main.MouseWorld - hoverDestination) * SubsumingVortex.GiantVortexMouseDriftFactor;

                Vector2 idealVelocity = Vector2.Zero.MoveTowards(hoverDestination - Projectile.Center, 32f);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, idealVelocity, 0.04f);
                Projectile.netSpam = 0;
                Projectile.netUpdate = true;
            }
        }


        public void AdjustPlayerValues()
        {
            Projectile.spriteDirection = Projectile.direction = Owner.direction;
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!HasBeenReleased || Projectile.timeLeft < ExplodeTime)
                return;

            SoundEngine.PlaySound(CISoundMenu.VortexBoom, Projectile.Center);
            Projectile.timeLeft = ExplodeTime;
            Projectile.netUpdate = true;
            //Buff magic attack speeds for a few seconds.
            Owner.CIMod().BuffSubsumingVortexFireRate = 720;
            DoNumourseProjsSpawn(target);
        }

        private void DoNumourseProjsSpawn(NPC target)
        {
            Player player = Main.player[Projectile.owner];
            //原灾暂时没有复写大漩涡的Onhit,因此这里如果判定到没开星流传颂，直接干掉AI就行了
            if (Projectile.owner == Main.myPlayer)
            {
                //这里最主要是为了确定生成的位置
                int offset = Main.rand.Next(200, 1080);
                //尽可能让射弹在屏幕外生成
                float xPos = player.position.X + offset * Main.rand.NextBool(2).ToDirectionInt();
                float yPos = player.position.Y + (Main.rand.NextBool() ? Main.rand.NextFloat(-600, -801): Main.rand.NextFloat(600, 801));
                Vector2 startPos = new(xPos, yPos);
                //指定好速度和方向
                Vector2 velocity = target.position - startPos;
                float dir = 10 / startPos.X;
                velocity.X *= dir * 150;
                velocity.Y *= dir * 150;
                velocity.X = MathHelper.Clamp(velocity.X, -15f, 15f);
                velocity.Y = MathHelper.Clamp(velocity.Y, -15f, 15f);
                //固定三个，因为这个玩意右键手持的时候是有判定的
                int pCounts = 3;
                //击杀的时候往多个方向生成大量的……台风弹幕.
                for (int j = 0; j < pCounts; j++) 
                {
                    //改色，或者说改饱和度
                    float hue = (j / (float)(pCounts- 1f) + Main.rand.NextFloat(0.3f)) % 1f;
                    int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), startPos, velocity, ModContent.ProjectileType<SubsumingVortexProj>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner, hue); 
                    Main.projectile[p].DamageType = DamageClass.Magic;
                    Main.projectile[p].scale *= 0.85f;
                    //2穿, 即2判
                    Main.projectile[p].penetrate = 2;
                }
            }
        }

        //Draw these vortices behind other projectiles to ensure that they do not obstruct SCal's projectiles.

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.EnterShaderRegion(BlendState.Additive);
            Texture2D worleyNoise = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/BlobbyNoise").Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Vector2 scale = Projectile.Size / worleyNoise.Size() * 2f;
            float spinRotation = Main.GlobalTimeWrappedHourly * 2.4f;

            GameShaders.Misc["CalamityMod:ExoVortex"].Apply();

            //Draw the vortex.
            for (int i = 0; i < CalamityUtils.ExoPalette.Length; i++)
            {
                float spinDirection = (i % 2f == 0f).ToDirectionInt();
                Vector2 drawOffset = (MathHelper.TwoPi * i / CalamityUtils.ExoPalette.Length + Main.GlobalTimeWrappedHourly * spinDirection * 4f).ToRotationVector2() * Projectile.scale * 15f;
                Main.spriteBatch.Draw(worleyNoise, drawPosition + drawOffset, null, CalamityUtils.ExoPalette[i] * Projectile.Opacity, spinDirection * spinRotation, worleyNoise.Size() * 0.5f, scale, 0, 0f);
            }
            Main.spriteBatch.ExitShaderRegion();
            return false;
        }
    }
}
