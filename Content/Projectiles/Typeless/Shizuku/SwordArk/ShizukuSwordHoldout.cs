using CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Texture;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku.SwordArk
{
    public class ShizukuSwordHoldout : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public float OffsetX => 20;
        public float OffsetY => -20;
        public float WeaponRotation => MathHelper.ToDegrees(MathHelper.PiOver4);
        public float AimResponsiveness => 0.25f;
        public int UseDelay = 0;
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
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
        }
        #region 手持射弹基类抄写
        public override void AI()
        {
            if (UseDelay > 0)
                UseDelay--;

            Vector2 offset = new(0, 0);

            Player player = Main.player[Projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true) - offset;

            UpdatePlayerVisuals(player, rrp);

            if (Projectile.owner == Main.myPlayer)
            {
                UpdateAim(rrp, player.HeldItem.shootSpeed);
                //手动抄写手持射弹基类是因为这个武器相对特殊，需要单独把魔力的情况单独写一个
                bool allowContinuedUse = player.CheckMana(player.ActiveItem(), (int)(20 * player.manaCost) * 2, false, false);
                bool stillInUse = (player.channel || player.controlUseTile) && !player.noItems && !player.CCed;
                if (stillInUse && allowContinuedUse)
                {
                    HoldoutAI();
                }
                else
                {
                    DelCondition();
                }
            }

            // 确保不会使用的时候消失
            Projectile.timeLeft = 2;
        }
        public virtual void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(WeaponRotation * player.direction);

            Vector2 offset = new Vector2(OffsetX, OffsetY * player.direction).RotatedBy(Projectile.rotation);
            Projectile.Center = playerHandPos + offset;

            Projectile.spriteDirection = Projectile.direction;


            player.ChangeDir(Main.MouseWorld.X - player.Center.X > 0 ? 1 : -1);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
        }

        public virtual void UpdateAim(Vector2 source, float speed)
        {
            Vector2 aim = Vector2.Normalize(Main.MouseWorld - source);
            if (aim.HasNaNs())
            {
                aim = -Vector2.UnitY;
            }

            aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), aim, AimResponsiveness));
            aim *= speed;

            if (aim != Projectile.velocity)
            {
                Projectile.netUpdate = true;
            }
            Projectile.velocity = aim;
        }
        #endregion
        public void DelCondition()
        {
            Projectile.Kill();
        }

        public void HoldoutAI()
        {
            if (GlowingFadingTimer is 0)
            {
                SoundEngine.PlaySound(CISoundMenu.ShizukuSwordCharge with { Volume = 0.9f, MaxInstances = 0 }, Projectile.Center);
                //生成的一瞬间象征性地消耗200点蓝量
                Owner.CheckMana(Owner.ActiveItem(), (int)(ShizukuSword.GetManaUsage() * Owner.manaCost), true, false);
            }
            GlowingFadingTimer += 1f;
            if (GlowingFadingTimer > 20f)
                GlowingFadingTimer = 20f;
            if (Owner.ownedProjectileCounts[ModContent.ProjectileType<ShizukuStarHoldout>()] < 1)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Owner.Center, Projectile.velocity, ModContent.ProjectileType<ShizukuStarHoldout>(), Projectile.damage, 0f, Owner.whoAmI);
                proj.DamageType = DamageClass.Magic;
            }
        }

        public SpriteBatch spriteBatch { get => Main.spriteBatch; }
        public GraphicsDevice graphicsDevice { get => Main.graphics.GraphicsDevice; }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            float drawRotation = Projectile.rotation + (Projectile.spriteDirection is -1 ? MathHelper.Pi : 0f);
            float rot = Projectile.spriteDirection is -1 ? drawRotation - MathHelper.ToRadians(7) : drawRotation + MathHelper.ToRadians(7);
            SpriteEffects flipSprite = (Projectile.spriteDirection * Owner.gravDir == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            DrawSword(rot, flipSprite, drawPosition, ref lightColor);
            DrawGlow(rot, flipSprite, drawPosition);
            return false;
        }
        //绘制辉光
        private void DrawGlow(float rot, SpriteEffects flipSprite, Vector2 drawPosition)
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

            //获取辉光。
            Texture2D Glowtexture = CITextureRegistry.ShizukuSwordGlow.Value;
            Vector2 glowRotationPoint = Glowtexture.Size() / 2f;
            //进行渐变
            Color setColor = Color.White;
            setColor.A = (byte)(255 / 20f * GlowingFadingTimer);
            Main.spriteBatch.Draw(Glowtexture, drawPosition, null, setColor, rot, glowRotationPoint, Projectile.scale * Owner.gravDir * 0.5f, flipSprite, 0f);
            #endregion
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                Main.Rasterizer,
                null,
                Main.GameViewMatrix.TransformationMatrix);
        }
        //绘制剑本体
        public void DrawSword(float rot, SpriteEffects flip, Vector2 drawPos, ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 rotationPoint = texture.Size() * 0.5f;
            Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(lightColor), rot, rotationPoint, Projectile.scale * Owner.gravDir, flip);
        }
    }
}