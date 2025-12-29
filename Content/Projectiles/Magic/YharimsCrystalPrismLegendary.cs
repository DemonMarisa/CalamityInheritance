using System;
using CalamityInheritance.Utilities;
using CalamityMod;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class YharimsCrystalPrismLegendary : ModProjectile, ILocalizedModType
    {

        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public const int BeamCounts = 6;
        public const float MaxCharge = 180f;
        public const float DamageStart = 30f;
        private const float EmitDust = 30f;
        private const float AimSpeed = 0.89f; //最终棱镜为0.92f
        private const int SetSounds = 20;
        private const float MaxManaConsumptionDelay = 15f;
        private const float MinManaConsumptionDelay = 5f;
        private const short FChanger = 0;
        private const short NFBeforeConsumeMana = 1;
        private const short FBetweenConsumeMana = 0;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 22;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
        }

        // ai[FChanger]:控制帧图, ai[NFBeforeConsumeMana] 消耗魔力的下一帧 localIA[FBetweenConsumeMana]允许消耗魔力的间隔帧
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);

            //不断更新当前的伤害，这样可以让魔力病影响他
            Projectile.damage = player.ActiveItem() is null ? 0 : player.GetWeaponDamage(player.ActiveItem());

            //控制帧图
            Projectile.ai[FChanger] += 1f;
            float chargeRatio = MathHelper.Clamp(Projectile.ai[FChanger] / MaxCharge, 0f, 1f);

            //更新水晶帧图，也就是加速水晶的旋转
            Projectile.frameCounter++;
            int framesPerAnimationUpdate = Projectile.ai[FChanger] >= MaxCharge ? 2 : Projectile.ai[FChanger] >= MaxCharge * 0.66f ? 3 : 4;
            if (Projectile.frameCounter >= framesPerAnimationUpdate)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 6)
                    Projectile.frame = 0;
            }

            //音效。
            if (Projectile.soundDelay <= 0)
            {
                Projectile.soundDelay = SetSounds;
                //不让其在第一帧发出音效
                if (Projectile.ai[FChanger] > 1f)
                    SoundEngine.PlaySound(SoundID.Item15, Projectile.Center);
            }

            //水晶蓄力达到一定条件时，我们开始产生粒子。蓄力越久 越多粒子
            if (Projectile.ai[FChanger] > EmitDust && Main.rand.NextFloat() < chargeRatio)
                SpawnEjectionDust(chargeRatio);

            UpdatePlayerVisuals(player, rrp);

            //时刻检测水晶的存在条件。
            if (Projectile.owner == Main.myPlayer)
            {
                //？
                float speedTimesScale = player.ActiveItem().shootSpeed * Projectile.scale;
                UpdateAim(rrp, speedTimesScale, player);

                //查看是否消耗魔力。
                bool allowContinuedUse = !AllowMana() || player.CheckMana(player.ActiveItem(), -1, true, false);
                bool crystalStillInUse = !player.JudgeHoldout() && allowContinuedUse;
                if (allowContinuedUse && player.CIMod().YharimsKilledExo)
                {
                    Projectile.netUpdate = true;
                }

                //光束只会在第一帧的时候建立
                if (crystalStillInUse && Projectile.ai[FChanger] == 1f)
                {
                    Vector2 beamVelocity = Vector2.Normalize(Projectile.velocity);
                    if (beamVelocity.HasNaNs())
                        beamVelocity = -Vector2.UnitY;

                    int damage = Projectile.damage;
                    float kb = Projectile.knockBack;
                    for (int b = 0; b < BeamCounts; b++)
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, beamVelocity, ProjectileType<YharimsCrystalBeamLegendary>(), damage, kb, Projectile.owner, b, Projectile.GetByUUID(Projectile.owner, Projectile.whoAmI));
                    Projectile.netUpdate = true;
                }
                else if (!crystalStillInUse)
                    Projectile.Kill();
            }

            //确保水晶如果出现任何差错都能直接消失
            Projectile.timeLeft = 2;
        }


        private bool AllowMana()
        {
            //如果魔力消耗的timer没有被初始化完成，那么将其初始化，并在第一帧消耗魔力
            if (Projectile.localAI[FBetweenConsumeMana] == 0f)
            {
                Projectile.ai[NFBeforeConsumeMana] = Projectile.localAI[FBetweenConsumeMana] = MaxManaConsumptionDelay;
                return true;
            }
            bool c = Projectile.ai[FChanger] == Projectile.ai[NFBeforeConsumeMana];
            if (c)
            {
                Projectile.localAI[FBetweenConsumeMana] = MathHelper.Clamp(Projectile.localAI[FBetweenConsumeMana] - 1f, MinManaConsumptionDelay, MaxManaConsumptionDelay);
                Projectile.ai[NFBeforeConsumeMana] += Projectile.localAI[FBetweenConsumeMana];
            }
            return c;
        }

        //调整水晶的朝向
        private void UpdateAim(Vector2 source, float speed, Player player)
        {
            Vector2 aimVector = Vector2.Normalize(player.LocalMouseWorld() - source);
            if (aimVector.HasNaNs())
                aimVector = -Vector2.UnitY;
            aimVector = Vector2.Normalize(Vector2.Lerp(aimVector, Vector2.Normalize(Projectile.velocity), AimSpeed));
            aimVector *= speed;

            if (aimVector != Projectile.velocity)
                Projectile.netUpdate = true;
            Projectile.velocity = aimVector;
        }

        private void UpdatePlayerVisuals(Player player, Vector2 rrp)
        {
            //更新水晶的视觉效果，让其在玩家手上
            Projectile.Center = rrp;
            //光束应当在水晶的最顶端发射，而非旁边
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.spriteDirection = Projectile.direction;

            //水晶是一个手持射弹，所以玩家变更朝向的时候他也i得变更朝向
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            //固定流程。
            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
        }

        private void SpawnEjectionDust(float charge)
        {
            Vector2 projDir = Vector2.Normalize(Projectile.velocity);
            int dustType = 90;
            float dustAngle = MathHelper.Pi * 0.76f * (Main.rand.NextBool() ? 1f : -1f);
            float scale = Main.rand.NextFloat(0.9f, 1.2f);
            float speed = 18f * charge;
            Vector2 dustVel = projDir.RotatedBy(dustAngle) * speed;
            float dustForwardOffset = 11f;
            Vector2 dustOrigin = Projectile.Center + dustForwardOffset * projDir;
            Dust d = Dust.NewDustDirect(dustOrigin, 1, 1, dustType, dustVel.X, dustVel.Y);
            d.position += Main.rand.NextVector2Circular(2f, 2f);
            d.noGravity = true;
            d.scale = scale;
        }

        //水晶是一个手持射弹，我们需要手动绘制他的手持贴图
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects eff = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
            int texYOffset = frameHeight * Projectile.frame;
            Vector2 sheetInsertVec = (Projectile.Center + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition).Floor();
            Main.spriteBatch.Draw(tex, sheetInsertVec, new Rectangle?(new Rectangle(0, texYOffset, tex.Width, frameHeight)), Color.White, Projectile.rotation, new Vector2(tex.Width / 2f, frameHeight / 2f), Projectile.scale, eff, 0f);
            return false;
        }
    }
}
