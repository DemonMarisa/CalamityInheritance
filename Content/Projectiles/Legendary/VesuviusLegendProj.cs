using CalamityInheritance.Content.Items.Weapons.Legendary;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Legendary
{
    public class VesuviusLegendProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            //略微提升其碰撞箱
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Player plr = Main.player[Projectile.owner];
            var mplr = GetInstance<VesuviusLegendary>();
            //距离玩家过远处死
            int tileLength = 150 * 16;
            if ((plr.Center - Projectile.Center).LengthSquared() > (tileLength*tileLength))
                Projectile.Kill();
            //T1样式加强：允许穿墙，提高hitbox 
            if (mplr.TierOneMatcher)
            {
                Projectile.tileCollide = false;
                Projectile.Resize(200, 200);
            }
            else
            {
                if (Projectile.position.Y > Main.player[Projectile.owner].position.Y - 300f)
                {
                    Projectile.tileCollide = true;
                }
                if (Projectile.position.Y < Main.worldSurface * 16.0)
                {
                    Projectile.tileCollide = true;
                }
            }
            Projectile.scale = Projectile.ai[1];
            Projectile.rotation += Projectile.velocity.X * 2f;
            Vector2 dPos = Projectile.Center + Vector2.Normalize(Projectile.velocity) * 10f;
            Dust d = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 0, new Color(255, Main.DiscoG, 0), 1f)];
            d.position = dPos;
            d.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver2, default) * 0.33f + Projectile.velocity / 4f;
            d.position += Projectile.velocity.RotatedBy(MathHelper.PiOver2, default);
            d.fadeIn = 0.5f;
            d.noGravity = true;
            d = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 0, new Color(255, Main.DiscoG, 0), 1f)];
            d.position = dPos;
            d.velocity = Projectile.velocity.RotatedBy(-MathHelper.PiOver2, default) * 0.33f + Projectile.velocity / 4f;
            d.position += Projectile.velocity.RotatedBy(-MathHelper.PiOver2, default);
            d.fadeIn = 0.5f;
            d.noGravity = true;

            int dFire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 0, new Color(255, Main.DiscoG, 0), 1f);
            Main.dust[dFire].velocity *= 0.5f;
            Main.dust[dFire].scale *= 1.3f;
            Main.dust[dFire].fadeIn = 1f;
            Main.dust[dFire].noGravity = true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item89, Projectile.Center);
            Projectile.position.X = Projectile.position.X + Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y + Projectile.height / 2;
            Projectile.width = (int)(128f * Projectile.scale);
            Projectile.height = (int)(128f * Projectile.scale);
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
            for (int i = 0; i < 8; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 100, new Color(255, Main.DiscoG, 0), 1.5f);
            }
            for (int j = 0; j < 32; j++)
            {
                int dFire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 100, new Color(255, Main.DiscoG, 0), 2.5f);
                Main.dust[dFire].noGravity = true;
                Main.dust[dFire].velocity *= 3f;
                dFire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 100, new Color(255, Main.DiscoG, 0), 1.5f);
                Main.dust[dFire].velocity *= 2f;
                Main.dust[dFire].noGravity = true;
            }
            if (Main.netMode != NetmodeID.Server)
            {
                for (int k = 0; k < 2; k++)
                {
                    int gType = Gore.NewGore(Projectile.GetSource_Death(), Projectile.position + new Vector2(Projectile.width * Main.rand.Next(100) / 100f, Projectile.height * Main.rand.Next(100) / 100f) - Vector2.One * 10f, default, Main.rand.Next(61, 64), 1f);
                    Gore g = Main.gore[gType];
                    g.velocity *= 0.3f;
                    g.velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                    g.velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                }
            }
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.localAI[1] = -1f;
                Projectile.maxPenetrate = 0;
                Projectile.Damage();
            }
            for (int l = 0; l < 5; l++)
            {
                int dustType = Utils.SelectRandom(Main.rand, new int[]
                {
                    244,
                    259,
                    158
                });
                int bomb = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 2.5f * Projectile.direction, -2.5f, 0, new Color(255, Main.DiscoG, 0), 1f);
                Main.dust[bomb].alpha = 200;
                Main.dust[bomb].velocity *= 2.4f;
                Main.dust[bomb].scale += Main.rand.NextFloat();
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.OnFire3, 180);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffID.OnFire3, 180);

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int type = (int)Projectile.ai[0];
            if(type != 0)
            {
                tex = Request<Texture2D>(Texture + type).Value;
            }
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1, tex);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(Texture+"Glow").Value;
            int type = (int)Projectile.ai[0];
            if (type == 4)
                return;
            if(type != 0)
            {
                tex = Request<Texture2D>(Texture + "Glow" + type).Value;
            }
            Vector2 origin = tex.Size() / 2f;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
        }
    }
}
