using System;
using System.CodeDom;
using CalamityInheritance.Content.Items;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Color = Microsoft.Xna.Framework.Color;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class PBGLegendaryBeam: ModProjectile, ILocalizedModType
    {
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";
        #region 音效
        public static string UsingSound => "CalamityMod/Sounds/Item";
        public static readonly SoundStyle HitSound3 = new($"{UsingSound}/WulfrumKnifeThrowFull") { PitchVariance = 0.4f };
        public static readonly SoundStyle HitSound2 = new($"{UsingSound}/WulfrumKnifeThrowTwo") { PitchVariance = 0.4f };
        public static readonly SoundStyle HitSound1 = new($"{UsingSound}/WulfrumKnifeThrowSingle") { PitchVariance = 0.4f };
        #endregion
        #region 射弹的一些基础属性
        //射弹是否允许追踪
        public bool GrantsHoming = false;
        //射弹是否允许转圈
        public bool GrantsDirection = true;
        //取消射弹转圈的时间
        public float CancelRotationTimer = 0f;
        //射弹每一帧旋转的角度
        public float RotationAngle = 0f;
        //发起追踪的计时器
        public float HomingTimer = -1f;
        //升级选项：是否有更多的判定次数
        public bool MoreHits = false;
        #endregion
        #region 别名
        //攻击AI的Timer, 给LocalAI[1]使用
        private const int ActiveAITimer = 1;
        //改变射弹方向的Timer, 给LocalAI[0]使用
        private const int ChangeDirTimer = 0;
        //存储击中的敌怪单位, 给ai[1]使用
        private const int StoredTar = 1;
        //最大超出玩家多少距离便强制其发起追踪
        private const float MaxAwayPlrDist = 1800f;
        //允许发起追踪的最晚时间(不过这个会受到eu的影响)
        private const float GrantsHomingTimer = 180f;
        #endregion
        #region 射弹颜色
        //默认颜色
        private static Color DefualtColor => new(107, 142 , 35);
        //开发者颜色: TrueScarlet, 近似深红
        private static Color TrueScarletColor => new(228, 1, 10);  
        //开发者颜色: DemonMarisa, 近似金黄
        private static Color DemonMarisaColor => new(255, 165, 0);
        //Tester颜色：Shizuku, 银白
        private static Color ShizukuColorSilver => new(248, 248, 255);
        //Tester颜色: Shizuku, 青蓝
        private static Color ShizukuColorAqua => new (152, 245, 255);
        //Tester颜色：KunojiIchika，近似纯黑
        private static Color IchikaColorBlack => new (79, 79, 79);
        //Supporter颜色: Plantare, 粉红
        private static Color PlantareColorPink => Color.HotPink;
        //彩蛋颜色: Tristan, 皇家蓝
        private static Color TristanColorRoyalBlue => Color.RoyalBlue;
        #endregion
        #region 一个同时存储射弹颜色与粒子类型的结构体
        public struct StoredColorAndDust
        {
            public Color pColor;
            public int dType;
        }
        #endregion
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
                Projectile.penetrate = 20;
                MoreHits = true;
            }
            //获取颜色
            StoredColorAndDust gType = SpecialColor(plr);

            Projectile.Opacity = 0f;
            Projectile.localAI[ActiveAITimer] += 1f;
            
            //不断检测与玩家的距离，如果本身超出追踪距离(2000f), 则强制其发起追踪
            float getXDist = plr.Center.X - Projectile.Center.X;
            float getYDist = plr.Center.Y - Projectile.Center.Y;
            float realDist = CIFunction.TryGetVectorMud(getXDist, getYDist);
            if (Projectile.localAI[ActiveAITimer] > 4f)
            {
                //潜伏的特效由下方的光效占据了主导, 因此飞行粒子会被压制一些。
                if (Main.rand.NextBool(3))
                    FlyingDust(gType.pColor, gType.dType);
                //光效, 灾厄的方法
                SparkParticle line = new SparkParticle(Projectile.Center - Projectile.velocity * 1.1f, Projectile.velocity * 0.01f, false, 18, 1f, gType.pColor);
                GeneralParticleHandler.SpawnParticle(line);
                //处理跟踪的AI
                //刚掷出的投刀不允许启用计时器的自增
                if (HomingTimer != -1f && realDist < MaxAwayPlrDist) 
                    HomingTimer += 1f;
                //每次击中敌怪时他都会改变一下转的角度
                if (GrantsDirection && !GrantsHoming)
                {
                    Projectile.localAI[ChangeDirTimer] += 1f; 
                    Projectile.velocity = Projectile.velocity.RotatedBy(RotationAngle);

                    if (Projectile.localAI[ChangeDirTimer] > CancelRotationTimer)
                    {
                        GrantsDirection = false;
                        Projectile.localAI[ChangeDirTimer] = 0f;
                    }
                }
                //其他状态下，都会间隔固定的时间刻度启用追踪, 或者离玩家足够远也可以
                if (HomingTimer > GrantsHomingTimer || realDist > MaxAwayPlrDist)
                {
                    GrantsHoming = true;
                    GrantsDirection = false;
                }
                //发起追踪
                if (GrantsHoming)
                    CIFunction.HomeInOnNPC(Projectile, false, 2400f, 12f, 36f);
            }
        }

        public static StoredColorAndDust SpecialColor(Player plr)
        {
            //默认颜色与例子
            Color getColor = DefualtColor;
            int d = DustID.TerraBlade;
            if (plr.CIMod().PBGLegendaryDyeable)
                getColor = plr.CIMod().PBGBeamColor;
            //特殊名字特殊颜色
            NameVariance(plr.name ,ref getColor, ref d);
            
            //初始化一个结构体, 并赋值
            StoredColorAndDust type;
            type.dType = d;
            type.pColor = getColor;
            //返回
            return type;
        }
        public static void NameVariance(string name,ref Color setColor, ref int d)
        {
            switch (name)
            {
                case "TrueScarlet":
                case "FakeAqua":
                    setColor = TrueScarletColor;
                    break;
                case "DemonMarisa":
                    setColor = DemonMarisaColor;
                    d = CIDustID.DustFallenStarsYellow;
                    break;
                case "Shizuku":
                case "shizuku":
                    setColor = Main.rand.NextBool() ? ShizukuColorSilver : ShizukuColorAqua;
                    d = DustID.GemDiamond;
                    break;
                case "KunojiIchika":
                    setColor = IchikaColorBlack;
                    break;
                case "Plantare":
                    setColor = PlantareColorPink;
                    d = DustID.PinkTorch;
                    break;
                case "Tristan":
                    setColor = TristanColorRoyalBlue;
                    d = DustID.GemSapphire;
                    break;
                default:
                    break;
            }
        }
        public void FlyingDust(Color dColor, int dType)
        {
            for (int i = 0; i < 3; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dType, 0f, 0f, 100, dColor, 0.75f);
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
            //允许转圈
            GrantsDirection = true;
            //取消转圈的timer
            CancelRotationTimer = Main.rand.NextFloat(15f, 25f);
            //随机提供转角
            RotationAngle = Main.rand.NextBool() ? 0.02f : -0.02f;
            //存储这个……敌对单位，但目前来说好像也不知道能用来干嘛
            Projectile.ai[StoredTar] = target.whoAmI;
            //减少1穿透次数
            //注释掉了
            // Projectile.penetrate -= 1;
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
            StoredColorAndDust getType = SpecialColor(plr);
            for (int i = 0; i < 7; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, getType.dType, 0f, 0f, 100, getType.pColor, 1.2f);
                Main.dust[d].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[d].scale = 0.5f;
                    Main.dust[d].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
                Main.dust[d].color = getType.pColor;
            }
            for (int j = 0; j < 3; j++)
            {
                int d2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, getType.dType, 0f, 0f, 100, getType.pColor, 1.7f);
                Main.dust[d2].noGravity = true;
                Main.dust[d2].velocity *= 5f;
                Main.dust[d2].color = getType.pColor;
                d2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, getType.dType, 0f, 0f, 100, getType.pColor, 1f);
                Main.dust[d2].velocity *= 2f;
                Main.dust[d2].color = getType.pColor;
            }
        }
    }
}