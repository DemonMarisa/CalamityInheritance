using CalamityInheritance.Content.Items;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public class ShizukuEdgeSword: ModProjectile, ILocalizedModType
    {
       public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public Player Owner => Main.player[Projectile.owner];
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Typeless/ShizukuItem/ShizukuEdge";
        public ref float AttackTimer => ref Projectile.ai[0];
        public ref float AttackType => ref Projectile.ai[1];
        #region AttackType
        const float IsShooted = 0f;
        const float IsHomingBack = 1f;
        #endregion
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.height = 108;
            Projectile.width = 84;
            Projectile.scale *= 1.2f;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 90000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.Opacity = 0f;
        }
        public override void AI()
        {
            Projectile.rotation += 0.10f;
            DoJustSpawn();
            DoGeneral();
            switch (AttackType)
            {
                case IsShooted:
                    DoShooted();
                    break;
                case IsHomingBack:
                    DoHomingBack();
                    break;
            }
        }
        public override void OnKill(int timeLeft)
        {
            #region 初始化
            int ghostCounts = Main.rand.Next(12, 18);
            int ghostType = Main.rand.Next(0, 3);
            ghostType = ghostType switch
            {
                0 => ModContent.ProjectileType<SoulSmallPlaceholder>(),
                1 => ModContent.ProjectileType<SoulMidPlaceholder>(),
                _ => ModContent.ProjectileType<SoulLargePlaceholder>(),
            };
            var projSrc = Projectile.GetSource_FromThis();
            int ghostDamage = (int)(Projectile.damage * 1.5f);
            #endregion
            for (int i = 0; i < ghostCounts; i++)
            {
                Vector2 srcPositon = Projectile.Center;
                Vector2 finalPostion = new Vector2(Projectile.Center.X + 15f, 0f).RotatedByRandom(MathHelper.TwoPi);
                //距离向量
                Vector2 distanceVector = srcPositon - finalPostion;
                //速度向量
                float projSpeed = 14f;
                float length = distanceVector.Length();
                length = projSpeed / length;
                distanceVector.X *= length;
                distanceVector.Y *= length;
                Projectile.NewProjectile(projSrc, srcPositon, distanceVector, ghostType, ghostDamage, 0f, Owner.whoAmI, ai1: ShizukuBaseGhost.MoreTrailingDust);
            }

        }
        private void ShootSwords()
        {
            /*
            if (AttackTimer % CIConfig.Instance.Debugint != 0)
                return;
            */
            int count = 2;
            int proj = ModContent.ProjectileType<ShizukuSwordProjectile>();
            for (int i = -1; i < count; i += 2)
            {
                Vector2 setSpeed = Projectile.velocity.RotatedBy(MathHelper.PiOver4 / 1.5f * i) * 1.1f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, setSpeed * 1.1f + Projectile.velocity, proj, Projectile.damage, Projectile.knockBack, Owner.whoAmI);
            }
        }

        private void DoGeneral()
        {
            Projectile.rotation += 0.05f;
        }

        private void DoJustSpawn()
        {
            //生成时的粒子
            if (AttackTimer == -1f)
            {
                SoundEngine.PlaySound(CISoundID.SoundIceRodBlockPlaced, Projectile.Center);
                PlayDust();
            }
        }

        private void DoShooted()
        {
            ShootSwords();
            AttackTimer += 1f;
            Projectile.Opacity += 0.1f;
            if (AttackTimer < 35f)
                return;
            
            AttackType = IsHomingBack;
            AttackTimer = 0f;
            Projectile.netUpdate = true;
        }

        private void DoHomingBack()
        {
            Projectile.velocity.X *= 0.9f;
            Projectile.rotation += 0.05f;
            Projectile.Opacity -= 0.1f;
            if (Projectile.Opacity < 0.5f)
            {
                AttackTimer += 1f + (AttackTimer < 15f).ToInt();
                Projectile.velocity.Y = -AttackTimer;
                if (Projectile.Opacity == 0f)
                {
                    Projectile.Kill();
                    Projectile.netUpdate = true;
                }
            }
        }

        public void PlayDust()
        {
            //一圈冰系粒子，受重力
            int dType = Main.rand.NextBool() ? DustID.IceRod : DustID.IceTorch;
            CIFunction.DustCircle(Projectile.Center, 32f, Main.rand.NextFloat(0.8f, 1.21f), dType, true, 12f, 200);
        }
        //我怎么还得手动接管绘制？
        // public override bool PreDraw(ref Color lightColor)
        // {
        //      //手动接管绘制
        //     Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
        //     //Clone多个射弹的位置, 并将原本射弹的指向临时记录下来
        //     Vector2[] multipleDrawPos = (Vector2[])Projectile.oldPos.Clone();
        //     Vector2 aimDirection = (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2();
        //         //给每个射弹贴图提供不同的角度
        //     multipleDrawPos[0] += aimDirection * -12f;
        //     multipleDrawPos[1] = multipleDrawPos[0] - (Projectile.rotation + MathHelper.PiOver4).ToRotationVector2() * Vector2.Distance(multipleDrawPos[0], multipleDrawPos[1]);
        //     for (int i = 0; i < multipleDrawPos.Length; i++)
        //     {
        //         //转角。
        //         multipleDrawPos[i] -= (Projectile.oldRot[i] + MathHelper.PiOver4).ToRotationVector2() * Projectile.height / 2f;
        //     }
        //     //常规绘制。
        //     Vector2 manProjDrawPos = Projectile.Center - Main.screenPosition;
        //     for (int j = 0; j < 6; j++)
        //     {
        //         float rotation = Projectile.oldRot[j] - MathHelper.PiOver2;
        //         rotation += 0.05f;
        //         Color afterImageColor = Color.Lerp(Color.White, Color.Transparent, 1f - (float)Math.Pow(Utils.GetLerpValue(0, 6, j), 1.4D)) * Projectile.Opacity;
        //         Main.EntitySpriteDraw(texture, manProjDrawPos, null, afterImageColor, rotation, texture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
        //     }
        //     return false;
        // }
    } 
}