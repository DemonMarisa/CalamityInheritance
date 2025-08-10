using System;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.FutureContent.CometQuasher
{
    public class CQuasherMeteor : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public static string MeteorPath => $"{GenericProjRoute.ProjRoute}" + "/" + "FutureContent/CometQuasher";
        public static string MeteorName => "CQuasherMeteor";
        public const int SecondMeteor = 2;
        public const int ThirdMeteor = 3;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 2;
            AIType = ProjectileID.Meteor1;
        }
        public override void AI()
        {
            if (Projectile.Center.Y > Projectile.ai[2])
                Projectile.tileCollide = true;
            else
                Projectile.tileCollide = false;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item89 with {MaxInstances = 0}, Projectile.position);

            Projectile.ExpandHitboxBy((int)(128f * Projectile.scale));

            for (int i = 0; i < 8; ++i)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0.0f, 0.0f, 100, new Color(), 1.5f);

            for (int j = 0; j < 32; ++j)
            {
                int fieryDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0.0f, 0.0f, 100, new Color(), 2.5f);
                Dust dust1 = Main.dust[fieryDust];
                dust1.noGravity = true;
                dust1.velocity *= 3f;
                int fieryDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0.0f, 0.0f, 100, new Color(), 1.5f);
                Dust dust2 = Main.dust[fieryDust2];
                dust2.velocity *= 2f;
                dust2.noGravity = true;
            }

            if (Main.netMode != NetmodeID.Server)
            {
                for (int j = 0; j < 2; ++j)
                {
                    int fieryDust = Gore.NewGore(Projectile.GetSource_Death(), Projectile.position + new Vector2(Projectile.width * Main.rand.Next(100) / 100f, Projectile.height * Main.rand.Next(100) / 100f) - Vector2.One * 10f, new Vector2(), Main.rand.Next(61, 64), 1f);
                    Gore gore = Main.gore[fieryDust];
                    gore.velocity *= 0.3f;
                    gore.velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                    gore.velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                }
            }

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 10;
                Projectile.localAI[1] = -1f;
                Projectile.maxPenetrate = 0;
                Projectile.Damage();
            }

            for (int j = 0; j < 5; ++j)
            {
                int fieryDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Utils.SelectRandom(Main.rand, [6, 259, 158]), 2.5f * Projectile.direction, -2.5f, 0, new Color(), 1f);
                Dust dust1 = Main.dust[fieryDust];
                dust1.alpha = 200;
                dust1.velocity *= 2.4f;
                dust1.scale += Main.rand.NextFloat();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            var cgp = Projectile.Calamity();
            if (cgp.lineColor == SecondMeteor - 1)
                tex = ModContent.Request<Texture2D>(MeteorPath + "/" + MeteorName + SecondMeteor.ToString()).Value;
            if (cgp.lineColor == ThirdMeteor - 1)
                tex = ModContent.Request<Texture2D>(MeteorPath + "/" + MeteorName + ThirdMeteor.ToString()).Value;
            //考虑了一会手动draw了一次
            Vector2 origin = tex.Size() / 2f;
            Vector2 baseDrawPosition = Projectile.Center - Main.screenPosition;
            Color mainColor = Color.White;
            Color edgeColor = Color.SkyBlue;
            int edgeDrawTime = 12;
            //描边
            for (int i = 0; i < edgeDrawTime; i++)
            {
                //生成一个“方向正确但长度为1” 的向量之后做放缩处理
                Vector2 edgeDrawPos = baseDrawPosition + (MathHelper.TwoPi * i / edgeDrawTime).ToRotationVector2() * 8f;
                Main.EntitySpriteDraw(tex, edgeDrawPos, null, edgeColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }
            //残影
            int trailingLength = 6;
            for (int i = 0; i < trailingLength; i++)
            {
                Vector2 trailDrawPos = baseDrawPosition - Projectile.velocity * i * 1.1f;
                //使用一个反比函数处理透明度的衰减速度
                float faded = 1 - (i / (float)trailingLength);
                //使用平方衰减增强
                faded = MathF.Pow(faded, 2);
                Color trailColor = mainColor * faded;
                Main.EntitySpriteDraw(tex, trailDrawPos, null, trailColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }
            // CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1, tex);
            return false;
        }
    }
}