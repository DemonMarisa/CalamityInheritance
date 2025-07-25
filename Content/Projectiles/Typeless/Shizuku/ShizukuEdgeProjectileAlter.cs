using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public class ShizukuEdgeProjectileAlter : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Typeless/ShizukuItem/ShizukuEdge";
        public Player Owner => Main.player[Projectile.owner];
        public static Vector2 HoldingOffset => new (-5, 10f);
        public ref float AttackTimer => ref Projectile.ai[1];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.height = 108;
            Projectile.width = 84;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 90000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
        }
        public override void AI()
        {
            KillProj();
            PlayerPostionRotation();
            AddLight();
            ShootProjectile();
        }

        public void ShootProjectile()
        {
            #region 初始化
            int scytherProj = ModContent.ProjectileType<ScythePlaceholder>();
            int ghostRandom = Main.rand.Next(0, 3);
            ghostRandom = ghostRandom switch
            {
                0 => ModContent.ProjectileType<SoulSmallPlaceholder>(),
                1 => ModContent.ProjectileType<SoulMidPlaceholder>(),
                _ => ModContent.ProjectileType<SoulLargePlaceholder>(),
            };
            var projSrc = Projectile.GetSource_FromThis();
            float scytheDmg = Projectile.damage * 1.2f;
            float ghostDmg = Projectile.damage * 0.4f;
            #endregion
            AttackTimer += 1f;
            if (AttackTimer % 15f == 0)
            {
                ShootGhost(ghostRandom, ghostDmg, projSrc);
                ShootScythe(scytherProj, scytheDmg, projSrc);
            }
        }

        public void ShootScythe(int scytherProj, float scytheDmg, IEntitySource projSrc)
        {
            Vector2 srcPos = Owner.MountedCenter - HoldingOffset;
            //最后给予一定的随机度
            Vector2 finalPos = new(srcPos.X, srcPos.Y + 5f);
            //最终位置
            Vector2 velocity = srcPos - finalPos;
            //转速度向量
            float projSpeed = 16f;
            float length = velocity.Length();
            length = projSpeed / length;
            velocity.X *= length;
            velocity.Y *= length;
            Projectile.NewProjectile(projSrc, srcPos, velocity, scytherProj, (int)scytheDmg, 0f, Owner.whoAmI);
        }

        public void ShootGhost(int ghostType, float ghostDmg, IEntitySource projSrc)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 srcPos = Owner.MountedCenter - HoldingOffset;
                //最后给予一定的随机度
                Vector2 finalPos = new(srcPos.X + Main.rand.NextFloat(-8f, 9f), srcPos.Y + 5f);
                //最终位置
                Vector2 velocity = srcPos - finalPos;
                //转速度向量
                float projSpeed = 16f;
                float length = velocity.Length();
                length = projSpeed / length;
                velocity.X *= length;
                velocity.Y *= length;
                Projectile.NewProjectile(projSrc, srcPos, velocity, ghostType, (int)ghostDmg, 0f, Owner.whoAmI);
            }
        }

        public void AddLight()
        {
            //补光
            Lighting.AddLight(Projectile.Center, TorchID.White);
        }

        //让我编译一下看看怎么个事
        private void PlayerPostionRotation()
        {
            //干掉Rotation，只让他水平旋转
            Projectile.rotation = MathHelper.PiOver4 + MathHelper.Pi;
            //更新射弹位置到对应的位置
            Vector2 pos = Owner.RotatedRelativePoint(Owner.MountedCenter, true);
            //我一想到我要为了这个编译十几次就想笑
            Projectile.Center = pos - HoldingOffset;
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = Owner.itemAnimation = 2;
            Owner.altFunctionUse = 2;
            Owner.ChangeDir(Projectile.direction);
            //将隐形的物品投射至玩家头顶上
            Owner.itemRotation = MathHelper.WrapAngle(MathHelper.PiOver2);
        }
        #region 方法列表
        public void KillProj()
        {
            if (Owner.dead)
            {
                Projectile.Kill();
                Owner.reuseDelay = 2;
                return;
            }
        }
        #endregion
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
                return true;
            float spinning = Projectile.rotation - MathHelper.PiOver4 * Math.Sign(Projectile.velocity.X);
            float staffRadiusHit = 110f;
            float useless = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + spinning.ToRotationVector2() * -staffRadiusHit, Projectile.Center + spinning.ToRotationVector2() * staffRadiusHit, 23f * Projectile.scale, ref useless))
            {
                return true;
            }
            return false;
        }
        //绘制需要考虑描边
        public override bool PreDraw(ref Color lightColor)
        {
            //手动接管绘制
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Rectangle rec = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 ori = texture.Size() / 2f;
            Color color = Color.White;
            SpriteEffects sE = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                sE = SpriteEffects.FlipHorizontally;
            Main.EntitySpriteDraw(texture, drawPos, new Rectangle?(rec), color, Projectile.rotation, ori, Projectile.scale, sE, 0);
            return false;
        }
    }
}