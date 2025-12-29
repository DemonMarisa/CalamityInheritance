using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Weapons;
using System.IO;
using CalamityInheritance.Content.Items.Weapons.Legendary;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class PBGLegendaryProj: ModProjectile, ILocalizedModType
    {
        public struct StoredColorAndDust
        {
            public Color pColor;
            public int dType;
        }
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => GetInstance<PBGLegendary>().Texture;
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
            Projectile.DamageType = GetInstance<RogueDamageClass>();
        }
        public override void SendExtraAI(BinaryWriter writer) => Projectile.DoSyncHandlerWrite(ref writer);
        public override void ReceiveExtraAI(BinaryReader reader) => Projectile.DoSyncHandlerRead(ref reader);
        public override void AI()
        {
            StoredColorAndDust gColor = SpecialColor(Main.player[Projectile.owner], Projectile);
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
                    int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, gColor.dType, 0f, 0f, 100, gColor.pColor, 0.75f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 0f;
                    Main.dust[d].color = gColor.pColor;
                }
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
        public override void OnKill(int timeLeft)
        {
            OnHitEffect();
        }
        public void OnHitEffect()
        {
            StoredColorAndDust gColor = SpecialColor(Main.player[Projectile.owner], Projectile);
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 48;
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
            for (int i = 0; i < 7; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, gColor.dType, 0f, 0f, 100, gColor.pColor, 1.2f);
                Main.dust[d].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[d].scale = 0.5f;
                    Main.dust[d].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    Main.dust[d].color = gColor.pColor;
                }
            }
            for (int j = 0; j < 15; j++)
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