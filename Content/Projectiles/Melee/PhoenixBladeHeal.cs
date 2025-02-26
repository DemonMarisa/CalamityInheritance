using System;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Xml;
using Terraria.Audio;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class PhoenixBladeHeal: ModProjectile, ILocalizedModType
    {
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 480;
            Projectile.extraUpdates = 3;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vel = new(Projectile.velocity.X + Projectile.width * 0.5f, Projectile.velocity.Y + Projectile.height * 0.5f);        
            int projCounter = (int)Projectile.ai[0];
            float wtfX = Main.player[projCounter].Center.X - vel.X;
            float wtfY = Main.player[projCounter].Center.Y - vel.Y;
            float wtfSum = (float)Math.Sqrt((double)(wtfX*wtfX + wtfY*wtfY));
            if(wtfSum < 50f && //???
               Projectile.position.X < Main.player[projCounter].position.X + Main.player[projCounter].width && 
               Projectile.position.X + Projectile.width > Main.player[projCounter].position.X && 
               Projectile.position.Y <Main.player[projCounter].position.Y + Main.player[projCounter].height && 
               Projectile.position.Y + Projectile.height > Main.player[projCounter].position.Y)
           {
                if(Projectile.owner == Main.myPlayer)
                {
                    int healAmt = 10;
                    player.Heal(healAmt);
                    Main.player[projCounter].HealEffect(10, true);
                    NetMessage.SendData(MessageID.SpiritHeal, -1, -1, null, projCounter, healAmt, 0f, 0f, 0, 0, 0);
                }
                Projectile.Kill();
           }
            for(int i=0;i<1;i++) //WHAT?
            {
                float dVelX = Projectile.velocity.X * 0.2f * projCounter;
                float dVelY = -Projectile.velocity.Y * 0.2f * projCounter;
                int dType = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 100, default, 1f);
                Main.dust[dType].noGravity = true;
                Main.dust[dType].velocity *= 0f;
                Dust dClone = Main.dust[dType];
                dClone.position.X -= dVelX; //???
                Dust dAlter = Main.dust[dType];
                dAlter.position.Y -= dVelY;
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item74 with {Volume = 0.5f});
        }
    }
}