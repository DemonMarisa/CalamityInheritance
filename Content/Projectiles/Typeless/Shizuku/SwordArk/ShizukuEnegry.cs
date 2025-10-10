using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Texture;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UtfUnknown.Core.Probers;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku.SwordArk
{
    public class ShizukuEnergy : ModProjectile, ILocalizedModType
    {
        public Player Owner => Projectile.GetProjOwner();
        public ref float Direction => ref Projectile.ai[0];
        public ref float MaxTime => ref Projectile.ai[1];
        public ref float Scale => ref Projectile.ai[2];
        public ref float CurTime => ref Projectile.localAI[0];
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => $"{Generic.WeaponPath}/Typeless/ShizukuItem/ShizukuSword";
        public override void SetDefaults()
        {
            // 这里宽度和高度并不重要，因为我们使用了自定义碰撞。
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<MeleeDamageClass>();
            Projectile.penetrate = 15;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.ownerHitCheck = true; // 投射物会进行视线检查，确保不会穿过瓦片造成伤害
            Projectile.ownerHitCheckDistance = 300f; // 投射物能击中目标的最大距离为300像素（即18.75个瓦片）
            Projectile.usesOwnerMeleeHitCD = true; // 投射物使用与近战攻击相同的免疫帧数量
                                                   // 通常，投射物在击中所有可能的敌人后会消失。但在这个情况下，我们希望投射物继续存在，以便展示挥动的视觉效果。
            Projectile.stopsDealingDamageAfterPenetrateHits = true;

            // 我们将为这个投射物使用自定义AI。原始的圣剑（Excalibur）使用aiStyle 190。
            Projectile.aiStyle = -1;
            // 将投射物的aiStyle设置为-1，表示使用自定义AI
            // AIType = ProjectileID.Excalibur;
            // 如果使用自定义AI，请添加这行代码。否则，来自药水的视觉效果将在投射物的中心而不是在弧周围生成。
            // 我们将在AI()中自己生成围绕弧的视觉效果。
            Projectile.noEnchantmentVisuals = true;
        }
        public override void AI()
        {
            CurTime++;
            float percentageOfLife = CurTime / MaxTime;
            float energyRot = Projectile.velocity.ToRotation();
            float adjustRot = (MathHelper.Pi + MathHelper.PiOver2)  * Direction * percentageOfLife + energyRot + Direction * MathHelper.Pi + Owner.fullRotation; 
            Projectile.rotation = adjustRot;
            //跟随玩家中心
            Vector2 offsetY = new Vector2(Owner.RotatedRelativePoint(Owner.MountedCenter).X, Owner.RotatedRelativePoint(Owner.Center).Y - 5);
            Projectile.Center = offsetY - Projectile.velocity;
            if (CurTime >= MaxTime)
                Projectile.Kill();

            Owner.heldProj = Projectile.whoAmI;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // 这是圆锥的周长大小，即范围的大小。原版游戏使用94f来匹配纹理的大小。
            float coneLength = 94f * Projectile.scale;
            // 这个数值影响碰撞开始和结束时的旋转程度。
            // 较大的Pi值会使碰撞逆时针旋转。
            // 较小的Pi值会使碰撞顺时针旋转。
            // (Projectile.ai[0]代表方向)
            float collisionRotation = MathHelper.Pi * 2f / 25f * Projectile.ai[0];
            float maximumAngle = MathHelper.PiOver4; // The maximumAngle is used to limit the rotation to create a dead zone.
            float coneRotation = Projectile.rotation + collisionRotation;

            // 最大角度用于限制旋转，以创建一个死区。
            // Dust.NewDustPerfect(Projectile.Center + coneRotation.ToRotationVector2() * coneLength, DustID.Pixie, Vector2.Zero);
            // Dust.NewDustPerfect(Projectile.Center, DustID.BlueFairy, new Vector2((float)Math.Cos(maximumAngle) * Projectile.ai[0], (float)Math.Sin(maximumAngle)) * 5f); // Assumes collisionRotation was not changed

            // 圆锥的旋转角度是投射物的旋转角度加上碰撞旋转。
            if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength, coneRotation, maximumAngle))
                return true;

            // 第一个圆锥并不是整个挥动弧线，因此我们需要检查第二个圆锥以覆盖弧线的后部。
            float backOfTheSwing = Utils.Remap(CurTime, MaxTime * 0.3f, MaxTime * 0.5f, 1f, 0f);
            if (backOfTheSwing > 0f)
            {
                float coneRotation2 = coneRotation - MathHelper.PiOver4 * CurTime * backOfTheSwing;
                if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength, coneRotation2, maximumAngle))
                    return true;
            }
            return false;
        }
        public override void CutTiles()
        {
            Vector2 starting = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 60f * Projectile.scale;
            Vector2 ending = (Projectile.rotation + MathHelper.PiOver4).ToRotationVector2() * 60f * Projectile.scale;
            float width = 60f * Projectile.scale;
            Utils.PlotTileLine(Projectile.Center + starting, Projectile.Center + ending, width, DelegateMethods.CutTiles);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hit.HitDirection = Owner.Center.X < target.Center.X ? 1 : -1;
        }
        public SpriteBatch spriteBatch { get => Main.spriteBatch; }
        public GraphicsDevice graphicsDevice { get => Main.graphics.GraphicsDevice; }
        public override bool PreDraw(ref Color lightColor)
        {

            Projectile.GetBaseDrawField(out Texture2D tex, out Vector2 posBase, out Vector2 orig);
            // orig.Y += CIConfig.Instance.Debugint;
            SpriteEffects flip = Direction < 0f? SpriteEffects.FlipVertically : SpriteEffects.None;
            BlendState blendState = spriteBatch.GraphicsDevice.BlendState;
            SamplerState samplerState = spriteBatch.GraphicsDevice.SamplerStates[0]; 
            // 直接尝试绘制辉光。
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.Additive,
                SamplerState.AnisotropicClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null,
                Main.GameViewMatrix.TransformationMatrix);
            //发光绘制只在准备发起攻击的时候进行
            //获取辉光。
            Texture2D Glowtexture = CITextureRegistry.ShizukuSwordGlow.Value;
            Vector2 glowPostion = Projectile.Center - Main.screenPosition;
            Vector2 glowRotationPoint = Glowtexture.Size() / 2f;

            //终极史山这一块
            if (Direction > 0)
            {
                orig.X -= 45;
                orig.Y += 45;
                glowRotationPoint.X -= 75;
                glowRotationPoint.Y += 95;
            }
            else if (Direction <= 0)
            {
                orig.X -= 40;
                orig.Y -= 40;
                glowRotationPoint.X -= 70;
                glowRotationPoint.Y -= 70;
            }
            // glowRotationPoint.Y += CIConfig.Instance.Debugint;
            //进行渐变
            Main.spriteBatch.Draw(tex, posBase, null, Color.White, Projectile.rotation, orig, Projectile.scale, flip, 1f);
            Color setColor = Color.White;
            Main.spriteBatch.Draw(Glowtexture, glowPostion, null, setColor, Projectile.rotation, glowRotationPoint, Projectile.scale * 0.5f, flip, 1f);
            spriteBatch.End();
            spriteBatch.Begin();
            return false;
        }
    }
}