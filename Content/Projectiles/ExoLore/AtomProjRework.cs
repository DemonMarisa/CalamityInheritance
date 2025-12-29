using System;
using System.IO;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoLore
{
    public class AtomProjRework : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        #region 别名
        ref float AttackType => ref Projectile.ai[0];
        ref float AttackTimer => ref Projectile.ai[1];
        public Player Owner => Main.player[Projectile.owner]; 
        #endregion
        #region 攻击枚举
        //不要修改这个为0f, 不然发射逻辑会出问题
        const float IsShooted = -1f;
        const float IsFading = 1f;
        #endregion
        #region 攻击属性
        public const int SlowdownTime = 80;
        public int IncreaseMent = 8;
        public int StealthIncre = -1;
        public int AnotherIncre = -1;
        public int FlipX = -1;
        public int FlipY = -1;
        public const float GapY = 100f;
        public bool Init = false;
        #endregion
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 124;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.timeLeft = SlowdownTime;
            Projectile.DamageType = GetInstance<RogueDamageClass>();
            Projectile.noEnchantmentVisuals = true;
        }
        #region 多人同步
        public override void SendExtraAI(BinaryWriter writer)
        {
            Projectile.DoSyncHandlerWrite(ref writer);
            writer.Write(StealthIncre);
            writer.Write(AnotherIncre);
            writer.Write(FlipX);
            writer.Write(FlipY);
            writer.Write(Init);
            writer.Write(IncreaseMent); 
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.DoSyncHandlerRead(ref reader);
            StealthIncre = reader.ReadInt32();
            AnotherIncre = reader.ReadInt32();
            IncreaseMent = reader.ReadInt32();
            FlipX = reader.ReadInt32();
            FlipY = reader.ReadInt32();
            Init = reader.ReadBoolean();
        }
        #endregion
        //AttackTimer会用于绘制上，所以如果你知道你在干嘛的话我不建议你去对AttackTimer做任何的修改
        public override void AI()
        {
            DoGeneral();
            switch (AttackType)
            {
                case IsShooted:
                    DoShooted();
                    break;
                case IsFading:
                    DoFading();
                    break;
            }
        }
        //手动接管
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() * 0.5f;
            Vector2 baseDrawPosition = Projectile.Center - Main.screenPosition;
            //渐进颜色
            float endFade = Utils.GetLerpValue(0f, 12f, Projectile.timeLeft, true);
            Color mainColor = Color.White * Projectile.Opacity * endFade * 1.5f;
            mainColor.A = (byte)(255 - Projectile.alpha);
            //轨迹颜色，曲高亮白
            Color afterimageLightColor = Color.White * endFade;
            afterimageLightColor.A = (byte)(255 - Projectile.alpha);
            if (endFade == 12f)
                Projectile.Kill();
            //这个用于绘制边沿
            for (int i = 0; i < 18; i++)
            {
                Vector2 drawPosition = baseDrawPosition + (MathHelper.TwoPi * i / 18f).ToRotationVector2() * (1f - Projectile.Opacity) * 16f;
                Main.EntitySpriteDraw(texture, drawPosition, null, afterimageLightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }
            //绘制实际意义的残影，这里的残影密度非常高，因此会显得有一种动态模糊的效果。
            for (int i = 0; i < 8; i++)
            {
                Vector2 drawPosition = baseDrawPosition - Projectile.velocity * i * 0.3f;
                Color afterimageColor = mainColor * (1f - i / 8f);
                Main.EntitySpriteDraw(texture, drawPosition, null, afterimageColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }
            return false;
        }
        #region 方法列表
        public void DoFading()
        {
            //开始Fading。
            //在Fading的过程中会一直试图生成射弹。
            float searching = Projectile.Calamity().stealthStrike ? 3600f : 1000f;
            NPC target = Projectile.FindClosestTarget(searching, true, true);
            //target为空. return
            if (target is null)
                return;
            if (Projectile.timeLeft % 5 == 0)
                DoRainDownProj(target);
        }
        public void DoRainDownProj(NPC target)
        {
            //新设定的：如果玩家执行了一次潜伏攻击，则为天降射弹，且不执行下方的AI。
            if (Projectile.Calamity().stealthStrike)
            {
                DoRainStealthProj(target);
                //从玩家身上发射额外的射弹
                DoExtraStealthProj(target);
                return;
            }
            DoNotStealthProj(target);
        }
        //非潜伏射弹
        private void DoNotStealthProj(NPC target)
        {
            //射弹非空，尝试设置生成位置。
            Vector2 realPos = target.Center + (MathHelper.TwoPi / MathHelper.PiOver4 * 8 * IncreaseMent ).ToRotationVector2() * 300f + target.velocity;
            IncreaseMent += 1;
            //设置距离向量，并转为速度向量
            Vector2 pDistVec = target.Center - realPos;
            float pDist = pDistVec.Length();
            pDist = 24f / pDist;
            pDistVec.X *= pDist;
            pDistVec.Y *= pDist;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), realPos, pDistVec, ProjectileType<AtomDuplicateRework>(), Projectile.damage, 0f, Owner.whoAmI, default, default, target.whoAmI);
        }

        //直接从敌怪左右侧生成
        private void DoExtraStealthProj(NPC target)
        {
            //这里会用一个嵌套循环设置位置。
            float horizonX = target.Center.X + 200f * FlipX;
            float horizonY = target.Center.Y + GapY * AnotherIncre; 
            Vector2 horizon = new (horizonX, horizonY);
            Vector2 tarDist = target.Center - horizon;
            float playerDist = tarDist.Length();
            playerDist = 18f / playerDist;
            tarDist.X *= playerDist;
            tarDist.Y *= playerDist;
            //速度？我不需要速度.
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), horizon, tarDist, ProjectileType<AtomDuplicateRework>(), Projectile.damage * 2, Projectile.knockBack, Owner.whoAmI, 0f, 0f, target.whoAmI);
            //只让内层循环自增
            AnotherIncre++;
            //FlipX == -1, 表射弹生成在目标左侧, AnotherIncre -1 0 1-> 上 中 下
            //当AnotherIncre == 3时，将射弹从右方开始重新生成，并重置循环。
            if (FlipX == -1 && AnotherIncre > 1)
            {
                FlipX = 1;
                AnotherIncre = -1;
            }
            if (FlipX == 1 && AnotherIncre > 1)
            {
                FlipX = -1;
                AnotherIncre = -1;
            }
        }
        //从天而降
        private void DoRainStealthProj(NPC target)
        {
            float stealthX = target.Center.X + 200f * StealthIncre;
            float stealthY = target.Center.Y - 300f * FlipY;
            Vector2 stealthPos = new (stealthX, stealthY);
            Vector2 stealthDist = target.Center - stealthPos;
            float dist = stealthDist.Length();
            dist = 24f / dist;
            stealthDist.X *= dist;
            stealthDist.Y *= dist;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), stealthPos, stealthDist, ProjectileType<AtomDuplicateRework>(), Projectile.damage, 0f, Owner.whoAmI, default, default, target.whoAmI);
            StealthIncre++; 
            //同上
            if (StealthIncre > 1 && FlipY == -1)
            {
                StealthIncre = -1;
                FlipY = 1;
            }
            if (StealthIncre > 1 && FlipY == 1)
            {
                StealthIncre = -1;
                FlipY = -1;
            }
        }

        public void DoShooted()
        {
            //转角
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            //固定套路。
            if (AttackTimer > 30f)
            {
                AttackType = IsFading;
                Projectile.netUpdate = true;
            }
            else 
                AttackTimer++;
        }

        public void DoGeneral()
        {
            Projectile.velocity *= 0.91f;
            Projectile.Opacity = (float)Math.Pow(1f - AttackTimer / SlowdownTime, 2D);
            //在timeleft合适之后，让这个射弹略微往上抬。
            if (Projectile.timeLeft < 8)
            {
                Projectile.velocity = new Vector2(4f, 0f).RotatedBy(-MathHelper.PiOver2);
                //加速度
                Projectile.velocity.Y -= 0.12f;
            }
        }
        #endregion
    }
}