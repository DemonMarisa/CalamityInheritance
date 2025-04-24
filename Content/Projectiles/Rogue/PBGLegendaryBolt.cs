using System;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Content.Items;
using System.IO;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class PBGLegendaryBolt: ModProjectile, ILocalizedModType
    {
        public int HitCounter = 0;
        public bool GrantsHoming = false;
        public struct StoredColorAndDust
        {
            public Color pColor;
            public int dType;
        }
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => $"{Generic.WeaponRoute}/Legendary/PBGLegendary";
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
        public override void SendExtraAI(BinaryWriter writer) => Projectile.DoSyncHandlerWrite(ref writer);
        public override void ReceiveExtraAI(BinaryReader reader) => Projectile.DoSyncHandlerRead(ref reader);
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
            StoredColorAndDust gColor = SpecialColor(Main.player[Projectile.owner], Projectile);
            for (int i = 0; i < 3; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, gColor.dType, 0f, 0f, 100, gColor.pColor, 0.75f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0f;
                Main.dust[d].color = gColor.pColor;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(Main.DiscoR, 142, 35, Projectile.alpha);        
        }

        public static StoredColorAndDust SpecialColor(Player plr, Projectile p)
        {
            StoredColorAndDust s;
            //默认颜色
            Color getColor = new Color(Main.DiscoR, 203, 103, p.alpha);
            int dType = CIDustID.DustTerraBlade;
            //若染色则取染色剂
            if (plr.CIMod().PBGLegendaryDyeable)
                getColor = plr.CIMod().PBGBeamColor;
            //特殊名字特殊颜色
            NameVariance(plr.name, ref getColor, ref p, ref dType);
            s.dType = dType;
            s.pColor = getColor;
            //返回
            return s;
        }
        public static void NameVariance(string name, ref Color reColor, ref Projectile p, ref int dType)
        {
            switch (name)
            {
                case "TrueScarlet":
                case "FakeAqua":
                    reColor = new(228, 1 ,10, p.alpha);
                    dType = DustID.GemRuby;
                    break;
                case "Shizuku":
                case "shizuku":
                    reColor = Main.rand.NextBool()? new(248, 248, 255, p.alpha) : new(152, 245, 255, p.alpha);
                    dType = DustID.GemDiamond;
                    break;
                case "DemonMarisa":
                    reColor = new (255, 165, 0, p.alpha);
                    dType = DustID.GoldCoin;
                    break;
                case "KunojiIchika":
                    reColor = new(79,79,79,p.alpha);
                    dType = DustID.WhiteTorch;
                    break;
                case "Plantare":
                    reColor = Color.HotPink;
                    dType = DustID.PinkTorch;
                    break;
                case "Tristan":
                case "30000Puslin":
                case "凋莫":
                    reColor = Color.RoyalBlue;
                    dType = DustID.GemSapphire;
                    break;
                default:
                    break;
            }
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
            StoredColorAndDust gColor = SpecialColor(Main.player[Projectile.owner], Projectile);
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 160;
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
            for (int i = 0; i < 70; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, gColor.dType, 0f, 0f, 100, gColor.pColor, 1.2f);
                Main.dust[d].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[d].scale = 0.5f;
                    Main.dust[d].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    Main.dust[d].color = gColor.pColor;
                }
            }
            for (int j = 0; j < 40; j++)
            {
                int d2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, gColor.dType, 0f, 0f, 100, gColor.pColor, 1.7f);
                Main.dust[d2].noGravity = true;
                Main.dust[d2].velocity *= 5f;
                Main.dust[d2].color = gColor.pColor;
                d2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, gColor.dType, 0f, 0f, 100, gColor.pColor, 1f);
                Main.dust[d2].velocity *= 2f;
                Main.dust[d2].color = gColor.pColor;
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

            StoredColorAndDust gColor = SpecialColor(Main.player[Projectile.owner], Projectile);
            for (int i = 0; i < 7; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, gColor.dType, 0f, 0f, 100, gColor.pColor, 1.2f);
                Main.dust[d].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[d].scale = 0.5f;
                    Main.dust[d].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    Main.dust[d].color = gColor.pColor;
                }
            }
            for (int j = 0; j < 3; j++)
            {
                int d2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, gColor.dType, 0f, 0f, 100, gColor.pColor, 1.7f);
                Main.dust[d2].noGravity = true;
                Main.dust[d2].velocity *= 5f;
                Main.dust[d2].color = gColor.pColor;
                d2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, gColor.dType, 0f, 0f, 100, gColor.pColor, 1f);
                Main.dust[d2].velocity *= 2f;
                Main.dust[d2].color = gColor.pColor;
            }
        }
    }
}