using System;
using CalamityInheritance.Buffs.StatDebuffs;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class StepToolShadowChairSmall: ModProjectile, ILocalizedModType 
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override void SetDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 140;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 420;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft <= 350 &&target.CanBeChasedBy(Projectile);
        public override void AI()
        {

           
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - MathHelper.PiOver2;
            /*小凳子会先飞行一段时间，然后再追踪敌怪*/
            float chairX = Projectile.position.X;
            float chairY = Projectile.position.Y;
            Projectile.velocity *= 1.01f;
            bool flyHoming = false;
            Projectile.ai[0] += 1f;
            if(Projectile.ai[0] > 15f)
            {
                Projectile.ai[0] = 15f;
                for(int enemy = 0; enemy < Main.maxNPCs; enemy++)
                {
                    if(Main.npc[enemy].CanBeChasedBy(Projectile, false))
                    {
                        float enemyX = Main.npc[enemy].position.X + Main.npc[enemy].width /2;
                        float enemyY = Main.npc[enemy].position.Y + Main.npc[enemy].height/2;
                        float enemyRange = Math.Abs(Projectile.position.X + Projectile.width/2 - enemyX) + Math.Abs(Projectile.position.Y + Projectile.height/2 - enemyY);
                        if (enemyRange < 10000f && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[enemy].position, Main.npc[enemy].width, Main.npc[enemy].height))
                        {
                            CIFunction.HomeInOnNPC(Projectile, false, 10000f, 24f, 16f);
                            flyHoming = true;
                        }
                    }
                }
            }
            if(!flyHoming)
            {
                chairX = Projectile.position.X + Projectile.width / 2 + Projectile.velocity.X * 100f;
                chairY = Projectile.position.Y + Projectile.height / 2 + Projectile.velocity.Y * 100f;
            }
        }
        public override bool PreDraw(ref Microsoft.Xna.Framework.Color lightColor)
        {
            Texture2D projectileTexture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
            int frameY = frameHeight * Projectile.frame;
            Main.spriteBatch.Draw(projectileTexture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, frameY, projectileTexture.Width, frameHeight)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2((float)projectileTexture.Width / 2f, (float)frameHeight / 2f), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //击中时梯凳驾到
            target.AddBuff(ModContent.BuffType<StepToolDebuff>(), 1145);
            //生成粒子
           
        }
        public override void OnKill(int timeLeft)
        {
            //小凳子在消失后恢复玩家18点或24点血量
            Player player = Main.player[Projectile.owner];
            player.Heal(Main.rand.NextBool(2)? 24 : 18);
            for(int i = 0; i < 10; i++)
            {
                    int rainbow = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height,
                                               DustID.WoodFurniture, Projectile.direction * 2, 0f, 150,
                                               Color.Brown , 2.0f);
                    Main.dust[rainbow].noGravity = false;
                    Main.dust[rainbow].velocity *= 1f;
            }
        }
    }
}