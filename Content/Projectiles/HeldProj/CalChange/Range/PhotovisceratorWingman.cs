using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.Content.Projectiles.CalProjChange;
using CalamityInheritance.System.Configs;
using CalamityMod;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;
using static CalamityInheritance.Utilities.CIFunction;
using static System.Net.Mime.MediaTypeNames;

namespace CalamityInheritance.Content.Projectiles.HeldProj.CalChange.Range
{
    public class PhotovisceratorWingman : BaseHeldProj, ILocalizedModType
    {
        public int Reentry = 1;
        public override LocalizedText DisplayName => CalamityUtils.GetItemName<Photoviscerator>();
        // 利用Y偏移和修改这个进度来实现动画
        public float aniYdistance = 0;
        public float maxYdistance = 80;
        public float minYdistance = -80;
        public override float OffsetX => -60;
        public override float OffsetY => aniYdistance;
        public override float BaseOffsetY => 0;
        public override float WeaponRotation => 0;
        // 旋转速度
        public override float AimResponsiveness => 0.1f;
        public Player Owner => Main.player[Projectile.owner];
        // 弹药消耗
        public static float AmmoNotConsumeChance = 0.95f;
        public override string Texture => "CalamityInheritance/Content/Projectiles/HeldProj/CalChange/Range/PhotovisceratorWingman";
        public float leftUseCD = 30;
        public float animationProgress = 40;
        public float AnimationCD = 20;
        public bool firstani = false;
        public bool firstFrame = false;
        public override void SetDefaults()
        {
            Projectile.width = 96;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
        }
        public Color sparkColor;
        public int attacktimer = 0;
        public override void HoldoutAI()
        {
            ref float timer = ref Projectile.ai[0];
            ref float animationCD = ref Projectile.ai[1];
            attacktimer++;
            if (!firstFrame)
            {
                Projectile.rotation = Projectile.AngleTo(Main.MouseWorld);
                Reentry = (int)Projectile.ai[2];
                firstFrame = true;
            }

            // 动画CD
            if (animationCD > 0)
                animationCD--;

            if (!firstani)
                FirstMove(ref timer);
            else
                NorMove(ref timer, ref animationCD);

            sparkColor = Main.rand.Next(4) switch
            {
                0 => Color.Red,
                1 => Color.MediumTurquoise,
                2 => Color.Orange,
                _ => Color.LawnGreen,
            };

            Color energyColor = sparkColor;
            Vector2 verticalOffset = new Vector2(10, 0).RotatedBy(Projectile.rotation);

            // 向后发射粒子
            Vector2 flameAngle = -Vector2.UnitY.RotatedBy(Projectile.rotation + MathHelper.ToRadians(Main.rand.NextFloat(180, 360)));
            SquishyLightParticle exoEnergy = new(Projectile.Center - verticalOffset, flameAngle * Main.rand.NextFloat(0.8f, 3.6f), 0.25f, energyColor, 20);
            GeneralParticleHandler.SpawnParticle(exoEnergy);
            SquishyLightParticle exoEnergy2 = new(Projectile.Center - verticalOffset, flameAngle * Main.rand.NextFloat(0.4f, 3.2f), 0.2f, energyColor, 12);
            GeneralParticleHandler.SpawnParticle(exoEnergy2);

            if (attacktimer % 8 == 0)
            {
                // 始终发射激光
                Owner.PickAmmo(Owner.ActiveItem(), out _, out float shootSpeed, out int damage, out float knockback, out _, Main.rand.NextFloat() <= AmmoNotConsumeChance);

                Vector2 Projdirection = Vector2.UnitX.RotatedBy(Projectile.rotation);
                Projdirection.SafeNormalize(Vector2.UnitX);

                Vector2 firseoffset = new Vector2(20, 0).RotatedBy(Projectile.rotation);

                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + firseoffset, Projdirection * shootSpeed, ModContent.ProjectileType<PhotovisceratorLaser>(), damage, knockback, Projectile.owner, 0f, Projectile.whoAmI, 1f);
                Projectile.localAI[0] = Main.projectile[p].whoAmI;
            }

        }
        #region 覆写玩家效果
        public override void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            if (Projectile.rotation < 0f)
                Projectile.rotation += MathHelper.TwoPi;
            else if (Projectile.rotation > MathHelper.TwoPi)
                Projectile.rotation -= MathHelper.TwoPi; //确保转角一直在2pi内

            Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.AngleTo(Main.MouseWorld), 0.35f);

            Vector2 playeraim = Vector2.Normalize(Main.MouseWorld - player.Center);
            Vector2 offset = new Vector2(OffsetX, OffsetY).RotatedBy(playeraim.ToRotation());

            Projectile.Center = Vector2.Lerp(Projectile.Center, player.Center + offset, AimResponsiveness);
        }
        #endregion
        #region 覆写指向目标
        public override void UpdateAim(Vector2 source, float speed)
        {
        }

        #endregion
        #region 移动
        // 处理第一次移动
        public void FirstMove(ref float timer)
        {
            if (timer < animationProgress)
            {
                timer++;
                float progress = EasingHelper.EaseOutExpo((float)timer / animationProgress);
                aniYdistance = MathHelper.Lerp(0, maxYdistance * Reentry, progress);
            }
            else
            {
                timer = 0;
                firstani = true;
            }
        }
        // 处理后续移动
        public void NorMove(ref float timer, ref float CD)
        {
            // 有CD不会处理移动
            if (CD <= 0)
            {
                timer++;
                if (timer < animationProgress)
                {
                    float progress = EasingHelper.EaseOutExpo((float)timer / animationProgress);
                    aniYdistance = MathHelper.Lerp(maxYdistance * Reentry, minYdistance * Reentry, progress);
                }

                if (timer >= animationProgress)
                {
                    if (Reentry == 1)
                        Reentry = -1;
                    else
                        Reentry = 1;

                    timer = 0;
                    CD = AnimationCD;
                    Shoot();
                }
            }
        }
        #endregion
        #region 发射
        public void Shoot()
        {
            // Consume ammo and retrieve projectile stats; has a chance to not consume ammo
            Owner.PickAmmo(Owner.ActiveItem(), out _, out float shootSpeed, out int damage, out float knockback, out _, Main.rand.NextFloat() <= AmmoNotConsumeChance);

            Vector2 Projdirection = Vector2.UnitX.RotatedBy(Projectile.rotation);
            Projdirection.SafeNormalize(Vector2.UnitX);
            for (int i = 0; i < 2; i++)
            {
                Vector2 bombPos = Projectile.Center + new Vector2(15f , 0).RotatedBy(Projectile.rotation);
                int yDirection = (i == 0).ToDirectionInt();
                Vector2 bombVel = Projdirection.RotatedBy(0.2f * yDirection);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), bombPos, bombVel * shootSpeed, ModContent.ProjectileType<ExoLight>(), damage, knockback, Projectile.owner, yDirection);
            }
        }
        #endregion
        #region 覆写绘制
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
        public override bool ExtraPreDraw(ref Color lightColor)
        {
            DrawLaserBeam();
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            float drawRotation = Projectile.rotation + (Projectile.ai[2] == -1 ? 0f : MathHelper.Pi);
            Vector2 rotationPoint = texture.Size() * 0.5f;
            SpriteEffects flipSprite = Projectile.ai[2] == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(texture, drawPosition, null, Projectile.GetAlpha(lightColor), drawRotation, rotationPoint, Projectile.scale * Main.player[Projectile.owner].gravDir, flipSprite);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>("CalamityInheritance/Content/Projectiles/HeldProj/CalChange/Range/PhotovisceratorWingmanGlow").Value, drawPosition, null, Color.White, drawRotation, rotationPoint, Projectile.scale * Main.player[Projectile.owner].gravDir, flipSprite);
            return false;
        }
        #endregion
        public void DrawLaserBeam()
        {
            Projectile Laser = Main.projectile[(int)Projectile.localAI[0]];
            // 基础参数
            const int laserLength = 4400;
            float alphaMultiplier = Math.Max(0, Laser.Opacity * Math.Min(1, Laser.timeLeft / 3f));
            float beamRotation = Projectile.rotation;
            float Scale = Laser.ai[2] == 1 ? 0.3f : 1.5f * Laser.ai[0];
            // 颜色
            Color baseColor = Color.White * alphaMultiplier;
            Color Auxiliarycolor = Color.LightGreen * alphaMultiplier;
            baseColor.A = Auxiliarycolor.A = 0;

            // 纹理
            Texture2D mainTexture = ModContent.Request<Texture2D>("CalamityInheritance/Content/Projectiles/CalProjChange/PhotovisceratorLaser").Value;
            Texture2D bloomTexture = Main.Assets.Request<Texture2D>("Images/Extra_197").Value;

            DrawBloomEffect(bloomTexture, Auxiliarycolor, beamRotation, laserLength, Scale, Vector2.Zero, Laser);
            //DrawBloomEffect(bloomTexture, Auxiliarycolor, beamRotation, laserLength, Scale, Vector2.Zero);

            DrawMainBeam(mainTexture, baseColor, beamRotation, laserLength, Scale * 1.4f, Vector2.Zero, Laser);
            DrawMainBeam(mainTexture, baseColor, beamRotation, laserLength, Scale, new Vector2(0, -8f), Laser);

            DrawBloomEffect(bloomTexture, Auxiliarycolor, beamRotation, laserLength, Scale, new Vector2(0, -8f), Laser);

            DrawMainBeam(mainTexture, baseColor, beamRotation, laserLength, Scale, new Vector2(0, 8f), Laser);

            DrawBloomEffect(bloomTexture, Auxiliarycolor, beamRotation, laserLength, Scale, new Vector2(0, 8f), Laser);
        }
        #region 绘制本体辉光
        public void DrawBloomEffect(Texture2D texture, Color color, float rotation, int length, float Scale, Vector2 offset, Projectile proj)
        {
            Rectangle rect = new Rectangle(0, 0, length, texture.Height);
            Vector2 scale = new Vector2(1, proj.localAI[0] / 60f / (proj.ai[2] + 2));
            Vector2 origin = new Vector2(0, texture.Height / 2);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + offset.RotatedBy(rotation), rect,
                color, rotation, origin, scale * Scale,
                SpriteEffects.None);
        }
        #endregion
        #region 绘制主光束
        private void DrawMainBeam(Texture2D texture, Color color, float rotation, int length, float Scale, Vector2 offset, Projectile proj)
        {
            Rectangle rect = new Rectangle(0, 0, length, texture.Height);
            Vector2 scale = new Vector2(1, proj.localAI[0] / 9f);
            Vector2 origin = new Vector2(0, texture.Height / 2);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + offset.RotatedBy(rotation),
                new Rectangle(0, 0, length, texture.Height),
                color, rotation, origin, scale * Scale,
                SpriteEffects.None);
        }
        #endregion
    }
}
