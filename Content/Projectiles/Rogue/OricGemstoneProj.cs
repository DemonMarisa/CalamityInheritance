using System;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Utilities;
using CalamityMod;
using LAP.Core.Utilities;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class OricGemstoneProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => GetInstance<OricGemstone>().Texture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 34;
            Projectile.penetrate = 8;
            Projectile.friendly = true;
            Projectile.DamageType = GetInstance<RogueDamageClass>();
            Projectile.aiStyle = ProjAIStyleID.ThrownProjectile;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.timeLeft = 240;
            Projectile.ignoreWater = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bool TrueRightClick = Projectile.CalamityInheritance().MouseRight && !Projectile.Calamity().stealthStrike;
            //防止潜伏攻击的音效过度
            if (Projectile.velocity.Length() > 2f)
            {
                SoundEngine.PlaySound(SoundID.Item53 with { MaxInstances = 0 }, Projectile.Center);
                for (int i = 0; i < 5; i++)
                {
                    int d = Dust.NewDust(Projectile.Center, 5, 5, DustID.Orichalcum, 0.1f, 0.1f);
                    Main.dust[d].noGravity = true;
                }
            }
            //Y速度除非大于8f我们才执行右键的功能，即触墙生成一些花瓣
            if (TrueRightClick)
            {
                Player player = Main.player[Projectile.owner];
                HitEffect(player.LocalMouseWorld(), 1);
                float oldVelY = -oldVelocity.Y;
                if (Projectile.velocity.Y < 8f)
                    oldVelY = -8f;
                if (Projectile.velocity.Y > 8f)
                    oldVelY /= 2;
                Vector2 boucingVec = new(-oldVelocity.X / 2, oldVelY);
                Projectile.BouncingOnTiles(oldVelocity, boucingVec);
                //返回，不执行下方潜伏和普通左键的逻辑
                return false;
            }
            
            if (Projectile.velocity.Y != oldVelocity.Y && (Projectile.velocity.X < -2f || Projectile.velocity.Y > 2f))
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            Projectile.BouncingOnTiles(oldVelocity, -oldVelocity.X / 2, -oldVelocity.Y / 2); 
            return false;
        }
        //手动绘制
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origi = tex.Size() / 2;
            int trailingLenght = Projectile.oldPos.Length;
            //手动绘制残影
            for (int i = 1; i < trailingLenght; i++)
            {
                Vector2 trailDrawPos = drawPos - Projectile.velocity * i * 0.4f;
                float feidiede = 1 - (i / (float)trailingLenght);
                //平方放缩
                feidiede = MathF.Pow(feidiede, 2);
                //添加残影转角，手动放缩
                // float trailingRotation = Projectile.rotation * (1 - i * 0.1f);
                //位置使用历史位置（oldPos），而非基于当前速度的预测
                // Vector2 trailDrawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                //转角使用对应时间点的历史旋转（与位置同步）
                float trailingRotation = Projectile.oldRot[i]; 
                Color trailcolor = lightColor * feidiede;
                //透明度过低直接break出去没必要在画了
                if (feidiede < 0.1f)
                    break;
                Main.spriteBatch.Draw(tex, trailDrawPos, null, trailcolor, trailingRotation, origi, Projectile.scale, SpriteEffects.None, 0);
            }
            //直接绘制主射弹位于最顶层
            Main.spriteBatch.Draw(tex, drawPos, null, lightColor, Projectile.rotation, origi, Projectile.scale, SpriteEffects.None, 0.1f);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            CIFunction.DustCircle(Projectile.Center, 32, 1.4f, DustID.Orichalcum, false, 16f);
            SoundEngine.PlaySound(SoundID.Item52 with { MaxInstances = 0 }, Projectile.Center);
            if (Projectile.Calamity().stealthStrike && !Projectile.CalamityInheritance().MouseRight)
                HitEffect(Projectile.Center, 3);
        }
        public void HitEffect(Vector2 finalPosition, int totalCount = 2)
        {
            if (Main.myPlayer != Projectile.owner)
                return;
            totalCount = Projectile.Calamity().stealthStrike ? totalCount : 1;

            for (int i = 0; i < totalCount; i++)
            {
                int direction = Main.player[Projectile.owner].direction;
                float xStart = Main.screenPosition.X;
                if (direction < 0)
                    xStart += Main.screenWidth;
                float yStart = Main.screenPosition.Y + Main.rand.Next(Main.screenHeight);
                Vector2 startPos = new(xStart, yStart);
                Vector2 pathToTravel = finalPosition - startPos;
                pathToTravel.X += Main.rand.NextFloat(-50f, 50f) * 0.1f;
                pathToTravel.Y += Main.rand.NextFloat(-50f, 50f) * 0.1f;
                float speedMult = 24f / pathToTravel.Length();
                pathToTravel.X *= speedMult;
                pathToTravel.Y *= speedMult;
                int petal = Projectile.NewProjectile(Projectile.GetSource_FromThis(), startPos, pathToTravel, ProjectileID.FlowerPetal, Projectile.damage, 0f, Projectile.owner);
                if (petal.WithinBounds(Main.maxProjectiles))
                    Main.projectile[petal].DamageType = GetInstance<RogueDamageClass>();
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(!Projectile.CalamityInheritance().MouseRight)
                HitEffect(target.Center);
        }
    }
}