using System;
using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityInheritance.Texture;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Content.Items;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class PBGLegendaryProj: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Rogue/PBGLegendary";
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.alpha = 255;
            Projectile.aiStyle = 93;
            AIType = 514;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }
        public override void AI()
        {
            Color gColor = SpecialColor(Main.player[Projectile.owner], Projectile);
            if (Projectile.localAI[1] < 4f)
                Projectile.alpha = 0;
            else if (Projectile.localAI[1] == 4f) 
                Projectile.alpha = 255;
                
            Projectile.alpha -= 3;
            if (Projectile.alpha < 30)
            {
                Projectile.alpha = 30;
            }
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] > 4f)
            {
                for (int i = 0; i < 3; i++)
                {
                    int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 0.75f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 0f;
                    Main.dust[d].color = gColor;
                }
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
                8 => Color.DarkRed,
                //默认
                _ => defualtColor,
            };
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
        public override void OnKill(int timeLeft)
        {
            OnHitEffect();
        }
        public void OnHitEffect()
        {
            Color gColor = SpecialColor(Main.player[Projectile.owner], Projectile);
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 48;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            for (int i = 0; i < 7; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 1.2f);
                Main.dust[d].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[d].scale = 0.5f;
                    Main.dust[d].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    Main.dust[d].color = gColor;
                }
            }
            for (int j = 0; j < 15; j++)
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