using System;
using CalamityMod;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoLore
{
    public class ExoChainsawProj: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public static string TexPath => "CalamityMod/Projectiles/Melee";
        public override string Texture => $"{TexPath}/PhotonRipperProjectile";

        public Player Owner => Main.player[Projectile.owner];
        public const float ZeroChargeDamageRatio = 0.36f;
        public const float ToothDamageRatio = 0.23f;
        //每一个锯齿的发射频率
        public const int ToothShootRate = 5;
        // 150 -> 50
        public const int ChargeUpTime = 10;
        public ref float Time => ref Projectile.ai[0];

        // 电锯伤害本身由锯齿造成。锯齿的伤害逻辑会类似于最终棱镜（相当于套了个棱镜的伤害机制）
        public ref float ToothDamage => ref Projectile.ai[1];
        public float ChargeUpPower => MathHelper.Clamp((float)Math.Pow(Time / ChargeUpTime, 1.6D), 0f, 1f);

        public override void SetDefaults()
        {
            Projectile.width = 132;
            Projectile.height = 56;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
            Projectile.noEnchantmentVisuals = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Texture2D glowmaskTexture = ModContent.Request<Texture2D>($"{TexPath}/PhotonRipperGlowmask").Value;
            Rectangle glowmaskRectangle = glowmaskTexture.Frame(1, 6, 0, Projectile.frame);
            Vector2 origin = texture.Size() * 0.5f;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            SpriteEffects direction = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(texture, drawPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, direction, 0);
            Main.EntitySpriteDraw(glowmaskTexture, drawPosition, glowmaskRectangle, Color.White, Projectile.rotation, origin, Projectile.scale, direction, 0);
            return false;
        }

        public override void AI()
        {
            //每一帧都会刷新这个手持射弹的伤害
            //不然你也不想看到这玩意固定面板吧
            Projectile.damage = Owner.ActiveItem() is null ? 0 : Owner.GetWeaponDamage(Owner.ActiveItem());
            DetermineDamage();

            PlayChainsawSounds();

            Vector2 playerRotatedPosition = Owner.RotatedRelativePoint(Owner.MountedCenter);
            if (Main.myPlayer == Projectile.owner)
            {
                if (!Owner.CantUseHoldout() && Projectile.ai[2] == 1 || Projectile.ai[2] == 0 && Owner.Calamity().mouseRight && Owner.active && !Owner.dead)
                    HandleChannelMovement(playerRotatedPosition);
                else
                    Projectile.Kill();
            }

            //这个用于处理手持射弹的朝向
            DetermineVisuals(playerRotatedPosition);
            ManipulatePlayerValues();
            EmitPrettyDust();

            if (Time % ToothShootRate == ToothShootRate - 1f)
                ReleasePrismTeeth();

            //保证射弹能正常处死 
            Projectile.timeLeft = 2;

            Time++;
        }

        public void PlayChainsawSounds()
        {
            if (Projectile.soundDelay <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item22, Projectile.Center);
                Projectile.soundDelay = (int)MathHelper.Lerp(30f, 12f, ChargeUpPower);
            }    
        }

        public void DetermineDamage()
        {
            // 锯齿的伤害需要正确的进行初始化
            if (Main.myPlayer == Projectile.owner && ToothDamage == 0f)
            {
                ToothDamage = ToothDamageRatio * Projectile.damage;
                Projectile.netUpdate = true;
            }

            //然后我们才开始计算锯齿的实际伤害。这个伤害也会被电锯射弹本身影响
            if (ToothDamage != 0f)
            {
                float fullMult = ToothDamageRatio;
                float zeroMult = ZeroChargeDamageRatio * ToothDamageRatio;
                ToothDamage = (int)MathHelper.SmoothStep(Projectile.damage * zeroMult, Projectile.damage * fullMult, ChargeUpPower);
            }
        }

        public void DetermineVisuals(Vector2 playerRotatedPosition)
        {
            float directionAngle = Projectile.velocity.ToRotation();
            Projectile.rotation = directionAngle;

            int oldDirection = Projectile.spriteDirection;
            if (oldDirection == -1)
                Projectile.rotation += MathHelper.Pi;

            Projectile.direction = Projectile.spriteDirection = (Math.Cos(directionAngle) > 0).ToDirectionInt();

            //如果朝向与旧朝向有所差别，修改Pi。
            //不然电锯会有一帧的鬼畜
            if (Projectile.spriteDirection != oldDirection)
                Projectile.rotation -= MathHelper.Pi;

            //保住手持射弹的位置
            Projectile.position = playerRotatedPosition - Projectile.Size * 0.5f + directionAngle.ToRotationVector2() * 30f;

            //每一帧都会略微更新射弹位置，好让电锯表现出正在“抖动”的感觉
            //这个会在下一帧重置
            Projectile.position += Main.rand.NextVector2Circular(1.4f, 1.4f);

            //刷新发光贴图。
            //SmoothStep会让这个刷新变得曲线顺滑。 
            Projectile.frameCounter += (int)MathHelper.SmoothStep(12f, 33f, ChargeUpPower);
            if (Projectile.frameCounter >= 32)
            {
                Projectile.frame = (Projectile.frame + 1) % 6;
                Projectile.frameCounter = 0;
            }
        }

        public void HandleChannelMovement(Vector2 playerRotatedPosition)
        {
            Vector2 idealAimDirection = (Main.MouseWorld - playerRotatedPosition).SafeNormalize(Vector2.UnitX * Owner.direction);

            float angularAimVelocity = 0.15f;
            float directionAngularDisparity = Projectile.velocity.AngleBetween(idealAimDirection) / MathHelper.Pi;

            angularAimVelocity += MathHelper.Lerp(0f, 0.25f, Utils.GetLerpValue(0.28f, 0.08f, directionAngularDisparity, true));

            if (directionAngularDisparity > 0.02f)
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, idealAimDirection, angularAimVelocity);
            else
                Projectile.velocity = idealAimDirection;

            Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX * Owner.direction);
        }

        public void ManipulatePlayerValues()
        {
            Owner.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;
            Owner.ChangeDir(Projectile.direction);
        }

        public void EmitPrettyDust()
        {
            if (Main.dedServ)
                return;

            for (int i = 0; i < 2; i++)
            {
                Vector2 spawnPosition = Projectile.Center + Projectile.velocity * 35f;
                //粒子。没啥好说
                spawnPosition += Main.rand.NextVector2CircularEdge(9f, 35f).RotatedBy(Projectile.velocity.ToRotation() + MathHelper.PiOver2);

                Dust rainbowSpark = Dust.NewDustPerfect(spawnPosition, 261);
                rainbowSpark.velocity = Projectile.velocity * 3f + Main.rand.NextVector2CircularEdge(1.5f, 1.5f);
                rainbowSpark.noGravity = true;
                rainbowSpark.color = Main.hslToRgb((Time / 40f + Main.rand.NextFloat(-0.1f, 0.1f)) % 1f, 0.95f, 0.6f);
                rainbowSpark.scale = Main.rand.NextFloat(0.9f, 1.25f);
            }
        }

        public void ReleasePrismTeeth()
        {

            SoundEngine.PlaySound(SoundID.Item101, Projectile.Center);

            if (Main.myPlayer != Projectile.owner)
                return;

            float shootReach = MathHelper.SmoothStep(Projectile.width * 1.8f, Projectile.width * 5.7f + 34f, ChargeUpPower);

            //修改这个也会让锯齿随着你鼠标的距离而改变
            shootReach *= Owner.ActiveItem().shootSpeed;

            float distanceFromMouse = Owner.Distance(Main.MouseWorld);

            //锯齿到鼠标的距离比最基础的距离短，只让他抵达鼠标距离。
            //而如果鼠标距离足够短，则设定一个最小值
            if (distanceFromMouse < shootReach)
            {
                if (distanceFromMouse > 40f)
                    shootReach = distanceFromMouse + 32f;
                else
                    shootReach = 72f;
            }

            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Owner.Center, Projectile.velocity, ModContent.ProjectileType<PrismTooth>(), (int)ToothDamage, 0f, Projectile.owner, shootReach, Projectile.whoAmI, Projectile.ai[2]);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            //防止射弹本体穿墙
            float _ = 0f;
            float width = Projectile.scale * 36f;
            Vector2 start = Projectile.Center;
            Vector2 end = Projectile.Center + Projectile.velocity * 70f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, width, ref _);
        }
    }
}
