using CalamityInheritance.Buffs.Statbuffs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using rail;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjEcho: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeHammerTriactisTruePaladinianMageHammerofMight";
        public float speed = 28f;
        public static readonly float HitRange = 70f;
        public static readonly int LifeTime = 600;
        public static readonly float DefualtRotatoin = 0.22f;
        bool ifSummonClone = false;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 160;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown= -1;
            Projectile.timeLeft = LifeTime;
            Projectile.scale *= 0.6f;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < (LifeTime - 50) && target.CanBeChasedBy(Projectile);

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            Lighting.AddLight(Projectile.Center, 0f, 0.5f, 0.75f);

            Projectile.ai[0] += 1f;
            if(Projectile.ai[0] > HitRange) //只允许Echo在飞行至大于这个距离时重击
            {   
                Projectile.extraUpdates = 2;
                if(Projectile.ai[0] == HitRange)
                ReturnDust(); //使Echo飞行之顶点时释放粒子提示
                CalamityInheritanceUtils.HomeInOnNPC(Projectile, true, 1800f, speed, 20f);
            }

            if(Projectile.ai[0] < HitRange - 25f) //Echo在上升过程中速度会一直增快， 旋转速度也一样
            {
                Projectile.velocity.Y *=1.01f;
                Projectile.rotation += MathHelper.ToRadians(Projectile.ai[0]*0.7f) * Projectile.localAI[0];

                NPC tar  = Main.npc[(int)Projectile.ai[1]];
                if(Projectile.timeLeft < LifeTime - 300) 
                {
                    if(!tar.CanBeChasedBy(Projectile, false) || !tar.active)  //无论如何, 如果大锤子在低于这个lifetime的时候未检索到目标的话, 锤子都应该被击杀了
                    Projectile.Kill();
                }

            }
            else if(Projectile.ai[0] > HitRange - 25f && Projectile.ai[0] < HitRange)//echo达到一定距离后, 速度将会不断缩短
            {
                Projectile.extraUpdates = 3;
                Projectile.velocity.Y *= 0.86f;
                Projectile.rotation += MathHelper.ToRadians(Projectile.ai[0]* 0.5f) * Projectile.localAI[0];
            }

            //无论何时, Echo都应该一直播放飞行的声音
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 60;
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }
            Projectile.rotation += DefualtRotatoin;

        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(152, 245, 249, 50);
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            SpawnExplosion();
            SpawnDust();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            SpawnDust();
        }
    
        private void SpawnExplosion()
        {
            Projectile.netUpdate = true;
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            if (Projectile.owner == Main.myPlayer)
            {
                int explo = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjExplosion>(), (int)(Projectile.damage * 0.45), Projectile.knockBack, Projectile.owner, 0f, 0f);
                if(explo.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[explo].tileCollide = true; 
                }
            }
        }

        private void ReturnDust()
        {
            for (int i = 0; i < 50; i++)
            {
                Dust fire = Dust.NewDustPerfect(Projectile.Center, DustID.GemEmerald);
                fire.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitY).RotatedByRandom(0.8f) * new Vector2(4f, 1.25f) * Main.rand.NextFloat(0.9f, 1f);
                fire.velocity = fire.velocity.RotatedBy(Projectile.rotation - MathHelper.PiOver2);
                fire.velocity += Projectile.velocity/4 *(5*0.04f);
                fire.noGravity = true;
                fire.scale = Main.rand.NextFloat(0.6f, 0.9f) * 5;
                fire = Dust.CloneDust(fire);
                fire.velocity = Main.rand.NextVector2Circular(3f, 3f);
                fire.velocity += Projectile.velocity/2*(5*0.04f);
            }
        }

        //正常击中敌人生成粒子
        private void SpawnDust()
        {
            for (int i = 0; i < 10; i++)
            {
                int triactisDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 1.2f);
                Main.dust[triactisDust].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[triactisDust].scale = 0.5f;
                    Main.dust[triactisDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }

            for (int j = 0; j < 5; j++)
            {
                int triactisDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 1.5f);
                Main.dust[triactisDust2].noGravity = true;
                Main.dust[triactisDust2].velocity *= 5f;
                triactisDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 1f);
                Main.dust[triactisDust2].velocity *= 2f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
