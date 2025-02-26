using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using rail;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeHammerTriactisTruePaladinianMageHammerofMight";
        bool ifSummonClone = false;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 160;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.usesLocalNPCImmunity= true;
            Projectile.localNPCHitCooldown= 10;
            Projectile.timeLeft = 1000;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            Lighting.AddLight(Projectile.Center, 0f, 0.5f, 0.75f);
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }
            /********************大锤子潜伏ai**********************
            *大锤子的潜伏ai可以说是相当暴力, 但也非常有效
            *如大部分锤子一样, ai[0]用于记录锤子是(1f)否(0f)处于返程
            *允许潜伏时, 返程的时候会使用类似于星神之杀的ai
            *回击后会生成一个克隆锤子, 使用类似于圣时之锤的挂载效果
            *而后, 大锤子将会不断地分裂出多个
            *
            *
            *
            */
            switch (Projectile.ai[0])
            {
                case 0f:
                    Projectile.ai[1] += 1f;
                    if (Projectile.ai[1] >= 45f)
                    {
                        Projectile.ai[0] = 1f;
                        Projectile.ai[1] = 0f;
                        Projectile.netUpdate = true;
                    }
                    break;

                case 1f:
                    Projectile.tileCollide = false;
                    float returnSpeed = 20f;
                    float acceleration = 3.2f;
                    CIFunction.BoomerangReturningAI(owner, Projectile, returnSpeed, acceleration);
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Rectangle projHitbox = new((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                        Rectangle mplrHitbox = new((int)owner.position.X, (int)owner.position.Y, (int)(owner.width * 1.5f), (int)(owner.height*1.5f));
                        if (projHitbox.Intersects(mplrHitbox))
                        {
                            if(Projectile.Calamity().stealthStrike && Projectile.ai[2] != -1f) //挂载过的锤子不会再执行一次追踪
                            {
                                Projectile.velocity *= -1f;
                                Projectile.timeLeft = 600;
                                Projectile.penetrate = 1;
                                Projectile.localNPCHitCooldown = -1;
                                Projectile.netUpdate = true;
                                Projectile.ai[0] = 2f;
                            }
                            else if(Projectile.ai[2] == -1f) //ai[2]用于查看锤子是否已经挂载过敌人，如果挂载过了就会赋一个-1f的值
                            {
                                ReturnDust(); //只有挂载在敌人身上的锤子回收在玩家身上的时候才会生成粒子
                                Projectile.ai[2] = 0f;
                            }
                            else
                            Projectile.Kill();
                        }
                    }
                    break;
                
                case 2f:
                    CIFunction.HomeInOnNPC(Projectile, true, 1250f, 20f, 20f);
                    ifSummonClone = true;
                    break;

                default:
                    break;
            }
            //无论状态，锤子都应当在飞行过程中旋转
            Projectile.rotation += 0.43f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(250, 250, 250, 50);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjExplosion>(), (int)(Projectile.damage * 0.25), Projectile.knockBack, Projectile.owner, 0f, 0f);
            if(ifSummonClone) //潜伏时生成的锤子才会具备挂载属性
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X*1.25f, Projectile.velocity.Y*1.05f, ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>(), (int)(Projectile.damage * 0.8f), Projectile.knockBack, Main.myPlayer);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            ifSummonClone = false;
            if(Projectile.Calamity().stealthStrike || Projectile.ai[2] == -1f) //潜伏时生成的粒子被压制到一个非常非常低的值
            StealthSpawnDust();
            else
            SpawnDust();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Projectile.owner == Main.myPlayer)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjExplosion>(), (int)(Projectile.damage * 0.25), Projectile.knockBack, Projectile.owner, 0f, 0f);
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            SpawnDust();
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

        private void StealthSpawnDust() //潜伏攻击的粒子, 被削减至一个非常非常低的值
        {
            for (int i = 0; i < 10; i++)
            {
                int triactisDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 2f);
                Main.dust[triactisDust].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[triactisDust].scale = 0.5f;
                    Main.dust[triactisDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 10; j++)
            {
                int triactisDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 3f);
                Main.dust[triactisDust2].noGravity = true;
                Main.dust[triactisDust2].velocity *= 5f;
                triactisDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 2f);
                Main.dust[triactisDust2].velocity *= 2f;
            }

        }
        //正常击中敌人生成粒子
        private void SpawnDust()
        {
            for (int i = 0; i < 40; i++)
            {
                int triactisDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 2f);
                Main.dust[triactisDust].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[triactisDust].scale = 0.5f;
                    Main.dust[triactisDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 70; j++)
            {
                int triactisDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 3f);
                Main.dust[triactisDust2].noGravity = true;
                Main.dust[triactisDust2].velocity *= 5f;
                triactisDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 2f);
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
