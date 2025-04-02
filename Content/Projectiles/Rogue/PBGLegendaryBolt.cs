using System;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityInheritance.Utilities;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Content.Items.Weapons;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class PBGLegendaryBolt: ModProjectile, ILocalizedModType
    {
        public int HitCounter = 0;
        public bool GrantsHoming = false;
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => $"{Generic.WeaponRoute}/Rogue/PBGLegendary";
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 2;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            //维持转角的必要逻辑.
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity. Y, Projectile.velocity.X) + MathHelper.PiOver2;
            NotStealthAI();
        }
        public void NotStealthAI() 
        {
            Projectile.alpha -= 3;
            if (Projectile.alpha < 100)
                Projectile.alpha = 100;
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] > 4f)
                FlyingDust();
        }
        //潜伏生成的粒子会更少一点
        public void FlyingDust()
        {
            Color gColor = SpecialColor(Main.player[Projectile.owner], Projectile);
            for (int i = 0; i < 3; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 0.75f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0f;
                Main.dust[d].color = gColor;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(Main.DiscoR, 142, 35, Projectile.alpha);        
        }

        public static Color SpecialColor(Player plr, Projectile p)
        {
            //默认颜色
            Color getColor = new Color(107, 142, 35, p.alpha);
            //若染色则取染色剂
            if (plr.CIMod().PBGLegendaryDyeable)
                getColor = plr.CIMod().PBGBeamColor;
            //特殊名字特殊颜色
            // getColor = NameTag(plr, getColor, p);
            getColor = TestColor(plr, getColor, p);
            //返回
            return getColor;
        }

        public static Color NameTag(Player plr, Color defualtColor, Projectile p)
        {
            return plr.name switch
            {
                "TrueScarlet" or "FakeAqua" => new(228, 1, 10, p.alpha),//近似深红
                "Shizuku" or "shizuku" => Main.rand.NextBool() ? new(248, 248, 255, p.alpha) : new(152, 245, 255, p.alpha),//随机取近似银白和近似青蓝
                "DemonMarisa" => new(255, 165, 0, p.alpha),//近似金黄
                "KunojiIchika" => new(79, 79, 79, p.alpha),//近似灰白
                "Plantare" => Color.HotPink,//字面意思
                "Tristan" => Color.RoyalBlue,
                "BaobhanSith" => Color.DarkRed,
                _ => defualtColor,
            };
        }
        public static Color TestColor(Player plr, Color defualtColor, Projectile p)
        {
            return CIRespriteConfig.Instance.PBGColorType switch
            {
                //开发者颜色：TrueScarlet，近似深红
                2 => new Color(228, 1, 10, p.alpha),
                //开发者颜色：DemonMarisa，近似金黄
                3 => new Color(255, 165, 0, p.alpha),
                //Tester颜色: Shizuku, 近似银白/浅蓝
                4 => Main.rand.NextBool() ? new(248, 248, 255, p.alpha) : new(152, 245, 255, p.alpha),
                //Tester颜色：KunojiIchika, 近似灰白
                5 => new(79, 79, 79, p.alpha),
                //Special颜色: Plantare, HotPink
                6 => Color.HotPink,
                //Special颜色: Tristan, 皇家蓝
                7 => Color.RoyalBlue,
                //Special颜色: BaobhanSith, 暗红
                8 => new (176, 48, 96, p.alpha),
                //默认
                _ => defualtColor,
            };
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //潜伏不会试图产生这个大爆炸的粒子
            if (Projectile.penetrate <= 1)
                OnHitEffect();
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            OnHitEffect();
        }
        
        public void OnHitEffect()
        {
            Color gColor = SpecialColor(Main.player[Projectile.owner], Projectile);
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 160;
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
            for (int i = 0; i < 70; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1.2f);
                Main.dust[d].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[d].scale = 0.5f;
                    Main.dust[d].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    Main.dust[d].color = gColor;
                }
            }
            for (int j = 0; j < 40; j++)
            {
                int d2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1.7f);
                Main.dust[d2].noGravity = true;
                Main.dust[d2].velocity *= 5f;
                Main.dust[d2].color = gColor;
                d2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1f);
                Main.dust[d2].velocity *= 2f;
                Main.dust[d2].color = gColor;
            }
        }
        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 16;
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
            OnKillDust();
        }
        public void OnKillDust()
        {

            Color gColor = SpecialColor(Main.player[Projectile.owner], Projectile);
            for (int i = 0; i < 7; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1.2f);
                Main.dust[d].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[d].scale = 0.5f;
                    Main.dust[d].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    Main.dust[d].color = gColor;
                }
            }
            for (int j = 0; j < 3; j++)
            {
                int d2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1.7f);
                Main.dust[d2].noGravity = true;
                Main.dust[d2].velocity *= 5f;
                Main.dust[d2].color = gColor;
                d2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1f);
                Main.dust[d2].velocity *= 2f;
                Main.dust[d2].color = gColor;
            }
        }
    }
}