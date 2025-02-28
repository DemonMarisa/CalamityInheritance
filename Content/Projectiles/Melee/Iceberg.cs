using System.Security.Cryptography;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class Iceberg: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => "CalamityInheritance/Content/Projectiles/Melee/IceBomb";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if(Projectile.ai[0] > 30f) //冰雹在飞行到30f的时候速度将会大幅度自减
            {
                if(Projectile.ai[0]>31f && Projectile.ai[0]<42f)
                Projectile.velocity *= 0.8f;
                if(Projectile.ai[0] == 42f && Projectile.ai[1] == 0f)
                {
                    SoundEngine.PlaySound(SoundID.Item30 with {Volume = 0.4f}, Projectile.Center);
                    SignalDust();
                }
                if(Projectile.ai[0] > 42f && Projectile.ai[2] < 0f) //计时器自增到这一步的时候获得5eu, 直接追踪敌人, 衍生弹幕什么的都是一样
                {
                    int getTar = -1;
                    if(Projectile.ai[0] > 50f)
                       Projectile.ai[0] = 50f;
                    foreach(NPC npc in Main.ActiveNPCs) //遍历npc数组寻找追踪目标
                    {
                        if(npc.chaseable && npc.lifeMax>5 && !npc.dontTakeDamage && !npc.friendly &&
                           npc.immortal && Collision.CanHit(Projectile.Center, 0, 0, npc.Center, 0, 0))
                        {
                            float getDist = Vector2.Distance(Projectile.Center, npc.Center);
                            if(getDist < 1800f)
                               getTar = npc.whoAmI;
                        }
                    }
                    Projectile.ai[2] = getTar; //有目标就把这个目标存放到ai[2]内
                    Projectile.netUpdate = true;
                }
                if(Projectile.ai[2] != -1f) //如果ai[2]内有目标
                {
                    NPC getNPC = Main.npc[(int)Projectile.ai[2]];
                    if(getNPC.active && !getNPC.dontTakeDamage) //且npc并没有无敌还是什么
                    {
                        Projectile.extraUpdates = 5; //获得5eu，直接超高速跟踪
                        Projectile.rotation += 0.8f;
                        CIFunction.HomeInOnNPC(Projectile, true, 1800f, 15f + Projectile.ai[0]/5f, 45f, 5f);
                        TrailDustHoming();
                    }
                }
                else
                {
                    Projectile.ai[2] = -1f;
                    Projectile.netUpdate = true;
                } 
            }
            else if(Projectile.ai[1] == 0f)//其他状态下让这个傻逼东西一直转啊转, 并不断衍生各种好玩的弹幕
            {
                if(Projectile.ai[0] % 10f == 0)
                { 
                    SoundEngine.PlaySound(SoundID.Item30, Projectile.Center);
                    Vector2 newVel = new(Projectile.velocity.X * 0.1f * MathHelper.PiOver2, Projectile.velocity.Y * 0.1f * MathHelper.PiOver2);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, newVel ,ModContent.ProjectileType<Iceberg>(), Projectile.damage/5, Projectile.knockBack, Main.myPlayer, Projectile.ai[0], -1f);
                }
                Projectile.rotation +=0.4f;
                Projectile.velocity *=1.01f;
                TrailDustNormal();
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for(int i = 0; i < 5; i++)
                Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, DustID.IceRod, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GlacialState>(), 60);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<GlacialState>(), 60);
        }

        public void TrailDustHoming()
        {
            for(int i =0 ;i < 2 ;i++)
            {
                Dust newD = Dust.NewDustPerfect(Projectile.Center, DustID.Water_Snow);
                newD.velocity = Projectile.velocity / 2f;
                newD.noGravity = false;
                newD.scale *= 1.2f;
            }
        }
        public void TrailDustNormal() 
        {
            int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Water_Snow); 
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0f;
            Main.dust[dust].scale *= 1.2f;
            Main.dust[dust].alpha = 100;
        }
        public void SignalDust()
        {
            CIFunction.DustCircle(Projectile.Center, 32f, 1.2f, DustID.SnowflakeIce, true, 10f);
        }
    }
}