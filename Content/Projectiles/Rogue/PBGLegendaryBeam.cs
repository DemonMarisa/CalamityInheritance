using System;
using System.Drawing;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Pets;
using CalamityMod.Particles;
using Microsoft.Build.Construction;
using Microsoft.Xna.Framework;
using MonoMod.Core.Platforms;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Color = Microsoft.Xna.Framework.Color;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class PBGLegendaryBeam: ModProjectile, ILocalizedModType
    {
        public static readonly SoundStyle HitSound3 = new("CalamityMod/Sounds/Item/WulfrumKnifeThrowFull") { PitchVariance = 0.4f };
        public static readonly SoundStyle HitSound2 = new("CalamityMod/Sounds/Item/WulfrumKnifeThrowTwo") { PitchVariance = 0.4f };
        public static readonly SoundStyle HitSound1 = new("CalamityMod/Sounds/Item/WulfrumKnifeThrowSingle") { PitchVariance = 0.4f };
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";
        public bool GrantsHoming = false;
        public float HomingTimer = -1f;
        public bool MoreHits = false;
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 15;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 12;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.localNPCHitCooldown = 12;
        }
        public override void AI()
        {
            Player plr = Main.player[Projectile.owner];
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity. Y, Projectile.velocity.X) + MathHelper.PiOver2;
            AITier1(plr);
        }
        public void AITier1(Player plr)
        {
            //完成第2样式任务, 增加判定数量
            if (plr.CIMod().PBGTier2 && !MoreHits)
            {
                Projectile.penetrate = 18;
                MoreHits = true;
            }
            //获取颜色
            Color getColor = SpecialColor(plr);

            Projectile.Opacity = 0f;
            Projectile.localAI[1] += 1f;
            //不断检测与玩家的距离，如果本身超出追踪距离(2000f), 则强制其发起追踪
            float getXDist = plr.Center.X - Projectile.Center.X;
            float getYDist = plr.Center.Y - Projectile.Center.Y;
            float realDist = CIFunction.TryGetVectorMud(getXDist, getYDist);
            if (Projectile.localAI[1] > 4f)
            {
                //潜伏的特效由下方的光效占据了主导, 因此飞行粒子会被压制一些。
                if (Main.rand.NextBool(3))
                    FlyingDust(getColor);
                //光效, 灾厄的方法
                SparkParticle line = new SparkParticle(Projectile.Center - Projectile.velocity * 1.1f, Projectile.velocity * 0.01f, false, 18, 1f, getColor);
                GeneralParticleHandler.SpawnParticle(line);
                //处理跟踪的AI
                //刚掷出的投刀不允许启用计时器的自增
                if (HomingTimer != -1f) 
                    HomingTimer += 1f;
                //其他状态下，都会间隔固定的时间刻度启用追踪, 或者离玩家足够远也可以
                if (HomingTimer % 180 == 0f || realDist > 1800f)
                    GrantsHoming = true;
                //发起追踪
                if (GrantsHoming)
                    CIFunction.HomeInOnNPC(Projectile, false, 2400f, 12f, 36f);
            }
        }

        public static Color SpecialColor(Player plr)
        {
            //默认颜色
            Color getColor = new Color(107, 142, 35);
            //若染色则取染色剂
            if (plr.CIMod().PBGLegendaryDyeable)
                getColor = plr.CIMod().PBGBeamColor;
            //特殊名字特殊颜色
            // getColor = NameTag(plr, getColor);
            getColor = TestColor(plr, getColor);
            //返回
            return getColor;
        }

        public static Color NameTag(Player plr, Color defualtColor)
        {
            return plr.name switch
            {
                "TrueScarlet" or "FakeAqua" => new(228, 1, 10),//近似深红
                "Shizuku" or "shizuku" => Main.rand.NextBool() ? new(248, 248, 255) : new(152, 245, 255),//随机取近似银白和近似青蓝
                "DemonMarisa" => new(255, 165, 0),//近似金黄
                "KunojiIchika" => new(79, 79, 79),//近似灰白
                "Plantare" => Color.HotPink,//字面意思
                "Tristan" => Color.RoyalBlue,
                "BaobhanSith" => Color.DarkRed,
                _ => defualtColor,
            };
        }
        public static Color TestColor(Player plr, Color defualtColor)
        {
            return CIRespriteConfig.Instance.PBGColorType switch
            {
                //开发者颜色：TrueScarlet，近似深红
                2 => Main.rand.NextBool() ? new (228, 1, 10) : new(135,1,2),
                //开发者颜色：DemonMarisa，近似金黄
                3 => new Color(255, 165, 0),
                //Tester颜色: Shizuku, 近似银白/浅蓝
                4 => Main.rand.NextBool() ? new(248, 248, 255) : new(38, 133, 249),
                //Tester颜色：KunojiIchika, 近似灰白
                5 => new(28, 28, 28),
                //Special颜色: Plantare, HotPink
                6 => Color.HotPink,
                //Special颜色: Tristan, 皇家蓝
                7 => Color.RoyalBlue,
                //Special颜色: BaobhanSith, 暗红
                8 => Color.PaleVioletRed,
                //默认
                _ => defualtColor,
            };
        }
        public void FlyingDust(Color dColor)
        {
            for (int i = 0; i < 3; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, dColor, 0.75f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0f;
                Main.dust[d].color = dColor;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            
            return new Color(Main.DiscoR, 203, 103, Projectile.alpha);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //第一次投掷出来的击中敌怪时允许其追踪
            if (HomingTimer == -1f)
                GrantsHoming = true;
            //其他状态下取消追踪
            else
                GrantsHoming = false;
            //标记为0f启用追踪计时器
            HomingTimer = 0f;
            Projectile.ai[1] = target.whoAmI;
            //减少1穿透次数
            Projectile.penetrate -= 1;
            //释放几个音效，这里用的是钨钢飞刀的投掷音
            SoundEngine.PlaySound(Main.rand.NextBool() ? (Main.rand.NextBool() ? HitSound1 : HitSound2) : HitSound3, Projectile.position);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //在墙体上反弹时无论什么情况都直接启用一次追踪
            if (Projectile.Calamity().stealthStrike)
            {
                if (!GrantsHoming)
                    GrantsHoming = true;
                if (Projectile.velocity.X != oldVelocity.X)
                    Projectile.velocity.X = -oldVelocity.X;
                if (Projectile.velocity.Y != oldVelocity.Y)
                    Projectile.velocity.Y = -oldVelocity.Y;
                OnKillDust();
                Projectile.penetrate -= 1; 
                return false;
            }
            return true;
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

            Player plr = Main.player[Projectile.owner];
            Color gColor = SpecialColor(plr);
            for (int i = 0; i < 7; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, gColor, 1.2f);
                Main.dust[d].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[d].scale = 0.5f;
                    Main.dust[d].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
                Main.dust[d].color = gColor;
            }
            for (int j = 0; j < 3; j++)
            {
                int d2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, gColor, 1.7f);
                Main.dust[d2].noGravity = true;
                Main.dust[d2].velocity *= 5f;
                Main.dust[d2].color = gColor;
                d2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, gColor, 1f);
                Main.dust[d2].velocity *= 2f;
                Main.dust[d2].color = gColor;
            }
        }
    }
}