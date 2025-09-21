using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Texture;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku.SwordArk
{
    public class ShizukuSwordHoldout : BaseHeldProj, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override float OffsetX => 20;
        public override float OffsetY => -20;
        public override float WeaponRotation => MathHelper.ToDegrees(MathHelper.PiOver4);
        public override float AimResponsiveness => 0.25f;
        public Player Owner => Main.player[Projectile.owner];
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Typeless/ShizukuItem/ShizukuSword";
        public int StarSpawnCD = 0;
        public const float Radius = 210f; 
        public ref float GlowingFadingTimer => ref Projectile.CalamityInheritance().ProjNewAI[1];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 84;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.ignoreWater = true;
        }
        public override bool PreKill(int timeLeft)
        {
            if (Owner.ownedProjectileCounts[ModContent.ProjectileType<ShizukuStarHoldout>()] > 0)
                Main.NewText($"先处死手持射弹");
            return base.PreKill(timeLeft);
        }
        public override void HoldoutAI()
        {
            GlowingFadingTimer += 1f;
            if (GlowingFadingTimer > 20f)
                GlowingFadingTimer = 20f;
            if (Owner.ownedProjectileCounts[ModContent.ProjectileType<ShizukuStarHoldout>()] == 0)
            {
                Projectile star = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Main.MouseWorld, Projectile.velocity, ModContent.ProjectileType<ShizukuStarHoldout>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI);

            }
            NPC target = Owner.FindClosestTargetPlayer(1800f);
            // if (Owner.ownedProjectileCounts[ModContent.ProjectileType<ShizukuStarMark>()] < 1)
            // {
            //     if (target != null)
            //     {
            //         Projectile mark = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<ShizukuStarMark>(), 0, 0f, Owner.whoAmI, target.whoAmI);
            //         mark.originalDamage = Projectile.damage;
            //     }
            // }
        }
        public SpriteBatch spriteBatch { get => Main.spriteBatch; }
        public GraphicsDevice graphicsDevice { get => Main.graphics.GraphicsDevice; }
        public override void MorePreDraw(ref Color lightColor)
        {
            #region 在基础弹幕上层绘制发光
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.Additive,
                SamplerState.AnisotropicClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Main.GameViewMatrix.TransformationMatrix);
            //发光绘制只在准备发起攻击的时候进行
            Player player = Main.player[Projectile.owner];
            //获取辉光。
            Texture2D Glowtexture = CITextureRegistry.ShizukuSwordGlow.Value;
            float glowRotation = Projectile.rotation + (Projectile.spriteDirection == -1 ? MathHelper.Pi : 0f);
            Vector2 glowPostion = Projectile.Center - Main.screenPosition;
            Vector2 glowRotationPoint = Glowtexture.Size() / 2f;
            SpriteEffects glowSpriteFlip = (Projectile.spriteDirection * player.gravDir == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            //进行渐变
            Color setColor = Color.White;
            setColor.A = (byte)(255 / 20f * GlowingFadingTimer);
            Main.spriteBatch.Draw(Glowtexture, glowPostion, null, setColor, glowRotation, glowRotationPoint, Projectile.scale * player.gravDir * 0.5f, glowSpriteFlip, 0f);
            #endregion
            spriteBatch.End();
            spriteBatch.Begin();
        }
    }
}