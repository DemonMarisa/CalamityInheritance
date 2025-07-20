using System;
using CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.NPCs.Boss.SCAL.Proj;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Projectiles.Boss;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public class MoonPlaceholder : ModProjectile, ILocalizedModType
    {
        public ref float Timer => ref Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];
        public int OwnedProjectileType = ModContent.ProjectileType<ShizukuEdgeProjectileAlter>();
        public override string Texture => "CalamityInheritance/Content/Projectiles/Typeless/Shizuku/HolyMollyMoon";
        public const float FloatingMoonDistance = 16f;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.width = 240;
            Projectile.height = 234;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 18000;
            Projectile.Opacity = 0f;
        }
        public override void AI()
        {
            DoReady();
            //这里会需要类似于虚空箭袋的逻辑
            DoRotated();
            //类虚空箭袋的增强
            DoBuffedCorrectedProj();
            //擦撞消弹的效果
            DoClearHostileProj();
        }
        public SpriteBatch Sprite { get => Main.spriteBatch; }
        public GraphicsDevice GraphicsMachine { get => Main.graphics.GraphicsDevice; }
        public override bool PreDraw(ref Color lightColor)
        {
            //我tm有点绷不住了
            #region DrawGlowMask
            Sprite.End();
            Sprite.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D moonTex = TextureAssets.Projectile[Projectile.type].Value;
            Projectile.BaseProjPreDraw(moonTex, lightColor, MathHelper.ToRadians(7));
            Sprite.End();
            Sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D glowMask = TextureAssets.Projectile[Projectile.type].Value;
            float glowRotation = Projectile.rotation + MathHelper.Pi * (Projectile.spriteDirection == -1).ToInt();
            Vector2 glowPosition = Projectile.Center - Main.screenPosition;
            Vector2 glowRotationPoint = glowMask.Size() / 2f;
            SpriteEffects glowSpriteFlipper = (Projectile.spriteDirection * Owner.gravDir == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Color glowColor = Color.White;
            glowColor.A = (byte)(255 * Projectile.Opacity);
            Sprite.Draw(glowMask, glowPosition, null, glowColor, glowRotation + MathHelper.ToRadians(7), glowRotationPoint, Projectile.scale * Owner.gravDir, glowSpriteFlipper, 0f);
            Sprite.End();
            Sprite.Begin();
            #endregion

            return false;
        }

        public void DoReady()
        {
            //往AI里面不断检查是否应该生成这个射弹，如果不允许生成，则自动干掉自己
            if (Owner.HeldItem.type != ModContent.ItemType<ShizukuSword>())
            {
                Projectile.Opacity -= 0.05f;
                Projectile.netUpdate = true;
                if (Projectile.Opacity == 0f)
                    Projectile.Kill();
                return;
            }
            //开始生成, 向上投射
            Projectile.Opacity += 0.05f;
            Projectile.velocity *= 0.84f;
        }

        public void DoRotated()
        {
            //水平位置固定与玩家一个方位
            if (Projectile.Center.X != Owner.Center.X)
            {

            };
            Projectile.rotation += 0.01f;
        }

        //Todo: 我应该需要用一种方法来降低开销...
        public void DoBuffedCorrectedProj()
        {
            //如果玩家没有激活武器，干掉下方的AI，不要执行
            //Todo: 这里改为检测手持射弹。
            /*
            if (Owner.ActiveItem().type != ModContent.ItemType<ShizukuEdge>() || Owner.dead)
                return;
            */
            //加强类型
            foreach (var proj in Main.ActiveProjectiles)
            {
                Projectile p = proj;
                if (!CheckProj<SoulLargePlaceholder>(p) || !CheckProj<SoulMidPlaceholder>(p) || !CheckProj<SoulSmallPlaceholder>(p))
                    continue;
                if (!CheckProj<ScythePlaceholder>(p))
                    continue;
                //穿过的弹幕赋予ai2 = -1f，我们在这里不执行1.5倍增伤，因为我们会考虑其启用一个更新的AI
                if (Projectile.Hitbox.Intersects(p.Hitbox))
                    p.ai[2] = -1f;
                
            }
        }
        public void DoClearHostileProj()
        {
            //获取月球与玩家的距离
            float distance = (Projectile.Center - Owner.Center).Length();
            //略微往外扩展一点
            distance += CIFunction.SetDistance(20);
            //我们需要以这个距离排除掉不符合条件的射弹，减少开销
            foreach (var proj in Main.ActiveProjectiles)
            {
                Projectile possibleProj = proj;
                //排除距离之外的射弹
                if ((possibleProj.Center - Owner.Center).Length() > distance)
                    continue;
                //排除非敌伤害
                if (!possibleProj.hostile)
                    continue;
                //补特判：别把红月干掉了，不然终灾AI会直接鬼畜
                if (CheckProj<BrimstoneMonster>(possibleProj) || CheckProj<BrimstoneMonsterLegacy>(possibleProj))
                    continue;
                //查看是否接触玩家，如果是则舍去
                if (Projectile.Hitbox.Intersects(proj.Hitbox))
                {
                    proj.Kill();
                    //有些射弹可能不可消除，这里补一个active
                    proj.active = false;
                    //每一下成功的消弹都发送一个tint
                    SendTint();
                }
            }
        }

        public void SendTint()
        {
        }

        public static bool CheckProj<T>(Projectile proj) where T : ModProjectile => proj.type == ModContent.ProjectileType<T>();
        public bool CheckHeldProj<T>(Projectile proj) where T : ModProjectile => Owner.ownedProjectileCounts[proj.type] <= 0;
    }
}